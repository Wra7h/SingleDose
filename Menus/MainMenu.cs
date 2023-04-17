using SingleDose.Misc;
using SingleDose.Techniques;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SingleDose.Menus
{
    internal class MainMenu
    {
        public static void CommandHandler(string Command)
        {
            ITechnique[] Loaders = Reflect.TechniquesFound.Where(c => c.IsLoader).ToArray();
            ITechnique[] Injects = Reflect.TechniquesFound.Where(c => !c.IsLoader).ToArray();

            switch (Command.ToUpper().Split()[0])
            {
                case "":
                    break;
                case "HELP":
                    Console.WriteLine("");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Command   |             Description                |       Usage       |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Settings  | Enter the Settings submenu             | > settings        |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Triggers  | Enter the Triggers submenu             | > triggers        |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Build     | Build a loader/inject technique        | > build r1        |");
                    Console.WriteLine("    |            | (See techniques below.)                | > build settimer  |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Save      | Save an entry from history to a file.  | > save h1         |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Show      | Display current config, techniques     | > show            |");
                    Console.WriteLine("    |            | or history entries                     | > show history    |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Describe  | See a description for a technique      | > describe r5     |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Load      | Load a booster                         | > load <path>     |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Reconfig  | Reconfigure the current trigger in use | > reconfig        |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Clear     | Clear the terminal, settings, or       | > clear           |");
                    Console.WriteLine("    |            | triggers                               | > clear triggers  |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Exit      | Exit Single Dose                       | > exit            |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("");
                    SDConsole.iConsoleLineNum += 28;
                    Console.WriteLine("                   LOADERS                          INJECTS            ");
                    Console.WriteLine("     +---------------------------------+-------------------------------+");
                    SDConsole.iConsoleLineNum += 2;
                    for (int i = 1; i <= Loaders.Count(); i++)
                    {
                        if (i - 1 < Injects.Count())
                        {
                            Console.WriteLine("     | L{0}. {1, -27} | R{2}. {3,-25} |", i, Loaders[i - 1].TechniqueName, i, Injects[i - 1].TechniqueName);
                        }
                        else
                        {
                            Console.WriteLine("     | L{0}. {1, -26} |{2,-30} |", i, Loaders[i - 1].TechniqueName, "");
                        }
                        SDConsole.iConsoleLineNum++;
                    }
                    Console.WriteLine("     +---------------------------------+-------------------------------+");
                    Console.WriteLine("");
                    SDConsole.iConsoleLineNum += 2;
                    break;
                case "SETTINGS":
                    Program.sCurrentMenu = "Settings";
                    int cLineHolder = SDConsole.iConsoleLineNum;
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
                case "TRIGGERS":
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
                case "BUILD":
                    if (SettingsMenu.szInjectMode != null && SettingsMenu.OutputDirectory != null)
                    {
                        ITechnique technique = null;
                        if (Command.Split().Count() > 1)
                        {
                            if (Command.Split()[1].ToUpper().StartsWith("L") && Command.Split()[1].Length < 5)
                            {
                                int i = Int32.Parse(Command.Split()[1].Trim().Substring(1));
                                if ((i - 1) < Loaders.Count() && i > 0)
                                    technique = Loaders[i - 1];
                            }
                            else if (Command.Split()[1].ToUpper().StartsWith("R") && Command.Split()[1].Length < 5)
                            {
                                int i = Int32.Parse(Command.Split()[1].Trim().Substring(1));
                                if ((i - 1) < Injects.Count() && i > 0)
                                    technique = Injects[i - 1];
                            }
                            else
                            {
                                if (Reflect.TechniquesFound.Any(c => c.TechniqueName.ToUpper() == Command.Split()[1].ToUpper().Trim()))
                                {
                                    technique = Reflect.TechniquesFound.Find(s => s.TechniqueName.ToUpper() == Command.Split()[1].ToUpper().Trim());
                                }
                            }

                            if (technique == null)
                            {
                                SDConsole.WriteError("Invalid selection.");
                                break;
                            }
                            else if (technique != null)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write("   [*] ");
                                Console.ResetColor();
                                Console.Write("Building technique: ");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine(technique.TechniqueName);
                                Console.ResetColor();
                                SDConsole.iConsoleLineNum++;
                                //Build the .cs 
                                Build.BuildBody(technique);
                                technique = null;
                            }
                        }
                    }
                    else
                    {
                        SDConsole.WriteError("REQUIRED: Set mode and output directory.");
                    }
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
                case "RECONFIG":
                    if (TriggersMenu.TriggerBody == null)
                    {
                        SDConsole.WriteError("No trigger to reconfigure.");
                        break;
                    }


                    string UserInput = null;
                    if (TriggersMenu.SelectedTrigger.ReqQuestions != null)
                    {
                        if (TriggersMenu.SelectedTrigger.ReqQuestions.Count != TriggersMenu.SelectedTrigger.ReqPatterns.Count)
                        {
                            SDConsole.WriteError("Trigger: Question/Pattern count mismatch.");
                            TriggersMenu.SelectedTrigger = null;
                            break;
                        }

                        TriggersMenu.TriggerBody = TriggersMenu.SelectedTrigger.Base;
                        for (int i = 0; i < TriggersMenu.SelectedTrigger.ReqQuestions.Count; i++)
                        {
                            SDConsole.WriteInfo(TriggersMenu.SelectedTrigger.ReqQuestions[i]);
                            do
                            {
                                Console.Write("       > ");
                                UserInput = Console.ReadLine();
                                SDConsole.iConsoleLineNum++;
                            } while (UserInput == null);

                            if (UserInput.StartsWith("exit", StringComparison.OrdinalIgnoreCase))
                            {
                                TriggersMenu.SelectedTrigger = null;
                                SDConsole.RefreshConfigPanel();
                                Console.SetCursorPosition(0, SDConsole.iConsoleLineNum);
                                return;
                            }

                            Regex pattern = new Regex(TriggersMenu.SelectedTrigger.ReqPatterns[i]);
                            TriggersMenu.TriggerBody = pattern.Replace(TriggersMenu.TriggerBody, UserInput);
                        }
                    }
                    break;
                case "SHOW":
                    if (Command.Split().Count() > 1)
                    {
                        switch (Command.Split()[1].ToUpper())
                        {
                            case "TECHNIQUES":
                                
                                Console.WriteLine("");
                                Console.WriteLine("                   LOADERS                          INJECTS            ");
                                Console.WriteLine("     +---------------------------------+-------------------------------+");
                                SDConsole.iConsoleLineNum+=3;
                                for (int i = 1; i <= Loaders.Count(); i++)
                                {
                                    if (i -1 < Injects.Count())
                                    {
                                        Console.WriteLine("     | L{0}. {1, -27} | R{2}. {3,-25} |", i, Loaders[i - 1].TechniqueName,i, Injects[i-1].TechniqueName);
                                    }
                                    else
                                    {
                                        Console.WriteLine("     | L{0}. {1, -26} |{2,-30} |", i, Loaders[i - 1].TechniqueName, "");
                                    }
                                    SDConsole.iConsoleLineNum++;
                                }
                                Console.WriteLine("     +---------------------------------+-------------------------------+");
                                Console.WriteLine("");
                                SDConsole.iConsoleLineNum+=2;
                                break;
                            case "HISTORY":
                                Shellcode.DisplayHistory();
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("   Available subcommands:");
                        Console.ResetColor();
                        Console.WriteLine("      history   techniques");
                        SDConsole.iConsoleLineNum += 2;
                    }
                    break;
                case "SAVE":
                    string item = "";
                    if (Command.Split().Count() > 1)
                    {
                        if (Command.Split()[1].Length < 3 && Shellcode.History.Count > 0)
                        {
                            item = Command.Split()[1];
                        }
                        else
                        {
                            SDConsole.WriteError("Invalid option.");
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                    
                    
                    if (!System.IO.Directory.Exists(SettingsMenu.OutputDirectory + @"\Payloads"))
                    {
                        try
                        {
                            Directory.CreateDirectory(SettingsMenu.OutputDirectory + @"\Payloads");
                        }
                        catch
                        {
                            SDConsole.WriteError("Error creating payload directory. Please try again.");
                            break;
                        }
                    }
                    
                    int entry = int.Parse(item[1].ToString());
                    if (entry <= Shellcode.History.Count && entry != 0)
                    {
                        string payloadFilename = Path.GetRandomFileName().Split('.')[0] + ".bin";
                        string payloadFullPath = SettingsMenu.OutputDirectory + @"\Payloads\" + payloadFilename;
                        File.WriteAllBytes(payloadFullPath, Shellcode.History[entry - 1].Shellcode);
                    
                        if (File.Exists(payloadFullPath))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("   [+] Payload saved: ");
                            Console.ResetColor();
                            Console.WriteLine("Payloads\\{0}", payloadFilename);
                            SDConsole.iConsoleLineNum++;
                        }
                        else
                        {
                            SDConsole.WriteError("Error saving payload.");
                        }
                    }
                    break;
                case "DESCRIBE":
                    if (Command.Split().Count() > 1)
                    {
                        ITechnique technique = null;
                        
                        if (Command.Split()[1].ToUpper().StartsWith("L") && Command.Split()[1].Length < 5)
                        {
                            int i = Int32.Parse(Command.Split()[1].Trim().Substring(1));
                            if ((i - 1) < Loaders.Count() && i > 0)
                                technique = Loaders[i - 1];
                        }
                        else if (Command.Split()[1].ToUpper().StartsWith("R") && Command.Split()[1].Length < 5)
                        {
                            int i = Int32.Parse(Command.Split()[1].Trim().Substring(1));
                            if ((i - 1) < Injects.Count() && i > 0)
                                technique = Injects[i - 1];
                        }
                        else
                        {
                            if (Reflect.TechniquesFound.Any(c => c.TechniqueName.ToUpper() == Command.Split()[1].ToUpper().Trim()))
                            {
                                technique = Reflect.TechniquesFound.Find(s => s.TechniqueName.ToUpper() == Command.Split()[1].ToUpper().Trim());
                            }
                        }

                        if (technique == null)
                        {
                            SDConsole.WriteError("Invalid selection.");
                            break;
                        }

                        //Display Name
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        SDConsole.Write("");
                        Console.Write("   Name: ");
                        Console.ResetColor();
                        Console.WriteLine(technique.TechniqueName);
                        SDConsole.iConsoleLineNum++;

                        //Display PInvokes
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("   APIs: ");
                        Console.ResetColor();
                        List<string> APIs = technique.Invokes;

                        if (SettingsMenu.szMemAlloc == "RW/RX" && technique.VProtect != null)
                        {
                            APIs.Add("VirtualProtectEx");
                        }

                        int index = APIs.FindIndex(s => s == "WriteProcessMemory_ByteArray");
                        if (index != -1)
                            APIs[index] = "WriteProcessMemory";

                        index = APIs.FindIndex(s => s == "WriteProcessMemory_IntPtr");
                        if (index != -1)
                            APIs[index] = "WriteProcessMemory";

                        for (int i = 0; i < APIs.Count; i++)
                        {
                            if (i + 3 < APIs.Count)
                            {
                                Console.Write("{0}, {1}, {2}, {3}", APIs[i], APIs[i + 1], APIs[i + 2], APIs[i + 3]);
                                i = i + 3;
                            }
                            else if (i + 2 < APIs.Count)
                            {
                                Console.Write("{0}, {1}, {2}", APIs[i], APIs[i + 1], APIs[i + 2]);
                                i = i + 2;
                            }
                            else if (i + 1 < APIs.Count)
                            {
                                Console.Write("{0}, {1}", APIs[i], APIs[i + 1]);
                                i = i + 1;
                            }
                            else
                            {
                                Console.Write("{0}", APIs[i]);
                            }
                            if (i < APIs.Count - 1)
                                Console.Write("\n\t ");
                            else
                                Console.Write("\n");
                            SDConsole.iConsoleLineNum++;
                        }

                        //Display description
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("   Description:");
                        Console.ResetColor();

                        if (technique.TechniqueDescription != null)
                        {
                            string description = SDConsole.SpliceText(technique.TechniqueDescription, 70);
                            Console.WriteLine("\n\t {0}", description);
                            SDConsole.iConsoleLineNum += SDConsole.CountLines(description) + 1;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(" -");
                            Console.ResetColor();
                            SDConsole.iConsoleLineNum++;
                        }

                        //Display References
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("   References: ");
                        Console.ResetColor();
                        SDConsole.iConsoleLineNum++;

                        foreach(string reference in technique.TechniqueReferences)
                        {
                            string techref = SDConsole.SpliceURLs(reference, 70);
                            Console.WriteLine("\t- {0}", techref);
                            SDConsole.iConsoleLineNum += SDConsole.CountLines(techref) + 1;
                        }

                    }
                    break;
                case "EXIT":
                    if (MiscFuncs.ConfirmExit())
                    {
                        Console.Clear();
                        Environment.Exit(0);
                    }
                    break;
                case "LOAD":
                    if (Command.Split().Count() > 1)
                    {
                        if (File.Exists(Command.Split()[1]))
                        {
                            string szFullPath = Path.GetFullPath(Command.Split()[1]);

                            int cTechniques = Reflect.TechniquesFound.Count();
                            int cTriggers = Reflect.TriggersFound.Count();

                            bool bRet = Reflect.LoadBoosterFromPath(szFullPath);
                            if (bRet)
                            {
                                SDConsole.WriteSuccess(String.Format("Loaded module: {0}", Path.GetFileName(szFullPath)));
                                SDConsole.Write(String.Format("+{0} techniques", (Reflect.TechniquesFound.Count() - cTechniques).ToString()));
                                SDConsole.Write(String.Format("+{0} triggers", (Reflect.TriggersFound.Count() - cTriggers).ToString()));
                            }
                            else
                            {
                                SDConsole.WriteError("Module load failed.");
                            }
                        }
                        else
                        {
                            SDConsole.WriteError(String.Format("File not found: {0}", Command.Split()[1]));
                        }
                    }
                    break;
                default:
                    SDConsole.WriteError(String.Format("Unknown Command: {0}", Command));
                    break;
            }
        }
    }
}

