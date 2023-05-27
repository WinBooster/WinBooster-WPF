using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WinBooster_WPF.RemoteControl.Pipeline;
using WinBooster_WPF.RemoteControl.Pipeline.Messages;
using WinBoosterNative.pipeline.messages;
using WinBoosterNative.security;
using WinBoosterNative.winapi;

namespace WinBooster_WPF.Forms
{
    /// <summary>
    /// Логика взаимодействия для AntiScreenShareForm.xaml
    /// </summary>
    public partial class AntiScreenShareForm : HandyControl.Controls.Window
    {
        public AntiScreenShareForm()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void ScrenKeyboard_Checked(object sender, RoutedEventArgs e)
        {
            string crypted2 = "4.exe";
            Pipeline pipeline = new Pipeline();
            if (!Directory.Exists("C:\\Program Files\\WinBooster\\SystemFiles"))
            {
                Directory.CreateDirectory("C:\\Program Files\\WinBooster\\SystemFiles");
            }
            GeneralMessage message = new GeneralMessage();
            MoveFileMessage moveFileMessage = new MoveFileMessage();
            moveFileMessage.from = @"C:\Windows\System32\osk.exe";
            moveFileMessage.to = @"C:\Program Files\WinBooster\SystemFiles\" + crypted2;
            message.moveFile = moveFileMessage;
            pipeline.Send(message);
        }

