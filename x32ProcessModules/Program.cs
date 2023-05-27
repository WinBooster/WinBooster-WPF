using System;
using System.Diagnostics;
using System.IO.Pipes;

namespace x32ProcessModules
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                int processId = int.Parse(args[0]);
                Process process = Process.GetProcessById(processId);
                if (process != null)
                {
                    var pipeServer = new NamedPipeServerStream($"x32ProcessModulesPipi({processId})");
                    pipeServer.WaitForConnection();
                    using (StreamWriter writer = new StreamWriter(pipeServer))
                    {
                        foreach (ProcessModule module in process.Modules)
                        {
                            writer.WriteLine(module.ModuleName);
                        }
                    }

                    pipeServer.Close();
                }
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}