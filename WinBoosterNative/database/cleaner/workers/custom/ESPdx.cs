using System;
using System.Diagnostics;
using System.IO;
using WinBoosterNative.database.sha3;

namespace WinBoosterNative.database.cleaner.workers.custom
{
    public class ESPdx
    {
        public string GetCategory()
        {
            return "Cheats";
        }

        public struct ESPdXResult
        {
            public bool found;
            public string path;

            public ESPdXResult()
            {
                found = false;
                path = string.Empty;
            }
        }

        public ESPdXResult TryDelete()
        {
            ESPdXResult result = new ESPdXResult();
            if (File.Exists("C:\\Program Files\\WinBooster\\DataBase\\sha3.json"))
            {
                string json = File.ReadAllText("C:\\Program Files\\WinBooster\\DataBase\\sha3.json");
                SHA3DataBase? database = SHA3DataBase.FromJson(json);
                bool found = false;
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady)
                    {
                        string[] folders = Directory.GetDirectories(drive.Name);
                        foreach (string folder in folders)
                        {
                            try
                            {
                                DirectoryInfo directory = new DirectoryInfo(folder);
                                FileInfo[] files = directory.GetFiles("*.exe");
                                foreach (FileInfo file in files)
                                {
                                    if (file.Extension == ".exe")
                                    {
                                        string hash = SHA3DataBase.GetHashString(SHA3DataBase.GetHash(File.ReadAllBytes(file.FullName)));
                                        
                                        SHA3FileInfo? fileinfo = database?.TryGetFileInfo(hash);
                                        if (fileinfo != null && fileinfo.name == "ESPdX")
                                        {
                                            found = true;
                                            result.found = true;
                                            result.path = file.FullName;
                                            break;
                                        }
                                    }
                                }
                                if (found)
                                    break;
                            }
                            catch { }
                        }
                    }
                    if (found)
                        break;
                }
            }
            return result;
        }
    }
}
