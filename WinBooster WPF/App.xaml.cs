using DiscordRPC;
using H.Formatters;
using H.Pipes;
using HandyControl.Themes;
using System.Diagnostics;
using System.Net;
using System.Windows;
using WinBooster_WPF.RemoteControl;
using WinBooster_WPF.RemoteControl.Pipeline.Messages;

namespace WinBooster_WPF
{

    public partial class App : Application
    {
        public static string version = "2.0.6";

        public static RemoteControlData remoteControlData = new RemoteControlData();
        public static TelegramRemoteControl telegramRemoteControl = new TelegramRemoteControl("6074872423:AAFLKuUDD-JFVPQmTxho1zpYVQNRjfsfzQQ");
        public static DiscordRpcClient client;
        public static Auth auth;


        public App()
        {

            using (WebClient wc = new WebClient())
            {
                try { remoteControlData.main.ip = wc.DownloadString("https://api.ipify.org"); } catch { }
            }
            client = new DiscordRpcClient("1084174375814713374");
            remoteControlData.main.hardwareID = WinBoosterNative.security.HardwareID.GetHardwareID();
            client.OnReady += (a, e) =>
            {
                remoteControlData.discord.id = client.CurrentUser.ID;
                remoteControlData.discord.name = client.CurrentUser.Username + "#" + client.CurrentUser.Discriminator.ToString();
            };
            client.Initialize();
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
