namespace WinBoosterNative.database.cleaner
{
    public interface ICleanerCategory
    {
        bool IsAvalible();
        string GetCategory();
        List<ICleanerWorker> GetWorkers();
    }
}
