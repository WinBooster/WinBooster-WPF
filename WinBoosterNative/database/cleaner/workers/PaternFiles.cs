namespace WinBoosterNative.database.cleaner.workers
{
    public class PaternFiles : ICleanerWorker
    {
        public string category;
        public string mainDirectory;
        public string patern;

        public PaternFiles(string directory, string patern, string category)
        {
            this.category = category;
            mainDirectory = directory;
            this.patern = patern;
        }

        public string GetCategory()
        {
            return category;
        }
        public string GetFolder()
        {
            return PlaceholderDataBaseParser.Parse(mainDirectory);
        }

        public bool IsAvalible()
        {
            CleanerResult result;
            result.bytes = 0;
            result.files = 0;
            string directoryDone = PlaceholderDataBaseParser.Parse(mainDirectory);
            if (Directory.Exists(directoryDone))
            {
                foreach (string file in Directory.GetFiles(directoryDone, patern))
                {
                    if (File.Exists(file))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public CleanerResult TryDelete()
        {
            CleanerResult result;
            result.bytes = 0;
            result.files = 0;
            string directoryDone = PlaceholderDataBaseParser.Parse(mainDirectory);
            if (Directory.Exists(directoryDone))
            {
                foreach (string file in Directory.GetFiles(directoryDone, patern))
                {
                    if (File.Exists(file))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        try { File.Delete(file); result.bytes += fileInfo.Length; result.files++; } catch { }
                    }
                }
            }
            return result;
        }
    }
}
