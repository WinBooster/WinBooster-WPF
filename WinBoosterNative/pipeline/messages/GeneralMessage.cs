using WinBooster_WPF.RemoteControl.Pipeline.Messages;

namespace WinBoosterNative.pipeline.messages
{
    [Serializable]
    public class GeneralMessage
    {
        public DeleteFolderMessage? deleteFolder;
        public DeleteFileMessage? deleteFile;
        public MoveFileMessage? moveFile;
    }
}
