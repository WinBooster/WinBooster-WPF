﻿using System.Diagnostics;
using System.Windows;

namespace WinBooster_WPF.Forms
{
    /// <summary>
    /// Логика взаимодействия для AboutForm.xaml
    /// </summary>
    public partial class AboutForm : HandyControl.Controls.Window
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/WinBooster",
                UseShellExecute = true
            });
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://t.me/nekiplay",
                UseShellExecute = true
            });
        }
    }
}