        private void ScrenKeyboard_Unchecked(object sender, RoutedEventArgs e)
        {
            string crypted2 = "4.exe";
            if (Directory.Exists("C:\\Program Files\\WinBooster\\SystemFiles"))
            {
                if (File.Exists(@"C:\Program Files\WinBooster\SystemFiles\" + crypted2))
                {
                    Pipeline pipeline = new Pipeline();
                    GeneralMessage message = new GeneralMessage();
                    MoveFileMessage moveFileMessage = new MoveFileMessage();
                    moveFileMessage.to = @"C:\Windows\System32\osk.exe";
                    moveFileMessage.from = @"C:\Program Files\WinBooster\SystemFiles\" + crypted2;
                    message.moveFile = moveFileMessage;
                    pipeline.Send(message);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(@"C:\Program Files\WinBooster\SystemFiles\5.exe"))
            {
                PowerShell.IsChecked = true;
            }
            if (File.Exists(@"C:\Program Files\WinBooster\SystemFiles\4.exe"))
            {
                ScrenKeyboard.IsChecked = true;
            }
            if (File.Exists(@"C:\Program Files\WinBooster\SystemFiles\2.exe"))
            {
                ResourceMonitor.IsChecked = true;
            }
            if (File.Exists(@"C:\Program Files\WinBooster\SystemFiles\1.exe"))
            {
                CMD.IsChecked = true;
            }
        }

        private void ResourceMonitor_Checked(object sender, RoutedEventArgs e)
        {
            string crypted = "2.exe";
            string crypted2 = "3.exe";
            Pipeline pipeline = new Pipeline();
            if (!Directory.Exists("C:\\Program Files\\WinBooster\\SystemFiles"))
            {
                Directory.CreateDirectory("C:\\Program Files\\WinBooster\\SystemFiles");
            }
            GeneralMessage message = new GeneralMessage();
            MoveFileMessage moveFileMessage = new MoveFileMessage();
            moveFileMessage.from = @"C:\Windows\System32\resmon.exe";
            moveFileMessage.to = @"C:\Program Files\WinBooster\SystemFiles\" + crypted;
            message.moveFile = moveFileMessage;
            pipeline.Send(message);

            GeneralMessage message2 = new GeneralMessage();
            MoveFileMessage moveFileMessage2 = new MoveFileMessage();
            moveFileMessage2.from = @"C:\Windows\System32\perfmon.exe";
            moveFileMessage2.to = @"C:\Program Files\WinBooster\SystemFiles\" + crypted2;
            message2.moveFile = moveFileMessage2;
            pipeline.Send(message2);
        }

        private void ResourceMonitor_Unchecked(object sender, RoutedEventArgs e)
        {
            string crypted = "2.exe";
            string crypted2 = "3.exe";
            if (Directory.Exists("C:\\Program Files\\WinBooster\\SystemFiles"))
            {
                if (File.Exists(@"C:\Program Files\WinBooster\SystemFiles\" + crypted))
                {
                    Pipeline pipeline = new Pipeline();
                    GeneralMessage message = new GeneralMessage();
                    MoveFileMessage moveFileMessage = new MoveFileMessage();
                    moveFileMessage.from = @"C:\Program Files\WinBooster\SystemFiles\" + crypted;
                    moveFileMessage.to = @"C:\Windows\System32\resmon.exe";
                    message.moveFile = moveFileMessage;
                    pipeline.Send(message);

                    GeneralMessage message2 = new GeneralMessage();
                    MoveFileMessage moveFileMessage2 = new MoveFileMessage();
                    moveFileMessage2.from = @"C:\Program Files\WinBooster\SystemFiles\" + crypted2;
                    moveFileMessage2.to = @"C:\Windows\System32\perfmon.exe";
                    message2.moveFile = moveFileMessage2;
                    pipeline.Send(message2);
                }
            }
        }

        private void CMD_Checked(object sender, RoutedEventArgs e)
        {
            string crypted = "1.exe";
            Pipeline pipeline = new Pipeline();
            if (!Directory.Exists("C:\\Program Files\\WinBooster\\SystemFiles"))
            {
                Directory.CreateDirectory("C:\\Program Files\\WinBooster\\SystemFiles");
            }
            GeneralMessage message = new GeneralMessage();
            MoveFileMessage moveFileMessage = new MoveFileMessage();
            moveFileMessage.from = @"C:\Windows\System32\cmd.exe";
            moveFileMessage.to = @"C:\Program Files\WinBooster\SystemFiles\" + crypted;
            message.moveFile = moveFileMessage;
            pipeline.Send(message);
        }

        private void CMD_Unchecked(object sender, RoutedEventArgs e)
        {
            string crypted = "1.exe";
            if (Directory.Exists("C:\\Program Files\\WinBooster\\SystemFiles"))
            {
                if (File.Exists(@"C:\Program Files\WinBooster\SystemFiles\" + crypted))
                {
                    Pipeline pipeline = new Pipeline();
                    GeneralMessage message = new GeneralMessage();
                    MoveFileMessage moveFileMessage = new MoveFileMessage();
                    moveFileMessage.from = @"C:\Program Files\WinBooster\SystemFiles\" + crypted;
                    moveFileMessage.to = @"C:\Windows\System32\cmd.exe";
                    message.moveFile = moveFileMessage;
                    pipeline.Send(message);
                }
            }
        }

        private void PowerShell_Checked(object sender, RoutedEventArgs e)
        {
            string crypted = "5.exe";
            Pipeline pipeline = new Pipeline();
            if (!Directory.Exists("C:\\Program Files\\WinBooster\\SystemFiles"))
            {
                Directory.CreateDirectory("C:\\Program Files\\WinBooster\\SystemFiles");
            }
            GeneralMessage message = new GeneralMessage();
            MoveFileMessage moveFileMessage = new MoveFileMessage();
            moveFileMessage.from = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
            moveFileMessage.to = @"C:\Program Files\WinBooster\SystemFiles\" + crypted;
            message.moveFile = moveFileMessage;
            pipeline.Send(message);
        }

        private void PowerShell_Unchecked(object sender, RoutedEventArgs e)
        {
            string crypted = "5.exe";
            if (Directory.Exists("C:\\Program Files\\WinBooster\\SystemFiles"))
            {
                if (File.Exists(@"C:\Program Files\WinBooster\SystemFiles\" + crypted))
                {
                    Pipeline pipeline = new Pipeline();
                    GeneralMessage message = new GeneralMessage();
                    MoveFileMessage moveFileMessage = new MoveFileMessage();
                    moveFileMessage.from = @"C:\Program Files\WinBooster\SystemFiles\" + crypted;
                    moveFileMessage.to = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
                    message.moveFile = moveFileMessage;
                    pipeline.Send(message);
                }
            }
        }

        private void Window_Activated(object sender, EventArgs e)
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
    }
}
