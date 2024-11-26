using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WinBoosterNative.database.cleaner.workers.language;

namespace WinBoosterNative.database.sha3
{
    public class FileNameLanguageDataBase
    {
        public Dictionary<string, ILanguageWorker.Language> database = new Dictionary<string, ILanguageWorker.Language>();
        public string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.SerializeObject(this, settings);
        }

        public static FileNameLanguageDataBase? FromJson(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.DeserializeObject<FileNameLanguageDataBase>(json, settings);
        }
    }
}
