﻿using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace WinBoosterNative
{
    public class DllInjector
    {
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
        ProcessAccessFlags processAccess,
        bool bInheritHandle,
        int processId);
        public static IntPtr OpenProcess(Process proc, ProcessAccessFlags flags)
        {
            return OpenProcess(flags, false, proc.Id);
        }

        // VirtualAllocEx signture https://www.pinvoke.net/default.aspx/kernel32.virtualallocex
        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        // VirtualFreeEx signture  https://www.pinvoke.net/default.aspx/kernel32.virtualfreeex
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
        int dwSize, AllocationType dwFreeType);

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            IntPtr dwSize,
            AllocationType flAllocationType,
            MemoryProtection flProtect);

        // WriteProcessMemory signture https://www.pinvoke.net/default.aspx/kernel32/WriteProcessMemory.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        [MarshalAs(UnmanagedType.AsAny)] object lpBuffer,
        int dwSize,
        out IntPtr lpNumberOfBytesWritten);

        // GetProcAddress signture https://www.pinvoke.net/default.aspx/kernel32.getprocaddress
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        // GetModuleHandle signture http://pinvoke.net/default.aspx/kernel32.GetModuleHandle
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        // CreateRemoteThread signture https://www.pinvoke.net/default.aspx/kernel32.createremotethread
        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(
        IntPtr hProcess,
        IntPtr lpThreadAttributes,
        uint dwStackSize,
        IntPtr lpStartAddress,
        IntPtr lpParameter,
        uint dwCreationFlags,
        IntPtr lpThreadId);

        // CloseHandle signture https://www.pinvoke.net/default.aspx/kernel32.closehandle
        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        public bool Inject(int ProcId, string dll)
        {
            string DllPath = dll;
            IntPtr Size = (IntPtr)DllPath.Length;

            // Make sure file exist
            if (File.Exists(DllPath))
            {
                if ((ProcId == 4) || (ProcId == 0))
                {
                    Debug.WriteLine("[!] SYSTEM process id {0} not allowed.", ProcId);
                }
                else
                {
                    // Get process by id
                    try
                    {
                        Process localById = Process.GetProcessById(ProcId);
                        string ProcName = localById.ProcessName;
                        Debug.WriteLine("[+] Process name '{0}' found.", ProcName);
                    }
                    catch (ArgumentException e)
                    {
                        Debug.WriteLine("[!] Error: {0}", e.Message);
                    }
                }

                // Open handle to the target process
                IntPtr ProcHandle = OpenProcess(ProcessAccessFlags.All, false, ProcId);
                if (ProcHandle == null)
                {
                    Debug.WriteLine("[!] Handle to target process could not be obtained!");
                    Console.WriteLine("[!] Handle to target process could not be obtained!");
                }
                else
                {
                    Debug.WriteLine("[+] Handle (0x" + ProcHandle + ") to target process has been be obtained.");
                    Console.WriteLine("[+] Handle (0x" + ProcHandle + ") to target process has been be obtained.");
                    // Allocate DLL space
                    IntPtr DllSpace = VirtualAllocEx(
                        ProcHandle,
                        IntPtr.Zero,
                        Size,
                        AllocationType.Reserve | AllocationType.Commit,
                        MemoryProtection.ExecuteReadWrite);

                    if (DllSpace == null)
                    {
                        Debug.WriteLine("[!] DLL space allocation failed.");
                        Console.WriteLine("[!] DLL space allocation failed.");
                    }
                    else
                    {
                        Debug.WriteLine("[+] DLL space (0x" + DllSpace + ") allocation is successful.");
                        Console.WriteLine("[+] DLL space (0x" + DllSpace + ") allocation is successful.");
                        // Write DLL content to VAS of target process
                        byte[] bytes = Encoding.ASCII.GetBytes(DllPath);
                        bool DllWrite = WriteProcessMemory(
                            ProcHandle,
                            DllSpace,
                            bytes,
                            (int)bytes.Length,
                            out var bytesread
                            );

                        if (DllWrite == false)
                        {
                            Debug.WriteLine("[!] Writing DLL content to target process failed.");
                            Console.WriteLine("[!] Writing DLL content to target process failed.");
                        }
                        else
                        {
                            Debug.WriteLine("[+] Writing DLL content to target process is successful.");
                            Console.WriteLine("[+] Writing DLL content to target process is successful.");
                            // Get handle to Kernel32.dll and get address for LoadLibraryA
                            IntPtr Kernel32Handle = GetModuleHandle("Kernel32.dll");
                            IntPtr LoadLibraryAAddress = GetProcAddress(Kernel32Handle, "LoadLibraryA");

                            if (LoadLibraryAAddress == null)
                            {
                                Debug.WriteLine("[!] Obtaining an addess to LoadLibraryA function has failed.");
                                Console.WriteLine("[!] Obtaining an addess to LoadLibraryA function has failed.");
                            }
                            else
                            {
                                Debug.WriteLine("[+] LoadLibraryA function address (0x" + LoadLibraryAAddress + ") has been obtained.");
                                Console.WriteLine("[+] LoadLibraryA function address (0x" + LoadLibraryAAddress + ") has been obtained.");
                                // Create remote thread in the target process
                                IntPtr RemoteThreadHandle = CreateRemoteThread(
                                    ProcHandle,
                                    IntPtr.Zero,
                                    0,
                                    LoadLibraryAAddress,
                                    DllSpace,
                                    0,
                                    IntPtr.Zero
                                    );

                                if (RemoteThreadHandle == null)
                                {
                                    Debug.WriteLine("[!] Obtaining a handle to remote thread in target process failed.");
                                    Console.WriteLine("[!] Obtaining a handle to remote thread in target process failed.");
                                }
                                else
                                {
                                    Debug.WriteLine("[+] Obtaining a handle to remote thread (0x" + RemoteThreadHandle + ") in target process is successful.");
                                    Console.WriteLine("[+] Obtaining a handle to remote thread (0x" + RemoteThreadHandle + ") in target process is successful.");
                                    bool FreeDllSpace = VirtualFreeEx(
                                    ProcHandle,
                                    DllSpace,
                                    0,
                                    AllocationType.Release);
                                    if (FreeDllSpace == false)
                                    {
                                        Debug.WriteLine("[!] Failed to release DLL memory in target process.");
                                        Console.WriteLine("[!] Failed to release DLL memory in target process.");
                                    }
                                    else
                                    {
                                        Debug.WriteLine("[+] Successfully released DLL memory in target process.");
                                        Console.WriteLine("[+] Successfully released DLL memory in target process.");
                                        CloseHandle(RemoteThreadHandle);
                                        CloseHandle(DllSpace);
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
