using H.Formatters;
using H.Pipes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WinBooster_WPF.RemoteControl.Pipeline.Messages;
using WinBoosterNative.pipeline.messages;

namespace SystemWorker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            new WinBoosterChecker();

            var server = new PipeServer<GeneralMessage>("WinBooster_SysWorkerGeneral", formatter: new NewtonsoftJsonFormatter());
            server.MessageReceived += (a, b) =>
            {
                GeneralMessage? message = b.Message;
                if (message != null)
                {
                    DeleteFolderMessage? deleteFolder = message.deleteFolder;
                    DeleteFileMessage? deleteFile = message.deleteFile;
                    MoveFileMessage? moveFile = message.moveFile;
                    if (deleteFile != null)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            try { File.Delete(deleteFile.to); } catch { }
                        });
                    }
                    if (moveFile != null)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            try { File.Move(moveFile.from, moveFile.to); } catch { }
                        });
                    }
                    if (deleteFolder != null)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            try { Directory.Delete(deleteFolder.to, true); } catch { }
                        });
                    }
                }
            };
            server.StartAsync().Wait();
        }
    }
}
