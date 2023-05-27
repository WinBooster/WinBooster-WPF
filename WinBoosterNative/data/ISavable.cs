using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinBoosterNative.security;

namespace WinBoosterNative.data
{
    public class ISavable<T> where T : new()
    {
        public const string protection_password = "nekiplay";
        public const string protection_salt = "winbooster";
        public virtual string GetPath()
        {
            return "";
        }
        public string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.SerializeObject(this, settings);
        }

        public static T? FromJson(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        public static T? FromFile(string file, string password, string salt)
        {
            if (File.Exists(file))
            {
                AESCryptor cryptor = new AESCryptor();
                cryptor.SetPassword(password, salt);

                byte[] encrypted = File.ReadAllBytes(file);
                byte[] decrypted = cryptor.Decrypt(encrypted);
                string json = Encoding.UTF8.GetString(decrypted);
                return FromJson(json);
            }
            return default(T);
        }
        public void SaveFile(string file, string password, string salt)
        {
            AESCryptor cryptor = new AESCryptor();
            cryptor.SetPassword(password, salt);
            if (!File.Exists(file))
                File.Create(file).Close();
            byte[] encrypted = cryptor.Encrypt(Encoding.UTF8.GetBytes(ToJson()));
            File.WriteAllBytes(file, encrypted);
        }
    }
}
