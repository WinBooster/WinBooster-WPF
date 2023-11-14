using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinBoosterNative.database.cleaner;
using WinBoosterNative.security;

namespace WinBooster_WPF.Forms
{
    /// <summary>
    /// Логика взаимодействия для ClearListForm.xaml
    /// </summary>
    public partial class ClearListForm : HandyControl.Controls.Window
    {
        public ObservableCollection<DataBaseGrid> list = new ObservableCollection<DataBaseGrid>();
        public Dictionary<string, DataBaseGrid> keyValues = new Dictionary<string, DataBaseGrid>();
        private const string databasePath = "C:\\Program Files\\WinBooster\\DataBase\\clear.json";

        public struct ListSctr
        {
            public DataBaseGrid dataBaseGrid;
            public string category;
        }
        public void CheckAfterScriptsLoad()
        {
            string json = File.ReadAllText(databasePath);
            AESCryptor cryptor = new AESCryptor();
            cryptor.SetPassword(WinBoosterNative.data.Settings.protection_password, WinBoosterNative.data.Settings.protection_salt);
            CleanerEnabledSettings? dataBase = CleanerEnabledSettings.FromJson(json);
            if (dataBase != null)
            {
                enabledSettings = dataBase;
                bool all_enabled = true;
                foreach (var enabled in enabledSettings.keyValues.Keys.ToArray())
                {
                    bool find = false;
                    foreach (var cleaner in list)
                    {
                        if (cleaner.Program == enabled)
                        {
                            find = true;
                            break;
                        }
                    }
                    if (!enabledSettings.keyValues[enabled])
                    {
                        all_enabled = false;
                    }
                    if (!find)
                    {
                        enabledSettings.keyValues.Remove(enabled);
                        byte[] bytes = Encoding.UTF8.GetBytes(enabledSettings.ToJson());
                        byte[] encrypted = cryptor.Encrypt(bytes);
                    
                        Directory.CreateDirectory("C:\\Program Files\\WinBooster\\Settings");
                        File.Create("C:\\Program Files\\WinBooster\\Settings\\Enabled.json").Close();
                        File.WriteAllBytes("C:\\Program Files\\WinBooster\\Settings\\Enabled.json", encrypted);
                    }
                }

                if (all_enabled)
                {
                    File.Delete("C:\\Program Files\\WinBooster\\Settings\\Enabled.json");
                }
            }
        }
        public void UpdateList()
        {
            if (File.Exists(databasePath))
            {
                string json = File.ReadAllText(databasePath);
                CleanerDataBase? dataBase = CleanerDataBase.FromJson(json);

                if (dataBase != null)
                {
                    List<Task<ListSctr>> tasks = new List<Task<ListSctr>>();

                    foreach (var script in App.auth.main.scripts.Values)
                    {
                        if (script != null)
                        {
                            script.OnCleanerInit(dataBase);
                        }
                    }

                    int index = 0;
                    foreach (var category in dataBase.cleaners)
                    {
                        Task<ListSctr> t = new Task<ListSctr>(() =>
                        {
                            ListSctr sctr = new ListSctr();
                            DataBaseGrid dataBaseGrid = new DataBaseGrid();
                            dataBaseGrid.Program = category.GetCategory();
                            dataBaseGrid.Detected = category.IsAvalible();
                            sctr.dataBaseGrid = dataBaseGrid;
                            sctr.category = category.GetCategory();
                            List<string> categories = new List<string>();

                            List<ICleanerWorker> workers = category.GetWorkers();
                            foreach (var worker in workers)
                            {

                                string categorys = worker.GetCategory();
                                if (!categories.Contains(categorys))
                                    categories.Add(categorys);
                            }

                            dataBaseGrid.Category = string.Join(", ", categories);

                            return sctr;
                        });
                        tasks.Add(t);
                        t.Start();
                    }

                    lock (list)
                    {
                        foreach (Task<ListSctr> task in tasks)
                        {
                            task.Wait();
                            ListSctr result = task.Result;

                            result.dataBaseGrid.Index = index;

                            if (!keyValues.ContainsKey(result.category))
                            {
                                list.Add(result.dataBaseGrid);
                                keyValues.Add(result.category, result.dataBaseGrid);
                            }
                            index++;
                        }
                    }
                }
            }

            if (File.Exists("C:\\Program Files\\WinBooster\\Settings\\Enabled.json"))
            {
                AESCryptor cryptor = new AESCryptor();
                cryptor.SetPassword(WinBoosterNative.data.Settings.protection_password, WinBoosterNative.data.Settings.protection_salt);
                string json = "";
                try { 
                    byte[] bytes2 = File.ReadAllBytes("C:\\Program Files\\WinBooster\\Settings\\Enabled.json");
                    byte[] decrypted = cryptor.Decrypt(bytes2);

                    json = Encoding.UTF8.GetString(decrypted);
                }
                catch
                {
                    File.Delete("C:\\Program Files\\WinBooster\\Settings\\Enabled.json");
                }

                CleanerEnabledSettings? dataBase = CleanerEnabledSettings.FromJson(json);
                if (dataBase != null)
                {
                    enabledSettings = dataBase;
                    bool all_enabled = true;
                    foreach (var enabled in enabledSettings.keyValues.Keys.ToArray())
                    {
                        bool find = false;
                        foreach (var cleaner in list)
                        {
                            if (cleaner.Program == enabled)
                            {
                                find = true;
                                break;
                            }
                        }
                        if (!enabledSettings.keyValues[enabled])
                        {
                            all_enabled = false;
                        }
                    }

                    if (all_enabled)
                    {
                        File.Delete("C:\\Program Files\\WinBooster\\Settings\\Enabled.json");
                    }
                }
            }
            else
            {
                foreach (var cleaner in list)
                {
                    enabledSettings.keyValues.Add(cleaner.Program, true);
                }

                AESCryptor cryptor = new AESCryptor();
                cryptor.SetPassword(WinBoosterNative.data.Settings.protection_password, WinBoosterNative.data.Settings.protection_salt);

                byte[] bytes = Encoding.UTF8.GetBytes(enabledSettings.ToJson());
                byte[] encrypted = cryptor.Encrypt(bytes);

                Directory.CreateDirectory("C:\\Program Files\\WinBooster\\Settings");
                File.Create("C:\\Program Files\\WinBooster\\Settings\\Enabled.json").Close();
                File.WriteAllBytes("C:\\Program Files\\WinBooster\\Settings\\Enabled.json", encrypted);
            }
        }
        public void UpdateList2()
        {
            Dispatcher.Invoke(() =>
            {
                lock (list)
                {
                    DataGrid.Items.Clear();
                    DataGrid.ItemsSource = null;
                    //foreach (var item in list)
                    //{
                    //    DataGrid.Items.Add(item);
                    //}
                    DataGrid.ItemsSource = list;
                    //DataGrid.ItemsSource = list;
                    //DataGrid.Items.Refresh();
                    double wi = 0;
                    foreach (var colum in DataGrid.Columns)
                    {
                        wi += colum.Width.Value;
                    }
                    this.Width = wi;
                }
            });
        }
        public ClearListForm()
        {
            InitializeComponent();
        }
        public static CleanerEnabledSettings enabledSettings = new CleanerEnabledSettings();

