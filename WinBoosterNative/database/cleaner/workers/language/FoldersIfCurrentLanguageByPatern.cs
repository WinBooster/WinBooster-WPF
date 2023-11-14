namespace WinBoosterNative.database.cleaner.workers.language
{
    public class FoldersIfCurrentLanguageByPatern : ILanguageWorker, ICleanerWorker
    {
        public string category;
        public string mainDirectory;
        public string patern;
        public bool ignoreUnknow;
        public FoldersIfCurrentLanguageByPatern(string directory, string patern, bool ignoreUnknowLanguage, string category)
        {
            this.ignoreUnknow = ignoreUnknowLanguage;
            this.category = category;
            this.mainDirectory = directory;
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
        public List<string> GetFolders()
        {
            return PlaceholderDataBaseParser.ParseMultiforlder(mainDirectory);
        }
        public bool IsAvalible()
        {
            string directoryDone = PlaceholderDataBaseParser.Parse(mainDirectory);
            if (Directory.Exists(directoryDone))
            {
                foreach (string file in Directory.GetDirectories(directoryDone, patern))
                {
                    DirectoryInfo fileInfo = new DirectoryInfo(file);
                    Language fileLanguage = FileNameToLanguage(fileInfo.Name);
                    if (fileLanguage != GetWindowsLanguage())
                    {
                        if (ignoreUnknow)
                        {
                            return true;
                        }
                        else if (fileLanguage != Language.Unknow)
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

            string directoryDone = PlaceholderDataBaseParser.Parse(mainDirectory);
            if (Directory.Exists(directoryDone))
            {
                foreach (string file in Directory.GetDirectories(directoryDone, patern))
                {
                    DirectoryInfo fileInfo = new DirectoryInfo(file);
                    Language fileLanguage = FileNameToLanguage(fileInfo.Name);
                    if (fileLanguage != GetWindowsLanguage())
                    {
                        long size = ListFolders.DirSize(fileInfo);
                        if (ignoreUnknow)
                        {
                            try { fileInfo.Delete(true); result.bytes += size; result.files++; } catch { }
                        }
                        else if (fileLanguage != Language.Unknow)
                        {
                            try { fileInfo.Delete(true); result.bytes += size; result.files++; } catch { }
                        }
                    }
                }
            }
            return result;
        }
    }
}
