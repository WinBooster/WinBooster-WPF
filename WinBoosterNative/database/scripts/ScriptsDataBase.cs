using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WinBoosterNative.database.scripts
{
    public class ScriptsDataBase
    {
        public List<ScriptInfo> scripts = new List<ScriptInfo>();

        public string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.SerializeObject(this, settings);
        }

        public static ScriptsDataBase? FromJson(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.DeserializeObject<ScriptsDataBase>(json, settings);
        }
    }
}
