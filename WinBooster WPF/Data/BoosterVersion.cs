using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WinBooster_WPF.Data
{
    public class BoosterVersion
    {
        public string version;
        public string download;
        public string description;

        public string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.SerializeObject(this, settings);
        }

        public static BoosterVersion? FromJson(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.DeserializeObject<BoosterVersion>(json, settings);
        }
    }
}
