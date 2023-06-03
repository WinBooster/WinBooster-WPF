using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WinBoosterNative.database.cleaner.workers;
using WinBoosterNative.database.cleaner.workers.language;

namespace WinBoosterNative.database.cleaner
{
    public class CleanerDataBase
    {
        public long Count
        {
            get
            {
                return CleanerDatabaseUtil.GetWorker(this).Count;
            }
        }

        //[JsonIgnore]
        public List<CleanerCategory> cleaners = new List<CleanerCategory>();

        public List<FilesIfCurrentLanguageByPatern> filesIsNotLanguageByPatern = new List<FilesIfCurrentLanguageByPatern>();
        public List<ListFilesIfCurrentLanguage> listFilesIsNotLanguage = new List<ListFilesIfCurrentLanguage>();
        public List<ListFiles> listFiles = new List<ListFiles>();
        public List<ListFolders> listFolders = new List<ListFolders>();
        public List<PaternFiles> paternFiles = new List<PaternFiles>();
        public List<AllFilesRecursive> allFilesRecursives = new List<AllFilesRecursive>();
        public List<PaternFilesRecursive> paternFilesRecursives = new List<PaternFilesRecursive>();

        public string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.SerializeObject(this, settings);
        }

        public static CleanerDataBase? FromJson(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.DeserializeObject<CleanerDataBase>(json, settings);
        }
    }
}
