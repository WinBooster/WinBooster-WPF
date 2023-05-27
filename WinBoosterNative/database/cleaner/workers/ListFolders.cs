namespace WinBoosterNative.database.cleaner.workers
{
    public class ListFolders : ICleanerWorker
    {
        public string category;
        public string mainDirectory;
        public List<string> folders = new List<string>();

        public ListFolders(string directory, string category, List<string> folders)
        {
            this.category = category;
            mainDirectory = directory;
            this.folders = folders;
        }

        public string GetCategory()
        {
            return category;
        }
        public string GetFolder()
        {
            return PlaceholderDataBaseParser.Parse(mainDirectory);
        }

        public CleanerResult TryDelete()
        {
            CleanerResult result;
            result.bytes = 0;
            result.files = 0;
            string directoryDone = PlaceholderDataBaseParser.Parse(mainDirectory);
            if (Directory.Exists(directoryDone))
            {
                foreach (string file in folders)
                {
                    string filePath = Path.Combine(directoryDone, file);
                    if (Directory.Exists(filePath))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(filePath);
                        long size = DirSize(dirInfo);
                        try { Directory.Delete(filePath, true); result.bytes += size; result.files++; } catch { }
                    }
                }
            }
            return result;
        }

        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        public bool IsAvalible()
        {
            string directoryDone = PlaceholderDataBaseParser.Parse(mainDirectory);
            if (Directory.Exists(directoryDone))
            {
                foreach (string file in folders)
                {
                    string filePath = Path.Combine(directoryDone, file);
                    if (Directory.Exists(filePath))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
