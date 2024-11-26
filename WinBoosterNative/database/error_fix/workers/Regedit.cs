using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace WinBoosterNative.database.error_fix.workers
{
    public class Regedit : IErrorFixerWorker
    {
        public string GetName()
        {
            return "No Access To Regedit";
        }

        public bool IsAvalible()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                RegistryKey? objRegistryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                if (objRegistryKey != null)
                    if (objRegistryKey.GetValue("DisableRegistryTools") != null)
                        return true;
            }

            return false;
        }

        public bool TryFix()
        {
            bool done = IsAvalible();
            bool disabled = SetRegeditManager(true);

            return done == disabled;
        }

        public bool SetRegeditManager(bool enable)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                if (enable && objRegistryKey.GetValue("DisableTaskMgr") != null)
                    objRegistryKey.DeleteValue("DisableRegistryTools");
                else
                    objRegistryKey.SetValue("DisableRegistryTools", "1");
                objRegistryKey.Close();

                return true;
            }
            return false;
        }
    }
}
