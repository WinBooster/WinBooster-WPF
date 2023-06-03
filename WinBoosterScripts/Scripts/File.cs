using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinBoosterScripts.Scripts
{
    public class File : Script
    {
        private string file = "";
        private string[] lines = new string[0];
        private string[] args = new string[0];
        private bool csharp;
        private Thread thread;
        private Dictionary<string, object> localVars = new Dictionary<string, object>();

        public File(string filename)
        {
            ParseArguments(filename);
        }

        private void ParseArguments(string argstr)
        {
            List<string> args = new List<string>();
            StringBuilder str = new StringBuilder();

            bool escape = false;
            bool quotes = false;

            foreach (char c in argstr)
            {
                if (escape)
                {
                    if (c != '"')
                        str.Append('\\');
                    str.Append(c);
                    escape = false;
                }
                else
                {
                    if (c == '\\')
                        escape = true;
                    else if (c == '"')
                        quotes = !quotes;
                    else if (c == ' ' && !quotes)
                    {
                        if (str.Length > 0)
                            args.Add(str.ToString());
                        str.Clear();
                    }
                    else str.Append(c);
                }
            }

            if (str.Length > 0)
                args.Add(str.ToString());

            if (args.Count > 0)
            {
                file = args[0];
                args.RemoveAt(0);
                this.args = args.ToArray();
            }
            else file = "";
        }

        public static bool LookForScript(ref string filename)
        {
            //Automatically look in subfolders and try to add ".txt" file extension
            char dir_slash = CharpManager.isUsingMono ? '/' : '\\';
            string[] files = new string[]
            {
                filename,
                filename + ".txt",
                filename + ".cs",
                "scripts" + dir_slash + filename,
                "scripts" + dir_slash + filename + ".txt",
                "scripts" + dir_slash + filename + ".cs",
                "config" + dir_slash + filename,
                "config" + dir_slash + filename + ".txt",
                "config" + dir_slash + filename + ".cs",
            };

            foreach (string possible_file in files)
            {
                if (System.IO.File.Exists(possible_file))
                {
                    filename = possible_file;
                    return true;
                }
            }

            string caller = "Script";
            try
            {
                StackFrame frame = new StackFrame(1);
                MethodBase method = frame.GetMethod();
                Type type = method.DeclaringType;
                caller = type.Name;
            }
            catch { }

            return false;
        }

        public override void Initialize()
        {
            if (LookForScript(ref file))
            {
                lines = System.IO.File.ReadAllLines(file, Encoding.UTF8);
                csharp = true;
                thread = null;

            }
            else
            {
                UnLoadScript();
            }
        }
        public override void Update()
        {
            if (csharp) //C# compiled script
            {
                //Initialize thread on first update
                if (thread == null)
                {
                    thread = new Thread(() =>
                    {
                        CharpLoader.Run(this, lines, args, localVars);
                    });
                    thread.Name = "MCC Script - " + file;
                    thread.Start();
                }

                //Unload bot once the thread has finished running
                if (thread != null && !thread.IsAlive)
                {
                    UnLoadScript();
                }
            }
        }
    }
}
