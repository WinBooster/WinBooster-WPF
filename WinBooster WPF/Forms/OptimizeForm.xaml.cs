﻿using Microsoft.Win32;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WinBooster_WPF.Forms.OptimizeClasses;
using WinBoosterNative;

namespace WinBooster_WPF.Forms
{
    /// <summary>
    /// Логика взаимодействия для OptimizeForm.xaml
    /// </summary>
    public partial class OptimizeForm : HandyControl.Controls.Window
    {
        public OptimizeForm()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region Проверка алгоритма Nagle
            Task.Factory.StartNew(() =>
            {
                bool enabled = false;
                try
                {
                    RegistryKey reg = Registry.LocalMachine;
                    RegistryKey Interfaces = reg.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces");
                    if (Interfaces == null)
                    {
                        reg.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces");
                    }
                    string[] names = Interfaces.GetSubKeyNames();
                    foreach (string name in names)
                    {
                        RegistryKey kk = Interfaces.OpenSubKey(name);
                        if (kk.GetValue("TcpAckFrequency") != null
                        && kk.GetValue("TcpNoDelay") != null
                        && kk.GetValue("TcpAckFrequency").ToString() == "1"
                        && kk.GetValue("TcpNoDelay").ToString() == "1")
                        {
                            enabled = true;
                        }
                        RegistryKey Software = reg.OpenSubKey(@"Software\Microsoft\MSMQ\Parameters", false);
                        if (Software == null)
                        {
                            reg.CreateSubKey(@"Software\Microsoft\MSMQ\Parameters");
                        }
                        if (Software.GetValue("TcpNoDelay") == null
                        || Software.GetValue("TcpNoDelay").ToString() != "1")
                        {
                            enabled = false;
                        }
                    }
                }
                catch { }
                this.Dispatcher.Invoke(() => NagleAlgorithm.IsChecked = enabled);
            });
            #endregion
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void NagleAlgorithm_Checked(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("А");
            RegistryKey reg = Registry.LocalMachine;
            /* Включение в интерфейсах */
            RegistryKey Interfaces = reg.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces", true);
            string[] names = Interfaces.GetSubKeyNames();
            foreach (string name in names)
            {
                RegistryKey keys = Interfaces.OpenSubKey(name, true);
                keys.SetValue("TcpNoDelay", 1);
                keys.SetValue("TcpAckFrequency", 1);
            }

            /* Включение по документаций Microsoft */
            RegistryKey Software = reg.OpenSubKey(@"Software\Microsoft\MSMQ\Parameters", true);
            Software.SetValue("TcpNoDelay", 1);
            Software.SetValue("TcpAckFrequency", 1);

            Task.Factory.StartNew(() =>
            {
                new ProcessUtils().StartCmd("netsh interface ip delete arpcache"
                    + " & netsh winsock reset catalog"
                    + " & netsh int ip reset c:resetlog.txt"
                    + " & netsh int ip reset C:\tcplog.txt"
                    + " & netsh winsock reset catalog"
                    + " & netsh int tcp set global rsc=enabled"
                    + " & netsh int tcp set heuristics disabled"
                    + " & netsh int tcp set global dca=enabled"
                    + " & netsh int tcp set global netdma=enabled"
                    );
            });
        }

        private void NagleAlgorithm_Unchecked(object sender, RoutedEventArgs e)
        {
            RegistryKey reg = Registry.LocalMachine;
            /* Удаление в интерфейсах */
            RegistryKey Interfaces = reg.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\Tcpip\Parameters\Interfaces", true);
            string[] names = Interfaces.GetSubKeyNames();
            foreach (string name in names)
            {
                RegistryKey keys = Interfaces.OpenSubKey(name, true);
                try { keys.DeleteValue("TcpNoDelay"); } catch { }
                try { keys.DeleteValue("TcpAckFrequency"); } catch { }
            }
            /* Удаление по документаций Microsoft */
            RegistryKey Software = reg.OpenSubKey(@"Software\Microsoft\MSMQ\Parameters", true);
            Software.DeleteValue("TcpNoDelay");
            Software.DeleteValue("TcpAckFrequency");
        }
    }
}