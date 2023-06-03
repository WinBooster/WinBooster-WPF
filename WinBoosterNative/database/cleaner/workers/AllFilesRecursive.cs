namespace WinBoosterNative.database.cleaner.workers
{
    public class AllFilesRecursive : ICleanerWorker
    {
        public string category;
        public string mainDirectory;
        public bool removeDirectory = false;
        public AllFilesRecursive(string directory, string category, bool removeDirectory = false)
        {
            this.category = category;
            mainDirectory = directory;
            this.removeDirectory = removeDirectory;
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
            return true;
        }

        public CleanerResult TryDelete()
        {
            return TryDelete("");
        }
        private CleanerResult TryDelete(string main = "")
        {
            CleanerResult result;
            result.bytes = 0;
            result.files = 0;
            if (string.IsNullOrEmpty(main))
            {
                main = mainDirectory;
            }
            string directoryDone = PlaceholderDataBaseParser.Parse(main);
            if (Directory.Exists(directoryDone))
            {
                foreach (string file in Directory.GetFiles(directoryDone))
                {
                    string filePath = Path.Combine(directoryDone, file);
                    if (File.Exists(filePath))
                    {
                        FileInfo fileInfo = new FileInfo(filePath);
                        long lenght = fileInfo.Length;
                        try { File.Delete(filePath); result.bytes += lenght; result.files++; } catch { }
                    }
                }

                foreach (string subdirectory in Directory.GetDirectories(directoryDone))
                {
                    CleanerResult result2 = TryDelete(subdirectory);
                    result.bytes += result2.bytes;
                    result.files += result2.files;
                    try
                    {
                        Directory.Delete(subdirectory);
                    }
                    catch { }
                }
                if (removeDirectory)
                {
                    Directory.Delete(directoryDone);
                }
            }
            return result;
        }
    }
}
