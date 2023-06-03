using WinBoosterNative.data;

namespace WinBoosterNative.database.cleaner.workers
{
    public class PaternFilesRecursive : ICleanerWorker
    {
        public string category;
        public string mainDirectory;
        public string patern;
        private bool deleteFolders = true;

        public PaternFilesRecursive(string directory, string patern, string category, bool deleteFolders = true)
        {
            this.category = category;
            mainDirectory = directory;
            this.patern = patern;
            this.deleteFolders = deleteFolders;
        }
        public string GetCategory()
        {
            return category;
        }
        public CleanerResult TryDelete()
        {
            return TryDelete("");
        }
        public string GetFolder()
        {
            return PlaceholderDataBaseParser.Parse(mainDirectory);
        }
        public List<string> GetFolders()
        {
            return PlaceholderDataBaseParser.ParseMultiforlder(mainDirectory);
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
                foreach (string file in Directory.GetFiles(directoryDone, patern))
                {
                    if (File.Exists(file))
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        long lenght = fileInfo.Length;
                        try { File.Delete(file); result.bytes += lenght; result.files++; } catch { }
                    }
                }
                foreach (string subdirectory in Directory.GetDirectories(directoryDone))
                {
                    var result2 = TryDelete(subdirectory);
                    result.bytes += result2.bytes;
                    result.files += result2.files;
                    if (deleteFolders)
                    {
                        try
                        {
                            Directory.Delete(subdirectory);
                        }
                        catch { }
                    }
                }
            }
            return result;
        }
        private bool IsAvalible(string main = "")
        {
            if (string.IsNullOrEmpty(main))
            {
                main = mainDirectory;
            }
            string directoryDone = PlaceholderDataBaseParser.Parse(main);
            if (Directory.Exists(directoryDone))
            {
                if (Directory.GetFiles(directoryDone, patern).Length > 0)
                {
                    return true;
                }
                foreach (string subdirectory in Directory.GetDirectories(directoryDone))
                {
                    return IsAvalible(subdirectory);
                }
            }
            return false;
        }


        public bool IsAvalible()
        {
            return IsAvalible("");
        }
    }
}
