using DiscordRPC;
using HandyControl.Controls;
using HandyControl.Data;
using System.Windows.Controls;
using System.Windows.Interop;
using WinBoosterNative.data;
using WinBoosterNative.winapi;

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
            App.auth.settings.DisableScreenCapture = ScreenShots.IsChecked;
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

            if (App.auth.settings.DisableScreenCapture == true)
            {
                ScreenShots.IsChecked = true;
            }
            else
            {
                ScreenShots.IsChecked = false;
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

        private void ScreenShots_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.auth.settings.DisableScreenCapture = true;
            var mainWindowHandle = new WindowInteropHelper(this).Handle;
            var ok = FormProtect.SetWindowDisplayAffinity(mainWindowHandle, 1);
            App.UpdateScreenCapture(App.auth.main);
            App.UpdateScreenCapture(App.auth.main.antiScreen);
            App.UpdateScreenCapture(App.auth.main.optimizeForm);
            App.UpdateScreenCapture(App.auth.main.settingsForm);
            App.UpdateScreenCapture(App.auth.main.cleanerForm);
            App.UpdateScreenCapture(App.auth.main.cleanerForm.clearListForm);
        }

        private void ScreenShots_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.auth.settings.DisableScreenCapture = false;
            var mainWindowHandle = new WindowInteropHelper(this).Handle;
            var ok = FormProtect.SetWindowDisplayAffinity(mainWindowHandle, 0);
            App.UpdateScreenCapture(App.auth.main);
            App.UpdateScreenCapture(App.auth.main.antiScreen);
            App.UpdateScreenCapture(App.auth.main.optimizeForm);
            App.UpdateScreenCapture(App.auth.main.settingsForm);
            App.UpdateScreenCapture(App.auth.main.cleanerForm);
            App.UpdateScreenCapture(App.auth.main.cleanerForm.clearListForm);
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            //var mainWindowHandle = new WindowInteropHelper(this).Handle;
            //if (App.auth.settings.DisableScreenCapture == true)
            //{
            //    var ok = FormProtect.SetWindowDisplayAffinity(mainWindowHandle, 1);
            //}
            //else
            //{
            //    var ok = FormProtect.SetWindowDisplayAffinity(mainWindowHandle, 0);
            //}
        }
    }
}
