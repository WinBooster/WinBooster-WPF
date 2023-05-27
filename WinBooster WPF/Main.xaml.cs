using DiscordRPC;
using HandyControl.Controls;
using HandyControl.Data;
using System.Diagnostics;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using WinBooster_WPF.Data;
using WinBooster_WPF.Forms;
using WinBoosterNative.database.cleaner.workers.language;

namespace WinBooster_WPF
{
    public partial class Main : Window
    {
        public CleanerForm cleanerForm = new CleanerForm();
        public SettingsForm settingsForm = new SettingsForm();
        public OptimizeForm optimizeForm = new OptimizeForm();
        public Main()
        {
            InitializeComponent();


            var timer = new System.Timers.Timer(150);
            timer.Elapsed += (sender, args) => { App.client.Invoke(); };
            timer.Start();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            App.SuperExit();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            cleanerForm.Show();
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            settingsForm.Show();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            App.SuperExit();
        }

        public BoosterVersion? version;
        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ILanguageWorker.WindowsLanguage() == ILanguageWorker.Language.Unknow)
            {
                GrowlInfo growl = new GrowlInfo
                {
                    Message = "You'r windows language not support\nfor clearing \"Language\"",
                    ShowDateTime = true,
                    IconKey = "WarningGeometry",
                    IconBrushKey = "WarningBrush",
                    IsCustom = true
                };
                Growl.InfoGlobal(growl);
            }

            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        version = BoosterVersion.FromJson(wc.DownloadString("https://github.com/Nekiplay/WinBooster_Cloud/raw/main/version.json"));
                        Debug.WriteLine(version.download);
                        RichPresence rich = new RichPresence()
                        {
                            Buttons = new Button[]
                            {
                                new DiscordRPC.Button() { Label = "Download", Url = version?.download }
                            },
                            Assets = new Assets()
                            {
                                LargeImageKey = "speed",
                            }
                        };
                        if (App.auth.settings.discordRich == true)
                        {
                            App.client.SetPresence(rich);
                        }

                        if (version.version != App.version)
                        {
                            string msg = "New update found\nNew version: " + version.version + "\nYou version: " + App.version;
                            if (!string.IsNullOrEmpty(version.description))
                            {
                                msg += "\n\nDescription:\n" + version.description;
                            }
                            GrowlInfo growl = new GrowlInfo
                            {
                                Message = msg,
                                ShowDateTime = true,
                                IconKey = "AskGeometry",
                                IconBrushKey = "WarningBrush",
                                IsCustom = true,
                                ConfirmStr = "Download",
                                CancelStr = "Skip",
                                ActionBeforeClose = isConfirmed =>
                                {
                                    if (isConfirmed)
                                    {
                                        Process.Start(new ProcessStartInfo(version.download) { UseShellExecute = true });
                                    }
                                    return true;
                                }
                            };
                            Growl.AskGlobal(growl);
                        }
                    }
                }
                catch { }
            });

            Task.Factory.StartNew(() =>
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo("C:\\Program Files\\WinBooster\\RunAsTI.exe");
                processStartInfo.Arguments = 
                Process.Start(new ProcessStartInfo("C:\\Program Files\\WinBooster\\") { UseShellExecute = true });
            });
        }

        private void Button_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            optimizeForm.Show();
        }
    }
}
