

using H.Formatters;
using H.Pipes;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using WinBooster_WPF.RemoteControl;
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
                        await Task.Delay(150);
                        StepBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            StepBar.Next();
                        }));

                        await Task.Delay(150);
                        this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                        {
                            OpenForm();
                        }));
                    }
                });
            }
            else
            {
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
                    var bytes = wc.DownloadData("https://github.com/WinBooster/WinBooster_Cloud/raw/main/files/RunAsTI.exe");
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
            }

            if (!System.IO.File.Exists("C:\\Program Files\\WinBooster\\IconInjector.exe"))
            {
                try
                {
                    var bytes = wc.DownloadData("https://github.com/WinBooster/WinBooster_Cloud/raw/main/files/IconInjector.exe");
                    if (!System.IO.File.Exists("C:\\Program Files\\WinBooster\\IconInjector.exe"))
                        System.IO.File.Create("C:\\Program Files\\WinBooster\\IconInjector.exe").Close();
                    System.IO.File.WriteAllBytes("C:\\Program Files\\WinBooster\\IconInjector.exe", bytes);
                }
                catch
                {
                    GrowlInfo growl = new GrowlInfo
                    {
                        Message = "Error downloading: IconInjector",
                        ShowDateTime = true,
                        IconKey = "ErrorGeometry",
                        IconBrushKey = "DangerBrush",
                        IsCustom = true
                    };
                    Growl.InfoGlobal(growl);
                }
            }

            if (!System.IO.File.Exists("C:\\Program Files\\WinBooster\\TrustedWorker.exe"))
            {
                try
                {
                    var bytes = wc.DownloadData("https://github.com/WinBooster/WinBooster_Cloud/raw/main/files/TrustedWorker.exe");
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
            }
            StepBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                StepBar.Next();
                StepBar2.Visibility = System.Windows.Visibility.Visible;
            }));
            StepBar2.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                StackPanel cleaner_stack = new StackPanel();
                TextBlock cleaner_text = new TextBlock();
                cleaner_text.Text = "Cleaner";
                cleaner_stack.Children.Add(cleaner_text);
                StepBar2.Items.Add(cleaner_stack);

                StackPanel scripts_stack = new StackPanel();
                TextBlock scripts_text = new TextBlock();
                scripts_text.Text = "Scripts";
                scripts_stack.Children.Add(scripts_text);
                StepBar2.Items.Add(scripts_stack);

                StackPanel sha3_stack = new StackPanel();
                TextBlock sha3_text = new TextBlock();
                sha3_text.Text = "Hash";
                sha3_stack.Children.Add(sha3_text);
                StepBar2.Items.Add(sha3_stack);

                StackPanel names_stack = new StackPanel();
                TextBlock names_text = new TextBlock();
                names_text.Text = "Names";
                names_stack.Children.Add(names_text);
                StepBar2.Items.Add(names_stack);
                StepBar2.Visibility = System.Windows.Visibility.Visible;
                StepBar2.Width = 245;
                this.Height = 180;
            }));
            #region Cleaner DataBase
            try
            {
                string bdPath = "C:\\Program Files\\WinBooster\\DataBase\\clear.json";
                string bd = wc.DownloadString("https://raw.githubusercontent.com/WinBooster/WinBooster_Cloud/main/database/clear.json");
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
            #endregion
            StepBar2.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                StepBar2.Next();
            }));
            #region Scripts DataBase
            try
            {
                string bdPath = "C:\\Program Files\\WinBooster\\DataBase\\scripts.json";
                string bd = wc.DownloadString("https://raw.githubusercontent.com/WinBooster/WinBooster_Scripts/main/list.json");
                if (!System.IO.File.Exists(bdPath))
                {
                    System.IO.File.Create(bdPath).Close();
                }

                string current = System.IO.File.ReadAllText(bdPath);
                if (current != bd)
                {
                    GrowlInfo growl = new GrowlInfo
                    {
                        Message = "Successfully updated: scripts database",
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
                string bdPath = "C:\\Program Files\\WinBooster\\DataBase\\scripts.json";
                string message = "Error updating: scripts database";
                if (!System.IO.File.Exists(bdPath))
                {
                    message = "Error downloading: scripts database";
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
            #endregion
            StepBar2.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                StepBar2.Next();
            }));
            #region SHA3 DataBase
            try
            {
                string bdPath = "C:\\Program Files\\WinBooster\\DataBase\\sha3.json";
                string bd = wc.DownloadString("https://raw.githubusercontent.com/WinBooster/WinBooster_Cloud/main/database/sha3/list.json");
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
            #endregion
            StepBar2.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                StepBar2.Next();
            }));
            #region File Name Languages DataBase
            try
            {
                string bdPath = "C:\\Program Files\\WinBooster\\DataBase\\fileNameLanguages.json";
                string bd = wc.DownloadString("https://raw.githubusercontent.com/WinBooster/WinBooster_Cloud/main/database/fileNameToLanguage.json");
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
            StepBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                StepBar.Next();
            }));
            #endregion
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
            this.Width = 240;

            string password = string.Empty;
            Settings? temp = Settings.FromFile(settings.GetPath(), Settings.protection_password, Settings.protection_salt);
            if (temp != null)
            {
                password = temp.password;
                settings = temp;
            }
            App.UpdateScreenCapture(this);
            bool work = true;
            try
            {
                var server = new PipeServer<string>("WinBoosterChecker", formatter: new NewtonsoftJsonFormatter());
                server.StartAsync().Wait(1000);
                tiWorkerServer = new PipeServer<string>("WinBoosterTiCkecker", formatter: new NewtonsoftJsonFormatter());
                tiWorkerServer.StartAsync().Wait(1000);
                var client2 = new PipeClient<string>("WinBoosterChecker", formatter: new NewtonsoftJsonFormatter());
                client2.ConnectAsync().Wait(1000);
            }
            catch
            {
                await Task.Delay(5);
                GrowlInfo growl = new GrowlInfo
                {
                    Message = "Program is already running!",
                    ShowDateTime = true,
                    IconKey = "ErrorGeometry",
                    IconBrushKey = "DangerBrush",
                    IsCustom = true
                };
                Growl.InfoGlobal(growl);
                work = false;
                this.Hide();
                await Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(5000);
                    App.SuperExit();
                });
            }

            if (work)
            {
                try
                {
                    if (!Directory.Exists("C:\\Program Files\\WinBooster"))
                    {
                        Directory.CreateDirectory("C:\\Program Files\\WinBooster");
                    }
                }
                catch { }
                if (string.IsNullOrEmpty(password) || password == "")
                {
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
                            await Task.Delay(150);
                            StepBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                            {
                                StepBar.Next();
                            }));

                            await Task.Delay(150);
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
}