        public class CleanerEnabledSettings
        {
            public Dictionary<string, bool> keyValues = new Dictionary<string, bool>();

            public string ToJson()
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new JavaScriptDateTimeConverter());
                settings.Converters.Add(new StringEnumConverter());
                settings.Formatting = Formatting.Indented;
                return JsonConvert.SerializeObject(this, settings);
            }

            public static CleanerEnabledSettings? FromJson(string json)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new JavaScriptDateTimeConverter());
                settings.Converters.Add(new StringEnumConverter());
                settings.Formatting = Formatting.Indented;
                return JsonConvert.DeserializeObject<CleanerEnabledSettings>(json, settings);
            }
        }

        public class DataBaseGrid
        {
            public int Index { get; set; }
            public bool Detected { get; set; }
            public string Program { get; set; }
            public string Category { get; set; }
            public bool Enabled 
            { 
                get
                {
                    if (enabledSettings.keyValues.ContainsKey(Program))
                        return enabledSettings.keyValues[Program];
                    return true;
                }
                set
                {
                    if (enabledSettings.keyValues.ContainsKey(Program))
                        enabledSettings.keyValues[Program] = value;
                    else
                        enabledSettings.keyValues.Add(Program, value);

                    AESCryptor cryptor = new AESCryptor();
                    cryptor.SetPassword(WinBoosterNative.data.Settings.protection_password, WinBoosterNative.data.Settings.protection_salt);

                    byte[] bytes = Encoding.UTF8.GetBytes(enabledSettings.ToJson());
                    byte[] encrypted = cryptor.Encrypt(bytes);

                    Directory.CreateDirectory("C:\\Program Files\\WinBooster\\Settings");
                    File.Create("C:\\Program Files\\WinBooster\\Settings\\Enabled.json").Close();
                    File.WriteAllBytes("C:\\Program Files\\WinBooster\\Settings\\Enabled.json", encrypted);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            SettingsForm.UpdateCapture();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            SettingsForm.UpdateCapture();
        }

        public void SearchCmd(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                //Dispatcher.Invoke(() =>
                //{
                //    keyValues.Clear();
                //    list.Clear();
                //});
                UpdateList();
                UpdateList2();
            });
        }
    }
}
