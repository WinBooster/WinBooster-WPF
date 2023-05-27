using Microsoft.Win32;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace WinBoosterNative.security
{
    public class HardwareID
    {
        public static string GetHardwareID()
        {
            string hardwareID = "";

            try
            {
                // Open the registry key that contains the Unique ID value
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Cryptography", false);

                if (registryKey != null)
                {
                    // Retrieve the Unique ID value from the registry
                    hardwareID = registryKey.GetValue("MachineGuid").ToString();
                    registryKey.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            
            return hardwareID;
        }
    }
}
