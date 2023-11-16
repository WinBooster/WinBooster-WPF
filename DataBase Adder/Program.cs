using System.Diagnostics;
using System.Globalization;
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
            Console.WriteLine("1. Cleaner");
            Console.WriteLine("2. SHA3");
            Console.WriteLine("3. Language");
            Console.WriteLine("4. Scripts");
            Console.Write("Select database: ");
            string selected = Console.ReadLine();
            if (selected == "1")
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
                java_category.listFiles.Add(new ListFiles("C:\\Program Files\\Java\\{unknowfolder}", "Logs", new List<string>() { "COPYRIGHT", "LICENSE", "release", "README" }));
                java_category.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\Java\\{unknowfolder}", "Logs", new List<string>() { "COPYRIGHT", "LICENSE", "release", "README" }));
                java_category.listFiles.Add(new ListFiles("C:\\Program Files\\Eclipse Adoptium\\jdk-8.0.362.9-hotspot", "Cache", new List<string>() { "src.zip", }));
                java_category.listFolders.Add(new ListFolders("C:\\Program Files\\Eclipse Adoptium\\jdk-8.0.362.9-hotspot", "Cache", new List<string>() { "sample", }));
                dataBase.cleaners.Add(java_category);
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
                #region IDA Pro
                CleanerCategory ida_pro_category = new CleanerCategory("IDA Pro");
                ida_pro_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\Hex-Rays\\IDA Pro", "*.lst", "Cache"));
                dataBase.cleaners.Add(ida_pro_category);
                #endregion
                #region HandBrake
                CleanerCategory handbrake_category = new CleanerCategory("HandBrake");
                handbrake_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\HandBrake\\logs", "*.txt", "Logs"));
                handbrake_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Program Files\\HandBrake\\doc", "Logs", true));
                dataBase.cleaners.Add(handbrake_category);
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
                #region Exodus
                CleanerCategory exodus_category = new CleanerCategory("Exodus Crypto Wallet");
                exodus_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Local\\exodus\\app-23.2.27\\locales", "*.pak", true, "Language"));
                exodus_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Local\\exodus\\app-23.3.13\\locales", "*.pak", true, "Language"));
                dataBase.cleaners.Add(exodus_category);
                #endregion
                #region Wasabi Wallet
                CleanerCategory wasabi_wallet = new CleanerCategory("Wasabi Wallet");
                wasabi_wallet.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\WalletWasabi\\Client", "*.txt", "Logs"));
                dataBase.cleaners.Add(wasabi_wallet);
                #endregion
                #region Discord
                CleanerCategory discord_category = new CleanerCategory("Discord");
                discord_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Local\\Discord\\app-1.0.9013\\locales", "*.pak", true, "Language"));
                discord_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\Discord", "*.log", "Logs"));
                dataBase.cleaners.Add(discord_category);
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
                #region Radmin VPN
                CleanerCategory radmin_vpn_caregory = new CleanerCategory("Radmin VPN");
                radmin_vpn_caregory.listFiles.Add(new ListFiles("C:\\Program Files (x86)\\Radmin VPN\\CHATLOGS", "Logs", new List<string>() { "info.txt" }));
                radmin_vpn_caregory.paternFiles.Add(new PaternFiles("C:\\ProgramData\\Famatech\\Radmin VPN", "*.txt", "Logs"));
                radmin_vpn_caregory.paternFiles.Add(new PaternFiles("C:\\Program Files (x86)\\Radmin VPN", "*.lng_rad", "Cache"));
                radmin_vpn_caregory.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files (x86)\\Radmin VPN", "*.qm", true, "Language"));
                dataBase.cleaners.Add(radmin_vpn_caregory);
                #endregion
                #region GameGuard
                CleanerCategory gameGuard_category = new CleanerCategory("GameGuard");
                gameGuard_category.paternFiles.Add(new PaternFiles("C:\\Program Files (x86)\\GameGuard\\cache", "*.cache", "Cache"));
                dataBase.cleaners.Add(gameGuard_category);
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
                #region Feather Launcher
                CleanerCategory feather_lanuncher_category = new CleanerCategory("Feather Launcher");
                feather_lanuncher_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files\\Feather Launcher\\locales", "*.pak", true, "Language"));
                feather_lanuncher_category.listFiles.Add(new ListFiles("C:\\Program Files\\Feather Launcher", "Logs", new List<string>() { "LICENSE.electron.txt", "LICENSES.chromium.html" }));
                dataBase.cleaners.Add(feather_lanuncher_category);
                #endregion
                #region CCleaner
                CleanerCategory ccLeaner_category = new CleanerCategory("CCleaner");
                ccLeaner_category.paternFiles.Add(new PaternFiles("C:\\Program Files\\CCleaner\\LOG", "*", "Logs"));
                dataBase.cleaners.Add(ccLeaner_category);
                #endregion
                #region FACEIT AC
                CleanerCategory faceit_ac_category = new CleanerCategory("FACEIT AC");
                faceit_ac_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Local\\FACEIT\\app-1.31.12\\locales", "*.pak", true, "Language"));
                faceit_ac_category.paternFiles.Add(new PaternFiles("C:\\Program Files\\FACEIT AC\\logs", "*.log", "Logs"));
                dataBase.cleaners.Add(faceit_ac_category);
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
                #region iTop VPN
                CleanerCategory iTop_vpn_category = new CleanerCategory("iTop VPN");
                iTop_vpn_category.listFiles.Add(new ListFiles("C:\\ProgramData\\iTop VPN", "Logs", new List<string>() { "iTop_setup.log", "Setup.log" }));
                dataBase.cleaners.Add(iTop_vpn_category);
                #endregion
                #region Process Lasso
                CleanerCategory process_lasso_category = new CleanerCategory("Process Lasso");
                process_lasso_category.paternFiles.Add(new PaternFiles("C:\\ProgramData\\ProcessLasso\\logs", "*", "Logs"));
                dataBase.cleaners.Add(process_lasso_category);
                #endregion
                #region ShareX
                CleanerCategory shareX_category = new CleanerCategory("ShareX");
                shareX_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Documents\\ShareX\\Logs", "*.log", "Logs"));
                shareX_category.paternFilesRecursives.Add(new PaternFilesRecursive("C:\\Users\\{username}\\Documents\\ShareX\\Screenshots", "*.png", "Photo", true));
                dataBase.cleaners.Add(shareX_category);
                #endregion
                #region EasyAntiCheat
                CleanerCategory easyAnticheat_category = new CleanerCategory("EasyAntiCheat");
                easyAnticheat_category.paternFilesRecursives.Add(new PaternFilesRecursive("C:\\Users\\{username}\\AppData\\Roaming\\EasyAntiCheat", "*.log", "Logs", false));
                dataBase.cleaners.Add(easyAnticheat_category);
                #endregion
                #region Roblox
                CleanerCategory roblox_category = new CleanerCategory("Roblox");
                roblox_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Pictures\\Roblox", "*.png", "Photo"));
                roblox_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\LocalLow", "*.rbx", "Logs"));
                roblox_category.paternFilesRecursives.Add(new PaternFilesRecursive("C:\\Program Files (x86)\\Roblox\\Versions", "*.txt", "Logs", false));
                dataBase.cleaners.Add(roblox_category);
                #endregion
                #region Krnl (Roblox) (Cheat)
                CleanerCategory krnl_roblox_cheat_category = new CleanerCategory("Krnl");
                krnl_roblox_cheat_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Roaming\\Krnl", "Cheats", true));
                dataBase.cleaners.Add(krnl_roblox_cheat_category);
                #endregion
                #region INTERIUM (CS:GO, CS:1.6) (Cheat)
                CleanerCategory interium_cheat_category = new CleanerCategory("INTERIUM");
                interium_cheat_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Roaming\\INTERIUM", "Cheats", true));
                dataBase.cleaners.Add(interium_cheat_category);
                #endregion
                #region Steam
                CleanerCategory steam_category = new CleanerCategory("Steam");
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files (x86)\\Steam\\resource", "*.txt", true, "Language"));
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files (x86)\\Steam\\clientui\\localization", "*.json", true, "Language"));
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files (x86)\\Steam\\steamui\\localization", "*.js", true, "Language"));
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files (x86)\\Steam\\bin\\cef\\cef.win7\\locales", "*.pak", true, "Language"));
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files (x86)\\Steam\\bin\\cef\\cef.win7x64\\locales", "*.pak", true, "Language"));
                // D
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("D:\\Program Files (x86)\\Steam\\resource", "*.txt", true, "Language"));
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("D:\\Program Files (x86)\\Steam\\clientui\\localization", "*.json", true, "Language"));
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("D:\\Program Files (x86)\\Steam\\steamui\\localization", "*.js", true, "Language"));
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("D:\\Program Files (x86)\\Steam\\bin\\cef\\cef.win7\\locales", "*.pak", true, "Language"));
                steam_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("D:\\Program Files (x86)\\Steam\\bin\\cef\\cef.win7x64\\locales", "*.pak", true, "Language"));

                steam_category.paternFiles.Add(new PaternFiles("C:\\Program Files (x86)\\Steam\\logs", "*.txt", "Logs"));
                steam_category.paternFiles.Add(new PaternFiles("D:\\Program Files (x86)\\Steam\\logs", "*.txt", "Logs"));
                steam_category.paternFiles.Add(new PaternFiles("C:\\Program Files (x86)\\Common Files\\Steam", "*.txt", "Logs"));
                steam_category.allFilesRecursives.Add(new AllFilesRecursive("C:\\Users\\{username}\\AppData\\Local\\Steam\\htmlcache", "Cache"));
                dataBase.cleaners.Add(steam_category);
                #endregion
                #region Black Desert
                CleanerCategory black_desert_category = new CleanerCategory("Black Desert");
                black_desert_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\Documents\\Black Desert", "Logs", new List<string>() { "debug.log", "EventLog.txt" }));
                black_desert_category.listFiles.Add(new ListFiles("Ñ:\\Pearlabyss\\BlackDesert", "Logs", new List<string>() { "debug.log", "console.log" }));
                black_desert_category.listFiles.Add(new ListFiles("D:\\Pearlabyss\\BlackDesert", "Logs", new List<string>() { "debug.log", "console.log" }));
                dataBase.cleaners.Add(black_desert_category);
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
                #region PowerToys
                CleanerCategory powerToys_category = new CleanerCategory("PowerToys");
                powerToys_category.listFiles.Add(new ListFiles("C:\\Program Files\\PowerToys", "Logs", new List<string>() { "License.rtf", "Notice.md" }));
                dataBase.cleaners.Add(powerToys_category);
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
                #region InkSpace
                CleanerCategory inkSpace_category = new CleanerCategory("InkSpace");
                inkSpace_category.listFiles.Add(new ListFiles("C:\\Program Files\\Inkscape", "Logs", new List<string>() { "NEWS.md", "README.md" }));
                inkSpace_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\Roaming\\inkscape", "Logs", new List<string>() { "extension-errors.log" }));
                dataBase.cleaners.Add(inkSpace_category);
                #endregion
                #region Genshin Impact
                CleanerCategory genshin_impact_category = new CleanerCategory("Genshin Impact");
                genshin_impact_category.listFiles.Add(new ListFiles("C:\\Program Files\\Genshin Impact", "Logs", new List<string>() { "README.txt" }));
                genshin_impact_category.listFiles.Add(new ListFiles("C:\\Users\\{username}\\AppData\\LocalLow\\miHoYo\\Genshin Impact", "Logs", new List<string>() { "output_log.txt", "output_log.txt.last" }));
                dataBase.cleaners.Add(genshin_impact_category);
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
                #region YouTube Music Desktop
                CleanerCategory youtube_music_category = new CleanerCategory("YouTube Music Desktop");
                youtube_music_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Users\\{username}\\AppData\\Local\\Programs\\youtube-music-desktop-app\\locales", "*.pak", true, "Language"));
                dataBase.cleaners.Add(youtube_music_category);
                #endregion
                #region GameLoop
                CleanerCategory gameloop_category = new CleanerCategory("Gameloop");
                gameloop_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("C:\\Program Files\\TxGameAssistant\\AppMarket\\locale", "*.pak", true, "Language"));
                gameloop_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("D:\\Program Files\\TxGameAssistant\\AppMarket\\locale", "*.pak", true, "Language"));
                gameloop_category.filesIsNotLanguageByPatern.Add(new FilesIfCurrentLanguageByPatern("E:\\Program Files\\TxGameAssistant\\AppMarket\\locale", "*.pak", true, "Language"));
                dataBase.cleaners.Add(gameloop_category);
                #endregion
                #region ExecHack
                CleanerCategory execHack = new CleanerCategory("ExecHack");
                execHack.allFilesRecursives.Add(new AllFilesRecursive("C:\\exechack", "Cheats", true));
                dataBase.cleaners.Add(execHack);
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
                #region Minecraft
                CleanerCategory minecraft_category = new CleanerCategory("Minecraft");
                minecraft_category.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\.minecraft\\logs", "*", "Logs"));
                dataBase.cleaners.Add(minecraft_category);
                #endregion
                #region The Long Drive
                CleanerCategory The_Long_Drive = new CleanerCategory("The Long Drive");
                The_Long_Drive.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Documents\\TheLongDrive\\Screenshots", "*", "Photo"));
                The_Long_Drive.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Documents\\TheLongDrive\\Saves", "*.jpg", "Photo"));
                The_Long_Drive.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Documents\\TheLongDrive\\Saves", "*.tlds", "Game save"));
                dataBase.cleaners.Add(The_Long_Drive);
                #endregion
                #region Terraria
                CleanerCategory terraria = new CleanerCategory("Terraria");
                terraria.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\Documents\\My Games\\Terraria\\Worlds", "*", "Game save"));
                dataBase.cleaners.Add(terraria);
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
                #region Magpie
                CleanerCategory magpie = new CleanerCategory("Magpie");

                magpie.paternFiles.Add(new PaternFiles("C:\\Program Files\\Magpie\\logs", "*.log", "Logs"));
                magpie.paternFiles.Add(new PaternFiles("C:\\Program Files\\Magpie\\cache", "*", "Cache"));
                dataBase.cleaners.Add(magpie);
                #endregion
                #region 7-Zip
                CleanerCategory sevenZip = new CleanerCategory("7-Zip");

                sevenZip.paternFiles.Add(new PaternFiles("C:\\Program Files\\7-Zip", "*.txt", "Logs"));
                sevenZip.foldersIsNotLanguageByPatern.Add(new FoldersIfCurrentLanguageByPatern("C:\\Program Files\\7-Zip\\Lang", "*.txt", false, "Language"));
                dataBase.cleaners.Add(sevenZip);
                #endregion
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
                #region Tribler
                CleanerCategory tribler = new CleanerCategory("Tribler");
                tribler.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\.Tribler", "*.log", "Logs"));
                dataBase.cleaners.Add(tribler);
                #endregion
                #region Notepad++
                CleanerCategory notepad_plus_plus = new CleanerCategory("Notepad++");
                notepad_plus_plus.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Roaming\\Notepad++", "*.log", "Logs"));
                dataBase.cleaners.Add(notepad_plus_plus);
                #endregion
                #region Element (Matrix)
                CleanerCategory element = new CleanerCategory("Element");
                element.paternFiles.Add(new PaternFiles("C:\\Users\\{username}\\AppData\\Local\\element-desktop", "*.log", "Logs"));
                dataBase.cleaners.Add(element);
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
                #region AAF Optimus DCH Audio
                CleanerCategory AAF = new CleanerCategory("AAF Optimus DCH Audio");
                AAF.listFiles.Add(new ListFiles("C:\\Program Files\\AAFTweak", "Logs", new List<string>()
                {
                    "RT.pdb"
                }));
                dataBase.cleaners.Add(AAF);
                #endregion
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
                Console.WriteLine("Cleaner database");
                ClipboardService.SetText(dataBase.ToJson());
            }
            if (selected == "2")
            {
                Console.WriteLine("SHA3 database");
                SHA3DataBase sha3DataBase = new SHA3DataBase();
                SHA3FileInfo ESPdX = new SHA3FileInfo("ESPdX", SHA3FileInfo.Category.Cheat);
                ESPdX.game = "CS:GO";
                sha3DataBase.database.Add("df2451fa916f786335b3f9383244e6cc93de5ffe6ef5063c2cc920cb44ac9856", ESPdX);
                ClipboardService.SetText(sha3DataBase.ToJson());

            }
            if (selected == "3")
            {
                Console.WriteLine("File name language database");
                {
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
                }
            }
            if (selected == "4")
            {
                Console.WriteLine("Scripts database");
                {
                    ScriptsDataBase scriptsDataBase = new ScriptsDataBase();

                    scriptsDataBase.scripts.Add(new ScriptInfo
                    {
                        name = "LastActivity Cleaner",
                        icon = "M9.99754 2.70682C10.0398 3.11887 9.73995 3.48714 9.32789 3.52936C6.05111 3.8651 3.49438 6.63471 3.49438 10.0005C3.49438 13.5933 6.40694 16.5059 9.99977 16.5059C13.3652 16.5059 16.1346 13.9498 16.4708 10.6735C16.5131 10.2615 16.8814 9.96171 17.2935 10.004C17.7055 10.0463 18.0053 10.4146 17.963 10.8267C17.549 14.8601 14.1422 18.0059 9.99977 18.0059C5.57852 18.0059 1.99438 14.4218 1.99438 10.0005C1.99438 5.85759 5.14088 2.45051 9.175 2.03717C9.58706 1.99495 9.95532 2.29476 9.99754 2.70682Z M11.0178 2.6409C11.1143 2.23806 11.519 1.98966 11.9218 2.08608C12.3006 2.17674 12.6691 2.29397 13.0251 2.43568C13.41 2.58885 13.5978 3.025 13.4446 3.40986C13.2915 3.79471 12.8553 3.98253 12.4705 3.82936C12.1807 3.71404 11.8809 3.61865 11.5727 3.54488C11.1698 3.44846 10.9214 3.04374 11.0178 2.6409Z M17.5639 6.97359C17.4107 6.58876 16.9745 6.40103 16.5897 6.55427C16.2049 6.70751 16.0171 7.14369 16.1704 7.52852C16.2858 7.81839 16.3813 8.11835 16.4551 8.42673C16.5516 8.82956 16.9563 9.07792 17.3591 8.98146C17.762 8.885 18.0103 8.48025 17.9139 8.07742C17.8231 7.69845 17.7058 7.3298 17.5639 6.97359Z M14.2902 3.92629C14.562 3.6138 15.0358 3.58089 15.3483 3.85278C15.6554 4.12001 15.9422 4.41 16.2061 4.72013C16.4745 5.0356 16.4364 5.50894 16.1209 5.77736C15.8054 6.04578 15.3321 6.00764 15.0637 5.69217C14.8484 5.43911 14.6143 5.20247 14.3637 4.9844C14.0512 4.71251 14.0183 4.23878 14.2902 3.92629Z M10 5.75024C10 5.33603 9.66421 5.00024 9.25 5.00024C8.83579 5.00024 8.5 5.33603 8.5 5.75024V10.7502C8.5 11.1645 8.83579 11.5002 9.25 11.5002H12.25C12.6642 11.5002 13 11.1645 13 10.7502C13 10.336 12.6642 10.0002 12.25 10.0002H10V5.75024Z",
                        version = "1.2.0",
                        description = "Cleans traces of user activity",
                        sha3 = "2dcd57246d8dab06e7885e9e35d71cfdd873952fc85751e2b688f013645c699c",
                        url = "https://github.com/WinBooster/WinBooster_Scripts/raw/main/scripts/LastActivity.cs",
                    });

                    ClipboardService.SetText(scriptsDataBase.ToJson());
                }
            }
        }
    }
}