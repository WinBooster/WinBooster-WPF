using System.IO;

namespace WinBooster_WPF.RemoteControl.Pipeline.Messages
{
    [Serializable]
    public class MoveFileMessage
    {
        public string from;
        public string to;
    }
}
