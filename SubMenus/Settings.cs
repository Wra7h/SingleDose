using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SingleDose
{
    class Settings
    {
        public static string SelectedTechnique = null;
        public static string InjectMode = null;
        public static bool CompileBinary = true;
        public static string OutputDirectory = null;
        public static bool helpBlurb = true;
        public static Dictionary<string,string> availableCSCVersions;
        public static string SelectedCompilerPath = @"C:\Windows\Microsoft.NET\Framework64\v3.5\csc.exe";
        public static string SelectedCscVersion = "v3.5";
        public static bool Logging = true;
        public static string CurrentLogFile = "";
        public static string LogBuffer = "";

        public static void SettingsMenu()
        {
            string settingsInput = null;
            Console.WriteLine("\n       +---------------------------+\n ______|         SETTINGS          |\n|      +---------------------------+");
            if (helpBlurb)
            {
                Console.WriteLine("|\n|\tmode      output   show   ");
                Console.WriteLine("|\tcompile   blurb    triggers");
                Console.WriteLine("|\tversion   clear    help");
                Console.WriteLine("|\tlog       exit");
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
                        if (availableCSCVersions.Any(CSCVersions => CSCVersions.Key == command.Split()[1]))
                        {
                            if (availableCSCVersions.TryGetValue(command.Split()[1], out SelectedCompilerPath))
                            {
                                SelectedCscVersion = command.Split()[1];
                                Program.WriteLog("Set Compiler: " + SelectedCscVersion + " :: " + SelectedCompilerPath, true);
                            }
                        }
                        else if (int.TryParse(command.Split()[1], out element) && element <= availableCSCVersions.Count()) //int.TryParse() sets the value of the "element" variable which will be evaluated in the next if statement if necessary
                        {
                            SelectedCscVersion = availableCSCVersions.ElementAt(element - 1).Key;
                            SelectedCompilerPath = availableCSCVersions.ElementAt(element - 1).Value;
                            Console.WriteLine("|");
                            Console.WriteLine("|   [~] Selected Version: {0}", SelectedCscVersion);
                            Console.WriteLine("|   [~] Compiler Path: {0}", SelectedCompilerPath);
                            Program.WriteLog("Set Compiler: " + SelectedCscVersion + " :: " + SelectedCompilerPath, true);
                        }
                        else if (command.Split()[1].ToUpper() == "CUSTOM" || element == (availableCSCVersions.Count() + 1))
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
                                SelectedCscVersion = "Custom";
                                SelectedCompilerPath = customPathInput;
                            }
                            else
                            {
                                Console.WriteLine("|   [!] Path must point to a \"csc.exe\" ");
                            }
                            Program.WriteLog("Set Compiler: " + SelectedCscVersion + " :: " + SelectedCompilerPath, true);
                        }
                        else
                        {
                            Console.WriteLine("|   [~] Version not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("|");
                        for (int i = 0; i < availableCSCVersions.Count(); i++)
                        {
                            Console.WriteLine("|\t{0}) {1}", (i + 1).ToString(), availableCSCVersions.ElementAt(i).Key);
                        }
                        Console.WriteLine("|\t{0}) {1}", (availableCSCVersions.Count() + 1).ToString(), "Custom");
                    }
                    break;
                case "LOG":
                    if (Settings.Logging)
                    {
                        Settings.Logging = false;
                        Console.WriteLine("|\n|   [~] Logging Disabled");
                    }
                    else if (!Settings.Logging)
                    {
                        Settings.Logging = true;
                        Console.WriteLine("|\n|   [~] Logging Enabled");
                        Program.WriteLog("Logging enabled", true);
                    }
                    break;
                case "HELP":
                    Console.WriteLine("|\n|\t\t\t\t Settings Commands");
                    Console.WriteLine("|\t\t\t\t-------------------");
                    Console.WriteLine("|\tMode :: Select whether to embed injection content or provide it at execution.");
                    Console.WriteLine("|\t   1) Static: Injection content will be embedded in binary.");
                    Console.WriteLine("|\t   2) Dynamic: Injection content will be provided at execution with -PID and -DLL/-Bin.");
                    Console.WriteLine("|\t   3) Download: Injection content will be provided at execution with -PID and -URI.");
                    Console.WriteLine("|\t");
                    Console.WriteLine("|\t   > Example Usage: mode static");
                    Console.WriteLine("|\t   > Example Usage: mode 2");
                    Console.WriteLine("|\n|\tOutput :: Set the output directory. The directory will be created if necessary.");
                    Console.WriteLine("|\t   > Example Usage: output .\\binaries");
                    Console.WriteLine("|\t   > Example Usage: output C:\\Users\\user\\Desktop\\output");
                    Console.WriteLine("|\n|\tCompile :: a switch to compile the generated .cs file. If this command is executed while true,");
                    Console.WriteLine("|\t           the value becomes false and vice versa. (Default = true)");
                    Console.WriteLine("|\t   > Example Usage: compile");
                    Console.WriteLine("|\n|\n|\tClear :: Clear the terminal, settings or triggers.");
                    Console.WriteLine("|\t   > Example Usage: clear");
                    Console.WriteLine("|\t   > Example Usage: clear settings");
                    Console.WriteLine("|\tVersion :: Change the version used for compiling. (Default = v3.5)");
                    Console.WriteLine("|\t   > Example Usage: version");
                    Console.WriteLine("|\t   > Example Usage: version Roslyn");
                    Console.WriteLine("|\t   > Example Usage: version v4.0.30319");
                    Console.WriteLine("|\tLog :: Enable/Disable logging. (Default = true)");
                    Console.WriteLine("|\tShow :: Display current configuration.");
                    Console.WriteLine("|\tBlurb :: A switch to display a command blurb when switching between menus. (Default = true)");
                    Console.WriteLine("|\tTriggers :: Enter the triggers submenu");
                    Console.WriteLine("|\tExit :: Return to Main Menu");
                    break;
                case "TRIGGERS":
                    Triggers.TriggersMenu();
                    break;
                case "BLURB":
                    if (Settings.helpBlurb)
                    {
                        Settings.helpBlurb = false;
                        Console.WriteLine("|\n|   [~] Help blurbs has been disabled.");
                    } 
                    else if (!Settings.helpBlurb)
                    {
                        helpBlurb = true;
                        Console.WriteLine("|\n|\tmode      output   show   ");
                        Console.WriteLine("|\tcompile   blurb    triggers");
                        Console.WriteLine("|\tversion   clear    help");
                        Console.WriteLine("|\tlog       exit");
                    }
                    break;
                case "MODE":
                    if (command.Split().Count() > 1)
                    {
                        switch (command.Split()[1].ToUpper())
                        {
                            case "STATIC":
                                Console.WriteLine("|\n|\t[*] Mode: Static. Injection content will be embedded in binary.");
                                Settings.InjectMode = "STATIC";
                                break;
                            case "1":
                                Console.WriteLine("|\n|\t[*] Mode: Static. Injection content will be embedded in binary.");
                                Settings.InjectMode = "STATIC";
                                break;
                            case "DYNAMIC":
                                Console.WriteLine("|\n|\t[*] Mode: Dynamic. Injection content will be provided at execution with -PID and -DLL/-Bin.");
                                Settings.InjectMode = "DYNAMIC";
                                break;
                            case "2":
                                Console.WriteLine("|\n|\t[*] Mode: Dynamic. Injection content will be provided at execution with -PID and -DLL/-Bin.");
                                Settings.InjectMode = "DYNAMIC";
                                break;
                            case "DOWNLOAD":
                                Console.WriteLine("|\n|\t[*] Mode: Download. Injection content will be provided at execution with -PID and -URI.");
                                Settings.InjectMode = "DOWNLOAD";
                                break;
                            case "3":
                                Console.WriteLine("|\n|\t[*] Mode: Download. Injection content will be provided at execution with -PID and -URI.");
                                Settings.InjectMode = "DOWNLOAD";
                                break;
                            default:
                                break;
                        }
                        if (Settings.OutputDirectory != null && Settings.InjectMode != null)
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
                            Settings.OutputDirectory = command.Split()[1];
                            try
                            {
                                Settings.OutputDirectory = Path.GetFullPath(Settings.OutputDirectory);
                            }
                            catch
                            {
                                Console.WriteLine("|\n|\t[!] Error creating output directory. Please try again.");
                                return;
                            }
                        }

                        if (!Directory.Exists(Settings.OutputDirectory))
                        {
                            try
                            {
                                Directory.CreateDirectory(Settings.OutputDirectory);
                            }
                            catch
                            {
                                Console.WriteLine("|\n|\t[!] Error creating output directory. Please try again.");
                            }

                            if (Directory.Exists(Settings.OutputDirectory))
                            {
                                Console.WriteLine("|\n|\t[*] Created directory: {0}", Settings.OutputDirectory);
                                Program.WriteLog("### New Session Started ###",false);
                                if (Settings.LogBuffer != "")
                                {
                                    Program.WriteLog(Settings.LogBuffer, false);
                                    Settings.LogBuffer = "";
                                }
                            }
                            else
                            {
                                Console.WriteLine("|\n|\t[!] Error creating directory: {0}", Settings.OutputDirectory);
                            }
                        }
                        else
                        {
                            Program.WriteLog("### New Session Started ###", false);
                            if (Settings.LogBuffer != "") 
                            {
                                Program.WriteLog(Settings.LogBuffer, false);
                                Settings.LogBuffer = "";
                            } 
                        }
                    }
                    else
                    {
                        Console.Write("\n   [~] Please enter output directory: ");
                        Settings.OutputDirectory = Console.ReadLine();
                        Settings.OutputDirectory = Path.GetFullPath(Settings.OutputDirectory);

                        if (Settings.OutputDirectory.ToLower() == "exit")
                        {
                            break;
                        }

                        if (!Directory.Exists(Settings.OutputDirectory))
                        {
                            Directory.CreateDirectory(Settings.OutputDirectory);
                            if (Directory.Exists(Settings.OutputDirectory))
                            {
                                Console.WriteLine("   [*] Created directory: {0}", Settings.OutputDirectory);
                                Program.WriteLog("New Session Started", false);
                            }
                            else
                            {
                                Console.WriteLine("   [!] Error creating directory: {0}", Settings.OutputDirectory);
                            }
                        }
                    }
                    if (Settings.OutputDirectory != null && Settings.InjectMode != null)
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
                                Console.WriteLine("|\n|                TECHNIQUES");
                                Console.WriteLine("|              --------------");
                                Console.WriteLine("|\t   1) CreateRemoteThread: Inject a DLL into a remote process and execute with CreateRemoteThread. [DLL]");
                                Console.WriteLine("|\t   2) SRDI: Convert DLL into shellcode and inject. [DLL]");
                                Console.WriteLine("|\t   3) EarlyBird_QueueUserAPC: Inject Shellcode into a newly spawned process. [Shellcode]");
                                Console.WriteLine("|\t   4) Suspend_QueueUserAPC: Inject Shellcode into a process currently running. [Shellcode]");
                                Console.WriteLine("|\t   5) Syscall_CreateThread: Inject Shellcode using direct syscalls. [Shellcode]");
                                Console.WriteLine("|\t   6) Fiber_Execution: Execute Shellcode via Fibers. [Shellcode]");
                                Console.WriteLine("|\t   7) EnumWindows: Execute Shellcode via Callback. [Shellcode]");
                                Console.WriteLine("|\t   8) EnumChildWindows: Execute Shellcode via Callback. [Shellcode]");
                                Console.WriteLine("|\t   9) EnumDateFormatsEx: Execute Shellcode via Callback. [Shellcode]");
                                Console.WriteLine("|\t  10) EnumDesktops: Execute Shellcode via Callback. [Shellcode]");
                                break;
                            case "VERSION":
                                Console.WriteLine("|");
                                for (int i = 0; i <availableCSCVersions.Count(); i++)
                                {
                                    Console.WriteLine("|\t{0}) {1}", (i + 1).ToString(), availableCSCVersions.ElementAt(i).Key);
                                }
                                Console.WriteLine("|\t{0}) {1}", (availableCSCVersions.Count() + 1).ToString(), "Custom");
                                break;
                            case "VERSIONS":
                                Console.WriteLine("|");
                                for (int i = 0; i < availableCSCVersions.Count(); i++)
                                {
                                    Console.WriteLine("|\t{0}) {1}", (i + 1).ToString(), availableCSCVersions.ElementAt(i).Key);
                                }
                                Console.WriteLine("|\t{0}) {1}", (availableCSCVersions.Count() + 1).ToString(), "Custom");
                                break;
                        }
                    }
                    else
                    {
                        Program.PrintSettings();
                    }
                    break;
                case "COMPILE":
                    if (Settings.CompileBinary)
                    {
                        Settings.CompileBinary = false;
                        Console.WriteLine("|\n|   [~] Compile has been set to false.");
                        Program.WriteLog("Compiling Disabled", true);
                    }
                    else if (!Settings.CompileBinary)
                    {
                        Settings.CompileBinary = true;
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
                                Settings.InjectMode = null;
                                Settings.OutputDirectory = null;
                                Settings.CompileBinary = true;
                                Console.WriteLine("|\n|   [~] Output and Mode have been cleared.");
                                break;
                            case "TRIGGERS":
                                Program.HIBERNATEPROCESSDETAILS = "";
                                Program.REQUIREDPROCESSDETAILS = "";
                                Console.WriteLine("|\n|   [~] Triggers have been cleared.");
                                break;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n       +---------------------------+\n ______|         SETTINGS          |\n|      +---------------------------+");
                        if (helpBlurb)
                        {
                            Console.WriteLine("|\n|\tmode      output   show   ");
                            Console.WriteLine("|\tcompile   blurb    triggers");
                            Console.WriteLine("|\tversion   clear    help");
                            Console.WriteLine("|\tlog       exit");
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
