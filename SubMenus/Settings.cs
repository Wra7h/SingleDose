using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SingleDose
{
    class Settings
    {
        public static string szSelectedTechnique = null;
        public static string szInjectMode = null;
        public static bool bvCompileBinary = true;
        public static string szOutputDirectory = null;
        public static bool bvHelpBlurb = true;
        public static Dictionary<string,string> dAvailableCSCVersions;
        public static string szSelectedCompilerPath = @"C:\Windows\Microsoft.NET\Framework64\v3.5\csc.exe";
        public static string szSelectedCscVersion = "v3.5";
        public static bool bvLogging = true;
        public static string szCurrentLogFile = "";
        public static string szLogBuffer = "";
        public static int cMaxHistorySize = 3;
        public static List<string> listPInvokeRecipe = new List<string>();


        public static void SettingsMenu()
        {
            string settingsInput = null;
            Console.WriteLine("\n       +---------------------------+\n ______|         SETTINGS          |\n|      +---------------------------+");
            if (bvHelpBlurb)
            {
                Console.WriteLine("|\n|\tmode      output   show   ");
                Console.WriteLine("|\tcompile   blurb    triggers");
                Console.WriteLine("|\tversion   clear    help");
                Console.WriteLine("|\thistory   log      exit");
            }

            do
            {
                Console.Write("|\n+-->> ");
                settingsInput = Console.ReadLine();
                SettingsShellCommand(settingsInput);
                if (settingsInput.ToUpper() == "TRIGGERS")
                {
                    return;
                }
            } while (settingsInput.ToUpper() != "EXIT");
        }

        public static void SettingsShellCommand(string command)
        {
            switch (command.ToUpper().Split()[0])
            {
                case "":
                    break;
                case "VERSION":
                    int element;
                    if (command.Split().Count() > 1)
                    {
                        if (dAvailableCSCVersions.Any(CSCVersions => CSCVersions.Key == command.Split()[1]))
                        {
                            if (dAvailableCSCVersions.TryGetValue(command.Split()[1], out szSelectedCompilerPath))
                            {
                                szSelectedCscVersion = command.Split()[1];
                                Program.WriteLog("Set Compiler: " + szSelectedCscVersion + " :: " + szSelectedCompilerPath, true);
                            }
                        }
                        else if (int.TryParse(command.Split()[1], out element) && element <= dAvailableCSCVersions.Count()) //int.TryParse() sets the value of the "element" variable which will be evaluated in the next if statement if necessary
                        {
                            szSelectedCscVersion = dAvailableCSCVersions.ElementAt(element - 1).Key;
                            szSelectedCompilerPath = dAvailableCSCVersions.ElementAt(element - 1).Value;
                            Console.WriteLine("|");
                            Console.WriteLine("|   [~] Selected Version: {0}", szSelectedCscVersion);
                            Console.WriteLine("|   [~] Compiler Path: {0}", szSelectedCompilerPath);
                            Program.WriteLog("Set Compiler: " + szSelectedCscVersion + " :: " + szSelectedCompilerPath, true);
                        }
                        else if (command.Split()[1].ToUpper() == "CUSTOM" || element == (dAvailableCSCVersions.Count() + 1))
                        {
                            Console.WriteLine("|");
                            string customPathInput = "";
                            Console.WriteLine("|   [~] Enter absolute path to csc: ");
                            do
                            {
                                Console.Write("|       > ");
                                customPathInput = Console.ReadLine();
                            } while (!File.Exists(customPathInput) && customPathInput.ToLower() != "exit");

                            if (customPathInput.ToLower() == "exit")
                            {
                                break;
                            }

                            if (customPathInput.ToLower().EndsWith("csc.exe"))
                            {
                                szSelectedCscVersion = "Custom";
                                szSelectedCompilerPath = customPathInput;
                            }
                            else
                            {
                                Console.WriteLine("|   [!] Path must point to a \"csc.exe\" ");
                            }
                            Program.WriteLog("Set Compiler: " + szSelectedCscVersion + " :: " + szSelectedCompilerPath, true);
                        }
                        else
                        {
                            Console.WriteLine("|   [~] Version not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("|");
                        for (int i = 0; i < dAvailableCSCVersions.Count(); i++)
                        {
                            Console.WriteLine("|\t{0}) {1}", (i + 1).ToString(), dAvailableCSCVersions.ElementAt(i).Key);
                        }
                        Console.WriteLine("|\t{0}) {1}", (dAvailableCSCVersions.Count() + 1).ToString(), "Custom");
                    }
                    break;
                case "LOG":
                    if (Settings.bvLogging)
                    {
                        Settings.bvLogging = false;
                        Console.WriteLine("|\n|   [~] bvLogging Disabled");
                    }
                    else if (!Settings.bvLogging)
                    {
                        Settings.bvLogging = true;
                        Console.WriteLine("|\n|   [~] bvLogging Enabled");
                        Program.WriteLog("bvLogging enabled", true);
                    }
                    break;
                case "HELP":
                    Console.WriteLine("|\n|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |                                   SETTINGS HELP                                               |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Command   |                       Description                        |     Example Usage     |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Triggers  | Enter the Triggers submenu                               | >> Triggers           |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Mode      | Select the mode to use when compiling                    | >> mode 1             |");
                    Console.WriteLine("|             |            | 1. Static: Embed injection data into binary              | >> mode download      |");
                    Console.WriteLine("|             |            | 2. Dynamic: Specify -PID & -DLL/-Bin at execution        |                       |");
                    Console.WriteLine("|             |            | 3. Download: Specify -PID & -URI at execution            |                       |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Ouput     | Set the output directory. Directory will be created      | > output .\\builds     |");
                    Console.WriteLine("|             |            | if necessary.                                            | > output <path>       |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Compile   | Specify whether or not to compile the generated .cs.     | >> compile            |");
                    Console.WriteLine("|             |            | Default = True                                           |                       |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Log       | Specify whether or not to log builds in a txt file.      | >> log                |");
                    Console.WriteLine("|             |            | Log is stored in the root of the output directory.       |                       |");
                    Console.WriteLine("|             |            | Default = True                                           |                       |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  History   | Change the maximum amount of entries kept in history.    | >> history 5          |");
                    Console.WriteLine("|             |            | Default = 3 entries                                      |                       |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Version   | Change the version of .NET used for compiling.           | >> version            |");
                    Console.WriteLine("|             |            | Just typing version will show versions found on system   | >> version v4.0.30319 |");
                    Console.WriteLine("|             |            | Default = .NET v3.5                                      | >> version Roslyn     |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Show      | Display current config, techniques or history entries    | > show                |");
                    Console.WriteLine("|             |            |                                                          | > show history        |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Clear     | Clear the terminal, settings, or triggers                | >> clear              |");
                    Console.WriteLine("|             |            |                                                          | >> clear triggers     |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Blurb     | Display available commands when switching/clearing menus | >> blurb              |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Help      | Display this help                                        | >> help               |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Exit      | Return to Main Menu                                      | >> exit               |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+\n|\n|");
                    break;
                case "TRIGGERS":
                    Triggers.TriggersMenu();
                    break;
                case "BLURB":
                    if (Settings.bvHelpBlurb)
                    {
                        Settings.bvHelpBlurb = false;
                        Console.WriteLine("|\n|   [~] Help blurbs has been disabled.");
                    } 
                    else if (!Settings.bvHelpBlurb)
                    {
                        bvHelpBlurb = true;
                        Console.WriteLine("|\n|\tmode      output   show   ");
                        Console.WriteLine("|\tcompile   blurb    triggers");
                        Console.WriteLine("|\tversion   clear    help");
                        Console.WriteLine("|\thistory   log      exit");
                    }
                    break;
                case "MODE":
                    if (command.Split().Count() > 1)
                    {
                        switch (command.Split()[1].ToUpper())
                        {
                            case "STATIC":
                                Console.WriteLine("|\n|\t[*] Mode: Static. Injection content will be embedded in binary.");
                                Settings.szInjectMode = "STATIC";
                                break;
                            case "1":
                                goto case "STATIC";
                            case "DYNAMIC":
                                Console.WriteLine("|\n|\t[*] Mode: Dynamic. Injection content will be provided at execution with -PID and -DLL/-Bin.");
                                Settings.szInjectMode = "DYNAMIC";
                                break;
                            case "2":
                                goto case "DYNAMIC";
                            case "DOWNLOAD":
                                Console.WriteLine("|\n|\t[*] Mode: Download. Injection content will be provided at execution with -PID and -URI.");
                                Settings.szInjectMode = "DOWNLOAD";
                                break;
                            case "3":
                                goto case "DOWNLOAD";
                            default:
                                break;
                        }
                        if (Settings.szOutputDirectory != null && Settings.szInjectMode != null)
                        {
                            Console.WriteLine("|\n|\t[*] All required settings configured. Return to main menu to configure triggers or build binary.");
                        }
                    }
                    break;
                case "OUTPUT":
                    if (command.Split().Count() > 1)
                    {
                        if (command.Split()[1] != " " && command.Split()[1] != null)
                        {
                            Settings.szOutputDirectory = command.Split()[1];
                            try
                            {
                                Settings.szOutputDirectory = Path.GetFullPath(Settings.szOutputDirectory);
                            }
                            catch
                            {
                                Console.WriteLine("|\n|\t[!] Error creating output directory. Please try again.");
                                return;
                            }
                        }

                        if (!Directory.Exists(Settings.szOutputDirectory))
                        {
                            try
                            {
                                Directory.CreateDirectory(Settings.szOutputDirectory);
                            }
                            catch
                            {
                                Console.WriteLine("|\n|\t[!] Error creating output directory. Please try again.");
                            }

                            if (Directory.Exists(Settings.szOutputDirectory))
                            {
                                Console.WriteLine("|\n|\t[*] Created directory: {0}", Settings.szOutputDirectory);
                                Program.WriteLog("### New Session Started ###",false);
                                if (Settings.szLogBuffer != "")
                                {
                                    Program.WriteLog(Settings.szLogBuffer, false);
                                    Settings.szLogBuffer = "";
                                }
                            }
                            else
                            {
                                Console.WriteLine("|\n|\t[!] Error creating directory: {0}", Settings.szOutputDirectory);
                            }
                        }
                        else
                        {
                            Program.WriteLog("### New Session Started ###", false);
                            if (Settings.szLogBuffer != "") 
                            {
                                Program.WriteLog(Settings.szLogBuffer, false);
                                Settings.szLogBuffer = "";
                            } 
                        }
                    }
                    else
                    {
                        Console.Write("\n   [~] Please enter output directory: ");
                        Settings.szOutputDirectory = Console.ReadLine();
                        Settings.szOutputDirectory = Path.GetFullPath(Settings.szOutputDirectory);

                        if (Settings.szOutputDirectory.ToLower() == "exit")
                        {
                            break;
                        }

                        if (!Directory.Exists(Settings.szOutputDirectory))
                        {
                            Directory.CreateDirectory(Settings.szOutputDirectory);
                            if (Directory.Exists(Settings.szOutputDirectory))
                            {
                                Console.WriteLine("   [*] Created directory: {0}", Settings.szOutputDirectory);
                                Program.WriteLog("New Session Started", false);
                            }
                            else
                            {
                                Console.WriteLine("   [!] Error creating directory: {0}", Settings.szOutputDirectory);
                            }
                        }
                    }
                    if (Settings.szOutputDirectory != null && Settings.szInjectMode != null)
                    {
                        Console.WriteLine("|\n|\t[*] All required settings configured. Return to main menu to configure triggers or build binary.");
                    }
                    break;
                case "SHOW":
                    if (command.Split().Count() > 1)
                    {
                        switch (command.Split()[1].ToUpper())
                        {
                            case "TECHNIQUES":
                                Console.WriteLine("|\n|\t   +--------------------------+-----------------------------------+");
                                Console.WriteLine("|\t   |    Shellcode Loaders     |          Process Injects          |");
                                Console.WriteLine("|\t   +--------------------------+-----------------------------------+");
                                Console.WriteLine("|\t   | L1. Syscall_CreateThread | R1. CreateRemoteThread-DLL [DLL]  |");
                                Console.WriteLine("|\t   | L2. SRDI-Loader          | R2. EarlyBird_QueueUserAPC        |");
                                Console.WriteLine("|\t   | L3. CreateFiber          | R3. Suspend_QueueUserAPC          |");
                                Console.WriteLine("|\t   | L4. EnumWindows          | R4. AddressOfEntryPoint           |");
                                Console.WriteLine("|\t   | L5. EnumChildWindows     | R5. KernelCallbackTable           |");
                                Console.WriteLine("|\t   | L6. EnumDateFormatsEx    | R6. NtCreateSection               |");
                                Console.WriteLine("|\t   | L7. EnumDesktops         |                                   |");
                                Console.WriteLine("|\t   +--------------------------+-----------------------------------+");
                                break;
                            case "VERSION":
                                Console.WriteLine("|");
                                for (int i = 0; i <dAvailableCSCVersions.Count(); i++)
                                {
                                    Console.WriteLine("|\t{0}) {1}", (i + 1).ToString(), dAvailableCSCVersions.ElementAt(i).Key);
                                }
                                Console.WriteLine("|\t{0}) {1}", (dAvailableCSCVersions.Count() + 1).ToString(), "Custom");
                                break;
                            case "VERSIONS":
                                Console.WriteLine("|");
                                for (int i = 0; i < dAvailableCSCVersions.Count(); i++)
                                {
                                    Console.WriteLine("|\t{0}) {1}", (i + 1).ToString(), dAvailableCSCVersions.ElementAt(i).Key);
                                }
                                Console.WriteLine("|\t{0}) {1}", (dAvailableCSCVersions.Count() + 1).ToString(), "Custom");
                                break;
                        }
                    }
                    else
                    {
                        Program.PrintSettings();
                    }
                    break;
                case "COMPILE":
                    if (Settings.bvCompileBinary)
                    {
                        Settings.bvCompileBinary = false;
                        Console.WriteLine("|\n|   [~] Compile has been set to false.");
                        Program.WriteLog("Compiling Disabled", true);
                    }
                    else if (!Settings.bvCompileBinary)
                    {
                        Settings.bvCompileBinary = true;
                        Console.WriteLine("|\n|   [~] Compile has been set to true.");
                        Program.WriteLog("Compiling Enabled", true);
                    }
                    break;
                case "CLEAR":
                    if (command.Split().Count() > 1)
                    {
                        switch (command.Split()[1].ToUpper())
                        {
                            case "SETTINGS":
                                Settings.szInjectMode = null;
                                Settings.szOutputDirectory = null;
                                Settings.bvCompileBinary = true;
                                Console.WriteLine("|\n|   [~] Output and Mode have been cleared.");
                                break;
                            case "TRIGGERS":
                                Program.HIBERNATEPROCESSDETAILS = "";
                                Program.REQUIREDPROCESSDETAILS = "";
                                Program.AVOIDPROCESSDETAILS = "";
                                Program.PERSISTPROCESSDETAILS = "";
                                Program.TIMERSECONDS = "";
                                Program.TriggersToUse.Clear();
                                Console.WriteLine("|\n|   [~] Triggers have been cleared.");
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n       +---------------------------+\n ______|         SETTINGS          |\n|      +---------------------------+");
                        if (bvHelpBlurb)
                        {
                            Console.WriteLine("|\n|\tmode      output   show   ");
                            Console.WriteLine("|\tcompile   blurb    triggers");
                            Console.WriteLine("|\tversion   clear    help");
                            Console.WriteLine("|\thistory   log      exit");
                        }
                    }
                    break;
                case "HISTORY":
                    if (command.Split().Count() > 1)
                    {
                        int newMax = 0;
                        bool success = int.TryParse(command.Split()[1], out newMax);
                        if (success)
                        {
                            Console.WriteLine("|\n|\t[*] Max history count set to {0}", newMax);
                            Settings.cMaxHistorySize = newMax;
                        }
                    }
                    break;
                case "EXIT":
                    return;
                default:
                    Console.WriteLine("|\n|\t[!] Unknown Command: {0}", command);
                    break;
            }
        }

        public static Dictionary<string, string> FetchCSCVersions()
        {
            Dictionary<string, string> cscDictionary = new Dictionary<string, string>();
            foreach (string folder in Directory.GetDirectories(@"C:\Windows\Microsoft.NET\Framework64\"))
            {
                if (File.Exists(folder + @"\csc.exe"))
                {
                    if (folder.Split('\\')[4] != "v2.0.50727") // No technique compiles with v2.0.50727 :(
                    { 
                        cscDictionary.Add(folder.Split('\\')[4], folder + @"\csc.exe");
                    }
                }
            }
            string[] RoslynPaths = new string[]{@"C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\Roslyn\csc.exe", @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\Roslyn\csc.exe"};

            foreach (string path in RoslynPaths)
            {
                if (File.Exists(path))
                {
                    cscDictionary.Add("Roslyn", path);
                    break;
                }
            }
            return cscDictionary;
        }

    }
}
