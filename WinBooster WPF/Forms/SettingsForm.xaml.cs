using DiscordRPC;
using HandyControl.Controls;
using HandyControl.Data;
using Microsoft.Win32;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
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
            e.Cancel = true;
            App.auth.settings.password = PasswordBox.Password;  
            App.auth.settings.discordRich = DiscordRich.IsChecked;
            App.auth.settings.DisableScreenCapture = ScreenShots.IsChecked;
            App.auth.settings.SaveFile(App.auth.settings.GetPath(), Settings.protection_password, Settings.protection_salt);
            this.Hide();
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

        private async void ScreenShots_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.auth.settings.DisableScreenCapture = true;
            var mainWindowHandle = new WindowInteropHelper(this).Handle;
            var ok = FormProtect.SetWindowDisplayAffinity(mainWindowHandle, 1);
            await Task.Delay(5);
            App.UpdateScreenCapture(App.auth.main);
            await Task.Delay(5);
            App.UpdateScreenCapture(App.auth.main.optimizeForm);
            await Task.Delay(5);
            App.UpdateScreenCapture(App.auth.main.settingsForm);
            await Task.Delay(5);
            App.UpdateScreenCapture(App.auth.main.antiScreen);
            await Task.Delay(5);
            App.UpdateScreenCapture(App.auth.main.cleanerForm);
            await Task.Delay(5);
            App.UpdateScreenCapture(App.auth.main.cleanerForm.clearListForm);
            await Task.Delay(5);
        }

        private async void ScreenShots_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.auth.settings.DisableScreenCapture = false;
            var mainWindowHandle = new WindowInteropHelper(this).Handle;
            var ok = FormProtect.SetWindowDisplayAffinity(mainWindowHandle, 1);
            await Task.Delay(5);
            App.UpdateScreenCapture(App.auth.main);
            await Task.Delay(5);
            App.UpdateScreenCapture(App.auth.main.optimizeForm);
            await Task.Delay(5);
            App.UpdateScreenCapture(App.auth.main.settingsForm);
            await Task.Delay(5);
            App.UpdateScreenCapture(App.auth.main.antiScreen);
            await Task.Delay(5);
            App.UpdateScreenCapture(App.auth.main.cleanerForm);
            await Task.Delay(5);
            App.UpdateScreenCapture(App.auth.main.cleanerForm.clearListForm);
            await Task.Delay(5);
        }

        private void Window_Activated(object sender, System.EventArgs e)
        {
            var mainWindowHandle = new WindowInteropHelper(this).Handle;
            if (App.auth.settings.DisableScreenCapture == true)
            {
                var ok = FormProtect.SetWindowDisplayAffinity(mainWindowHandle, 1);
            }
            else
            {
                var ok = FormProtect.SetWindowDisplayAffinity(mainWindowHandle, 0);
            }
        }
        public static Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Select icon";
            openFileDialog.Filter = "Icons (*.ico)|*.ico";

            if (openFileDialog.ShowDialog() == true)
            {

                string file = openFileDialog.FileName;
                FileInfo fileInfo = new FileInfo(file);
                Debug.WriteLine(fileInfo.Extension);
                if (fileInfo.Extension == ".png")
                {
                    File.Create("temp.ico").Close();

                    FileInfo icon = new FileInfo("temp.ico");

                    using (FileStream stream = File.OpenWrite("temp.ico"))
                    {
                        Bitmap bitmap = (Bitmap)System.Drawing.Image.FromFile(file);
                        Bitmap done = ResizeImage(bitmap, 256, 256);
                        System.Drawing.Icon.FromHandle(done.GetHicon()).Save(stream);
                    }
                    IconInjector.Change(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, icon.FullName, icon.FullName, 1);
                    App.SuperExit();
                }
                else
                {
                    IconInjector.Change(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, file, 1);
                    App.SuperExit();
                }
            }
        }
    }
}
