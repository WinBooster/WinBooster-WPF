namespace WinBoosterNative.database.cleaner
{
    public struct CleanerResult
    {
        public long bytes;
        public int files;

        public CleanerResult()
        {
            bytes = 0;
            files = 0;
        }
    }
}
