using DiscordRPC;
using HandyControl.Controls;
using HandyControl.Data;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using WinBoosterNative.data;

namespace WinBooster_WPF
{
    /// <summary>
    /// Логика взаимодействия для Cleaner.xaml
    /// </summary>
    public partial class SettingsForm : Window
    {
        public SettingsForm()
        {
            InitializeComponent();
            PasswordBox.Password = App.auth.settings.password;
            DiscordRich.IsChecked = App.auth.settings.discordRich;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            App.auth.settings.password = PasswordBox.Password;  
            App.auth.settings.discordRich = DiscordRich.IsChecked;
            App.auth.settings.SaveFile(App.auth.settings.GetPath(), Settings.protection_password, Settings.protection_salt);
            this.Hide();
            e.Cancel = true;
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (App.auth.settings.discordRich == true)
            {
                DiscordRich.IsChecked = true;
            }
            else
            {
                DiscordRich.IsChecked = false;
            }
        }

        private void DiscordRich_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                RichPresence rich = new RichPresence()
                {
                    Buttons = new DiscordRPC.Button[]
                    {
                    new DiscordRPC.Button() { Label = "Download", Url = App.auth.main.version?.download }
                    },
                    Assets = new Assets()
                    {
                        LargeImageKey = "speed",
                    }
                };
                App.client.SetPresence(rich);
            }
            catch { }
        }

        private void DiscordRich_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.client.SetPresence(null);
            App.client.ClearPresence();
        }
    }
}
