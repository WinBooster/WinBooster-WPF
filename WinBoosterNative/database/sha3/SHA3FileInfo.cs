using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using WinBoosterNative.database.cleaner;

namespace WinBoosterNative.database.sha3
{
    public class SHA3FileInfo
    {
        public string? name;
        public string? version;
        public string? author;
        public string? game;
        public string? category;
        public string? extension;
        public string? decription;
        public SHA3FileInfo(string? name, string? version, string? author, string? category, string? game, string? extension, string? decription)
        {
            this.name = name;
            this.version = version;
            this.author = author;
            this.category = category;
            this.game = game;
            this.extension = extension;
            this.decription = decription;
        }

        public override string ToString()
        {
            return $"Name: {name}, Version: {version}, Author: {author}, Category: {category}";
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
