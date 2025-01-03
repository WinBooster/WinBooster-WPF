﻿using System.Diagnostics;
using System.Text;

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
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.StandardOutputEncoding = Encoding.UTF8;
            startInfo.StandardInputEncoding = Encoding.UTF8;


            return startInfo;
        }
        public List<string> StartCmd(string command)
        {
            List<string> read = new List<string>();
            if (!string.IsNullOrEmpty(command))
            {
                System.Diagnostics.Process started = process(command, true);
                started.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    string? data = e.Data;
                    if (!String.IsNullOrEmpty(data))
                    {
                        read.Add(data);
                    }
                });
                started.Start();
            }
            return read;
        }
    }
}
