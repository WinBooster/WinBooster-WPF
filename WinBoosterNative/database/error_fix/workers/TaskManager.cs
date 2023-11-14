using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinBoosterNative.data;

namespace WinBoosterNative.database.error_fix.workers
{
    public class TaskManager : IErrorFixerWorker
    {
        public string GetName()
        {
            return "Task Manager";
        }

        public bool IsAvalible()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                if (objRegistryKey.GetValue("DisableTaskMgr") == null)
                    return true;
            }

            return false;
        }

        public bool TryFix()
        {
            bool done = IsAvalible();
            bool disabled = SetTaskManager(true);

            return done == disabled;
        }

        public bool SetTaskManager(bool enable)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(
                @"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                if (enable && objRegistryKey.GetValue("DisableTaskMgr") != null)
                    objRegistryKey.DeleteValue("DisableTaskMgr");
                else
                    objRegistryKey.SetValue("DisableTaskMgr", "1");
                objRegistryKey.Close();

                return true;
            }
            return false;
        }
    }
}
