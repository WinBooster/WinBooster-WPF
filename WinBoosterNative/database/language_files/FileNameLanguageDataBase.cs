using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
