using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ChangerWorker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        // Constants
        private const int RT_ICON = 3;
        private const int RT_GROUP_ICON = 14;

        // WinAPI functions
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr BeginUpdateResource(string pFileName, bool bDeleteExistingResources);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool UpdateResource(IntPtr hUpdate, IntPtr lpType, IntPtr lpName, ushort wLanguage, byte[] lpData, uint cbData);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

        // Main method to change the icon
        public static bool ChangeIcon(string exePath, string iconPath)
        {
            // Load the icon from file
            byte[] iconData = System.IO.File.ReadAllBytes(iconPath);

            // Begin updating the resource
            IntPtr hUpdate = BeginUpdateResource(exePath, false);
            if (hUpdate == IntPtr.Zero)
            {
                Debug.WriteLine("Failed to begin resource update.");
                return false;
            }

            // Update the icon resource
            bool success = UpdateResource(hUpdate, (IntPtr)RT_ICON, (IntPtr)1, 0, iconData, (uint)iconData.Length);
            if (!success)
            {
                Debug.WriteLine("Failed to update icon resource.");
                EndUpdateResource(hUpdate, true);
                return false;
            }

            // Update the group icon resource
            success = UpdateResource(hUpdate, (IntPtr)RT_GROUP_ICON, (IntPtr)1, 0, iconData, (uint)iconData.Length);
            if (!success)
            {
                Debug.WriteLine("Failed to update group icon resource.");
                EndUpdateResource(hUpdate, true);
                return false;
            }

            // End the resource update
            success = EndUpdateResource(hUpdate, false);
            if (!success)
            {
                Debug.WriteLine("Failed to end resource update.");
                return false;
            }

            Debug.WriteLine("Icon changed successfully.");
            return true;
        }
        public App()
        {
            string exePath = @"C:\Users\NekiPlay\Desktop\WinBooster.exe";
            string iconPath = @"C:\Users\NekiPlay\Downloads\russia.ico";

            bool success = ChangeIcon(exePath, iconPath);
            if (success)
            {
                Debug.WriteLine("Icon changed successfully.");
            }
            else
            {
                Debug.WriteLine("Failed to change the icon.");
            }
        }
    }
    
}
