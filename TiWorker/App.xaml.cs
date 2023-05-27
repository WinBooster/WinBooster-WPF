﻿using H.Formatters;
using H.Pipes;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using WinBooster_WPF.RemoteControl.Pipeline.Messages;
using WinBoosterNative.pipeline.messages;

namespace TiWorker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            new WinBoosterChecker();

            var server = new PipeServer<GeneralMessage>("WinBooster_TiWorkerGeneral", formatter: new NewtonsoftJsonFormatter());
            server.MessageReceived += (a, b) =>
            {
                GeneralMessage? message = b.Message;
                if (message != null)
                {
                    DeleteFileMessage? deleteFile = message.deleteFile;
                    MoveFileMessage? moveFile = message.moveFile;
                    if (deleteFile != null)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            try { File.Delete(deleteFile.to); } catch { }
                        });
                    }
                    else if (moveFile != null)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            try { File.Move(moveFile.from, moveFile.to); } catch { }
                        });
                    }
                }
            };
            server.StartAsync().Wait();
        }
    }
}
