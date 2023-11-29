using CSScripting;
using CSScriptLib;
using DiscordRPC;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinBooster_WPF.Data;
using WinBooster_WPF.Forms;
using WinBooster_WPF.ScriptAPI;
using WinBoosterNative.database.cleaner.workers.language;
using WinBoosterNative.database.sha3;

namespace WinBooster_WPF
{
    public partial class Main : HandyControl.Controls.Window
    {
        public ScriptsForm scriptsForm = new ScriptsForm();
        public CleanerForm cleanerForm = new CleanerForm();
        public SettingsForm settingsForm = new SettingsForm();
        public OptimizeForm optimizeForm = new OptimizeForm();
        public AboutForm aboutForm = new AboutForm();
        public AntiScreenShareForm antiScreenForm = new AntiScreenShareForm();
        public MarketForm marketForm = new MarketForm();

        public Dictionary<string, IScript?> scripts = new Dictionary<string, IScript?>();
        public Dictionary<string, string> scripts_sha3 = new Dictionary<string, string>();
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
            cleanerForm.UpdateCheckboxes();
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
        public void LoadScripts()
        {
            Task t = new Task(async () =>
            {
                string[] files = Directory.GetFiles("C:\\Program Files\\WinBooster\\Scripts");

                if (files.Count() > 0)
                {
                    GrowlInfo growl_loading = new GrowlInfo
                    {
                        Message = "⏱ Loading scripts...",
                        ShowDateTime = true,
                        WaitTime = 1
                    };
                    Growl.InfoGlobal(growl_loading);
                }
               

                Dictionary<string, Exception> errored_sripts = new Dictionary<string, Exception>();

                var script_tasks = new Task<bool>[files.Length];


                //var loader = CSScript.Evaluator.ReferenceDomainAssemblies().ReferenceAssembly(Assembly.GetExecutingAssembly().Location);
                int index = 0;
                foreach (string file in files)
                {
                    var loader = CSScript.Evaluator.ReferenceDomainAssemblies().ReferenceAssembly(Assembly.GetExecutingAssembly().Location);

                    FileInfo info = new FileInfo(file);
                    script_tasks[index] = Task<bool>.Run(() =>
                    {
                        try
                        {
                            string hash = SHA3DataBase.GetHashString(SHA3DataBase.GetHash(File.ReadAllBytes(info.FullName)));
                            scripts_sha3.Add(hash, info.FullName);
                            List<string> dlls = new List<string>();
                            List<string> lines = new List<string>();
                            using (StreamReader streamReader = new StreamReader(file, Encoding.UTF8))
                            {
                                string line = null;
                                while ((line = streamReader.ReadLine()) != null)
                                {
                                    if (line.StartsWith("//dll ") && !line.IsNullOrEmpty())
                                    {
                                        dlls.Add(line.Replace("//dll ", "").Trim());
                                    }
                                    else
                                    {
                                        lines.Add(line);
                                    }
                                }
                            }

                            foreach (string dll in dlls)
                            {
                                loader = loader.ReferenceAssembly(dll);
                            }

                            string code = string.Join("\n", lines);
                            var script = loader.LoadCode<IScript>(code);
                            if (script != null)
                            {
                                string scriptname = script.GetScriptName();
                                if (String.IsNullOrEmpty(scriptname))
                                {
                                    scriptname = info.Name;
                                }
                                bool added = false;
                                lock (scripts)
                                {
                                    if (!scripts.ContainsKey(scriptname))
                                    {
                                        scripts.Add(scriptname, script);
                                        added = true;
                                    }
                                    if (!scripts.ContainsKey(scriptname))
                                    {
                                        scripts.Add(scriptname, script);
                                        added = true;
                                    }
                                }
                                if (added)
                                {
                                    script.OnEnabled();
                                    return true;
                                }
                            }
                            else
                            {
                                errored_sripts.Add(info.Name, null);
                            }
                            return false;
                        }
                        catch (Exception e)
                        {
                            errored_sripts.Add(info.Name, e);
                            Debug.WriteLine("Errored script: " + info.Name);
                            return false;
                        }
                    });
                    index++;
                }

                await Task.WhenAll(script_tasks);


                await Task.Delay(5);
                lock (scripts)
                {
                    var orderedInput = scripts.OrderBy(key => key.Key);
                    var newDict = new Dictionary<string, IScript?>(orderedInput);
                    scripts = newDict;
                }
                await Task.Delay(5);
                cleanerForm.UpdateCheckboxes();
                await Task.Delay(5);
                await cleanerForm.clearListForm.Dispatcher.BeginInvoke(() =>
                {
                    cleanerForm.clearListForm.UpdateList();
                });
                await Task.Delay(5);
                await cleanerForm.clearListForm.Dispatcher.BeginInvoke(() =>
                {
                    cleanerForm.clearListForm.UpdateList2();
                });
                await Task.Delay(5);
                await cleanerForm.clearListForm.Dispatcher.BeginInvoke(() =>
                {
                    cleanerForm.clearListForm.CheckAfterScriptsLoad();
                });

                foreach (var error_script in errored_sripts.ToArray())
                {
                    GrowlInfo growl_scripts = new GrowlInfo
                    {
                        Message = "📁 Error script:\n" + error_script.Key + "\n" + error_script.Value.ToString(),
                        ShowDateTime = true,
                        StaysOpen = true,
                        IconKey = "ErrorGeometry",
                        IconBrushKey = "DangerBrush",
                        IsCustom = true,
                    };

                    Growl.ErrorGlobal(growl_scripts);
                }

                lock (scripts)
                {
                    if (!scripts.IsEmpty())
                    {
                        
                        string print = string.Join("\n", scripts.Keys);
                        GrowlInfo growl_scripts = new GrowlInfo
                        {
                            Message = "📁 Loaded scripts:\n" + print,
                            ShowDateTime = true,
                        };
                        Growl.InfoGlobal(growl_scripts);
                    }
                }
            });
            t.Start();
        }
        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadScripts();

            Task language_and_version_checker = new Task(() =>
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

                try
                {
                    using (WebClient wc = new WebClient())
                    {
                        version = BoosterVersion.FromJson(wc.DownloadString("https://raw.githubusercontent.com/WinBooster/WinBooster_Cloud/main/version.json"));
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

                        if (version != null && version.version != App.version)
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
                                        System.Diagnostics.Process.Start(new ProcessStartInfo(version.download) { UseShellExecute = true });
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
            language_and_version_checker.Start();
            Task.Factory.StartNew(() =>
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo("C:\\Program Files\\WinBooster\\RunAsTI.exe");
                processStartInfo.Arguments = "\"C:\\Program Files\\WinBooster\\TrustedWorker.exe\"";
                var process = System.Diagnostics.Process.Start(processStartInfo);
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

        private void Window_Activated(object sender, EventArgs e)
        {
            SettingsForm.UpdateCapture();
        }

        #region Buttons

        private void Button_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            optimizeForm.Show();
        }

        private void Button_Click_3(object sender, System.Windows.RoutedEventArgs e)
        {
            antiScreenForm.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            aboutForm.Show();
        }

        private void MarketButton_Click(object sender, RoutedEventArgs e)
        {
            scriptsForm.Show();
        }
        #endregion

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
