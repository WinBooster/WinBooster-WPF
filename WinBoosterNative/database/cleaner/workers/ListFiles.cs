namespace WinBoosterNative.database.cleaner.workers
{
    public class ListFiles : ICleanerWorker
    {
        public string category;
        public string mainDirectory;
        public List<string> files = new List<string>();
        public ListFiles(string directory, string category, List<string> files)
        {
            this.category = category;
            mainDirectory = directory;
            this.files = files;
        }

        public string GetCategory()
        {
            return category;
        }

        public string GetFolder()
        {
            return PlaceholderDataBaseParser.Parse(mainDirectory);
        }
        public List<string> GetFolders()
        {
            return PlaceholderDataBaseParser.ParseMultiforlder(mainDirectory);
        }
        public bool IsAvalible()
        {
            List<string> directoryDone = PlaceholderDataBaseParser.ParseMultiforlder(mainDirectory);
            foreach (string dir in directoryDone)
            {
                if (Directory.Exists(dir))
                {
                    foreach (string file in files)
                    {
                        string filePath = Path.Combine(dir, file);
                        if (File.Exists(filePath))
                        {
                            return true;
                        }
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
            List<string> directoryDone = PlaceholderDataBaseParser.ParseMultiforlder(mainDirectory);

            foreach (string dir in directoryDone)
            {
                if (Directory.Exists(dir))
                {
                    foreach (string file in files)
                    {
                        string filePath = Path.Combine(dir, file);
                        if (File.Exists(filePath))
                        {
                            FileInfo fileInfo = new FileInfo(filePath);
                            try { File.Delete(filePath); result.bytes += fileInfo.Length; result.files++; } catch { }
                        }
                    }
                }
            }
            return result;
        }
    }
}
