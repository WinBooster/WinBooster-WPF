using WinBooster_WPF.RemoteControl.Pipeline.Messages;

namespace WinBoosterNative.pipeline.messages
{
    [Serializable]
    public class GeneralMessage
    {
        public DeleteFileMessage? deleteFile;
        public MoveFileMessage? moveFile;
    }
}
