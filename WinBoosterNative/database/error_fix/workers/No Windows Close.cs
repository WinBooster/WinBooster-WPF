using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace WinBoosterNative.database.error_fix.workers
{
    public class No_Windows_Close : IErrorFixerWorker
    {
        public string GetName()
        {
            return "No Windows Close";
        }

        public bool IsAvalible()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
                try
                {
                    RegistryKey?  regkey = Registry.CurrentUser.OpenSubKey(subKey, true);
                    if (regkey != null)
                    {
                        object? value = regkey.GetValue("DisableRegistryTools");
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
                string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
                try
                {
                    RegistryKey? regkey = Registry.CurrentUser.OpenSubKey(subKey, true);
                    if (regkey != null)
                    {
                        regkey.DeleteValue("DisableRegistryTools");
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
