using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.AccessControl;
using TextCopy;
using WinBoosterNative.database.cleaner;
using WinBoosterNative.database.cleaner.workers;
using WinBoosterNative.database.cleaner.workers.language;
using WinBoosterNative.database.scripts;
using WinBoosterNative.database.sha3;

namespace DataBase_Adder
{
    internal static class Program
    {
        static void Main()
        {
            Console.WriteLine("Avalible");
            Console.WriteLine("0. Cleaner");
            Console.WriteLine("1. SHA3");
            Console.WriteLine("2. Language");
            Console.WriteLine("3. Scripts");
            Console.Write("Select database: ");

            string selected = Console.ReadLine();
            int selectedi1 = 0;
            int.TryParse(selected, out selectedi1);
            if (selectedi1 == 0)
            {
                CleanerDataBase dataBase = new CleanerDataBase();
                #region Windows
                CleanerCategory windows_category = new CleanerCategory("Windows");
                windows_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Temp", "Logs"));
                windows_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Windows\\Panther", "Logs"));
                windows_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Windows\\Temp", "Logs"));
                windows_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Windows\\Logs", "Logs"));
                windows_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Windows\\Logs\\WindowsUpdate", "Logs"));
                windows_category.listFiles.Add(new ListFiles("C:\\Windows\\debug\\WIA", "Logs", new List<string>() { "wiatrace.log", }));
                windows_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Local\\Temp", "Logs"));
                windows_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\ProgramData\\USOShared\\Logs", "Logs"));
                windows_category.paternFiles.Add(new PaternFiles("C:\\Windows\\Prefetch", "*", "Logs"));
                windows_category.paternFiles.Add(new PaternFiles("C:\\Windows\\Minidump", "*", "Cache"));
                windows_category.paternFiles.Add(new PaternFiles("C:\\Windows\\security\\logs", "*.log", "Logs"));
                windows_category.paternFiles.Add(new PaternFiles("C:\\Windows\\security\\database", "*.log", "Logs"));

                windows_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Local\\ConnectedDevicesPlatform", "LastActivity"));
                windows_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Local\\CrashDumps", "Logs"));
                windows_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\Downloads", "Downloads"));
                dataBase.cleaners.Add(windows_category);
                #endregion
                #region NVIDIA Corporation
                CleanerCategory NVIDIA_Corporation = new CleanerCategory("NVIDIA Corporation");
                NVIDIA_Corporation.listFiles.Add(new ListFiles("C:\\Program Files\\NVIDIA Corporation", "Logs", new List<string>()
                {
                    "license.txt",
                }));
                NVIDIA_Corporation.listFiles.Add(new ListFiles("C:\\Program Files\\NVIDIA Corporation\\NVSMI", "Logs", new List<string>()
                {
                    "nvidia-smi.1.pdf",
                }));
                dataBase.cleaners.Add(NVIDIA_Corporation);
                #endregion
                #region Java
                CleanerCategory java_category = new CleanerCategory("Java");
                java_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\.jdks\\{unknowfolder}", "Cache", new List<string>() { "javafx-src.zip", "src.zip" }));
                java_category.listFolders.Add(new ListFolders("C:\\Users\\{username}\\.jdks\\{unknowfolder}", "Cache", new List<string>() { "sample", "demo" }));
                java_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\.jdks\\{unknowfolder}", "Logs", new List<string>() { "COPYRIGHT", "LICENSE", "release", "README", "ADDITIONAL_LICENSE_INFO", "ASSEMBLY_EXCEPTION" }));

                java_category.paternFiles.Add(new PaternFiles("C:\\Program Files (x86)\\Java\\{unknowfolder}", "*.txt", "Logs"));
                java_category.paternFiles.Add(new PaternFiles("C:\\Program Files (x86)\\Java\\{unknowfolder}", "*.html", "Logs"));
                java_category.paternFiles.Add(new PaternFiles("C:\\ProgramData\\Oracle\\Java\\.oracle_jre_usage", "*.timestamp", "Logs"));
                java_category.listFiles.Add(new ListFiles("C:\\Program Files\\Java\\{unknowfolder}", "Logs", new List<string>() { "COPYRIGHT", "LICENSE", "release", "README" }));
                java_category.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\Java\\{unknowfolder}", "Logs", new List<string>() { "COPYRIGHT", "LICENSE", "release", "README" }));
                java_category.listFiles.Add(new ListFiles("C:\\Program Files\\Eclipse Adoptium\\jdk-8.0.362.9-hotspot", "Cache", new List<string>() { "src.zip", }));
                java_category.listFolders.Add(new ListFolders("C:\\Program Files\\Eclipse Adoptium\\jdk-8.0.362.9-hotspot", "Cache", new List<string>() { "sample", }));

                java_category.paternFiles.Add(new PaternFiles("C:\\Program Files\\Zulu\\{unknowfolder}", "*.txt", "Logs"));

                java_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Roaming\\.loliland\\java", "Logs", new List<string>() { "COPYRIGHT", "LICENSE", "README.txt", "release", "THIRDPARTYLICENSEREADME.txt", "THIRDPARTYLICENSEREADME-JAVAFX.txt", "Welcome.html" }));
                dataBase.cleaners.Add(java_category);
                #endregion
                #region 4uKey for Android
                CleanerCategory uKey_for_androind_category = new CleanerCategory("4uKey for Android");
                uKey_for_androind_category.paternFiles.Add(new PaternFiles("C:\\Program Files (x86)\\Tenorshare\\4uKey for Android\\Logs", "*", "Logs"));
                uKey_for_androind_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\TSMonitor\\4uKey for Android\\logs", "*", "Logs"));
                dataBase.cleaners.Add(uKey_for_androind_category);
                #endregion
                #region Postman
                CleanerCategory postman = new CleanerCategory("Postman");
                postman.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\PostmanAgent\\logs", "*.log", "Logs"));
                postman.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\Postman-Agent", "*.log", "Logs"));
                dataBase.cleaners.Add(postman);
                #endregion
                #region IDA Pro
                CleanerCategory ida_pro_category = new CleanerCategory("IDA Pro");
                ida_pro_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\Hex-Rays\\IDA Pro", "*.lst", "Cache"));
                dataBase.cleaners.Add(ida_pro_category);
                #endregion
                #region Borderless Gaming
                CleanerCategory borderless_gaming_category = new CleanerCategory("Borderless Gaming");
                borderless_gaming_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Roaming\\Andrew Sampson\\Borderless Gaming\\Languages", "*.pak", true, "Language"));
                dataBase.cleaners.Add(borderless_gaming_category);
                #endregion
                #region Xamarin
                CleanerCategory xamarin_category = new CleanerCategory("Xamarin");
                xamarin_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\Xamarin\\Logs\\17.0", "*.log", "Logs"));
                dataBase.cleaners.Add(xamarin_category);
                #endregion
                #region Windscribe
                CleanerCategory windscire_category = new CleanerCategory("Windscribe");
                windscire_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\Windscribe\\Windscribe2", "*.txt", "Logs"));
                dataBase.cleaners.Add(windscire_category);
                #endregion
                #region GitHub Desktop
                CleanerCategory github_desktop_category = new CleanerCategory("GitHub Desktop");
                github_desktop_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\GitHub Desktop", "*.log", "Logs"));
                github_desktop_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\GitHub Desktop\\logs", "*.log", "Logs"));
                github_desktop_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Local\\GitHubDesktop\\app-3.2.0\\locales", "*.pak", true, "Language"));
                github_desktop_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Local\\GitHubDesktop\\app-3.2.1\\locales", "*.pak", true, "Language"));
                github_desktop_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Local\\GitHubDesktop\\app-3.2.3\\locales", "*.pak", true, "Language"));
                dataBase.cleaners.Add(github_desktop_category);
                #endregion
                #region Panda Security
                CleanerCategory panda_security_category = new CleanerCategory("Panda Security");
                panda_security_category.paternFiles.Add(new PaternFiles("C:\\ProgramData\\Panda Security\\PSLogs", "*.log", "Logs"));
                dataBase.cleaners.Add(panda_security_category);
                #endregion
                #region NetLimiter
                CleanerCategory netLimiter_category = new CleanerCategory("NetLimiter");
                netLimiter_category.paternFiles.Add(new PaternFiles("CC:\\ProgramData\\Locktime\\NetLimiter\\5\\logs", "*.log", "Logs"));
                dataBase.cleaners.Add(netLimiter_category);
                #endregion
                #region MiniBin
                CleanerCategory miniBin_category = new CleanerCategory("MiniBin");
                miniBin_category.paternFiles.Add(new PaternFiles("C:\\Program Files (x86)\\MiniBin", "*.txt", "Logs"));
                dataBase.cleaners.Add(miniBin_category);
                #endregion
                #region Brave Browser
                CleanerCategory brave_browser_category = new CleanerCategory("Brave Browser");
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Program Files\\BraveSoftware\\Brave-Browser\\Application", "Logs", new List<string>() { "debug.log" }));
                brave_browser_category.paternFiles.Add(new PaternFiles("C:\\Program Files\\BraveSoftware\\Brave-Browser\\Application\\SetupMetrics", "*.pma", "Logs"));

                brave_browser_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\Code Cache\\js", "Cache", false));
                brave_browser_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\Cache\\Cache_Data", "Cache", false));
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default", "Cache", new List<string>() {
                    "Favicons",
                    "Favicons-journal",
                    "History",
                    "History-journal",
                    "Visited Links"
                }));
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\DawnCache", "Cache", new List<string>() { 
                    "data_0", 
                    "data_1", 
                    "data_2", 
                    "data_3", 
                    "index" 
                }));
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\GPUCache", "Cache", new List<string>() { 
                    "data_0", 
                    "data_1", 
                    "data_2", 
                    "data_3", 
                    "index" 
                }));
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\BudgetDatabase", "Logs", new List<string>() { 
                    "LOCK", 
                    "LOG", 
                    "LOG.old" 
                }));
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\commerce_subscription_db", "Logs", new List<string>() { 
                    "LOCK", 
                    "LOG", 
                    "LOG.old" 
                }));
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\coupon_db", "Logs", new List<string>() { 
                    "LOCK", 
                    "LOG", 
                    "LOG.old" 
                }));
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\Download Service\\EntryDB", "Logs", new List<string>() { 
                    "LOCK", 
                    "LOG", 
                    "LOG.old" 
                }));
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\Extension Rules", "Logs", new List<string>() { 
                    "000003.log" 
                }));
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\Extension Scripts", "Logs", new List<string>() { 
                    "000003.log",
                    "LOCK",
                    "LOG",
                    "LOG.old",
                }));
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\Extension State", "Logs", new List<string>() { 
                    "000003.log" 
                }));
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\Feature Engagement Tracker\\AvailabilityDB", "Logs", new List<string>() { 
                    "LOCK", 
                    "LOG", 
                    "LOG.old" 
                }));
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\Feature Engagement Tracker\\EventDB", "Logs", new List<string>() { 
                    "LOCK", 
                    "LOG", 
                    "LOG.old" 
                }));
                brave_browser_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\BraveSoftware\\Brave-Browser\\User Data\\Default\\BraveWallet\\Brave Wallet Storage", "Logs", new List<string>() {
                    "LOCK",
                    "LOG",
                    "LOG.old",
                    "000003.log"
                }));
                dataBase.cleaners.Add(brave_browser_category);
                #endregion
                #region Mem Reduct
                CleanerCategory mem_reduct_category = new CleanerCategory("Mem Reduct");
                mem_reduct_category.listFiles.Add(new ListFiles("C:\\Program Files\\Mem Reduct", "Logs", new List<string>() { "History.txt", "License.txt", "Readme.txt", "memreduct.exe.sig" }));
                dataBase.cleaners.Add(mem_reduct_category);
                #endregion
                #region qBittorrent
                CleanerCategory qBittorrent_category = new CleanerCategory("qBittorrent");
                qBittorrent_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files\\qBittorrent\\translations", "*.qm", true, "Language"));
                qBittorrent_category.listFiles.Add(new ListFiles("C:\\Program Files\\qBittorrent", "Logs", new List<string>() { "qbittorrent.pdb" }));
                qBittorrent_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\qBittorrent\\logs", "Logs", new List<string>() { "qbittorrent.log" }));
                dataBase.cleaners.Add(qBittorrent_category);
                #endregion
                #region CCleaner
                CleanerCategory ccLeaner_category = new CleanerCategory("CCleaner");
                ccLeaner_category.paternFiles.Add(new PaternFiles("C:\\Program Files\\CCleaner\\LOG", "*", "Logs"));
                dataBase.cleaners.Add(ccLeaner_category);
                #endregion
                #region IObit
                #region IObit Malware Fighter
                CleanerCategory IObit_Malware_Fighter = new CleanerCategory("IObit Malware Fighter");
                IObit_Malware_Fighter.listFiles.Add(new ListFiles("C:\\ProgramData\\IObit\\IObit Malware Fighter", "Logs", new List<string>() { "imf_setup.log", "init.log", "Setup.log", "License.log" }));
                IObit_Malware_Fighter.listFiles.Add(new ListFiles("C:\\ProgramData\\IObit\\IObit Malware Fighter\\Homepage Advisor", "Logs", new List<string>() { "BrowserProtect.log", "IMFsrv.log" }));
                dataBase.cleaners.Add(IObit_Malware_Fighter);
                #endregion
                #region IObit Driver Booster
                CleanerCategory IObit_Driver_Booster = new CleanerCategory("IObit Driver Booster");
                IObit_Driver_Booster.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Roaming\\IObit\\Driver Booster\\Logs", "Logs"));
                IObit_Driver_Booster.paternFilesRecursives.Add(new PaternFilesRecursive("C:\\Program Files (x86)\\IObit\\Driver Booster", "*.log", "Logs", false));
                IObit_Driver_Booster.paternFilesRecursives.Add(new PaternFilesRecursive("C:\\Program Files (x86)\\IObit\\Driver Booster", "*.txt", "Logs", false));
                dataBase.cleaners.Add(IObit_Driver_Booster);
                #endregion
                #endregion
                #region Process Lasso
                CleanerCategory process_lasso_category = new CleanerCategory("Process Lasso");
                process_lasso_category.paternFiles.Add(new PaternFiles("C:\\ProgramData\\ProcessLasso\\logs", "*", "Logs"));
                dataBase.cleaners.Add(process_lasso_category);
                #endregion
                #region OBS Studio
                CleanerCategory obs_studio_category = new CleanerCategory("OBS Studio");
                obs_studio_category.listFiles.Add(new ListFiles("C:\\Program Files\\obs-studio\\bin\\64bit", "Logs", new List<string>() { "debug.log" }));
                obs_studio_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\obs-studio\\logs", "*.txt", "Logs"));
                dataBase.cleaners.Add(obs_studio_category);
                #endregion
                #region Unity Hub
                CleanerCategory unity_hub_category = new CleanerCategory("Unity Hub");
                unity_hub_category.listFiles.Add(new ListFiles("C:\\Program Files\\Unity Hub", "Logs", new List<string>() { "LICENSES.chromium.html" }));
                dataBase.cleaners.Add(unity_hub_category);
                #endregion
                #region KeePass 2
                CleanerCategory keePass_password_safe_2 = new CleanerCategory("KeePass 2");
                keePass_password_safe_2.listFiles.Add(new ListFiles("C:\\Program Files\\KeePass Password Safe 2", "Logs", new List<string>() { "License.txt" }));
                dataBase.cleaners.Add(keePass_password_safe_2);
                #endregion
                #region KeePass XC
                CleanerCategory keepassXC = new CleanerCategory("KeePass XC");
                keepassXC.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files\\KeePassXC\\share\\translations", "*.qm", true, "Language"));
                dataBase.cleaners.Add(keepassXC);
                #endregion
                #region LGHUB
                CleanerCategory lghub_category = new CleanerCategory("LGHUB");
                lghub_category.paternFiles.Add(new PaternFiles("C:\\Program Files\\LGHUB\\locales", "*.pak", "Cache"));
                lghub_category.listFiles.Add(new ListFiles("C:\\Program Files\\LGHUB", "Logs", new List<string>() { "LICENSE", "LICENSES.chromium.html" }));
                dataBase.cleaners.Add(lghub_category);
                #endregion
                #region 1Password
                CleanerCategory password1 = new CleanerCategory("1Password");
                password1.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\1Password\\logs\\setup", "*.log", "Logs"));
                dataBase.cleaners.Add(password1);
                #endregion
                #region DeepL
                CleanerCategory deepL_category = new CleanerCategory("DeepL");
                deepL_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\DeepL_SE\\logs", "*.log", "Logs"));
                deepL_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\DeepL_SE\\cache", "*.log", "Logs"));
                dataBase.cleaners.Add(deepL_category);
                #endregion
                #region Microsoft Lobe
                CleanerCategory microsoft_lobe_category = new CleanerCategory("Microsoft Lobe");
                microsoft_lobe_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Roaming\\Lobe\\logs", "Logs", true));
                microsoft_lobe_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files\\Lobe\\locales", "*.pak", true, "Language"));
                dataBase.cleaners.Add(microsoft_lobe_category);
                #endregion
                #region Tonfotos Telegram Connector
                CleanerCategory tonfotos = new CleanerCategory("Tonfotos Telegram Connector");
                tonfotos.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\Pictures\\Tonfotos Telegram Connector", "Photo", true));
                dataBase.cleaners.Add(tonfotos);
                #endregion
                #region DotNet
                CleanerCategory dotnet_category = new CleanerCategory("DotNet");
                dotnet_category.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\dotnet", "Logs", new List<string>() { "LICENSE.txt", "ThirdPartyNotices.txt" }));
                dotnet_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\.dotnet\\TelemetryStorageService", "*", "Cache"));
                dataBase.cleaners.Add(dotnet_category);
                #endregion
                #region FileZilla
                CleanerCategory fileZilla = new CleanerCategory("FileZilla");
                fileZilla.foldersIsNotLanguageByPatern.Add(new FoldersIfCurrentLanguageByPatern("C:\\Program Files\\FileZilla FTP Client\\locales", "*", true, "Language"));
                dataBase.cleaners.Add(fileZilla);
                #endregion
                #region MCCreator
                CleanerCategory mccreator = new CleanerCategory("MCCreator");
                
                mccreator.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\.mcreator\\logs", "*.log", "Logs"));
                dataBase.cleaners.Add(mccreator);
                #endregion
                #region 7-Zip
                CleanerCategory sevenZip = new CleanerCategory("7-Zip");

                sevenZip.paternFiles.Add(new PaternFiles("C:\\Program Files\\7-Zip", "*.txt", "Logs"));
                sevenZip.foldersIsNotLanguageByPatern.Add(new FoldersIfCurrentLanguageByPatern("C:\\Program Files\\7-Zip\\Lang", "*.txt", false, "Language"));
                dataBase.cleaners.Add(sevenZip);
                #endregion
                #region Tribler
                CleanerCategory tribler = new CleanerCategory("Tribler");
                tribler.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\.Tribler", "*.log", "Logs"));
                dataBase.cleaners.Add(tribler);
                #endregion
                #region I2P
                CleanerCategory i2p = new CleanerCategory("I2P");
                i2p.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\i2peasy\\addressbook", "Logs", new List<string>()
                {
                    "log.txt"
                }));
                i2p.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Local\\i2peasy", "Logs", new List<string>()
                {
                    "eventlog.txt",
                    "wrapper.log"
                }));
                i2p.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Local\\i2peasy\\logs", "Logs"));
                i2p.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Local\\i2peasy\\licenses", "Logs"));
                dataBase.cleaners.Add(i2p);
                #endregion
                #region BoxedAppPacker
                CleanerCategory boxedAppPacker = new CleanerCategory("BoxedAppPacker");
                boxedAppPacker.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\BoxedAppPacker", "Logs", new List<string>()
                {
                    "changelog.txt",
                    "HomePage.url",
                    "purchase.url"
                }));
                dataBase.cleaners.Add(boxedAppPacker);
                #endregion 
                #region Enigma Virtual Box
                CleanerCategory enigmaVirtualBox = new CleanerCategory("Enigma Virtual Box");
                enigmaVirtualBox.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\Enigma Virtual Box", "Logs", new List<string>()
                {
                    "help.chm",
                    "History.txt",
                    "License.txt",
                    "site.url",
                    "forum.url",
                    "support.url"
                }));
                dataBase.cleaners.Add(enigmaVirtualBox);
                #endregion
                #region GnuPG
                CleanerCategory GnuPG = new CleanerCategory("GnuPG");
                GnuPG.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\GnuPG", "Logs", new List<string>()
                {
                    "README.txt",
                    "VERSION"
                }));
                dataBase.cleaners.Add(GnuPG);
                #endregion
                #region Gpg4win
                CleanerCategory Gpg4win = new CleanerCategory("Gpg4win");
                Gpg4win.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\Gpg4win", "Logs", new List<string>()
                {
                    "VERSION"
                }));
                Gpg4win.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files (x86)\\Gpg4win\\bin\\translations", "*.qm", true, "Language"));
                dataBase.cleaners.Add(Gpg4win);
                #endregion
                #region Inno Setup 6
                CleanerCategory innoSetup6 = new CleanerCategory("Inno Setup 6");
                innoSetup6.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\Inno Setup 6", "Logs", new List<string>()
                {
                    "whatsnew.htm",
                    "isfaq.url",
                    "license.txt"
                }));
                innoSetup6.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\Inno Setup 6\\Examples", "Logs", new List<string>()
                {
                    "Readme.txt",
                    "Readme-Dutch.txt",
                    "Readme-German.txt",
                    "License.txt",
                    "ISPPExample1License.txt",
                }));
                innoSetup6.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files (x86)\\Inno Setup 6\\Languages", "*.isl", true, "Language"));
                dataBase.cleaners.Add(innoSetup6);
                #endregion
                #region VirtualBox
                CleanerCategory virtualBox = new CleanerCategory("VirtualBox");
                virtualBox.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\VirtualBox VMs\\{unknowfolder}\\Logs", "*log*", "Logs"));
                virtualBox.paternFiles.Add(new PaternFiles("C:\\Program Files\\Oracle\\VirtualBox", "*.rtf", "Logs"));
                virtualBox.allFilesRecursives.Add(new AllFilesRecursive("C:\\Program Files\\Oracle\\VirtualBox\\doc", "Logs", true));
                dataBase.cleaners.Add(virtualBox);
                #endregion
                #region Recaf
                CleanerCategory recaf = new CleanerCategory("Recaf");
                recaf.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\Recaf", "*log*", "Logs"));
                dataBase.cleaners.Add(recaf);
                #endregion
                #region Process Hacker 2
                CleanerCategory Process_Hacker_2 = new CleanerCategory("Process Hacker 2");
                Process_Hacker_2.listFiles.Add(new ListFiles("C:\\Program Files\\Process Hacker 2", "Logs", new List<string>()
                {
                    "CHANGELOG.txt",
                    "COPYRIGHT.txt",
                    "LICENSE.txt",
                    "README.txt",
                    "ProcessHacker.sig",
                }));
                dataBase.cleaners.Add(Process_Hacker_2);
                #endregion
                #region Docker
                CleanerCategory docker = new CleanerCategory("Docker");
                docker.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\Docker\\log\\host", "*", "Logs"));
                docker.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\Docker\\log\\vm", "*", "Logs"));
                docker.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\Docker", "install-log.txt", "Logs"));
                docker.listFiles.Add(new ListFiles("C:\\ProgramData\\DockerDesktop", "Logs", new List<string>()
                {
                    "install-cli-log-admin.txt",
                    "install-log-admin.txt"
                }));
                dataBase.cleaners.Add(docker);
                #endregion
                #region HiAlgo Boost
                CleanerCategory hialgo_boost_category = new CleanerCategory("HiAlgo Boost");
                hialgo_boost_category.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\HiAlgo\\Plugins\\BOOST", "Logs", new List<string>()
                {
                    "hialgo_eula.txt",
                    "Update Boost.log",
                    "UpdateListing.txt",
                }));
                dataBase.cleaners.Add(hialgo_boost_category);
                #endregion
                #region SoundWire Server
                CleanerCategory sound_wire_server_category = new CleanerCategory("SoundWire Server");
                sound_wire_server_category.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\SoundWire Server", "Logs", new List<string>()
                {
                    "license.txt",
                    "opus_license.txt",
                    "readerwriterqueue_license.txt",
                }));
                dataBase.cleaners.Add(sound_wire_server_category);
                #endregion
                #region System Informer
                CleanerCategory system_informer_category = new CleanerCategory("System Informer");
                system_informer_category.listFiles.Add(new ListFiles("C:\\Program Files\\SystemInformer", "Logs", new List<string>()
                {
                    "LICENSE.txt",
                    "README.txt",
                }));
                dataBase.cleaners.Add(system_informer_category);
                #endregion
                #region Sandboxie Plus
                CleanerCategory sandboxie_plus_category = new CleanerCategory("Sandboxie Plus");
                sandboxie_plus_category.listFiles.Add(new ListFiles("C:\\Program Files\\Sandboxie-Plus", "Logs", new List<string>()
                {
                    "Manifest0.txt",
                    "Manifest1.txt",
                    "Manifest2.txt"
                }));
                dataBase.cleaners.Add(sandboxie_plus_category);
                #endregion
                #region JetBrains
                CleanerCategory JetBrains_category = new CleanerCategory("JetBrains");
                JetBrains_category.listFiles.Add(new ListFiles("C:\\Program Files\\JetBrains\\{unknowfolder}\\license", "Logs", new List<string>()
                {
                    "javahelp_license.txt",
                    "javolution_license.txt",
                    "launcher-third-party-libraries.html",
                    "saxon-conditions.html",
                    "third-party-libraries.html",
                    "third-party-libraries.json",
                    "yourkit-license-redist.txt",
                    "remote-dev-server.html"
                }));
                JetBrains_category.listFiles.Add(new ListFiles("C:\\Program Files\\JetBrains\\{unknowfolder}", "Logs", new List<string>()
                {
                    "LICENSE.txt",
                    "NOTICE.txt"
                }));
                dataBase.cleaners.Add(JetBrains_category);
                #endregion

                #region Messengers

                #region Discord
                CleanerCategory discord_category = new CleanerCategory("Discord");
                discord_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Local\\Discord\\app-1.0.9013\\locales", "*.pak", true, "Language"));
                discord_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\Discord", "*.log", "Logs"));
                dataBase.cleaners.Add(discord_category);
                #endregion
                #region Guilded
                CleanerCategory guilded = new CleanerCategory("Guilded");
                guilded.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\Guilded", "*.log", "Logs"));
                dataBase.cleaners.Add(guilded);
                #endregion
                #region Element (Matrix)
                CleanerCategory element = new CleanerCategory("Element");
                element.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\element-desktop", "*.log", "Logs"));
                dataBase.cleaners.Add(element);
                #endregion
                #region Telegram Desktop
                CleanerCategory telegram_category = new CleanerCategory("Telegram");
                telegram_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\Telegram Desktop", "*.txt", "Logs"));
                telegram_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\Telegram Desktop", ".log", "Logs"));
                telegram_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Roaming\\Telegram Desktop\\tdata", "Accounts", new List<string>() { "key_datas", }));
                telegram_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Roaming\\Telegram Desktop\\tdata\\user_data\\cache\\0", "Cache", true));
                telegram_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Roaming\\Telegram Desktop\\tdata\\user_data\\media_cache\\0", "Cache", true));
                telegram_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\Telegram Desktop\\tdata\\emoji", "*cache_*", "Cache"));
                dataBase.cleaners.Add(telegram_category);
                #endregion
                #region Signal
                CleanerCategory signal = new CleanerCategory("Signal");
                signal.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\Signal\\logs", "*", "Logs"));
                signal.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\Signal\\update-cache", "*", "Cache"));
                dataBase.cleaners.Add(signal);
                #endregion
                #endregion
                #region VPN Clients

                #region Amnezia VPN
                CleanerCategory amneziaVPN = new CleanerCategory("Amnezia VPN");
                amneziaVPN.listFiles.Add(new ListFiles("C:\\Program Files\\AmneziaVPN", "Logs", new List<string>
                {
                    "InstallationLog.txt"
                }));
                amneziaVPN.listFiles.Add(new ListFiles("C:\\Program Files\\AmneziaVPN\\tap", "Logs", new List<string>
                {
                    "license.txt"
                }));
                dataBase.cleaners.Add(amneziaVPN);
                #endregion
                #region Radmin VPN
                CleanerCategory radmin_vpn_caregory = new CleanerCategory("Radmin VPN");
                radmin_vpn_caregory.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\Radmin VPN\\CHATLOGS", "Logs", new List<string>() { "info.txt" }));
                radmin_vpn_caregory.listFiles.Add(new ListFiles("C:\\ProgramData\\Famatech\\Radmin VPN", "Logs", new List<string>() { "service.log", "eula.txt" }));
                radmin_vpn_caregory.paternFiles.Add(new PaternFiles("C:\\ProgramData\\Famatech\\Radmin VPN", "*.txt", "Logs"));
                radmin_vpn_caregory.paternFiles.Add(new PaternFiles("C:\\Program Files (x86)\\Radmin VPN", "*.lng_rad", "Cache"));
                radmin_vpn_caregory.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files (x86)\\Radmin VPN", "*.qm", true, "Language"));
                dataBase.cleaners.Add(radmin_vpn_caregory);
                #endregion
                #region UrbanVPN
                CleanerCategory urban_vpn = new CleanerCategory("UrbanVPN");
                urban_vpn.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\UrbanVPN\\log", "*", "Logs"));
                dataBase.cleaners.Add(urban_vpn);
                #endregion
                #region CloudFlare
                CleanerCategory cloud_flare = new CleanerCategory("CloudFlare");
                cloud_flare.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\Cloudflare", "*.log", "Logs"));
                dataBase.cleaners.Add(cloud_flare);
                #endregion
                #region PlanetVPN
                CleanerCategory PlanetVPN = new CleanerCategory("PlanetVPN");
                PlanetVPN.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\PlanetVPN\\cache\\qmlcache", "*", "Cache"));
                dataBase.cleaners.Add(PlanetVPN);
                #endregion
                #region iTop VPN
                CleanerCategory iTop_vpn_category = new CleanerCategory("iTop VPN");
                iTop_vpn_category.listFiles.Add(new ListFiles("C:\\ProgramData\\iTop VPN", "Logs", new List<string>() { "iTop_setup.log", "Setup.log" }));
                dataBase.cleaners.Add(iTop_vpn_category);
                #endregion

                #endregion
                #region Audio
                #region YouTube Music Desktop
                CleanerCategory youtube_music_category = new CleanerCategory("YouTube Music Desktop");
                youtube_music_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Local\\Programs\\youtube-music-desktop-app\\locales", "*.pak", true, "Language"));
                dataBase.cleaners.Add(youtube_music_category);
                #endregion
                #region AAF Optimus DCH Audio
                CleanerCategory AAF = new CleanerCategory("AAF Optimus DCH Audio");
                AAF.listFiles.Add(new ListFiles("C:\\Program Files\\AAFTweak", "Logs", new List<string>()
                {
                    "RT.pdb"
                }));
                dataBase.cleaners.Add(AAF);
                #endregion
                #region FL Studio 21
                CleanerCategory flStudio = new CleanerCategory("FL Studio");
                flStudio.listFiles.Add(new ListFiles("C:\\Program Files\\Image-Line\\FL Studio 21", "Logs", new List<string>() { "WhatsNew.rtf" }));
                flStudio.listFiles.Add(new ListFiles("C:\\Program Files\\Image-Line\\Shared\\Start\\FL Studio 21", "Logs", new List<string>() { "What's new.lnk" }));
                flStudio.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\ASIO4ALL v2", "Logs", new List<string>() { "ASIO4ALL Web Site.url" }));
                flStudio.allFilesRecursives.Add(new AllFilesRecursive("C:\\Program Files\\Image-Line\\FL Studio 21\\System\\Legal", "Logs", true));
                dataBase.cleaners.Add(flStudio);
                #endregion
                #endregion
                #region Video
                #region VideoLAN
                CleanerCategory vlc_category = new CleanerCategory("VideoLAN");
                vlc_category.listFiles.Add(new ListFiles("C:\\Program Files\\VideoLAN\\VLC", "Logs", new List<string>()
                {
                    "AUTHORS.txt",
                    "COPYING.txt",
                    "NEWS.txt",
                    "README.txt",
                    "THANKS.txt",
                    "VideoLAN Website.url"
                }));
                dataBase.cleaners.Add(vlc_category);
                #endregion
                #region HandBrake
                CleanerCategory handbrake_category = new CleanerCategory("HandBrake");
                handbrake_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\HandBrake\\logs", "*.txt", "Logs"));
                handbrake_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Program Files\\HandBrake\\doc", "Logs", true));
                dataBase.cleaners.Add(handbrake_category);
                #endregion
                #region Topaz Video AI
                CleanerCategory Topaz_Video_AI = new CleanerCategory("Topaz Video AI");
                Topaz_Video_AI.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files\\Topaz Labs LLC\\Topaz Video AI\\translations", "*.qm", true, "Language"));
                Topaz_Video_AI.listFiles.Add(new ListFiles("C:\\ProgramData\\Topaz Labs LLC\\Topaz Video AI", "Language", new List<string> { "oss-licenses.txt" }));
                dataBase.cleaners.Add(Topaz_Video_AI);
                #endregion
                #region AVCLabs Video Enhancer AI
                CleanerCategory AVCLabs_Video_Enhancer_AI = new CleanerCategory("AVCLabs Video Enhancer AI");
                AVCLabs_Video_Enhancer_AI.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files (x86)\\AVCLabs\\AVCLabs Video Enhancer AI\\locales", "*.pak", true, "Language"));
                AVCLabs_Video_Enhancer_AI.paternFiles.Add(new PaternFiles("C:\\Program Files (x86)\\AVCLabs\\AVCLabs Video Enhancer AI", "*.txt", "Logs"));
                AVCLabs_Video_Enhancer_AI.paternFiles.Add(new PaternFiles("C:\\Program Files (x86)\\AVCLabs\\AVCLabs Video Enhancer AI", "*.html", "Logs"));
                AVCLabs_Video_Enhancer_AI.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\AVCLabs Video Enhancer AI\\logs", "*.log", "Logs"));
                dataBase.cleaners.Add(AVCLabs_Video_Enhancer_AI);
                #endregion
                #region iTop Screen Recorder
                CleanerCategory iTop_Screen_Recorder = new CleanerCategory("iTop Screen Recorder");
                iTop_Screen_Recorder.paternFiles.Add(new PaternFiles("C:\\Users\\Administrator\\AppData\\Roaming\\iTop Screen Recorder\\Logs", "*.log", "Logs"));
                dataBase.cleaners.Add(iTop_Screen_Recorder);
                #endregion
                #region Rave
                CleanerCategory rave = new CleanerCategory("Rave");
                rave.paternFiles.Add(new PaternFiles("C:\\Users\\Administrator\\AppData\\Roaming\\Rave\\logs", "*.log", "Logs"));
                rave.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\Administrator\\AppData\\Roaming\\Rave\\Cache", "Cache", false));
                rave.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\Administrator\\AppData\\Roaming\\Rave\\Code Cache", "Cache", false));
                dataBase.cleaners.Add(rave);
                #endregion
                #endregion
                #region Image
                #region ImageGlass
                CleanerCategory image_glass = new CleanerCategory("ImageGlass");
                image_glass.listFiles.Add(new ListFiles("C:\\Program Files\\ImageGlass", "Logs", new List<string>()
                {
                    "ReadMe.rtf",
                    "CliWrap.xml",
                    "DotNetZip.pdb",
                    "DotNetZip.xml",
                    "ImageGlass.ImageBox.xml",
                    "ImageGlass.ImageListView.xml",
                    "LICENSE",
                    "Magick.NET.Core.xml",
                    "Magick.NET.SystemDrawing.xml",
                    "Magick.NET-Q16-HDRI-OpenMP-x64.xml",
                    "Microsoft.Bcl.AsyncInterfaces.xml",
                    "System.Buffers.xml",
                    "System.Memory.xml",
                    "System.Numerics.Vectors.xml",
                    "System.Runtime.CompilerServices.Unsafe.xml",
                    "System.Text.Encodings.Web.xml",
                    "System.Text.Json.xml",
                    "System.Threading.Tasks.Extensions.xml",
                    "System.ValueTuple.xml",
                    "ImageGlass.WebP.pdb",
                    "Visit ImageGlass website.url",
                    "default.jpg"
                }));
                image_glass.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files\\ImageGlass\\Languages", "*.iglang", true, "Language"));
                dataBase.cleaners.Add(image_glass);
                #endregion
                #region InkSpace
                CleanerCategory inkSpace_category = new CleanerCategory("InkSpace");
                inkSpace_category.listFiles.Add(new ListFiles("C:\\Program Files\\Inkscape", "Logs", new List<string>() { "NEWS.md", "README.md" }));
                inkSpace_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Roaming\\inkscape", "Logs", new List<string>() { "extension-errors.log" }));
                dataBase.cleaners.Add(inkSpace_category);
                #endregion
                #region Magpie
                CleanerCategory magpie = new CleanerCategory("Magpie");

                magpie.paternFiles.Add(new PaternFiles("C:\\Program Files\\Magpie\\logs", "*.log", "Logs"));
                magpie.paternFiles.Add(new PaternFiles("C:\\Program Files\\Magpie\\cache", "*", "Cache"));
                dataBase.cleaners.Add(magpie);
                #endregion
                #region ShareX
                CleanerCategory shareX_category = new CleanerCategory("ShareX");
                shareX_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Documents\\ShareX\\Logs", "*.log", "Logs"));
                shareX_category.paternFilesRecursives.Add(new PaternFilesRecursive("C:\\Users\\{username}\\Documents\\ShareX\\Screenshots", "*.png", "Photo", true));
                dataBase.cleaners.Add(shareX_category);
                #endregion
                #endregion
                #region Text Editors
                #region Notepad++
                CleanerCategory notepad_plus_plus = new CleanerCategory("Notepad++");
                notepad_plus_plus.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\Notepad++", "*.log", "Logs"));
				notepad_plus_plus.paternFiles.Add(new PaternFiles("C:\\Program Files\\Notepad++", "*.log", "Logs"));
				notepad_plus_plus.paternFiles.Add(new PaternFiles("C:\\Program Files\\Notepad++", "*readme.txt*", "Logs"));
				notepad_plus_plus.paternFiles.Add(new PaternFiles("C:\\Program Files\\Notepad++", "*LICENSE*", "Logs"));
                dataBase.cleaners.Add(notepad_plus_plus);
                #endregion
                #region Sublime Text
                CleanerCategory sublimeText = new CleanerCategory("Sublime Text");
                sublimeText.listFiles.Add(new ListFiles("C:\\Program Files\\Sublime Text", "Logs", new List<string>()
                {
                    "changelog.txt"
                }));
                dataBase.cleaners.Add(sublimeText);
                #endregion
                #region LibreOffice
                CleanerCategory libreOffice = new CleanerCategory("LibreOffice");
                libreOffice.listFiles.Add(new ListFiles("C:\\Program Files\\LibreOffice", "Logs", new List<string>()
                {
                    "CREDITS.fodt",
                    "LICENSE.html",
                    "license.txt",
                    "NOTICE",
                }));
                libreOffice.allFilesRecursives.Add(new AllFilesRecursive("C:\\Program Files\\LibreOffice\\readmes", "Logs", true));
                dataBase.cleaners.Add(libreOffice);
                #endregion
                #endregion
                #region Crypto wallets
                #region Exodus
                CleanerCategory exodus_category = new CleanerCategory("Exodus Crypto Wallet");
                exodus_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Local\\exodus\\{unknowfolder}\\locales", "*.pak", true, "Language"));
                exodus_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\exodus", "*.log", "Logs"));
                dataBase.cleaners.Add(exodus_category);
                #endregion
                #region Wasabi Wallet
                CleanerCategory wasabi_wallet = new CleanerCategory("Wasabi Wallet");
                wasabi_wallet.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\WalletWasabi\\Client", "*.txt", "Logs"));
                dataBase.cleaners.Add(wasabi_wallet);
                #endregion
                #region Monero GUI
                CleanerCategory bitMonero = new CleanerCategory("Bit Monero");
                bitMonero.paternFiles.Add(new PaternFiles("C:\\ProgramData\\bitmonero", "*.log", "Logs"));
                dataBase.cleaners.Add(bitMonero);
                #endregion
                #endregion
                #region Emulators
                #region Memu
                CleanerCategory memu = new CleanerCategory("Memu");
               
                memu.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\.MemuHyperv", "*log*", "Logs"));
                dataBase.cleaners.Add(memu);
                #endregion
                #region GameLoop
                CleanerCategory gameloop_category = new CleanerCategory("Gameloop");
                gameloop_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files\\TxGameAssistant\\AppMarket\\locale", "*.pak", true, "Language"));
                gameloop_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\com.gametop.launcher\\logs", "*", "Logs"));
                dataBase.cleaners.Add(gameloop_category);
                #endregion
                #region BlueStacks X
                CleanerCategory blueStacks_x_category = new CleanerCategory("BlueStacks X");
                blueStacks_x_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files (x86)\\BlueStacks X\\language", "*.qm", true, "Language"));
                dataBase.cleaners.Add(blueStacks_x_category);
                #endregion
                #region BlueStacks 5
                CleanerCategory blueStacks_five_category = new CleanerCategory("BlueStacks 5");
                blueStacks_five_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files\\BlueStacks_nxt\\translations\\qtwebengine_locales", "*.qm", true, "Language"));
                blueStacks_five_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Local\\BlueStacksSetup", "*.exe", true, "Cache"));
                blueStacks_five_category.paternFiles.Add(new PaternFiles("C:\\ProgramData\\BlueStacks_nxt\\Logs", "*.log", "Logs"));
                blueStacks_five_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Pictures\\BlueStacks", "*.png", "Photo"));
                dataBase.cleaners.Add(blueStacks_five_category);
                #endregion
                #endregion
                #region AntiCheats
                #region GameGuard
                CleanerCategory gameGuard_category = new CleanerCategory("GameGuard");
                gameGuard_category.paternFiles.Add(new PaternFiles("C:\\Program Files (x86)\\GameGuard\\cache", "*.cache", "Cache"));
                dataBase.cleaners.Add(gameGuard_category);
                #endregion
                #region FACEIT AC
                CleanerCategory faceit_ac_category = new CleanerCategory("FACEIT AC");
                faceit_ac_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Local\\FACEIT\\app-1.31.12\\locales", "*.pak", true, "Language"));
                faceit_ac_category.paternFiles.Add(new PaternFiles("C:\\Program Files\\FACEIT AC\\logs", "*.log", "Logs"));
                dataBase.cleaners.Add(faceit_ac_category);
                #endregion
                #region EasyAntiCheat
                CleanerCategory easyAnticheat_category = new CleanerCategory("EasyAntiCheat");
                easyAnticheat_category.paternFilesRecursives.Add(new PaternFilesRecursive("C:\\Users\\{username}\\AppData\\Roaming\\EasyAntiCheat", "*.log", "Logs", false));
                dataBase.cleaners.Add(easyAnticheat_category);
                #endregion
                #endregion
                #region Cheats
                #region Cheat Engine
                CleanerCategory cheat_engine_category = new CleanerCategory("Cheat Engine");
                cheat_engine_category.listFiles.Add(new ListFiles("C:\\Program Files\\Cheat Engine 7.5", "Logs", new List<string>()
                {
                    "celua.txt",
                }));
                dataBase.cleaners.Add(cheat_engine_category);
                #endregion
                #region Fatality
                CleanerCategory fatality = new CleanerCategory("Fatality");
                fatality.allFilesRecursives.Add(new AllFilesRecursive("{steam}\\steamapps\\common\\Counter-Strike Global Offensive\\fatality", "Cheats", true));
                fatality.listFiles.Add(new ListFiles("{steam}\\steamapps\\common\\Counter-Strike Global Offensive", "Logs", new List<string>()
                {
                    "slot1",
                    "skins",
                    "flog.log",
                }));
                dataBase.cleaners.Add(fatality);
                #endregion
                #region Pandora
                CleanerCategory pandora = new CleanerCategory("Pandora");
                pandora.allFilesRecursives.Add(new AllFilesRecursive("{steam}\\steamapps\\common\\Counter-Strike Global Offensive\\Pandora", "Cheats", true));
                pandora.listFiles.Add(new ListFiles("{steam}\\steamapps\\common\\Counter-Strike Global Offensive", "Logs", new List<string>()
                {
                    "log.pdr",
                }));
                dataBase.cleaners.Add(pandora);
                #endregion
                #region OneTap
                CleanerCategory otc = new CleanerCategory("OneTap");
                otc.allFilesRecursives.Add(new AllFilesRecursive("{steam}\\steamapps\\common\\Counter-Strike Global Offensive\\ot", "Cheats", true));
                dataBase.cleaners.Add(otc);
                #endregion
                #region INTERIUM (CS:GO, CS:1.6) (Cheat)
                CleanerCategory interium_cheat_category = new CleanerCategory("INTERIUM");
                interium_cheat_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Roaming\\INTERIUM", "Cheats", true));
                dataBase.cleaners.Add(interium_cheat_category);
                #endregion
                #region Krnl (Roblox) (Cheat)
                CleanerCategory krnl_roblox_cheat_category = new CleanerCategory("Krnl");
                krnl_roblox_cheat_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Roaming\\Krnl", "Cheats", true));
                dataBase.cleaners.Add(krnl_roblox_cheat_category);
                #endregion
                #region ExecHack
                CleanerCategory execHack = new CleanerCategory("ExecHack");
                execHack.allFilesRecursives.Add(new AllFilesRecursive("C:\\exechack", "Cheats", true));
                dataBase.cleaners.Add(execHack);
                #endregion
                #region Weave
                CleanerCategory weave = new CleanerCategory("Weave");
                weave.allFilesRecursives.Add(new AllFilesRecursive("C:\\Weave", "Cheats", true));
                dataBase.cleaners.Add(weave);
                #endregion
                #endregion
                #region Games
                #region PowerToys
                CleanerCategory voidTrain_category = new CleanerCategory("VoidTrain");
                voidTrain_category.listFiles.Add(new ListFiles("C:\\VoidTrain", "Logs", new List<string>() { "favicon.ico", "FreeTP.Org.url", "Manifest_NonUFSFiles_Win64.txt", "ReadMe - Как играть по сети.url" }));
                dataBase.cleaners.Add(voidTrain_category);
                #endregion
                #region PowerToys
                CleanerCategory powerToys_category = new CleanerCategory("PowerToys");
                powerToys_category.listFiles.Add(new ListFiles("C:\\Program Files\\PowerToys", "Logs", new List<string>() { "License.rtf", "Notice.md" }));
                dataBase.cleaners.Add(powerToys_category);
                #endregion
                #region Steam
                CleanerCategory steam_category = new CleanerCategory("Steam");
                steam_category.listFiles.Add(new ListFiles("{steam}", "Logs", new List<string>()
                {
                    "ThirdPartyLegalNotices.doc",
                    "ThirdPartyLegalNotices.html",
                    "GameOverlayRenderer.log",
                    "GameOverlayUI.exe.log",
                }));
                steam_category.allFilesRecursives.Add(new AllFilesRecursive("{steam}\\dumps", "Logs"));
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("{steam}\\resource", "*.txt", true, "Language"));
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("{steam}\\clientui\\localization", "*.json", true, "Language"));
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("{steam}\\steamui\\localization", "*.js", true, "Language"));
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("{steam}\\bin\\cef\\cef.win7\\locales", "*.pak", true, "Language"));
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("{steam}\\bin\\cef\\cef.win7x64\\locales", "*.pak", true, "Language")); ;
                steam_category.allFilesRecursives.Add(new AllFilesRecursive("{steam}\\appcache", "Cache"));
                steam_category.paternFiles.Add(new PaternFiles("{steam}\\logs", "*.txt", "Logs"));
                steam_category.paternFiles.Add(new PaternFiles("{steam}", "*.txt", "Logs"));
                steam_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Local\\Steam\\htmlcache", "Cache"));
                dataBase.cleaners.Add(steam_category);
                #endregion
                #region Black Desert
                CleanerCategory black_desert_category = new CleanerCategory("Black Desert");
                black_desert_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\Documents\\Black Desert", "Logs", new List<string>() { "debug.log", "EventLog.txt" }));
                black_desert_category.listFiles.Add(new ListFiles("�:\\Pearlabyss\\BlackDesert", "Logs", new List<string>() { "debug.log", "console.log" }));
                black_desert_category.listFiles.Add(new ListFiles("D:\\Pearlabyss\\BlackDesert", "Logs", new List<string>() { "debug.log", "console.log" }));
                dataBase.cleaners.Add(black_desert_category);
                #endregion
                #region Genshin Impact
                CleanerCategory genshin_impact_category = new CleanerCategory("Genshin Impact");
                genshin_impact_category.listFiles.Add(new ListFiles("C:\\Program Files\\Genshin Impact", "Logs", new List<string>() { "README.txt" }));
                genshin_impact_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\LocalLow\\miHoYo\\Genshin Impact", "Logs", new List<string>() { "output_log.txt", "output_log.txt.last" }));
                dataBase.cleaners.Add(genshin_impact_category);
                #endregion
                #region Counter-Strike Global Offensive
                CleanerCategory csgo = new CleanerCategory("Counter-Strike Global Offensive");
                csgo.listFiles.Add(new ListFiles("{steam}\\steamapps\\common\\Counter-Strike Global Offensive", "Logs", new List<string>()
                {
                    "csgo.signatures",
                    "system.signatures",
                    "thirdpartylegalnotices.doc",
                }));
                csgo.listFiles.Add(new ListFiles("{steam}\\steamapps\\common\\Counter-Strike Global Offensive\\game", "Logs", new List<string>()
                {
                    "thirdpartylegalnotices.txt",
                }));
                dataBase.cleaners.Add(csgo);
                #endregion
                #region Geometry Dash
                #region Original
                CleanerCategory geometryDash = new CleanerCategory("Geometry Dash");
                geometryDash.listFiles.Add(new ListFiles("C:\\Games\\Geometry Dash", "Logs", new List<string>()
                {
                    "ReadMe.txt",
                }));
                geometryDash.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\GeometryDash", "*.mp3", "Cache"));
                geometryDash.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\GeometryDash", "*.dat", "Game save"));
                geometryDash.allFilesRecursives.Add(new AllFilesRecursive("C:\\Games\\Geometry Dash\\Profile", "Game save", true));
                #endregion
                #region Private servers
                geometryDash.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\GDPS-2.2-by-user666", "*.mp3", "Cache"));
                geometryDash.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\GDPS-2.2-by-user666", "*.dat", "Game save"));
                #endregion
                dataBase.cleaners.Add(geometryDash);
                #endregion
                #region Plants Vs Zombies
                CleanerCategory plantsVsZombies = new CleanerCategory("Plants Vs Zombies");
                plantsVsZombies.allFilesRecursives.Add(new AllFilesRecursive("C:\\ProgramData\\Steam\\PlantsVsZombies\\userdata", "Game save", true));
                plantsVsZombies.allFilesRecursives.Add(new AllFilesRecursive("C:\\ProgramData\\Steam\\PlantsVsZombies\\cached\\sounds", "Cache", true));
                dataBase.cleaners.Add(plantsVsZombies);
                #endregion
                #region Terraria
                CleanerCategory terraria = new CleanerCategory("Terraria");
                terraria.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Documents\\My Games\\Terraria\\Worlds", "*", "Game save"));
                dataBase.cleaners.Add(terraria);
                #endregion
                #region Cossacks 3
                CleanerCategory cossacks3 = new CleanerCategory("Cossacks 3");
                cossacks3.listFiles.Add(new ListFiles("C:\\GOG Games\\Cossacks 3", "Logs", new List<string>()
                {
                    "EULA.txt",
                    "gog.ico",
                    "support.ico",
                    "goggame-1797227701.hashdb",
                    "goggame-1797227701.info",
                    "goggame-1797227701.script",
                    "goggame-galaxyFileList.ini",
                    "goglog.ini",
                }));
                cossacks3.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Documents\\cossacks\\profiles\\Cossack\\saves", "*.map", "Game save"));
                dataBase.cleaners.Add(cossacks3);
                #endregion
                #region The Long Drive
                CleanerCategory The_Long_Drive = new CleanerCategory("The Long Drive");
                The_Long_Drive.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Documents\\TheLongDrive\\Screenshots", "*", "Photo"));
                The_Long_Drive.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Documents\\TheLongDrive\\Saves", "*.jpg", "Photo"));
                The_Long_Drive.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Documents\\TheLongDrive\\Saves", "*.tlds", "Game save"));
                dataBase.cleaners.Add(The_Long_Drive);
                #endregion
                #region Minecraft launchers
                #region Minecraft
                CleanerCategory minecraft_category = new CleanerCategory("Minecraft");
                minecraft_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\.minecraft\\logs", "*", "Logs"));
                dataBase.cleaners.Add(minecraft_category);
                #endregion
                #region Lunar Client
                CleanerCategory lunarClient = new CleanerCategory("Lunar Client");
                lunarClient.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\.lunarclient\\logs\\launcher", "*", "Logs"));
                lunarClient.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\.lunarclient\\offline\\multiver\\logs", "*", "Logs"));
                lunarClient.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\.lunarclient\\licenses", "Logs"));

                dataBase.cleaners.Add(lunarClient);
                #endregion
                #region PrismLauncher
                CleanerCategory prismLauncher = new CleanerCategory("PrismLauncher");
                prismLauncher.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\PrismLauncher", "*.log", "Logs"));
                prismLauncher.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\Administrator\\AppData\\Roaming\\PrismLauncher\\cache", "Cache", false));
                prismLauncher.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\PrismLauncher\\logs", "*.log", "Logs"));
                prismLauncher.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\PrismLauncher\\instances\\{unknowfolder}\\.minecraft\\crash-reports", "*", "Logs"));
                prismLauncher.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\PrismLauncher\\instances\\{unknowfolder}\\.minecraft\\logs", "*", "Logs"));
                prismLauncher.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\PrismLauncher\\instances\\{unknowfolder}\\.minecraft\\screenshots", "*.png", "Photo"));
                dataBase.cleaners.Add(prismLauncher);
                #endregion
                #region PolyMC
                CleanerCategory polyMC = new CleanerCategory("PolyMC");
                polyMC.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\PolyMC", "*.log", "Logs"));
                polyMC.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\PolyMC\\instances\\{unknowfolder}\\.minecraft\\logs", "*", "Logs"));
                polyMC.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\PolyMC\\instances\\{unknowfolder}\\.minecraft\\screenshots", "*.png", "Photo"));
                dataBase.cleaners.Add(polyMC);
                #endregion
                #region ATLauncher
                CleanerCategory ATLauncher = new CleanerCategory("ATLauncher");
                ATLauncher.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\ATLauncher\\logs", "*.log", "Logs"));
                ATLauncher.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\ATLauncher\\logs\\old", "*.log", "Logs"));
                ATLauncher.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\ATLauncher\\instances\\{unknowfolder}\\logs", "*log*", "Logs"));
                ATLauncher.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\ATLauncher\\instances\\{unknowfolder}\\logs\\telemetry", "*json*", "Logs"));
                ATLauncher.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\ATLauncher\\instances\\{unknowfolder}\\logs\\crash-reports", "*.txt", "Logs"));
                ATLauncher.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\ATLauncher\\instances\\{unknowfolder}\\screenshots", "*.png", "Photo"));
                dataBase.cleaners.Add(ATLauncher);
                #endregion
                #region Cristalix
                CleanerCategory cristalixCategory = new CleanerCategory("Cristalix");
                cristalixCategory.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\.cristalix\\updates\\{unknowfolder}\\logs", "*.log", "Logs"));
                cristalixCategory.listFiles.Add(new ListFiles("C:\\Users\\{username}\\.cristalix", "Accounts", new List<string>() { ".launcher" }));
                dataBase.cleaners.Add(cristalixCategory);
                #endregion
                #region LoliLand
                CleanerCategory loliLandCategory = new CleanerCategory("LoliLand");
                loliLandCategory.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\loliland\\updates\\clients\\{unknowfolder}", "*.log", "Logs"));
                loliLandCategory.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\loliland\\updates\\clients\\{unknowfolder}\\logs", "*", "Logs"));
                loliLandCategory.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\loliland\\updates\\clients\\{unknowfolder}\\DivineRPG", "*", "Logs"));
                loliLandCategory.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\loliland\\updates\\clients\\{unknowfolder}\\screenshots", "*.png", "Photo"));
                loliLandCategory.listFiles.Add(new ListFiles("C:\\Users\\{username}\\loliland", "Accounts", new List<string>() { "auth.json" }));
                dataBase.cleaners.Add(loliLandCategory);
                #endregion
                #region AurMine
                CleanerCategory aurMineCategory = new CleanerCategory("AurMine");
                aurMineCategory.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\.AurMine\\updates\\{unknowfolder}", "*.log", "Logs"));
                aurMineCategory.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\.AurMine\\updates\\{unknowfolder}\\logs", "*", "Logs"));
                aurMineCategory.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\.AurMine\\updates\\{unknowfolder}\\screenshots", "*.png", "Photo"));
                aurMineCategory.listFiles.Add(new ListFiles("C:\\Users\\{username}\\.AurMine", "Accounts", new List<string>() { "settings.bin" }));
                dataBase.cleaners.Add(aurMineCategory);
                #endregion
                #region Excalubur Craft
                CleanerCategory excaluburCraftCategory = new CleanerCategory("Excalubur Craft");
                excaluburCraftCategory.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\.exlauncher\\clients\\{unknowfolder}\\logs", "*", "Logs"));
                excaluburCraftCategory.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\.exlauncher\\clients\\{unknowfolder}\\screenshots", "*.png", "Photo"));
                dataBase.cleaners.Add(excaluburCraftCategory);
                #endregion
                #region Minecraft Only
                CleanerCategory minecraftOnlyCategory = new CleanerCategory("Minecraft Only");
                minecraftOnlyCategory.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\.minecraftonly\\{unknowfolder}\\logs", "*", "Logs"));
                minecraftOnlyCategory.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\.minecraftonly\\{unknowfolder}\\screenshots", "*.png", "Photo"));
                dataBase.cleaners.Add(minecraftOnlyCategory);
                #endregion
                #region Feather Launcher
                CleanerCategory feather_lanuncher_category = new CleanerCategory("Feather Launcher");
                feather_lanuncher_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files\\Feather Launcher\\locales", "*.pak", true, "Language"));
                feather_lanuncher_category.listFiles.Add(new ListFiles("C:\\Program Files\\Feather Launcher", "Logs", new List<string>() { "LICENSE.electron.txt", "LICENSES.chromium.html" }));
                dataBase.cleaners.Add(feather_lanuncher_category);
                #endregion
                #region Modrinth
                CleanerCategory modrinth = new CleanerCategory("Modrinth");
                modrinth.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\ModrinthApp\\launcher_logs", "*.log", "Logs"));
                dataBase.cleaners.Add(modrinth);
                #endregion
                #region CurseForge
                CleanerCategory curse_forge_category = new CleanerCategory("CurseForge");
                curse_forge_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\curseforge\\minecraft\\Install", "Accounts", new List<string> { "launcher_accounts.json" }));
                curse_forge_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\curseforge\\minecraft\\Install", "*log*.txt", "Logs"));
                curse_forge_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\curseforge\\minecraft\\Install", "*Log*.txt", "Logs"));
                curse_forge_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\CurseForge\\logs", "*", "Logs"));
                curse_forge_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\curseforge\\minecraft\\Instances\\{unknowfolder}\\crash-reports", "*", "Logs"));
                curse_forge_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\curseforge\\minecraft\\Instances\\{unknowfolder}\\logs", "*", "Logs"));
                curse_forge_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\curseforge\\minecraft\\Instances\\{unknowfolder}\\screenshots", "*", "Photo"));
                dataBase.cleaners.Add(curse_forge_category);
                #endregion
                #endregion
                #region Roblox
                CleanerCategory roblox_category = new CleanerCategory("Roblox");
                roblox_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Pictures\\Roblox", "*.png", "Photo"));
                roblox_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\LocalLow", "*.rbx", "Logs"));
                roblox_category.paternFilesRecursives.Add(new PaternFilesRecursive("C:\\Program Files (x86)\\Roblox\\Versions", "*.txt", "Logs", false));
                dataBase.cleaners.Add(roblox_category);
                #endregion
                #region Lords Mobile
                CleanerCategory lords_mobile = new CleanerCategory("Lords Mobile");
                lords_mobile.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\LocalLow\\IGG\\Lords Mobile", "*.log", "Logs"));
                lords_mobile.paternFiles.Add(new PaternFiles("C:\\Lords Mobile PC\\Logs", "*.log", "Logs"));
                dataBase.cleaners.Add(lords_mobile);
                #endregion
                #region ArcheAge
                CleanerCategory arche_age_category = new CleanerCategory("ArcheAge");
                arche_age_category.paternFiles.Add(new PaternFiles("C:\\ArcheAge\\Documents", "*.log", "Logs"));
                arche_age_category.paternFiles.Add(new PaternFiles("C:\\ArcheAge\\Working\\-gup-", "*.log", "Logs"));
                dataBase.cleaners.Add(arche_age_category);
                #endregion

                #endregion
                ClipboardService.SetText(dataBase.ToJson());
                Console.WriteLine("Cleaner database saved to clipboard");
            }
            if (selectedi1 == 1)
            {
                Dictionary<int, SHA3FileInfo> infos = new Dictionary<int, SHA3FileInfo>();
                #region ESPdX
                SHA3FileInfo ESPdX = new SHA3FileInfo(name: "ESPdX", version: "1.0.0", author: "Avira", decription: "CS:GO Internal Cheat", category: "Cheat", game: "CS:GO", extension: "exe");
                infos.Add(0, ESPdX);

                Console.Clear();
                foreach (var tuple in infos)
                {
                    Console.WriteLine(tuple.Key + ". | " + tuple.Value.ToString());
                }
                Console.Write("Select info for copy to clipboard: ");
                string selected2 = Console.ReadLine();
                int selectedi = 0; 
                int.TryParse(selected2, out selectedi);
                foreach (var tuple in infos)
                {
                    if (tuple.Key == selectedi)
                    {
                        ClipboardService.SetText(tuple.Value.ToJson());
                        Console.WriteLine($"{tuple.Value.ToString()} json saved to clipboard, press any key to contine");
                        return;
                    }
                }
                
                Console.ReadKey();
                #endregion

                Console.WriteLine("SHA3 database done");

            }
            if (selectedi1 == 2)
            {
                Console.WriteLine("File name language database");
                FileNameLanguageDataBase fileNameLanguageDataBase = new FileNameLanguageDataBase();

                fileNameLanguageDataBase.database.Add("ru-RU", ILanguageWorker.Language.Russian_RU);
                fileNameLanguageDataBase.database.Add("Russian", ILanguageWorker.Language.Russian_RU);
                fileNameLanguageDataBase.database.Add("RvRvpnGui_ru_RU", ILanguageWorker.Language.Russian_RU);
                fileNameLanguageDataBase.database.Add("keepassxc_ru", ILanguageWorker.Language.Russian_RU);
                fileNameLanguageDataBase.database.Add("qt_ru", ILanguageWorker.Language.Russian_RU);
                fileNameLanguageDataBase.database.Add("qtbase_ru", ILanguageWorker.Language.Russian_RU);
                fileNameLanguageDataBase.database.Add("ru", ILanguageWorker.Language.Russian_RU);
                fileNameLanguageDataBase.database.Add("steamui_russian-json", ILanguageWorker.Language.Russian_RU);
                fileNameLanguageDataBase.database.Add("shared_russian-json", ILanguageWorker.Language.Russian_RU);
                fileNameLanguageDataBase.database.Add("overlay_russian", ILanguageWorker.Language.Russian_RU);
                fileNameLanguageDataBase.database.Add("platform_russian", ILanguageWorker.Language.Russian_RU);
                fileNameLanguageDataBase.database.Add("shared_russian", ILanguageWorker.Language.Russian_RU);
                fileNameLanguageDataBase.database.Add("vgui_russian", ILanguageWorker.Language.Russian_RU);

                fileNameLanguageDataBase.database.Add("eu", ILanguageWorker.Language.English_US);
                fileNameLanguageDataBase.database.Add("en-US", ILanguageWorker.Language.English_US);
                fileNameLanguageDataBase.database.Add("English", ILanguageWorker.Language.English_US);
                fileNameLanguageDataBase.database.Add("RvRvpnGui_en_US", ILanguageWorker.Language.English_US);
                fileNameLanguageDataBase.database.Add("keepassxc_en_US", ILanguageWorker.Language.English_US);
                fileNameLanguageDataBase.database.Add("qt_en", ILanguageWorker.Language.English_US);
                fileNameLanguageDataBase.database.Add("qtbase_en", ILanguageWorker.Language.English_US);
                fileNameLanguageDataBase.database.Add("steamui_english-json", ILanguageWorker.Language.English_US);
                fileNameLanguageDataBase.database.Add("shared_english-json", ILanguageWorker.Language.English_US);
                fileNameLanguageDataBase.database.Add("overlay_english", ILanguageWorker.Language.English_US);
                fileNameLanguageDataBase.database.Add("platform_english", ILanguageWorker.Language.English_US);
                fileNameLanguageDataBase.database.Add("shared_english", ILanguageWorker.Language.English_US);
                fileNameLanguageDataBase.database.Add("vgui_english", ILanguageWorker.Language.English_US);

                fileNameLanguageDataBase.database.Add("uk-UK", ILanguageWorker.Language.Ukraine_UK);
                fileNameLanguageDataBase.database.Add("Ukrainian", ILanguageWorker.Language.Ukraine_UK);
                fileNameLanguageDataBase.database.Add("RvRvpnGui_uk_UA", ILanguageWorker.Language.Ukraine_UK);
                fileNameLanguageDataBase.database.Add("keepassxc_uk", ILanguageWorker.Language.Ukraine_UK);
                fileNameLanguageDataBase.database.Add("qt_uk", ILanguageWorker.Language.Ukraine_UK);
                fileNameLanguageDataBase.database.Add("qtbase_uk", ILanguageWorker.Language.Ukraine_UK);
                fileNameLanguageDataBase.database.Add("uk", ILanguageWorker.Language.Ukraine_UK);
                fileNameLanguageDataBase.database.Add("steamui_ukrainian-json", ILanguageWorker.Language.Ukraine_UK);
                fileNameLanguageDataBase.database.Add("shared_ukrainian-json", ILanguageWorker.Language.Ukraine_UK);
                fileNameLanguageDataBase.database.Add("overlay_ukrainian", ILanguageWorker.Language.Ukraine_UK);
                fileNameLanguageDataBase.database.Add("platform_ukrainian", ILanguageWorker.Language.Ukraine_UK);
                fileNameLanguageDataBase.database.Add("shared_ukrainian", ILanguageWorker.Language.Ukraine_UK);
                fileNameLanguageDataBase.database.Add("vgui_ukrainian", ILanguageWorker.Language.Ukraine_UK);

                fileNameLanguageDataBase.database.Add("fr-FR", ILanguageWorker.Language.French_FR);
                fileNameLanguageDataBase.database.Add("French", ILanguageWorker.Language.French_FR);
                fileNameLanguageDataBase.database.Add("RvRvpnGui_fr_FR", ILanguageWorker.Language.French_FR);
                fileNameLanguageDataBase.database.Add("keepassxc_fr", ILanguageWorker.Language.French_FR);
                fileNameLanguageDataBase.database.Add("qt_fr", ILanguageWorker.Language.French_FR);
                fileNameLanguageDataBase.database.Add("qtbase_fr", ILanguageWorker.Language.French_FR);
                fileNameLanguageDataBase.database.Add("fr", ILanguageWorker.Language.French_FR);
                fileNameLanguageDataBase.database.Add("steamui_french-json", ILanguageWorker.Language.French_FR);
                fileNameLanguageDataBase.database.Add("shared_french-json", ILanguageWorker.Language.French_FR);
                fileNameLanguageDataBase.database.Add("overlay_french", ILanguageWorker.Language.French_FR);
                fileNameLanguageDataBase.database.Add("platform_french", ILanguageWorker.Language.French_FR);
                fileNameLanguageDataBase.database.Add("shared_french", ILanguageWorker.Language.French_FR);
                fileNameLanguageDataBase.database.Add("vgui_french", ILanguageWorker.Language.French_FR);

                ClipboardService.SetText(fileNameLanguageDataBase.ToJson());
                Console.WriteLine("Languages saved to clipboard");
            }
            if (selectedi1 == 3)
            {
                Console.WriteLine("Scripts database");
                {
                    ScriptsDataBase scriptsDataBase = new ScriptsDataBase();

                    scriptsDataBase.scripts.Add(new ScriptInfo
                    {
                        name = "LastActivity Cleaner",
                        icon = "M46.4375 -0.03125C46.269531 -0.0390625 46.097656 -0.0234375 45.9375 0C45.265625 0.09375 44.6875 0.421875 44.28125 1.03125L44.25 1.09375L44.21875 1.125L35.65625 17.21875C34.691406 16.859375 33.734375 16.648438 32.84375 16.625C31.882813 16.601563 30.976563 16.75 30.15625 17.09375C28.574219 17.753906 27.378906 19.046875 26.59375 20.6875C26.558594 20.738281 26.527344 20.789063 26.5 20.84375C26.496094 20.851563 26.503906 20.867188 26.5 20.875C26.488281 20.894531 26.476563 20.917969 26.46875 20.9375C26.457031 20.976563 26.445313 21.019531 26.4375 21.0625C25.894531 22.417969 25.269531 23.636719 24.5625 24.71875C24.554688 24.730469 24.539063 24.738281 24.53125 24.75C24.441406 24.828125 24.367188 24.925781 24.3125 25.03125C24.308594 25.039063 24.316406 25.054688 24.3125 25.0625C24.277344 25.113281 24.246094 25.164063 24.21875 25.21875C21.832031 28.636719 18.722656 30.695313 15.78125 31.96875C11.773438 33.703125 7.9375 33.886719 7.09375 33.8125C6.691406 33.773438 6.304688 33.976563 6.113281 34.332031C5.925781 34.6875 5.964844 35.125 6.21875 35.4375C17.613281 49.5 34.375 50 34.375 50C34.574219 50.003906 34.769531 49.949219 34.9375 49.84375C34.9375 49.84375 37.007813 48.53125 39.5 45.40625C41.371094 43.058594 43.503906 39.664063 45.34375 34.96875C45.355469 34.957031 45.363281 34.949219 45.375 34.9375C45.605469 34.722656 45.722656 34.410156 45.6875 34.09375C45.6875 34.082031 45.6875 34.074219 45.6875 34.0625C46.171875 32.753906 46.640625 31.378906 47.0625 29.875C47.078125 29.8125 47.089844 29.75 47.09375 29.6875C47.09375 29.675781 47.09375 29.667969 47.09375 29.65625C48.425781 26.21875 46.941406 22.433594 43.75 20.78125L49.9375 3.625L49.9375 3.59375L49.96875 3.5625C50.171875 2.851563 49.9375 2.167969 49.5625 1.625C49.207031 1.113281 48.6875 0.710938 48.0625 0.4375L48.0625 0.40625C48.042969 0.398438 48.019531 0.414063 48 0.40625C47.988281 0.402344 47.980469 0.378906 47.96875 0.375C47.480469 0.144531 46.945313 -0.0117188 46.4375 -0.03125 Z M 46.3125 2.0625C46.539063 2.027344 46.835938 2.027344 47.15625 2.1875L47.1875 2.21875L47.21875 2.21875C47.542969 2.347656 47.8125 2.566406 47.9375 2.75C48.0625 2.933594 48.027344 3.042969 48.03125 3.03125L41.9375 19.9375C41.203125 19.605469 40.695313 19.371094 39.65625 18.90625C38.882813 18.558594 38.148438 18.222656 37.5 17.9375L45.9375 2.15625C45.929688 2.164063 46.085938 2.097656 46.3125 2.0625 Z M 4 8C1.800781 8 0 9.800781 0 12C0 14.199219 1.800781 16 4 16C6.199219 16 8 14.199219 8 12C8 9.800781 6.199219 8 4 8 Z M 4 10C5.117188 10 6 10.882813 6 12C6 13.117188 5.117188 14 4 14C2.882813 14 2 13.117188 2 12C2 10.882813 2.882813 10 4 10 Z M 13 11C11.894531 11 11 11.894531 11 13C11 14.105469 11.894531 15 13 15C14.105469 15 15 14.105469 15 13C15 11.894531 14.105469 11 13 11 Z M 11.5 18C8.472656 18 6 20.472656 6 23.5C6 26.527344 8.472656 29 11.5 29C14.527344 29 17 26.527344 17 23.5C17 20.472656 14.527344 18 11.5 18 Z M 32.8125 18.625C33.507813 18.644531 34.269531 18.785156 35.125 19.125C35.144531 19.136719 35.167969 19.148438 35.1875 19.15625C35.414063 19.511719 35.839844 19.6875 36.25 19.59375C36.363281 19.640625 36.351563 19.636719 36.46875 19.6875C37.144531 19.980469 37.996094 20.339844 38.84375 20.71875C40.085938 21.273438 40.871094 21.613281 41.59375 21.9375C41.613281 21.960938 41.632813 21.980469 41.65625 22C41.871094 22.296875 42.230469 22.453125 42.59375 22.40625C42.605469 22.40625 42.613281 22.40625 42.625 22.40625C45.015625 23.5 46.070313 26.105469 45.25 28.625C44.855469 28.613281 44.554688 28.632813 43.8125 28.46875C43.257813 28.347656 42.71875 28.152344 42.3125 27.90625C41.90625 27.660156 41.671875 27.417969 41.5625 27.09375C41.476563 26.8125 41.269531 26.585938 40.996094 26.472656C40.726563 26.355469 40.417969 26.367188 40.15625 26.5C39.820313 26.667969 38.972656 26.605469 38.21875 26.21875C37.84375 26.027344 37.507813 25.757813 37.28125 25.53125C37.054688 25.304688 36.992188 25.089844 37 25.125C36.945313 24.832031 36.765625 24.578125 36.503906 24.433594C36.246094 24.289063 35.933594 24.269531 35.65625 24.375C35.628906 24.386719 35.296875 24.417969 34.90625 24.34375C34.515625 24.269531 34.0625 24.109375 33.625 23.90625C33.1875 23.703125 32.785156 23.457031 32.53125 23.25C32.277344 23.042969 32.253906 22.828125 32.28125 23.09375C32.214844 22.566406 31.75 22.179688 31.21875 22.21875C30.214844 22.3125 29.273438 21.574219 28.71875 21.09375C29.304688 20.105469 30.03125 19.316406 30.9375 18.9375C31.492188 18.707031 32.117188 18.605469 32.8125 18.625 Z M 11.5 20C13.445313 20 15 21.554688 15 23.5C15 25.445313 13.445313 27 11.5 27C9.554688 27 8 25.445313 8 23.5C8 21.554688 9.554688 20 11.5 20 Z M 27.8125 22.96875C28.507813 23.46875 29.472656 23.988281 30.625 24.09375C30.808594 24.363281 31.007813 24.582031 31.25 24.78125C31.683594 25.140625 32.21875 25.457031 32.78125 25.71875C33.34375 25.980469 33.933594 26.199219 34.53125 26.3125C34.839844 26.371094 35.15625 26.253906 35.46875 26.25C35.617188 26.476563 35.683594 26.777344 35.875 26.96875C36.28125 27.375 36.765625 27.71875 37.3125 28C38.125 28.417969 39.101563 28.5625 40.0625 28.4375C40.390625 28.929688 40.785156 29.34375 41.25 29.625C41.933594 30.035156 42.679688 30.285156 43.375 30.4375C43.863281 30.542969 44.308594 30.589844 44.71875 30.625C44.441406 31.523438 44.140625 32.367188 43.84375 33.1875C43.484375 33.175781 43.042969 33.15625 42.5625 33.0625C41.46875 32.851563 40.433594 32.367188 40 31.53125C39.765625 31.09375 39.246094 30.894531 38.78125 31.0625C38.285156 31.238281 37.386719 31.164063 36.625 30.8125C35.863281 30.460938 35.285156 29.851563 35.15625 29.40625C35.074219 29.136719 34.878906 28.914063 34.621094 28.796875C34.367188 28.675781 34.074219 28.671875 33.8125 28.78125C33.570313 28.882813 32.625 28.855469 31.84375 28.5C31.0625 28.144531 30.558594 27.546875 30.5 27.21875C30.449219 26.941406 30.285156 26.703125 30.046875 26.554688C29.808594 26.40625 29.519531 26.363281 29.25 26.4375C28.304688 26.691406 27.566406 26.355469 26.96875 25.90625C26.761719 25.753906 26.609375 25.585938 26.46875 25.4375C26.953125 24.667969 27.402344 23.851563 27.8125 22.96875 Z M 25.3125 27.09375C25.460938 27.230469 25.601563 27.363281 25.78125 27.5C26.519531 28.054688 27.65625 28.449219 28.9375 28.375C29.402344 29.246094 30.15625 29.914063 31.03125 30.3125C31.894531 30.707031 32.816406 30.832031 33.71875 30.71875C34.21875 31.535156 34.914063 32.226563 35.78125 32.625C36.707031 33.050781 37.746094 33.160156 38.75 33C39.683594 34.167969 41.011719 34.804688 42.1875 35.03125C42.5 35.089844 42.808594 35.128906 43.09375 35.15625C41.429688 39.175781 39.566406 42.117188 37.9375 44.15625C35.851563 46.769531 34.441406 47.757813 34.125 47.96875C33.769531 47.953125 31.164063 47.769531 27.5 46.75C27.800781 46.554688 28.125 46.351563 28.46875 46.09375C30.136719 44.84375 32.320313 42.804688 34.4375 39.65625C34.660156 39.332031 34.675781 38.910156 34.472656 38.574219C34.269531 38.234375 33.890625 38.046875 33.5 38.09375C33.207031 38.125 32.945313 38.285156 32.78125 38.53125C30.796875 41.484375 28.753906 43.375 27.25 44.5C25.820313 45.570313 24.992188 45.902344 24.90625 45.9375C22.65625 45.144531 20.164063 44.058594 17.625 42.53125C17.992188 42.410156 18.382813 42.25 18.8125 42.0625C20.710938 41.234375 23.25 39.6875 25.84375 36.78125C26.15625 36.46875 26.226563 35.988281 26.019531 35.601563C25.808594 35.210938 25.371094 35.003906 24.9375 35.09375C24.707031 35.132813 24.496094 35.257813 24.34375 35.4375C21.9375 38.128906 19.683594 39.496094 18.03125 40.21875C16.378906 40.941406 15.4375 41 15.4375 41C15.394531 41.007813 15.351563 41.019531 15.3125 41.03125C13.238281 39.570313 11.167969 37.792969 9.21875 35.65625C11.121094 35.507813 13.570313 35.121094 16.59375 33.8125C19.578125 32.519531 22.761719 30.410156 25.3125 27.09375Z",
                        version = "1.2.0",
                        description = "Cleans traces of user activity",
                        sha3 = "117942a164be149d941e559c797f5eeae8148a8987a9db66b88bd913bbf574a3",
                        url = "https://raw.githubusercontent.com/WinBooster/WinBooster_Scripts/main/scripts/LastActivity%20Cleaner.cs",
                        winbooster_version = "2.0.8.8",
                        type = "Cleaner"
                    });

                    scriptsDataBase.scripts.Add(new ScriptInfo
                    {
                        name = "Maximum electrical circuit",
                        icon = "M213.607,56.578H0v120h213.607v-32.086h19.549V88.664h-19.549V56.578z M193.607,156.578H20v-80h173.607V156.578z\r\n\t M58.804,143.035h-24V90.121h24V143.035z M98.804,143.035h-24V90.121h24V143.035z M138.804,143.035h-24V90.121h24V143.035z\r\n\t M178.804,143.035h-24V90.121h24V143.035z",
                        version = "1.0.0",
                        description = "Set maximum electro scheme",
                        sha3 = "a0f4b64a66884102242ba7226c5e405e13024aee42dc9759e1975743ca74c436",
                        url = "https://raw.githubusercontent.com/WinBooster/WinBooster_Scripts/main/scripts/Maximum%20electrical%20circuit.cs",
                        winbooster_version = "2.0.9.0",
                        type = "Tweaks"
                    });

                    scriptsDataBase.scripts.Add(new ScriptInfo
                    {
                        name = "Brave Telemetry Disabler",
                        icon = "M39.43,13.84l.92-2.24s-1.15-1.25-2.57-2.66-4.41-.6-4.41-.6L30,4.5H18L14.6,8.37s-3-.82-4.41.59-2.57,2.67-2.57,2.67l.92,2.24L7.38,17.18s3.43,13,3.82,14.55c.8,3.11,1.33,4.32,3.57,5.92s6.3,4.31,7,4.73A5.24,5.24,0,0,0,24,43.5c.74,0,1.57-.71,2.25-1.12s4.73-3.17,7-4.73S36,34.87,36.8,31.73l3.82-14.55Z M18.71,31.52a19.57,19.57,0,0,1-2.84-3.78,3.59,3.59,0,0,1,.32-3.22,1.49,1.49,0,0,0-.59-1.75c-.29-.32-2.72-2.89-3.28-3.49s-1.12-.85-1.12-2,4.37-6.44,4.37-6.44,3.7.71,4.2.71a13.77,13.77,0,0,0,2.57-.74A5.92,5.92,0,0,1,24,10.47l-1.66.33A5.92,5.92,0,0,1,24,10.47a5.92,5.92,0,0,1,1.66.33,13.77,13.77,0,0,0,2.57.74c.5,0,4.2-.71,4.2-.71s4.37,5.29,4.37,6.44-.56,1.42-1.12,2-3,3.17-3.28,3.49a1.49,1.49,0,0,0-.59,1.75,3.59,3.59,0,0,1,.32,3.22,19.57,19.57,0,0,1-2.84,3.78 M32.19,14.82a13.89,13.89,0,0,0-3.73-.09,13.45,13.45,0,0,0-2.66.86c-.11.2-.23.2-.11.94s.82,4.2.88,4.82.21,1-.47,1.19a17.94,17.94,0,0,1-2.16.41,18.31,18.31,0,0,1-2.16-.41c-.65-.15-.53-.57-.47-1.19s.74-4.08.89-4.82,0-.74-.12-.94a12.78,12.78,0,0,0-2.66-.86,12.76,12.76,0,0,0-3.73.09 M24,28.75V23 M29.14,31.08c.27.18.12.5-.14.65s-3.58,2.75-3.88,3.05-.79.74-1.12.74-.8-.48-1.12-.74S19.24,31.91,19,31.73s-.41-.5-.14-.65,1.12-.59,2.27-1.21A12.5,12.5,0,0,1,24,28.75a12.5,12.5,0,0,1,2.87,1.12Z",
                        version = "1.0.0",
                        description = "Disable brave browser telemetry",
                        sha3 = "53bc1201be74e0dfe0bbc217b6d0b9867180b22f58cbe9f5b52d5e71c31adb63",
                        url = "https://raw.githubusercontent.com/WinBooster/WinBooster_Scripts/main/scripts/Brave%20Telemetry%20Disabler.cs",
                        winbooster_version = "2.0.9.0",
                        type = "Tweaks"
                    });

                    scriptsDataBase.scripts.Add(new ScriptInfo
                    {
                        name = "Process Screen Protector",
                        icon = "M3 4V16H21V4H3M3 2H21C22.1 2 23 2.89 23 4V16C23 16.53 22.79 17.04 22.41 17.41C22.04 17.79 21.53 18 21 18H14V20H16V22H8V20H10V18H3C2.47 18 1.96 17.79 1.59 17.41C1.21 17.04 1 16.53 1 16V4C1 2.89 1.89 2 3 2M10.84 8.93C11.15 8.63 11.57 8.45 12 8.45C12.43 8.46 12.85 8.63 13.16 8.94C13.46 9.24 13.64 9.66 13.64 10.09C13.64 10.53 13.46 10.94 13.16 11.25C12.85 11.56 12.43 11.73 12 11.73C11.57 11.73 11.15 11.55 10.84 11.25C10.54 10.94 10.36 10.53 10.36 10.09C10.36 9.66 10.54 9.24 10.84 8.93M10.07 12C10.58 12.53 11.28 12.82 12 12.82C12.72 12.82 13.42 12.53 13.93 12C14.44 11.5 14.73 10.81 14.73 10.09C14.73 9.37 14.44 8.67 13.93 8.16C13.42 7.65 12.72 7.36 12 7.36C11.28 7.36 10.58 7.65 10.07 8.16C9.56 8.67 9.27 9.37 9.27 10.09C9.27 10.81 9.56 11.5 10.07 12M6 10.09C6.94 7.7 9.27 6 12 6C14.73 6 17.06 7.7 18 10.09C17.06 12.5 14.73 14.18 12 14.18C9.27 14.18 6.94 12.5 6 10.09Z",
                        version = "1.0.1",
                        description = "Makes selected processes unavailable for screenshots and recording",
                        sha3 = "91df6bce15e8d66f0ebce9a9a432e4118086468d248fff2ad5769307f782993f",
                        url = "https://raw.githubusercontent.com/WinBooster/WinBooster_Scripts/main/scripts/Process%20Screen%20Protector.cs",
                        winbooster_version = "2.0.8.9",
                        type = "Anti ScreenShare"
                    });

                    scriptsDataBase.scripts.Add(new ScriptInfo
                    {
                        name = "X7 Oscar Keyboard Editor Fixer",
                        icon = "M8.14,14.94v4.53h4.53V14.94Zm6.8,0v4.53h4.53V14.94Zm6.79,0v4.53h4.54V14.94Zm6.8,0v4.53h4.53V14.94Zm6.8,0v4.53h4.53V14.94ZM8.14,21.73v4.54h4.53V21.73Zm6.8,0v4.54h4.53V21.73Zm6.79,0v4.54h4.54V21.73Zm6.8,0v4.54h4.53V21.73Zm6.8,0v4.54h4.53V21.73ZM8.14,28.53v4.53h4.53V28.53Zm6.8,0v4.53H33.06V28.53Zm20.39,0v4.53h4.53V28.53Z M43.5,35.5v-23a2,2,0,0,0-2-2H6.5a2,2,0,0,0-2,2v23a2,2,0,0,0,2,2h35A2,2,0,0,0,43.5,35.5Z",
                        version = "1.0.1",
                        description = "Fixes saving macros",
                        sha3 = "1623a919920cc7d6c46f81620be8e12621e067de34544623663412f1af0b7d30",
                        url = "https://raw.githubusercontent.com/WinBooster/WinBooster_Scripts/refs/heads/main/scripts/Oscar%20Keyboard%20Editor%20Fixer.cs",
                        winbooster_version = "2.0.9.4",
                        type = "Error fixer"
                    });

					scriptsDataBase.scripts.Add(new ScriptInfo
                    {
                        name = "WinRar Activator",
                        icon = "M19 9C19.9319 9 20.3978 9 20.7654 9.15224C21.2554 9.35523 21.6448 9.74458 21.8478 10.2346C22 10.6022 22 11.0681 22 12C22 12.9319 22 13.3978 21.8478 13.7654C21.6448 14.2554 21.2554 14.6448 20.7654 14.8478C20.3978 15 19.9319 15 19 15M19 9C19.9319 9 20.3978 9 20.7654 8.84776C21.2554 8.64477 21.6448 8.25542 21.8478 7.76537C22 7.39782 22 6.93188 22 6C22 5.06812 22 4.60218 21.8478 4.23463C21.6448 3.74458 21.2554 3.35523 20.7654 3.15224C20.3978 3 19.9319 3 19 3H17.7321M19 9H18M19 15C19.9319 15 20.3978 15 20.7654 15.1522C21.2554 15.3552 21.6448 15.7446 21.8478 16.2346C22 16.6022 22 17.0681 22 18C22 18.9319 22 19.3978 21.8478 19.7654C21.6448 20.2554 21.2554 20.6448 20.7654 20.8478C20.3978 21 19.9319 21 19 21H17.7321M19 15H18M5 15C4.06812 15 3.60218 15 3.23463 14.8478C2.74458 14.6448 2.35523 14.2554 2.15224 13.7654C2 13.3978 2 12.9319 2 12C2 11.0681 2 10.6022 2.15224 10.2346C2.35523 9.74458 2.74458 9.35523 3.23463 9.15224C3.60218 9 5.06812 9 6 9C5.06812 9 3.60218 9 3.23463 8.84776C2.74458 8.64477 2.35523 8.25542 2.15224 7.76537C2 7.39782 2 6.93188 2 6C2 5.06812 2 4.60218 2.15224 4.23463C2.35523 3.74458 2.74458 3.35523 3.23463 3.15224C3.60218 3 4.06812 3 5 3H12.2679M5 15C4.06812 15 3.60218 15 3.23463 15.1522C2.74458 15.3552 2.35523 15.7446 2.15224 16.2346C2 16.6022 2 17.0681 2 18C2 18.9319 2 19.3978 2.15224 19.7654C2.35523 20.2554 2.74458 20.6448 3.23463 20.8478C3.60218 21 4.06812 21 5 21M5 15H12M12.2679 3C12.2245 3.07519 12.1858 3.15353 12.1522 3.23463C12 3.60218 12 4.06812 12 5V9M12.2679 3C12.4869 2.62082 12.8257 2.32164 13.2346 2.15224C13.6022 2 14.0681 2 15 2C15.9319 2 16.3978 2 16.7654 2.15224C17.1743 2.32164 17.5131 2.62082 17.7321 3M17.7321 3C17.7755 3.07519 17.8142 3.15353 17.8478 3.23463C18 3.60218 18 4.06812 18 5V9M18 9L18 15M12 9V15M12 9H10M18 15V19C18 19.9319 18 20.3978 17.8478 20.7654C17.8142 20.8465 17.7755 20.9248 17.7321 21M12 15L12 19C12 19.9319 12 20.3978 12.1522 20.7654C12.1858 20.8465 12.2245 20.9248 12.2679 21M12.2679 21C12.4869 21.3792 12.8257 21.6784 13.2346 21.8478C13.6022 22 14.0681 22 15 22C15.9319 22 16.3978 22 16.7654 21.8478C17.1743 21.6784 17.5131 21.3792 17.7321 21M12.2679 21H9 M15 11L15 13",
                        version = "1.0.0",
                        description = "Activate WinRar Program",
                        sha3 = "ed44bb2f2f3b49bf26bac74b86d9f66beb897da38b89c103b82dcec3fc3e2fd4",
                        url = "https://raw.githubusercontent.com/WinBooster/WinBooster_Scripts/refs/heads/main/scripts/WinRar%20Activator.cs",
                        winbooster_version = "2.0.9.6",
                        type = "Tweaks"
                    });

                    ClipboardService.SetText(scriptsDataBase.ToJson());
                    Console.WriteLine("Scripts saved to clipboard");
                }
            }
        }
    }
}