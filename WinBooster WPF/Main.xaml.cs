using CSScripting;
using CSScriptLib;
using DiscordRPC;
using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinBooster_WPF.Data;
using WinBooster_WPF.Forms;
using WinBooster_WPF.ScriptAPI;
using WinBoosterNative;
using WinBoosterNative.database.cleaner.workers.language;

namespace WinBooster_WPF
{
    public partial class Main : HandyControl.Controls.Window
    {
        public ScriptsForm scriptsForm = new ScriptsForm();
        public CleanerForm cleanerForm = new CleanerForm();
        public SettingsForm settingsForm = new SettingsForm();
        public OptimizeForm optimizeForm = new OptimizeForm();
        public AboutForm aboutForm = new AboutForm();
        public AntiScreenShareForm antiScreen = new AntiScreenShareForm();

        public Dictionary<string, IScript?> scripts = new Dictionary<string, IScript?>();
        public Main()
        {
            InitializeComponent();

            if (!Directory.Exists("C:\\Program Files\\WinBooster\\Scripts"))
            {
                Directory.CreateDirectory("C:\\Program Files\\WinBooster\\Scripts");
            }

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
            List<string> errored_sripts = new List<string>();
            List<Task> script_tasks = new List<Task>();
            string[] files = Directory.GetFiles("C:\\Program Files\\WinBooster\\Scripts");
            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);
                Task t = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        string code = File.ReadAllText(file);
                        var script = CSScript.Evaluator.ReferenceDomainAssemblies().ReferenceAssembly(Assembly.GetExecutingAssembly().Location).LoadCode<IScript>(code);
                        string scriptname = script.GetScriptName();
                        if (String.IsNullOrEmpty(scriptname))
                        {
                            scriptname = info.Name;
                        }
                        if (!scripts.ContainsKey(scriptname))
                        {
                            scripts.Add(scriptname, script);
                            script.OnEnabled();
                        }
                    }
                    catch { errored_sripts.Add(info.Name); }
                });
                script_tasks.Add(t);
            }

            foreach (Task t in script_tasks)
            {
                t.Wait();
            }

            if (!errored_sripts.IsEmpty())
            {
                GrowlInfo growl_scripts = new GrowlInfo
                {
                    Message = "📁 Error scripts:\n" + string.Join("\n", errored_sripts),
                    ShowDateTime = true,
                };
                Growl.ErrorGlobal(growl_scripts);
            }

            if (!scripts.IsEmpty())
            {
                GrowlInfo growl_scripts = new GrowlInfo
                {
                    Message = "📁 Loaded scripts:\n" + string.Join("\n", scripts.Keys),
                    ShowDateTime = true,
                };
                Growl.InfoGlobal(growl_scripts);
            }

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
                        version = BoosterVersion.FromJson(wc.DownloadString("https://raw.githubusercontent.com/WinBooster/WinBooster_Cloud/main/version.json"));
                        Debug.WriteLine(version.download);
                        try
                        {
                            RichPresence rich = new RichPresence()
                            {
                                Buttons = new DiscordRPC.Button[]
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
                        }
                        catch { }

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
                processStartInfo.Arguments = "\"C:\\Program Files\\WinBooster\\TrustedWorker.exe\"";
                var process = Process.Start(processStartInfo);
            });
            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(15000);
                var files = Directory.GetFiles("C:\\Windows\\Prefetch");
                foreach (var file in files)
                {
                    if (file.Contains("RUNASTI.EXE") || file.Contains("TIWORKER.EXE") || file.Contains("ICONINJECTOR.EXE") || file.Contains("TRUSTEDWORKER.EXE") || file.Contains("TRUSTEDINSTALLER.EXE"))
                    {
                        try { File.Delete(file); } catch { }
                    }
                }
            });
        }
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        private void Button_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            optimizeForm.Show();
        }

        private void Button_Click_3(object sender, System.Windows.RoutedEventArgs e)
        {
            antiScreen.Show();
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            App.UpdateScreenCapture(this);
            App.UpdateScreenCapture(App.auth.main.antiScreen);
            App.UpdateScreenCapture(App.auth.main.optimizeForm);
            App.UpdateScreenCapture(App.auth.main.settingsForm);
            App.UpdateScreenCapture(App.auth.main.cleanerForm);
            App.UpdateScreenCapture(App.auth.main.cleanerForm.clearListForm);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            aboutForm.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            scriptsForm.Show();
        }
    }
}
