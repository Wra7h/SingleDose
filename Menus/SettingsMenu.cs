using SingleDose.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SingleDose.Menus
{
    internal class SettingsMenu
    {
        public static int SuccessfulBuildCount = 0;
        public static string OutputDirectory = null;
        public static string szInjectMode = null;
        public static string szMemAlloc = "RWX";
        public static string szInvokeMethod = "PInvoke";
        public static string SelectedCscVersion = null;
        public static string SelectedCompilerPath = null;

        public static bool CompileBinary = true;
        public static bool UseLogging = true;

        public static int MaxHistoryEntries = 3;
        public static Dictionary<string, string> dAvailableCSCVersions;

        public static void CommandHandler(string Command)
        {
            switch (Command.ToUpper().Split()[0])
            {
                case "":
                    break;
                case "VERSION":
                    int element;
                    if (Command.Split().Count() > 1)
                    {
                        if (dAvailableCSCVersions.Any(CSCVersions => CSCVersions.Key == Command.Split()[1]))
                        {
                            if (dAvailableCSCVersions.TryGetValue(Command.Split()[1], out SelectedCompilerPath))
                            {
                                SelectedCscVersion = Command.Split()[1];
                            }
                        }
                        else if (int.TryParse(Command.Split()[1], out element) && element <= dAvailableCSCVersions.Count()) //int.TryParse() sets the value of the "element" variable which will be evaluated in the next if statement if necessary
                        {
                            SelectedCscVersion = dAvailableCSCVersions.ElementAt(element - 1).Key;
                            SelectedCompilerPath = dAvailableCSCVersions.ElementAt(element - 1).Value;
                            SDConsole.WriteInfo(String.Format("Selected Version: {0}", SelectedCscVersion));
                            SDConsole.WriteInfo(String.Format("Compiler Path: {0}", SelectedCompilerPath));
                        }
                        else if (Command.Split()[1].ToUpper() == "CUSTOM" || element == (dAvailableCSCVersions.Count() + 1))
                        {
                            string customPathInput = "";
                            SDConsole.WriteInfo("Enter absolute path to csc: ");
                            do
                            {
                                Console.Write("       > ");
                                customPathInput = Console.ReadLine();
                                SDConsole.iConsoleLineNum++;
                            } while (!File.Exists(customPathInput) && customPathInput.ToLower() != "exit");

                            if (customPathInput.ToLower() == "exit")
                            {
                                break;
                            }

                            if (customPathInput.ToLower().EndsWith("csc.exe"))
                            {
                                SelectedCscVersion = "Custom";
                                SelectedCompilerPath = customPathInput;
                            }
                            else
                            {
                                SDConsole.WriteError("Path must point to a \"csc.exe\"");
                            }
                        }
                        else
                        {
                            SDConsole.WriteError("Specified version not found.");
                        }
                    }
                    else
                    {
                        for (int i = 0; i < dAvailableCSCVersions.Count(); i++)
                        {
                            Console.WriteLine("  {0}) {1}", (i + 1).ToString(), dAvailableCSCVersions.ElementAt(i).Key);
                            SDConsole.iConsoleLineNum++;
                        }
                        Console.WriteLine("  {0}) {1}", (dAvailableCSCVersions.Count() + 1).ToString(), "Custom");
                        SDConsole.iConsoleLineNum++;
                    }
                    SDConsole.RefreshConfigPanel();
                    break;
                case "LOG":
                    if (SettingsMenu.UseLogging)
                    {
                        SettingsMenu.UseLogging = false;
                        SDConsole.WriteWarning("Logging Disabled");
                    }
                    else if (!SettingsMenu.UseLogging)
                    {
                        SettingsMenu.UseLogging = true;
                        SDConsole.WriteSuccess("Logging Enabled");
                    }
                    SDConsole.RefreshConfigPanel();
                    break;
                case "MEMSET":
                    if (Command.Split().Count() > 1)
                    {
                        switch (Command.Split()[1].ToUpper())
                        {
                            case "RWX":
                                SettingsMenu.szMemAlloc = "RWX  ";
                                SDConsole.WriteInfo("Memory Allocation set to RWX.");
                                break;
                            case "RW/RX":
                                SettingsMenu.szMemAlloc = "RW/RX";
                                SDConsole.WriteInfo("Memory Allocation set to RW/RX.");
                                break;
                            case "1":
                                goto case "RWX";
                            case "2":
                                goto case "RW/RX";
                            default:
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("   Memory Allocation Methods:");
                        Console.WriteLine("     1) RWX  ");
                        Console.WriteLine("     2) RW/RX");
                        SDConsole.iConsoleLineNum += 3;
                    }

                    SDConsole.RefreshConfigPanel();
                    break;
                case "INVOKE":
                    if (Command.Split().Count() > 1)
                    {
                        switch (Command.Split()[1].ToUpper())
                        {
                            case "PINVOKE":
                                SettingsMenu.szInvokeMethod = "PInvoke";
                                SDConsole.WriteInfo("Invoke method set to PInvoke");
                                break;
                            case "DINVOKE":
                                SettingsMenu.szInvokeMethod = "DInvoke";
                                SDConsole.WriteInfo("Invoke method set to DInvoke");
                                break;
                            case "1":
                                goto case "PINVOKE";
                            case "2":
                                goto case "DINVOKE";
                            default:
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("   Available Invoke Methods:");
                        Console.WriteLine("     1) PInvoke  ");
                        Console.WriteLine("     2) DInvoke  ");
                        SDConsole.iConsoleLineNum += 3;
                    }

                    SDConsole.RefreshConfigPanel();
                    break;
                case "HELP":
                    Console.WriteLine("");
                    Console.WriteLine("    +------------+-----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Command   |              Description                |       Usage       |");
                    Console.WriteLine("    +------------+-----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Mode      | Select the mode to use when building    | > mode            |");
                    Console.WriteLine("    |            | 1. Static: Embed payload/data           | > mode static     |");
                    Console.WriteLine("    |            | 2. Dynamic: Specify flags at execution  | > mode 1          |");
                    Console.WriteLine("    |            | 2. Download: Specify flags at execution |                   |");
                    Console.WriteLine("    +------------+-----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Ouput     | Set the output directory. Directory     | > output .        |");
                    Console.WriteLine("    |            | will be created, if necessary.          | > output <path>   |");
                    Console.WriteLine("    +------------+-----------------------------------------+-------------------+");
                    Console.WriteLine("    |  MemSet    | Change how the payload will be          | > memset 2        |");
                    Console.WriteLine("    |            | allocated in memory. Default: RWX       | > memset rw/rx    |");
                    Console.WriteLine("    +------------+-----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Version   | Change the version of .NET used for     | > version         |");
                    Console.WriteLine("    |            | compiling.                              | > version v3.5    |");
                    Console.WriteLine("    +------------+-----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Invoke    | Change the invocation method of Windows | > invoke dinvoke  |");
                    Console.WriteLine("    |            | APIs. Default: PInvoke                  | > invoke pinvoke  |");
                    Console.WriteLine("    +------------+-----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Compile   | Enable/Disable compilation of builds    | > compile         |");
                    Console.WriteLine("    |            | Default: Enabled                        |                   |");
                    Console.WriteLine("    +------------+-----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Log       | Enable/Disable logging                  | > log             |");
                    Console.WriteLine("    |            | Default: Enabled                        |                   |");
                    Console.WriteLine("    +------------+-----------------------------------------+-------------------+");
                    Console.WriteLine("    |  History   | Set the number of entries kept in       | > history 5       |");
                    Console.WriteLine("    |            | history. Default: 3 entries             |                   |");
                    Console.WriteLine("    +------------+-----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Triggers  | Enter the Triggers submenu              | > triggers        |");
                    Console.WriteLine("    +------------+-----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Clear     | Clear the terminal, settings, or        | > clear           |");
                    Console.WriteLine("    |            | triggers                                | > clear settings  |");
                    Console.WriteLine("    +------------+-----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Exit      | Return to Main Menu                     | > exit            |");
                    Console.WriteLine("    +------------+-----------------------------------------+-------------------+");
                    Console.WriteLine("");
                    SDConsole.iConsoleLineNum += 38;
                    break;
                case "TRIGGERS":
                    int cLineHolder = SDConsole.iConsoleLineNum;
                    Program.sCurrentMenu = "Triggers";
                    cLineHolder = SDConsole.iConsoleLineNum;
                    if (!SDConsole.bvShowHeader)
                    {
                        Console.SetCursorPosition(Console.WindowWidth - 59, 0);
                        Console.Write("{0}{1}", "+", String.Concat(Enumerable.Repeat("-", 58).ToArray()));
                        SDConsole.iConsoleLineNum = 1;
                        SDConsole.PrintSettings(Console.WindowWidth - 59, SDConsole.iConsoleLineNum);
                        SDConsole.PrintCommandHelp(Console.WindowWidth - 59, 11, Program.sCurrentMenu);

                    }
                    else
                    {
                        SDConsole.PrintSettings(Console.WindowWidth - 59, 6);
                        SDConsole.PrintCommandHelp(Console.WindowWidth - 59, 16, Program.sCurrentMenu);
                    }
                    SDConsole.iConsoleLineNum = cLineHolder;
                    break;
                case "MODE":
                    if (Command.Split().Count() > 1)
                    {
                        switch (Command.Split()[1].ToUpper())
                        {
                            case "STATIC":
                                SDConsole.WriteInfo("Mode: Static");
                                SettingsMenu.szInjectMode = "Static";
                                break;
                            case "1":
                                goto case "STATIC";
                            case "DYNAMIC":
                                SDConsole.WriteInfo("Mode: Dynamic");
                                SettingsMenu.szInjectMode = "Dynamic";
                                break;
                            case "2":
                                goto case "DYNAMIC";
                            case "DOWNLOAD":
                                SDConsole.WriteInfo("Mode: Download");
                                SettingsMenu.szInjectMode = "Download";
                                break;
                            case "3":
                                goto case "DOWNLOAD";
                            default:
                                break;
                        }
                        if (SettingsMenu.OutputDirectory != null && SettingsMenu.szInjectMode != null)
                        {
                            SDConsole.WriteInfo("All required settings configured.");
                        }
                    }
                    else
                    {
                        SDConsole.WriteInfo("Modes:");
                        SDConsole.Write("1. Static");
                        SDConsole.Write("2. Dynamic");
                        SDConsole.Write("3. Download");
                    }
                    SDConsole.RefreshConfigPanel();
                    break;
                case "OUTPUT":
                    if (Command.Split().Count() > 1)
                    {
                        if (Command.Split()[1].Trim() != "" && Command.Split()[1] != null)
                        {
                            SettingsMenu.OutputDirectory = Command.Split()[1];
                            try
                            {
                                SettingsMenu.OutputDirectory = Path.GetFullPath(SettingsMenu.OutputDirectory);
                            }
                            catch
                            {
                                SDConsole.WriteError("Error creating output directory. Please try again.");
                                SettingsMenu.OutputDirectory = null;
                                return;
                            }
                        }

                        if (!Directory.Exists(SettingsMenu.OutputDirectory))
                        {
                            try
                            {
                                Directory.CreateDirectory(SettingsMenu.OutputDirectory);
                            }
                            catch
                            {
                                SDConsole.WriteError("Error creating output directory. Please try again.");
                                SettingsMenu.OutputDirectory = null;
                                return;
                            }

                            if (Directory.Exists(SettingsMenu.OutputDirectory))
                            {
                                
                                SDConsole.WriteInfo(String.Format("Created directory: {0}", SettingsMenu.OutputDirectory));
                            }
                            else
                            {
                                SDConsole.WriteError(String.Format("Error creating directory: {0}", SettingsMenu.OutputDirectory));
                                SettingsMenu.OutputDirectory = null;
                            }
                        }
                    }
                    else
                    {
                        SDConsole.Write("Please enter output directory: ");
                        Console.Write("       > ");
                        SettingsMenu.OutputDirectory = Console.ReadLine();
                        SDConsole.iConsoleLineNum++;
                        SettingsMenu.OutputDirectory = Path.GetFullPath(SettingsMenu.OutputDirectory);

                        if (SettingsMenu.OutputDirectory.ToLower() == "exit")
                        {
                            break;
                        }

                        if (!Directory.Exists(SettingsMenu.OutputDirectory))
                        {
                            try
                            {
                                Directory.CreateDirectory(SettingsMenu.OutputDirectory);
                            
                                if (Directory.Exists(SettingsMenu.OutputDirectory))
                                {
                                    SDConsole.WriteInfo(String.Format("Created directory: {0}", SettingsMenu.OutputDirectory));
                                }
                                else
                                {
                                    SDConsole.WriteError(String.Format("Error creating directory: {0}", SettingsMenu.OutputDirectory));
                                    SettingsMenu.OutputDirectory = null;
                                }
                            }
                            catch
                            {
                                SettingsMenu.OutputDirectory = null;
                                break;
                            };
                        }
                    }

                    if (SettingsMenu.OutputDirectory != null && SettingsMenu.szInjectMode != null)
                    {
                        SDConsole.WriteInfo("All required settings configured.");
                    }

                    SDConsole.RefreshConfigPanel();
                    break;
                case "SHOW":
                    if (Command.Split().Count() > 1)
                    {
                        switch (Command.Split()[1].ToUpper())
                        {
                            case "TECHNIQUES":
                                Console.WriteLine("       +----------------------------------+-------------------------------+");
                                Console.WriteLine("       |       Shellcode Loaders          |        Process Injects        |");
                                Console.WriteLine("       +----------------------------------+-------------------------------+");
                                Console.WriteLine("       | L1.  FLSSetValue                 | R1. CreateRemoteThread        |");
                                Console.WriteLine("       | L2.  ImageGetDigest              | R2. EarlyBirdQueueUserAPC     |");
                                Console.WriteLine("       | L3.  CreateFiber                 | R3. SuspendQueueUserAPC       |");
                                Console.WriteLine("       | L4.  NtTestAlert                 | R4. AddressOfEntryPoint       |");
                                Console.WriteLine("       | L5.  ThreadPoolWait              | R5. KernelCallbackTable       |");
                                Console.WriteLine("       | L6.  CreateThread                | R6. NtCreateSection           |");
                                Console.WriteLine("       | L7.  EnumDesktops                | R7. PEResource                |");
                                Console.WriteLine("       | L8.  SetTimer                    | R8. ThreadHijack              |");
                                Console.WriteLine("       | L9.  SetupCommitFileQueue        | R9. SpawnThreadHijack         |");
                                Console.WriteLine("       | L10. CertEnumSystemStore         |                               |");
                                Console.WriteLine("       | L11. EnumChildWindows            |                               |");
                                Console.WriteLine("       | L12. EnumDateFormatsEx           |                               |");
                                Console.WriteLine("       | L13. EnumWindows                 |                               |");
                                Console.WriteLine("       | L14. GetOpenFileName             |                               |");
                                Console.WriteLine("       | L15. VerifierEnumerateResource   |                               |");
                                Console.WriteLine("       | L16. ThreadpoolTimer             |                               |");
                                Console.WriteLine("       | L17. ThreadpoolWork              |                               |");
                                Console.WriteLine("       +----------------------------------+-------------------------------+");
                                SDConsole.iConsoleLineNum += 21;
                                break;
                            case "VERSION":
                                for (int i = 0; i < dAvailableCSCVersions.Count(); i++)
                                {
                                    Console.WriteLine(" {0}) {1}", (i + 1).ToString(), dAvailableCSCVersions.ElementAt(i).Key);
                                    SDConsole.iConsoleLineNum++;
                                }
                                Console.WriteLine(" {0}) {1}", (dAvailableCSCVersions.Count() + 1).ToString(), "Custom");
                                SDConsole.iConsoleLineNum++;
                                break;
                            case "VERSIONS":
                                goto case "VERSION";
                        }
                    }
                    break;
                case "COMPILE":
                    if (SettingsMenu.CompileBinary)
                    {
                        SettingsMenu.CompileBinary = false;
                        SDConsole.WriteInfo("Compiling has been disabled.");
                    }
                    else if (!SettingsMenu.CompileBinary)
                    {
                        SettingsMenu.CompileBinary = true;
                        SDConsole.WriteInfo("Compiling has been enabled.");
                    }
                    SDConsole.RefreshConfigPanel();
                    break;

                case "CLEAR":
                    if (Command.Split().Count() > 1)
                    {
                        switch (Command.Split()[1].ToUpper())
                        {
                            case "SETTINGS":
                                SettingsMenu.szInjectMode = null;
                                SettingsMenu.OutputDirectory = null;
                                SettingsMenu.CompileBinary = true;
                                SettingsMenu.UseLogging = true;
                                SettingsMenu.szMemAlloc = "RWX";
                                SDConsole.WriteInfo("Default settings restored.");
                                SDConsole.RefreshConfigPanel();
                                break;
                            case "TRIGGER":
                                TriggersMenu.SelectedTrigger = null;
                                TriggersMenu.TriggerBody = null;
                                SDConsole.RefreshConfigPanel();
                                Console.SetCursorPosition(0, SDConsole.iConsoleLineNum + 1);
                                SDConsole.WriteInfo("Triggers have been cleared.");
                                break;
                            case "TRIGGERS":
                                goto case "TRIGGER";
                            default:
                                break;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        SDConsole.bvShowHeader = false;
                        Console.Write("{0," + (Console.WindowWidth - 58) + "}{1}", "+", String.Concat(Enumerable.Repeat("-", 58).ToArray()));
                        SDConsole.PrintSettings(Console.WindowWidth - 59, 1);
                        SDConsole.PrintCommandHelp(Console.WindowWidth - 59, 11, Program.sCurrentMenu);
                        SDConsole.iConsoleLineNum = -1; //This will increment before the next prompt for command making it 0 for the next prompt ( 0 = the top of the console)
                    }
                    break;
                case "HISTORY":
                    if (Command.Split().Count() > 1)
                    {
                        int newMax = 0;
                        bool success = int.TryParse(Command.Split()[1], out newMax);
                        if (success)
                        {
                            SettingsMenu.MaxHistoryEntries = newMax;
                            SDConsole.WriteSuccess(String.Format("Max history count set to {0}", newMax));
                        }
                    }
                    break;
                case "EXIT":
                    Program.sCurrentMenu = "Main";
                    SDConsole.RefreshConfigPanel();
                    break;
                default:
                    SDConsole.WriteError(String.Format("Unknown Command: {0}", Command));
                    break;
            }            
        }

        public static Dictionary<string, string> FetchCSCVersions()
        {
            Dictionary<string, string> dCSCFound = new Dictionary<string, string>();
            foreach (string folder in Directory.GetDirectories(@"C:\Windows\Microsoft.NET\Framework64\"))
            {
                if (File.Exists(folder + @"\csc.exe"))
                {
                    if (folder.Split('\\')[4] != "v2.0.50727") // No technique tested to compile with v2.0.50727
                    {
                        dCSCFound.Add(folder.Split('\\')[4], folder + @"\csc.exe");
                    }
                }
            }
            string[] RoslynPaths = new string[] { @"C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\Roslyn\csc.exe", @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\Roslyn\csc.exe" };

            foreach (string path in RoslynPaths)
            {
                if (File.Exists(path))
                {
                    dCSCFound.Add("Roslyn", path);
                    break;
                }
            }

            if (dCSCFound.Count == 0)
            {
                SDConsole.WriteError("No supported version of csc found. Exiting...");
                System.Threading.Thread.Sleep(5000); //Sleep 5 seconds to allow user to read message
                Environment.Exit(1);
            }

            SelectedCscVersion = dCSCFound.Keys.First();
            SelectedCompilerPath = dCSCFound.Values.First();
            
            return dCSCFound;
        }
    }
}
