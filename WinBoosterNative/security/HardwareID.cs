using Microsoft.Win32;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace WinBoosterNative.security
{
    public class HardwareID
    {
        public static string? GetHardwareID()
        {
            string? hardwareID = "";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    // Open the registry key that contains the Unique ID value
                    RegistryKey? registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", false);

                    if (registryKey != null)
                    {
                        var hardwareGuild = registryKey.GetValue("MachineGuid");
                        if (hardwareGuild != null)
                        {
                            hardwareID = hardwareGuild.ToString();
                        }
                        registryKey.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            
            return hardwareID;
        }
    }
}
