using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinBoosterNative.database.cleaner.workers.language
{
    public class ListFilesIfCurrentLanguage : ILanguageWorker, ICleanerWorker
    {
        public Language[] languages;
        public string category;
        public string mainDirectory;
        public List<string> files = new List<string>();

        public ListFilesIfCurrentLanguage(string directory, List<string> files, string category, params Language[] languages)
        {
            
            this.languages = languages;
            this.category = category;
            mainDirectory = directory;
            this.files = files;
        }
        public string GetFolder()
        {
            return PlaceholderDataBaseParser.Parse(mainDirectory);
        }
        public List<string> GetFolders()
        {
            return PlaceholderDataBaseParser.ParseMultiforlder(mainDirectory);
        }
        public string GetCategory()
        {
            return category;
        }

        public CleanerResult TryDelete()
        {
            CleanerResult result;
            result.bytes = 0;
            result.files = 0;

            Language os = GetWindowsLanguage();

            if (languages.Contains(os))
            {
                string directoryDone = PlaceholderDataBaseParser.Parse(mainDirectory);
                if (Directory.Exists(directoryDone))
                {
                    foreach (string file in files)
                    {
                        string filePath = Path.Combine(directoryDone, file);
                        if (File.Exists(filePath))
                        {
                            FileInfo fileInfo = new FileInfo(filePath);
                            try { File.Delete(filePath); result.bytes += fileInfo.Length; result.files++; } catch { }
                        }
                    }
                }
            }
            return result;
        }

        public bool IsAvalible()
        {
            Language os = GetWindowsLanguage();

            if (languages.Contains(os))
            {
                string directoryDone = PlaceholderDataBaseParser.Parse(mainDirectory);
                if (Directory.Exists(directoryDone))
                {
                    foreach (string file in files)
                    {
                        string filePath = Path.Combine(directoryDone, file);
                        if (File.Exists(filePath))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsRunAsTi()
        {
            return false;
        }

        public bool IsRunAsSystem()
        {
            return false;
        }
    }
}
