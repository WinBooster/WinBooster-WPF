﻿using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WinBoosterNative.database.cleaner;
using System.Windows.Interop;
using WinBoosterNative.winapi;
using WinBoosterNative.security;

namespace WinBooster_WPF.Forms
{
    /// <summary>
    /// Логика взаимодействия для ClearListForm.xaml
    /// </summary>
    public partial class ClearListForm : HandyControl.Controls.Window
    {
        public List<DataBaseGrid> list = new List<DataBaseGrid>();
        public Dictionary<string, DataBaseGrid> keyValues = new Dictionary<string, DataBaseGrid>();
        private const string databasePath = "C:\\Program Files\\WinBooster\\DataBase\\clear.json";
        public void UpdateList()
        {
            if (File.Exists(databasePath))
            {
                string json = File.ReadAllText(databasePath);
                CleanerDataBase? dataBase = CleanerDataBase.FromJson(json);

                if (dataBase != null)
                {

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
                        DataBaseGrid dataBaseGrid = new DataBaseGrid();
                        dataBaseGrid.Program = category.GetCategory();
                        dataBaseGrid.Detected = category.IsAvalible();

                        List<string> categories = new List<string>();

                        List<ICleanerWorker> workers = category.GetWorkers();
                        foreach (var worker in workers)
                        {

                            string categorys = worker.GetCategory();
                            if (!categories.Contains(categorys))
                                categories.Add(categorys);
                        }

                        dataBaseGrid.Category = string.Join(", ", categories);
                        dataBaseGrid.Index = index;
                        index++;
                        if (!keyValues.ContainsKey(category.GetCategory()))
                        {
                            list.Add(dataBaseGrid);
                            keyValues.Add(category.GetCategory(), dataBaseGrid);
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


                //string json = File.ReadAllText("C:\\Program Files\\WinBooster\\Settings\\Enabled.json");
                CleanerEnabledSettings? dataBase = CleanerEnabledSettings.FromJson(json);
                if (dataBase != null)
                {
                    enabledSettings = dataBase;
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
                        if (!find)
                        {
                            enabledSettings.keyValues.Remove(enabled);
                            byte[] bytes = Encoding.UTF8.GetBytes(enabledSettings.ToJson());
                            byte[] encrypted = cryptor.Encrypt(bytes);

                            Directory.CreateDirectory("C:\\Program Files\\WinBooster\\Settings");
                            File.Create("C:\\Program Files\\WinBooster\\Settings\\Enabled.json").Close();
                            //File.WriteAllText("C:\\Program Files\\WinBooster\\Settings\\Enabled.json", enabledSettings.ToJson());
                            File.WriteAllBytes("C:\\Program Files\\WinBooster\\Settings\\Enabled.json", encrypted);
                        }
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
                //File.WriteAllText("C:\\Program Files\\WinBooster\\Settings\\Enabled.json", enabledSettings.ToJson());
                File.WriteAllBytes("C:\\Program Files\\WinBooster\\Settings\\Enabled.json", encrypted);
            }
        }
        public void UpdateList2()
        {
            DataGrid.ItemsSource = list;
            double wi = 0;
            foreach (var colum in DataGrid.Columns)
            {
                wi += colum.Width.Value;
            }
            this.Width = wi;
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
                    //File.WriteAllText("C:\\Program Files\\WinBooster\\Settings\\Enabled.json", enabledSettings.ToJson());
                    File.WriteAllBytes("C:\\Program Files\\WinBooster\\Settings\\Enabled.json", encrypted);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Window_Activated(object sender, EventArgs e)
        {

        }

        public void SearchCmd(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateList();
            UpdateList2();
        }
    }
}
