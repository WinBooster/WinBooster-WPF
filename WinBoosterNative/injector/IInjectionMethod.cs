using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinBoosterNative.injector
{
    public enum ExecutionType
    {
        CreateThread,
        HijackThread
    }

    public struct InjectionOptions
    {
        public bool ElevateHandle;
        public bool EraseHeaders;
        public bool CreateLoaderReference;
        public string LoaderImagePath;
    }

    public interface IInjectionMethod
    {
        ExecutionType TypeOfExecution { get; }
        Process TargetProcess { get; }
        InjectionOptions Options { get; }

        bool InjectImage(string imagePath);
        bool InjectImage(byte[] rawImage);
    }
}
