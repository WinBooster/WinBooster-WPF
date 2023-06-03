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
        public List<string> GetFolders()
        {
            return PlaceholderDataBaseParser.ParseMultiforlder(mainDirectory);
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
                    foreach (string file in folders)
                    {
                        string filePath = Path.Combine(dir, file);
                        if (Directory.Exists(filePath))
                        {
                            DirectoryInfo dirInfo = new DirectoryInfo(filePath);
                            long size = DirSize(dirInfo);
                            try { Directory.Delete(filePath, true); result.bytes += size; result.files++; } catch { }

                        }
                    }
                }
            }
            return result;
        }

        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        public bool IsAvalible()
        {
            List<string> directoryDone = PlaceholderDataBaseParser.ParseMultiforlder(mainDirectory);
            foreach (string dir in directoryDone)
            {
                if (Directory.Exists(dir))
                {
                    foreach (string file in folders)
                    {
                        string filePath = Path.Combine(dir, file);
                        if (Directory.Exists(filePath))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
