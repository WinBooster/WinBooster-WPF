using CSScriptLib;
using HandyControl.Controls;
using HandyControl.Data;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.Xml;
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
using WinBooster_WPF.ScriptAPI;
using WinBoosterNative.database.cleaner;
using WinBoosterNative.database.scripts;
using WinBoosterNative.database.sha3;

namespace WinBooster_WPF.Forms
{
    /// <summary>
    /// Логика взаимодействия для ScriptsForm.xaml
    /// </summary>
    public partial class ScriptsForm : HandyControl.Controls.Window
    {
        public ScriptsForm()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
        private void UpdateInstalledScripts()
        {
            string[] files = Directory.GetFiles("C:\\Program Files\\WinBooster\\Scripts");
            App.auth.main.scripts_sha3.Clear();
            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);

                string hash = SHA3DataBase.GetHashString(SHA3DataBase.GetHash(File.ReadAllBytes(info.FullName)));
                App.auth.main.scripts_sha3.Add(hash, info.FullName);
            }
        }
        public void UpdateList()
        {
            if (File.Exists(databasePath))
            {
                string json = File.ReadAllText(databasePath);
                ScriptsDataBase? dataBase = ScriptsDataBase.FromJson(json);
                if (dataBase != null)
                {
                    int row = 0;
                    int col = 0;
                    Dispatcher.Invoke(() =>
                    {
                        Categories.Children.Clear();
                    });
                    foreach (var script in dataBase.scripts.ToArray())
                    {
                        if (!App.auth.main.scripts_sha3.ContainsKey(script.sha3))
                        {
                            Dispatcher.Invoke(() =>
                            {
                                StackPanel stack = new StackPanel();
                                stack.Margin = new System.Windows.Thickness(5);
                                stack.SetValue(Grid.RowProperty, row);
                                stack.SetValue(Grid.ColumnProperty, col);

                                Grid grid = new Grid();
                                GroupBox groupBox = new GroupBox();
                                groupBox.Height = 120;
                                groupBox.Width = 280;
                                groupBox.VerticalAlignment = VerticalAlignment.Center;
                                groupBox.HorizontalAlignment = HorizontalAlignment.Left;
                                HeaderedContentControl headered = new HeaderedContentControl();

                                Grid header_grid = new Grid();

                                System.Windows.Shapes.Path animation = new System.Windows.Shapes.Path();
                                animation.Margin = new Thickness(-4, 0, 0, 0);
                                animation.Data = Geometry.Parse(script.icon);
                                animation.Width = 20;
                                animation.Height = 20;
                                animation.Stretch = Stretch.Fill;
                                //animation.Duration = new Duration(TimeSpan.FromSeconds(5));
                                animation.Stroke = new SolidColorBrush(Colors.CornflowerBlue);
                                animation.StrokeThickness = 0.5;
                                animation.HorizontalAlignment = HorizontalAlignment.Left;
                                header_grid.Children.Add(animation);

                                TextBlock name = new TextBlock();
                                name.Text = script.name;
                                name.HorizontalAlignment = HorizontalAlignment.Left;
                                name.Margin = new Thickness(22, 0, 0, 0);
                                header_grid.Children.Add(name);

                                headered.Header = header_grid;
                                groupBox.Header = headered;
                                grid.Children.Add(groupBox);
                                stack.Children.Add(grid);

                                Grid content_grid = new Grid();

                                for (int i = 0; i > 3; i++)
                                {
                                    var rowDefinition = new RowDefinition();
                                    rowDefinition.Height = GridLength.Auto;
                                    content_grid.RowDefinitions.Add(rowDefinition);
                                }

                                for (int i = 0; i > 3; i++)
                                {
                                    var colDefinition = new ColumnDefinition();
                                    colDefinition.Width = GridLength.Auto;
                                    content_grid.ColumnDefinitions.Add(colDefinition);
                                }

                                StackPanel info_panel = new StackPanel();
                                info_panel.SetValue(Grid.RowProperty, 0);
                                info_panel.SetValue(Grid.ColumnProperty, 0);
                                info_panel.Margin = new Thickness(2);

                                TextBlock version_text = new TextBlock();
                                version_text.Text = "Version: " + script.version;
                                info_panel.Children.Add(version_text);

                                TextBlock description_text = new TextBlock();
                                description_text.Text = "Description: " + script.description;
                                info_panel.Children.Add(description_text);


                                StackPanel buttons_panel = new StackPanel();
                                buttons_panel.Margin = new Thickness(0, 5, 0, 0);
                                Grid buttons_grid = new Grid();
                                for (int i = 0; i > 3; i++)
                                {
                                    var rowDefinition = new RowDefinition();
                                    rowDefinition.Height = GridLength.Auto;
                                    buttons_grid.RowDefinitions.Add(rowDefinition);
                                }

                                var colDefinition2 = new ColumnDefinition();
                                colDefinition2.Width = new GridLength(115);
                                var colDefinition3 = new ColumnDefinition();
                                colDefinition3.Width = new GridLength(110);
                                buttons_grid.ColumnDefinitions.Add(colDefinition2);



                                Button install_button = new Button();
                                install_button.Click += ((a, b) =>
                                {
                                    string file_path = "C:\\Program Files\\WinBooster\\Scripts\\" + script.name + ".cs";
                                    using (WebClient wc = new WebClient())
                                    {
                                        wc.DownloadFile(script.url, file_path);
                                    }
                                    FileInfo info = new FileInfo(file_path);
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        UpdateInstalledScripts();
                                        UpdateList();
                                    });
                                    var loader = CSScript.Evaluator.ReferenceDomainAssemblies().ReferenceAssembly(Assembly.GetExecutingAssembly().Location);
                                    string code;
                                    using (StreamReader streamReader = new StreamReader(file_path, Encoding.UTF8))
                                    {
                                        code = streamReader.ReadToEnd();
                                    }
                                    var script_loaded = loader.LoadCode<IScript>(code);
                                    if (script_loaded != null)
                                    {
                                        string scriptname = script_loaded.GetScriptName();
                                        if (String.IsNullOrEmpty(scriptname))
                                        {
                                            scriptname = info.Name;
                                        }
                                        bool added = false;
                                        lock (App.auth.main.scripts)
                                        {
                                            if (!App.auth.main.scripts.ContainsKey(scriptname))
                                            {
                                                App.auth.main.scripts.Add(scriptname, script_loaded);
                                                added = true;
                                            }
                                        }
                                        if (added)
                                        {
                                            script_loaded.OnEnabled();

                                            var orderedInput = App.auth.main.scripts.OrderBy(key => key.Key);
                                            var newDict = new Dictionary<string, IScript?>(orderedInput);
                                            App.auth.main.scripts = newDict;
                                            App.auth.main.cleanerForm.UpdateCheckboxes();
                                            App.auth.main.cleanerForm.clearListForm.Dispatcher.Invoke(() =>
                                            {
                                                App.auth.main.cleanerForm.clearListForm.UpdateList();
                                                App.auth.main.cleanerForm.clearListForm.UpdateList2();
                                                App.auth.main.cleanerForm.clearListForm.CheckAfterScriptsLoad();
                                            });

                                            GrowlInfo growl_scripts = new GrowlInfo
                                            {
                                                Message = "📁 Installed and loaded script:\n" + scriptname,
                                                ShowDateTime = true,
                                            };
                                            Growl.InfoGlobal(growl_scripts);
                                        }
                                    }
                                    else
                                    {
                                        GrowlInfo growl_scripts = new GrowlInfo
                                        {
                                            Message = "📁 Installing error script:\n" + script.name,
                                            ShowDateTime = true,
                                        };

                                        Growl.ErrorGlobal(growl_scripts);
                                    }
                                });
                                install_button.SetValue(Grid.RowProperty, 0);
                                install_button.HorizontalAlignment = HorizontalAlignment.Left;
                                TextBlock install_text = new TextBlock();
                                install_text.Text = "Install";
                                install_button.Content = install_text;
                                buttons_grid.ColumnDefinitions.Add(colDefinition3);
                                buttons_grid.Children.Add(install_button);

                                Button source_button = new Button();
                                source_button.Click += ((a, b) =>
                                {
                                    System.Diagnostics.Process.Start(new ProcessStartInfo
                                    {
                                        FileName = script.url,
                                        UseShellExecute = true
                                    });
                                });
                                source_button.SetValue(Grid.RowProperty, 1);
                                source_button.HorizontalAlignment = HorizontalAlignment.Right;
                                TextBlock source_text = new TextBlock();
                                source_text.Text = "Source";
                                buttons_grid.Children.Add(source_button);
                                source_button.Content = source_text;



                                buttons_panel.Children.Add(buttons_grid);


                                info_panel.Children.Add(buttons_panel);

                                content_grid.Children.Add(info_panel);



                                groupBox.Content = content_grid;
                                col++;
                                if (col == 2)
                                {
                                    col = 0;
                                    var rowDefinition2 = new RowDefinition();
                                    rowDefinition2.Height = GridLength.Auto;
                                    Categories.RowDefinitions.Add(rowDefinition2);
                                    row++;
                                }
                                Categories.Children.Add(stack);
                            });
                        }

                    }
                }
            }
        }
        private const string databasePath = @"C:\Program Files\WinBooster\DataBase\scripts.json";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            SettingsForm.UpdateCapture();
        }
    }
}
