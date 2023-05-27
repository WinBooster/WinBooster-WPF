using H.Formatters;
using H.Pipes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiWorker
{
    public class WinBoosterChecker
    {
        public WinBoosterChecker()
        {
            try
            {
                var tiWorkerServer = new PipeClient<string>("WinBoosterTiCkecker", formatter: new NewtonsoftJsonFormatter());
                tiWorkerServer.ExceptionOccurred += TiWorkerServer_ExceptionOccurred;
                tiWorkerServer.Disconnected += TiWorkerServer_Disconnected;
                tiWorkerServer.Connected += TiWorkerServer_Connected;
                tiWorkerServer.ConnectAsync().Wait(1000);
                if (!tiWorkerServer.IsConnected)
                {
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        private void TiWorkerServer_Connected(object? sender, H.Pipes.Args.ConnectionEventArgs<string> e)
        {
            Debug.WriteLine("Connected");
        }

        private void TiWorkerServer_Disconnected(object? sender, H.Pipes.Args.ConnectionEventArgs<string> e)
        {
            Debug.WriteLine("Disconnected");
            Process.GetCurrentProcess().Kill();
        }

        private void TiWorkerServer_ExceptionOccurred(object? sender, H.Pipes.Args.ExceptionEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
