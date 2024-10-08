﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Org.BouncyCastle.Crypto.Digests;
using System.Net;

namespace WinBoosterNative.database.sha3
{
    public class SHA3DataBase
    {
        public SHA3FileInfo? TryGetFileInfo(string hash)
        {
            try
            {
                using (WebClient wc = new WebClient())
                {
                    string json = wc.DownloadString(string.Format("https://github.com/WinBooster/WinBooster_hash_list/raw/refs/heads/main/list/{0}.json", hash));
                    SHA3FileInfo? info = SHA3FileInfo.FromJson(json);
                    return info;
                }
            }
            catch { }
            return null;
        }
        public static byte[] GetHash(byte[] bytes)
        {
            Sha3Digest sha3 = new Sha3Digest(256); // 256-bit hash
            byte[] hashBytes = new byte[sha3.GetDigestSize()];
            sha3.BlockUpdate(bytes, 0, bytes.Length);
            sha3.DoFinal(hashBytes, 0);
            return hashBytes;
        }
        public static string GetHashString(byte[] hashBytes)
        {
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hashString;
        }
        public string ToJson()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.SerializeObject(this, settings);
        }

        public static SHA3DataBase? FromJson(string json)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new JavaScriptDateTimeConverter());
            settings.Converters.Add(new StringEnumConverter());
            settings.Formatting = Formatting.Indented;
            return JsonConvert.DeserializeObject<SHA3DataBase>(json, settings);
        }
    }
}
