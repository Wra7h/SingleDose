using SingleDose.Misc;
using SingleDose.Triggers;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SingleDose.Menus
{
    internal class TriggersMenu
    {
        public static ITrigger SelectedTrigger = null;
        public static string TriggerBody = null;
        public static void CommandHandler(string Command)
        {
            string UserInput = null;

            switch (Command.ToUpper().Split()[0].Trim())
            {
                case "":
                    break;
                case "USE":

                    if (Command.Split().Length < 2 || Command.Split()[1] == "")
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("   Available triggers:");
                        Console.ResetColor();
                        SDConsole.iConsoleLineNum++;

                        int i = 1;
                        foreach (ITrigger t in Reflect.TriggersFound)
                        {
                            string description = SDConsole.SpliceText(t.TriggerDescription, 60);
                            SDConsole.Write(String.Format("{0}) {1}: {2}", i++, t.TriggerName, description.TrimEnd()));
                        }
                        break;
                    }


                    if (Reflect.TriggersFound.Any(c => c.TriggerName.ToUpper() == Command.Split()[1].ToUpper().Trim()))
                    {
                        SelectedTrigger = Reflect.TriggersFound.Find(s => s.TriggerName.ToUpper() == Command.Split()[1].ToUpper().Trim());
                    }

                    if (SelectedTrigger == null)
                    {
                        SDConsole.WriteError("Unknown trigger.");
                        break;
                    }

                    SDConsole.RefreshConfigPanel();
                    Console.SetCursorPosition(0, SDConsole.iConsoleLineNum + 1);

                    //Ask any specified questions to get necessary data for regex replacement.
                    if (SelectedTrigger.ReqQuestions != null)
                    {
                        if (SelectedTrigger.ReqQuestions.Count != SelectedTrigger.ReqPatterns.Count)
                        {
                            SDConsole.WriteError("Trigger: Question/Pattern count mismatch.");
                            SelectedTrigger = null;
                            break;
                        }

                        TriggerBody = SelectedTrigger.Base;
                        for (int i = 0; i < SelectedTrigger.ReqQuestions.Count; i++)
                        {
                            SDConsole.WriteInfo(SelectedTrigger.ReqQuestions[i]);
                            do
                            {
                                Console.Write("       > ");
                                UserInput = Console.ReadLine();
                                SDConsole.iConsoleLineNum++;
                            } while (UserInput == null);

                            if (UserInput.StartsWith("exit", StringComparison.OrdinalIgnoreCase))
                            {
                                SelectedTrigger = null;
                                SDConsole.RefreshConfigPanel();
                                Console.SetCursorPosition(0, SDConsole.iConsoleLineNum);
                                return;
                            }

                            Regex pattern = new Regex(SelectedTrigger.ReqPatterns[i]);
                            TriggerBody = pattern.Replace(TriggerBody, UserInput);
                        }
                    }
                    break;
                case "RECONFIG":
                    if (TriggerBody == null)
                    {
                        SDConsole.WriteError("No trigger to reconfigure.");
                        break;
                    }

                    if (SelectedTrigger.ReqQuestions != null)
                    {
                        if (SelectedTrigger.ReqQuestions.Count != SelectedTrigger.ReqPatterns.Count)
                        {
                            SDConsole.WriteError("Trigger: Question/Pattern count mismatch.");
                            SelectedTrigger = null;
                            break;
                        }

                        TriggerBody = SelectedTrigger.Base;
                        for (int i = 0; i < SelectedTrigger.ReqQuestions.Count; i++)
                        {
                            SDConsole.WriteInfo(SelectedTrigger.ReqQuestions[i]);
                            do
                            {
                                Console.Write("       > ");
                                UserInput = Console.ReadLine();
                                SDConsole.iConsoleLineNum++;
                            } while (UserInput == null);

                            if (UserInput.StartsWith("exit", StringComparison.OrdinalIgnoreCase))
                            {
                                SelectedTrigger = null;
                                SDConsole.RefreshConfigPanel();
                                Console.SetCursorPosition(0, SDConsole.iConsoleLineNum);
                                return;
                            }

                            Regex pattern = new Regex(SelectedTrigger.ReqPatterns[i]);
                            TriggerBody = pattern.Replace(TriggerBody, UserInput);
                        }
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
                case "SETTINGS":
                    Program.sCurrentMenu = "Settings";
                    SDConsole.RefreshConfigPanel();
                    break;
                case "EXIT":
                    Program.sCurrentMenu = "Main";
                    SDConsole.RefreshConfigPanel();
                    break;
                case "HELP":
                    Console.WriteLine("");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Command   |             Description                |       Usage       |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Use       | Configure and apply a new trigger      | > use             |");
                    Console.WriteLine("    |            |                                        | > use timer       |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Load      | Load a booster                         | > load <path>     |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Reconfig  | Reconfigure the settings for the       | > reconfig        |");
                    Console.WriteLine("    |            | current trigger                        |                   |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Settings  | Enter the Settings submenu             | > settings        |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Clear     | Clear the terminal, settings, or       | > clear           |");
                    Console.WriteLine("    |            | triggers                               | > clear settings  |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("    |  Exit      | Return to Main Menu                    | > exit            |");
                    Console.WriteLine("    +------------+----------------------------------------+-------------------+");
                    Console.WriteLine("");
                    SDConsole.iConsoleLineNum += 20;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("                           Available Triggers");
                    Console.ResetColor();
                    SDConsole.iConsoleLineNum++;
                    foreach (ITrigger t in Reflect.TriggersFound)
                    {
                        string description = SDConsole.SpliceText(t.TriggerDescription, 65);
                        SDConsole.Write(String.Format("- {0}: {1}", t.TriggerName, description));
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
