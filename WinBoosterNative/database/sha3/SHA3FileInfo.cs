using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using WinBoosterNative.database.cleaner;

namespace WinBoosterNative.database.sha3
{
    public class SHA3FileInfo
    {
        public string name;
        public string game;
        public Category category;

        public SHA3FileInfo(string name, Category category)
        {
            this.name = name;
            this.category = category;
        }

        public enum Category
        {
            Cheat,
        }

        public string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.SerializeObject(this, settings);
        }

        public static SHA3FileInfo? FromJson(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.DeserializeObject<SHA3FileInfo>(json, settings);
        }
    }
}
