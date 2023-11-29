using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace WinBoosterNative.database.error_fix.workers
{
    public class No_Context_Menu : IErrorFixerWorker
    {
        public string GetName()
        {
            return "No Context Menu";
        }

        public bool IsAvalible()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
                try
                {
                    RegistryKey? regkey = Registry.CurrentUser.OpenSubKey(subKey, true);
                    if (regkey != null)
                    {
                        object? value = regkey.GetValue("NoViewContextMenu");
                        if (value != null && value.GetType() == typeof(int))
                        {
                            int valueInt = (int)value;
                            if (valueInt > 0)
                            {
                                return true;
                            }
                        }
                        regkey.Close();
                    }
                }
                catch { }
            }
            return false;
        }

        public bool TryFix()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer";
                try
                {
                    RegistryKey? regkey = Registry.CurrentUser.OpenSubKey(subKey, true);
                    if (regkey != null)
                    {
                        regkey.DeleteValue("NoViewContextMenu");
                        regkey.Close();
                        return true;
                    }
                }
                catch { }
            }
            return false;
        }
    }
}
