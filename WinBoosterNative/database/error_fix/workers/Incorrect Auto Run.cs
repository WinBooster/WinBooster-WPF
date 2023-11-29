using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace WinBoosterNative.database.error_fix.workers
{
    public class Incorrect_Auto_Run : IErrorFixerWorker
    {
        public string GetName()
        {
            return "Incorrect Auto Run";
        }

        public bool IsAvalible()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                const string pathRegistryKeyStartup = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

                using (RegistryKey? registryKeyStartup = Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup, true))
                {
                    if (registryKeyStartup != null)
                    {
                        string[] names = registryKeyStartup.GetValueNames();
                        foreach (string name in names)
                        {
                            object? value = registryKeyStartup.GetValue(name);
                            if (value != null)
                            {
                                string? regString = value.ToString();
                                if (regString != null)
                                {
                                    regString = regString.Replace("\"", "");

                                    regString = regString.ToLower();

                                    int splitIndex = regString.IndexOf(".exe");

                                    if (splitIndex != -1)
                                    {
                                        splitIndex += ".exe".Length;

                                        string executable = regString.Substring(0, splitIndex);
                                        if (!File.Exists(executable))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public bool TryFix()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                const string pathRegistryKeyStartup = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

                using (RegistryKey? registryKeyStartup = Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup, true))
                {
                    if (registryKeyStartup != null)
                    {
                        string[] names = registryKeyStartup.GetValueNames();
                        bool isFixed = false;
                        foreach (string name in names)
                        {
                            object? value = registryKeyStartup.GetValue(name);
                            if (value != null)
                            {
                                string? regString = value.ToString();
                                if (regString != null)
                                {
                                    regString = regString.Replace("\"", "");

                                    regString = regString.ToLower();

                                    int splitIndex = regString.IndexOf(".exe");

                                    if (splitIndex != -1)
                                    {
                                        splitIndex += ".exe".Length;

                                        string executable = regString.Substring(0, splitIndex);
                                        if (!File.Exists(executable))
                                        {
                                            registryKeyStartup.DeleteValue(name);
                                            isFixed = true;
                                        }
                                    }
                                }
                            }
                        }
                        return isFixed;
                    }
                }
            }
            return false;
        }
    }
}
