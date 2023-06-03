using DiscordRPC;
using H.Formatters;
using H.Pipes;
using HandyControl.Themes;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Interop;
using WinBooster_WPF.RemoteControl;
using WinBooster_WPF.RemoteControl.Pipeline.Messages;
using WinBoosterNative.winapi;

namespace WinBooster_WPF
{

    public partial class App : Application
    {
        public static string version = "2.0.8.3";

        public static RemoteControlData remoteControlData = new RemoteControlData();
        public static TelegramRemoteControl telegramRemoteControl = new TelegramRemoteControl("6074872423:AAFLKuUDD-JFVPQmTxho1zpYVQNRjfsfzQQ");
        public static DiscordRpcClient client;
        public static Auth auth;

        public static void UpdateScreenCapture(Window window)
        {
            if (window != null)
            {
                var mainWindowHandle = new WindowInteropHelper(window).Handle;
                if (App.auth.settings.DisableScreenCapture == true)
                {
                    var ok = FormProtect.SetWindowDisplayAffinity(mainWindowHandle, 1);
                }
                else
                {
                    var ok = FormProtect.SetWindowDisplayAffinity(mainWindowHandle, 0);
                }
            }
        }

        public App()
        {
            auth = new Auth();
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            Current.MainWindow = auth;
            Current.MainWindow.Show();
        }

        public static void SuperExit()
        {
            try { client?.ClearPresence(); } catch { }
            try { client?.Dispose(); } catch { }
            try { Auth.tiWorkerServer?.DisposeAsync(); } catch { }
            Process.GetCurrentProcess().Kill();
        }
    }
}
