using System.IO;
using System.Text.RegularExpressions;

namespace WinBoosterNative
{
    public class PlaceholderDataBaseParser
    {
        public static string Parse(string original)
        {
            string directory = original;
            directory = directory.Replace("{username}", Environment.UserName);
            DirectoryInfo steamDirectory = WinBoosterUtils.FindSteamDirectory();
            if (steamDirectory != null)
            {
                directory = directory.Replace("{steam}", steamDirectory.FullName);
            }
            return directory;
        }
        public static List<string> ParseMultiforlder(string original)
        {
            List<string> tempList = new List<string>();
            string directory = Parse(original);
            if (directory.Contains("{unknowfolder}"))
            {
                try
                {
                    var reg = Regex.Match(directory, "(.*){unknowfolder}(.*)");
                    if (reg.Success)
                    {
                        string first = reg.Groups[1].Value;
                        if (Directory.Exists(first))
                        {
                            string last = reg.Groups[2].Value;
                            var dirs = Directory.GetDirectories(first);
                            foreach (var dir2 in dirs)
                            {
                                DirectoryInfo directoryInfo = new DirectoryInfo(dir2);
                                tempList.Add(first + directoryInfo.Name + last);
                            }
                        }
                    }
                    else
                    {
                        var reg2 = Regex.Match(directory, "(.*){unknowfolder}");
                        if (reg2.Success)
                        {
                            string first = reg2.Groups[1].Value;
                            if (Directory.Exists(first))
                            {
                                var dirs = Directory.GetDirectories(first);
                                foreach (var dir2 in dirs)
                                {
                                    DirectoryInfo directoryInfo = new DirectoryInfo(dir2);
                                    tempList.Add(first + directoryInfo.Name);
                                }
                            }
                        }
                    }
                }
                catch { }
            }
            else
            {
                tempList.Add(directory);
            }
            return tempList;
        }
    }
}
