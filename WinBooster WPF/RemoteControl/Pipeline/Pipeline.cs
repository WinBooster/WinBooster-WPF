using H.Formatters;
using H.Pipes;
using WinBooster_WPF.RemoteControl.Pipeline.Messages;
using WinBoosterNative.pipeline.messages;

namespace WinBooster_WPF.RemoteControl.Pipeline
{
    public class Pipeline
    {
        public void Send(GeneralMessage obj)
        {
            var server = new PipeClient<GeneralMessage>("WinBooster_TiWorkerGeneral", formatter: new NewtonsoftJsonFormatter());
            server.ConnectAsync().Wait();
            server.WriteAsync(obj).Wait();
        }
    }
}
