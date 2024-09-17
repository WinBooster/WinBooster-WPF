namespace WinBoosterNative.database.cleaner
{
    public interface ICleanerWorker
    {
        CleanerResult TryDelete();
        string GetCategory();
        string GetFolder();
        List<string> GetFolders();
        bool IsAvalible();
    }
}
