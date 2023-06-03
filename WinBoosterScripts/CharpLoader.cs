using System.Reflection;

namespace WinBoosterScripts
{
    public class CharpLoader
    {
        private static readonly Dictionary<ulong, Assembly> CompileCache = new Dictionary<ulong, Assembly>();
        public static object Run(Script apiHandler, string[] lines, string[] args, Dictionary<string, object> localVars, bool run = true)
        {
            //Script hash for determining if it was previously compiled
            ulong scriptHash = QuickHash(lines);
            Assembly assembly = null;

            //No need to compile two scripts at the same time
            lock (CompileCache)
            {
                ///Process and compile script only if not already compiled
                if (!CompileCache.ContainsKey(scriptHash))
                {
                    //Process different sections of the script file
                    bool scriptMain = true;
                    List<string> script = new List<string>();
                    List<string> extensions = new List<string>();
                    List<string> libs = new List<string>();
                    List<string> dlls = new List<string>();
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("//using"))
                        {
                            libs.Add(line.Replace("//", "").Trim());
                        }
                        else if (line.StartsWith("//dll"))
                        {
                            dlls.Add(line.Replace("//dll ", "").Trim());
                        }
                        else if (line.StartsWith("//Script"))
                        {
                            if (line.EndsWith("Extensions ") || line.EndsWith("Extensions"))
                                scriptMain = false;
                        }
                        else if (scriptMain)
                            script.Add(line);
                        else extensions.Add(line);
                    }
                    //Console.WriteLine(String.Join("\n", extensions));
                    //Add return statement if missing
                    if (script.All(line => !line.StartsWith("return ") && !line.Contains(" return ")))
                        script.Add("return null;");

                    //Generate a class from the given script
                    string code = String.Join("\n", new string[]
                    {
                        "using System;",
                        "using System.Collections.Generic;",
                        "using System.Text.RegularExpressions;",
                        "using System.Linq;",
                        "using System.Text;",
                        "using System.IO;",
                        "using System.Net;",
                        "using System.Threading;",
                        "using WinBoosterCharpScripts;",
                        String.Join("\n", libs),
                        "namespace ScriptLoader {",
                        "public class CharpScript {",
                        "public CSharpAPI MCC;",
                        "public object __run(CSharpAPI __apiHandler, string[] args) {",
                            "this.MCC = __apiHandler;",
                            String.Join("\n", script),
                        "}",
                            String.Join("\n", extensions),
                        "}}"
                    });

                    //Compile the C# class in memory using all the currently loaded assemblies
                    CSharpCodeProvider compiler = new CSharpCodeProvider();
                    CompilerParameters parameters = new CompilerParameters();
                    parameters.ReferencedAssemblies
                        .AddRange(AppDomain.CurrentDomain
                                .GetAssemblies()
                                .Where(a => !a.IsDynamic)
                                .Select(a => a.Location).ToArray());
                    parameters.CompilerOptions = "/t:library";
                    parameters.GenerateInMemory = true;
                    parameters.ReferencedAssemblies.AddRange(dlls.ToArray());
                    CompilerResults result = compiler.CompileAssemblyFromSource(parameters, code);
                    for (int i = 0; i < result.Errors.Count; i++)
                    {
                        //Console.WriteLine(result.Errors[i].ErrorText + " | " + result.Errors[i].Line);
                        throw new CSharpException(CSErrorType.LoadError,
                            new InvalidOperationException(result.Errors[i].ErrorText + " | " + result.Errors[i].Line));
                    }

                    //Retrieve compiled assembly
                    assembly = result.CompiledAssembly;
                    CompileCache[scriptHash] = result.CompiledAssembly;
                }
                assembly = CompileCache[scriptHash];
            }

            //Run the compiled assembly with exception handling
            if (run)
            {
                try
                {
                    object compiledScript = assembly.CreateInstance("ScriptLoader.CharpScript");
                    return
                        compiledScript
                        .GetType()
                        .GetMethod("__run")
                        .Invoke(compiledScript,
                            new object[] { new CSharpAPI(apiHandler, localVars), args });
                }
                catch (Exception e) { throw new CSharpException(CSErrorType.RuntimeError, e); }
            }
            else return null;
        }
        private static ulong QuickHash(string[] lines)
        {
            ulong hashedValue = 3074457345618258791ul;
            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    hashedValue += lines[i][j];
                    hashedValue *= 3074457345618258799ul;
                }
                hashedValue += '\n';
                hashedValue *= 3074457345618258799ul;
            }
            return hashedValue;
        }
        public enum CSErrorType { FileReadError, InvalidScript, LoadError, RuntimeError };

        public class CSharpException : Exception
        {
            private CSErrorType _type;
            public CSErrorType ExceptionType { get { return _type; } }
            public override string Message { get { return InnerException.Message; } }
            public override string ToString() { return InnerException.ToString(); }
            public CSharpException(CSErrorType type, Exception inner)
                : base(inner != null ? inner.Message : "", inner)
            {
                _type = type;
            }
        }

    }
    public class CSharpAPI : Script
    {
        public CSharpAPI(Script apiHandler, Dictionary<string, object> localVars)
        {
            SetMaster(apiHandler);
        }
        new public void LoadScript(Script bot)
        {
            base.LoadScript(bot);
        }
    }
}