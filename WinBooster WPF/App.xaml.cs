﻿using HandyControl.Themes;
using System;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using WinBoosterNative.winapi;

namespace WinBooster_WPF
{

    public partial class App : Application
    {
        public static string version = "2.1.0.0";

        public static Auth auth;

        public static void UpdateScreenCapture(Window window)
        {
            if (window != null)
            {
                var handle = new WindowInteropHelper(window).Handle;
                if (App.auth.settings.disableScreenCapture == true)
                {
                    UpdateScreenCapture(handle, true);
                }
                else
                {
                    UpdateScreenCapture(handle, false);
                }
            }
        }
        public static void UpdateScreenCapture(Window window, bool force)
        {
            if (window != null)
            {
                var handle = new WindowInteropHelper(window).Handle;
                if (force)
                {
                    UpdateScreenCapture(handle, true);
                }
                else
                {
                    UpdateScreenCapture(handle, false);
                }
            }
        }
        public static void UpdateScreenCapture(IntPtr handle)
        {
            if (App.auth.settings.disableScreenCapture == true)
            {
                UpdateScreenCapture(handle, true);
            }
            else
            {
                UpdateScreenCapture(handle, false);
            }
        }

        public static void UpdateScreenCapture(IntPtr handle, bool hide)
        {
            if (hide && handle != IntPtr.Zero)
            {       
                var ok = FormProtect.SetWindowDisplayAffinity(handle, 0x00000011);
            }
            else
            {
                var ok = FormProtect.SetWindowDisplayAffinity(handle, 0x00000000);
            }
        }

        public App()
        {
            auth = new Auth();
            if (File.Exists("temp.ico"))
            {
                try { File.Delete("temp.ico"); } catch { }
            }
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            Current.MainWindow = auth;
            Current.MainWindow.Show();
        }

        public static void SuperExit()
        {
            try { Auth.tiWorkerServer?.DisposeAsync(); } catch { }
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
