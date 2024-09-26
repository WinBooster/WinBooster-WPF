using DiscordRPC;
using HandyControl.Controls;
using HandyControl.Data;
using Microsoft.Win32;
using System;
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
            DebugMode.IsChecked = App.auth.settings.DebugMode;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            SaveSettings();
            this.Hide();
        }

        private void SaveSettings()
        {
            App.auth.settings.password = PasswordBox.Password;
            App.auth.settings.DisableScreenCapture = ScreenShots.IsChecked;
            App.auth.settings.DebugMode = DebugMode.IsChecked;
            App.auth.settings.SaveFile(App.auth.settings.GetPath(), Settings.protection_password, Settings.protection_salt);
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
        }

        public static async Task<bool> UpdateCapture()
        {
            App.UpdateScreenCapture(App.auth.main, false);
            App.UpdateScreenCapture(App.auth.main);
            await Task.Delay(15);
            App.UpdateScreenCapture(App.auth.main.optimizeForm, false);
            App.UpdateScreenCapture(App.auth.main.optimizeForm);
            await Task.Delay(15);
            App.UpdateScreenCapture(App.auth.main.settingsForm, false);
            App.UpdateScreenCapture(App.auth.main.settingsForm);
            await Task.Delay(15);
            App.UpdateScreenCapture(App.auth.main.antiScreenForm, false);
            App.UpdateScreenCapture(App.auth.main.antiScreenForm);
            await Task.Delay(15);
            App.UpdateScreenCapture(App.auth.main.cleanerForm, false);
            App.UpdateScreenCapture(App.auth.main.cleanerForm);
            await Task.Delay(15);
            App.UpdateScreenCapture(App.auth.main.cleanerForm.clearListForm, false);
            App.UpdateScreenCapture(App.auth.main.cleanerForm.clearListForm);
            await Task.Delay(15);
            App.UpdateScreenCapture(App.auth.main.aboutForm, false);
            App.UpdateScreenCapture(App.auth.main.aboutForm);
            await Task.Delay(15);
            App.UpdateScreenCapture(App.auth.main.settingsForm, false);
            App.UpdateScreenCapture(App.auth.main.settingsForm);
            await Task.Delay(15);
            App.UpdateScreenCapture(App.auth.main.scriptsForm, false);
            App.UpdateScreenCapture(App.auth.main.scriptsForm);
            await Task.Delay(15);

            return true;
        }

        private async void ScreenShots_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.auth.settings.DisableScreenCapture = true;
            await UpdateCapture();
            SaveSettings();
        }

        private async void ScreenShots_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.auth.settings.DisableScreenCapture = false;
            await UpdateCapture();
            SaveSettings();
        }

        private async void Debug_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.auth.settings.DebugMode = true;
            await UpdateCapture();
            SaveSettings();
        }

        private async void Debug_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            App.auth.settings.DebugMode = false;
            await UpdateCapture();
            SaveSettings();
        }


        private async void Window_Activated(object sender, System.EventArgs e)
        {
            await SettingsForm.UpdateCapture();
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
        public static bool Convert(System.IO.Stream input_stream, System.IO.Stream output_stream, int size, bool keep_aspect_ratio = false)
        {
            System.Drawing.Bitmap input_bit = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromStream(input_stream);
            if (input_bit != null)
            {
                int width, height;
                if (keep_aspect_ratio)
                {
                    width = size;
                    height = input_bit.Height / input_bit.Width * size;
                }
                else
                {
                    width = height = size;
                }
                System.Drawing.Bitmap new_bit = new System.Drawing.Bitmap(input_bit, new System.Drawing.Size(width, height));
                if (new_bit != null)
                {
                    // save the resized png into a memory stream for future use
                    System.IO.MemoryStream mem_data = new System.IO.MemoryStream();
                    new_bit.Save(mem_data, System.Drawing.Imaging.ImageFormat.Png);

                    System.IO.BinaryWriter icon_writer = new System.IO.BinaryWriter(output_stream);
                    if (output_stream != null && icon_writer != null)
                    {
                        // 0-1 reserved, 0
                        icon_writer.Write((byte)0);
                        icon_writer.Write((byte)0);

                        // 2-3 image type, 1 = icon, 2 = cursor
                        icon_writer.Write((short)1);

                        // 4-5 number of images
                        icon_writer.Write((short)1);

                        // image entry 1
                        // 0 image width
                        icon_writer.Write((byte)width);
                        // 1 image height
                        icon_writer.Write((byte)height);

                        // 2 number of colors
                        icon_writer.Write((byte)0);

                        // 3 reserved
                        icon_writer.Write((byte)0);

                        // 4-5 color planes
                        icon_writer.Write((short)0);

                        // 6-7 bits per pixel
                        icon_writer.Write((short)32);

                        // 8-11 size of image data
                        icon_writer.Write((int)mem_data.Length);

                        // 12-15 offset of image data
                        icon_writer.Write((int)(6 + 16));

                        // write image data
                        // png data must contain the whole png data file
                        icon_writer.Write(mem_data.ToArray());

                        icon_writer.Flush();

                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        public static bool Convert(string input_image, string output_icon, int size, bool keep_aspect_ratio = false)
        {
            System.IO.FileStream input_stream = new System.IO.FileStream(input_image, System.IO.FileMode.Open);
            System.IO.FileStream output_stream = new System.IO.FileStream(output_icon, System.IO.FileMode.OpenOrCreate);

            bool result = Convert(input_stream, output_stream, size, keep_aspect_ratio);

            input_stream.Close();
            output_stream.Close();

            return result;
        }
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Select icon";
            openFileDialog.Filter = "Icons (*.ico;*.png)|*.ico;*.png";

            if (openFileDialog.ShowDialog() == true)
            {

                string file = openFileDialog.FileName;
                FileInfo fileInfo = new FileInfo(file);
                Debug.WriteLine(fileInfo.Extension);
                if (fileInfo.Extension == ".png")
                {
                    File.Create("temp.ico").Close();

                    FileInfo icon = new FileInfo("temp.ico");

                    Convert(fileInfo.FullName, icon.FullName, 16, false);

                    Icon image2 = new System.Drawing.Icon(icon.FullName, -1, -1);
                    if (image2.Size.Width == 16 && image2.Size.Height == 16)
                    {
                        IconInjector.Change(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, icon.FullName, 1);
                        App.SuperExit();
                    }
                    else
                    {
                        GrowlInfo growl = new GrowlInfo
                        {
                            Message = "Icon size not 16x16",
                            ShowDateTime = true,
                            IconKey = "WarningGeometry",
                            IconBrushKey = "WarningBrush",
                            IsCustom = true
                        };
                        Growl.InfoGlobal(growl);
                    }
                }
                else
                {
                    Icon image2 = new System.Drawing.Icon(fileInfo.FullName, -1, -1);
                    if (image2.Size.Width == 16 && image2.Size.Height == 16)
                    {
                        IconInjector.Change(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, file, 1);
                        App.SuperExit();
                    }
                    else
                    {
                        GrowlInfo growl = new GrowlInfo
                        {
                            Message = "Icon size not 16x16",
                            ShowDateTime = true,
                            IconKey = "WarningGeometry",
                            IconBrushKey = "WarningBrush",
                            IsCustom = true
                        };
                        Growl.InfoGlobal(growl);
                    }
                }
            }
        }
    }
}
