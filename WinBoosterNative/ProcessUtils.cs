using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WinBoosterNative.winapi;

namespace WinBoosterNative
{
    public class ProcessUtils
    {
        private System.Diagnostics.Process process(string command, bool redirect = false)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo = info(command, redirect);
            return process;
        }
        private ProcessStartInfo info(string command, bool redirect)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + command;
            startInfo.RedirectStandardOutput = redirect;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            return startInfo;
        }
        public List<string> StartCmd(string command)
        {
            List<string> read = new List<string>();
            if (!string.IsNullOrEmpty(command))
            {
                System.Diagnostics.Process started = process(command, true);
                started.Start();
                using (StreamReader sr = started.StandardOutput)
                {
                    while (!sr.EndOfStream)
                    {
                        read.Add(sr.ReadLine());
                    }
                }
            }
            return read;
        }
    }
}
