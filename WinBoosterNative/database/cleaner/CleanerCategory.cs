using WinBoosterNative.database.cleaner.workers.language;
using WinBoosterNative.database.cleaner.workers;
using Newtonsoft.Json;

namespace WinBoosterNative.database.cleaner
{
    public class CleanerCategory : ICleanerCategory
    {
        [JsonPropertyAttribute("category")]
        public string _category;

        public List<FilesIfCurrentLanguageByPatern> filesIsNotLanguageByPatern = new List<FilesIfCurrentLanguageByPatern>();
        public List<ListFilesIfCurrentLanguage> listFilesIsNotLanguage = new List<ListFilesIfCurrentLanguage>();
        public List<ListFiles> listFiles = new List<ListFiles>();
        public List<ListFolders> listFolders = new List<ListFolders>();
        public List<PaternFiles> paternFiles = new List<PaternFiles>();
        public List<AllFilesRecursive> allFilesRecursives = new List<AllFilesRecursive>();
        public List<PaternFilesRecursive> paternFilesRecursives = new List<PaternFilesRecursive>();
        public CleanerCategory(string category)
        {
            _category = category;
        }

        public string GetCategory()
        {
            return _category;
        }

        public List<ICleanerWorker> GetWorkers()
        {
            return CleanerDatabaseUtil.GetWorker(this);
        }

        public bool IsAvalible()
        {
            bool avalible = false;
            List<ICleanerWorker> workers = GetWorkers();
            foreach (ICleanerWorker worker in workers)
            {
                string folder = worker.GetFolder();
                if (Directory.Exists(folder) && worker.IsAvalible())
                {
                    avalible = true;
                    break;
                }
            }
            return avalible;
        }
    }
}
