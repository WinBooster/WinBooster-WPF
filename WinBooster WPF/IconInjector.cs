using WinBoosterNative;

namespace WinBooster_WPF
{
    public static class IconInjector
    {
        public static void Change(string file, string icon, int delay = 2)
        {
            ProcessUtils processUtils = new ProcessUtils();
            processUtils.StartCmd($"echo \"{file}\" & TIMEOUT /T {delay} & \"C:\\Program Files\\WinBooster\\IconInjector.exe\" \"{file}\" \"{icon}\"");
        }
        public static void Change(string file, string icon, string delete, int delay = 2)
        {
            ProcessUtils processUtils = new ProcessUtils();
            processUtils.StartCmd($"echo \"{file}\" & TIMEOUT /T {delay} & \"C:\\Program Files\\WinBooster\\IconInjector.exe\" \"{file}\" \"{icon}\" & del /f \"{delete}\"");
        }
    }
}
