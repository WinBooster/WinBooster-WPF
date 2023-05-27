using System.Globalization;
using WinBoosterNative.database.sha3;
using static WinBoosterNative.database.cleaner.workers.language.ILanguageWorker;

namespace WinBoosterNative.database.cleaner.workers.language
{
    public class ILanguageWorker
    {
        public enum Language
        {
            Unknow = 0,
            English_US = 1,
            Russian_RU = 2,
            Ukraine_UK = 3,
            French_FR = 4,
        }

        public Language FileNameToLanguage(string name)
        {
            if (File.Exists("C:\\Program Files\\WinBooster\\DataBase\\fileNameLanguages.json"))
            {
                string json = File.ReadAllText("C:\\Program Files\\WinBooster\\DataBase\\fileNameLanguages.json");
                FileNameLanguageDataBase? database = FileNameLanguageDataBase.FromJson(json);
                if (database != null && database.database.ContainsKey(name))
                {
                    return database.database[name];
                }

                switch (name)
                {
                    #region English US
                    case "en-US":
                        return Language.English_US;
                    case "qt_en":
                        return Language.English_US;
                    case "keepassxc_en_US":
                        return Language.English_US;
                    case "qtbase_en":
                        return Language.English_US;
                    case "RvRvpnGui_en_US":
                        return Language.English_US;
                    case "steamui_english-json":
                        return Language.English_US;
                    case "shared_english-json":
                        return Language.English_US;
                    case "overlay_english":
                        return Language.English_US;
                    case "platform_english":
                        return Language.English_US;
                    case "vgui_english":
                        return Language.English_US;
                    case "shared_english":
                        return Language.English_US;
                    #endregion
                    #region Ukraine UK
                    case "uk":
                        return Language.Ukraine_UK;
                    case "qt_uk":
                        return Language.Ukraine_UK;
                    case "qtbase_uk":
                        return Language.Ukraine_UK;
                    case "keepassxc_uk":
                        return Language.Ukraine_UK;
                    case "RvRvpnGui_uk_UA":
                        return Language.Ukraine_UK;
                    case "steamui_ukrainian-json":
                        return Language.Ukraine_UK;
                    case "shared_ukrainian-json":
                        return Language.Ukraine_UK;
                    case "overlay_ukrainian":
                        return Language.Ukraine_UK;
                    case "platform_ukrainian":
                        return Language.Ukraine_UK;
                    case "vgui_ukrainian":
                        return Language.Ukraine_UK;
                    case "shared_ukrainian":
                        return Language.Ukraine_UK;
                    #endregion
                    #region French FR
                    case "fr":
                        return Language.French_FR;
                    case "RvRvpnGui_fr_FR":
                        return Language.French_FR;
                    case "keepassxc_fr":
                        return Language.French_FR;
                    case "qtbase_fr":
                        return Language.French_FR;
                    case "qt_fr":
                        return Language.French_FR;
                    case "steamui_french-json":
                        return Language.French_FR;
                    case "shared_french-json":
                        return Language.French_FR;
                    case "overlay_french":
                        return Language.French_FR;
                    case "platform_french":
                        return Language.French_FR;
                    case "vgui_french":
                        return Language.French_FR;
                    case "shared_french":
                        return Language.French_FR;
                    #endregion
                    default:
                        return Language.Unknow;
                }
            }
            return Language.Unknow;
        }
        public Language FileNameToLanguage(FileInfo fileInfo)
        {
            return FileNameToLanguage(Path.GetFileNameWithoutExtension(fileInfo.Name));
        }
        public static Language WindowsLanguage()
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            switch (ci.EnglishName)
            {
                case "Russian (Russia)":
                    return Language.Russian_RU;
                case "English (United States)":
                    return Language.English_US;
                case "Ukrainian (Ukraine)":
                    return Language.Ukraine_UK;
                case "French (France)":
                    return Language.French_FR;
                default:
                    return Language.Unknow;
            }
        }
        public Language GetWindowsLanguage()
        {
            return WindowsLanguage();
        }
    }
}
