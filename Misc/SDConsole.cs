using SingleDose.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SingleDose.Misc
{
    internal class SDConsole
    {
        public static int iConsoleLineNum = 0;
        public static bool bvShowHeader = true;
        public static void PrintHeader()
        {
            Console.WriteLine(@"
                                                                                                    █▀▀ ▀█▀ █▀█ █▀▀ █   █▀▀ █▀▄ █▀█ █▀▀ █▀▀
                                                                                                    ▀▀█  █  █ █ █ █ █   █▀▀ █ █ █ █ ▀▀█ █▀▀
                                                                                                    ▀▀▀ ▀▀▀ ▀ ▀ ▀▀▀ ▀▀▀ ▀▀▀ ▀▀  ▀▀▀ ▀▀▀ ▀▀▀
                                                                                                                            [GitHub: Wra7h]");
            Console.SetCursorPosition(Console.WindowWidth - 59, 5);
            Console.Write("+{0}",String.Concat(Enumerable.Repeat("-", 58).ToArray()));
            SDConsole.iConsoleLineNum += 6;
        }

        public static void PrintSettings(int iStartPrint, int iLine)
        {
            Console.SetCursorPosition(iStartPrint, iLine);
            Console.Write("| Builds: ");
            Console.Write("{0}\n", SettingsMenu.SuccessfulBuildCount);
            iLine++;
            Console.SetCursorPosition(iStartPrint, iLine);

            Console.Write("| Output: ");
            if (SettingsMenu.OutputDirectory != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (SettingsMenu.OutputDirectory.Length > 48)
                {
                    Console.Write("{0}...\n", SettingsMenu.OutputDirectory.Substring(0,45));
                }
                else
                {
                    Console.Write("{0}\n", SettingsMenu.OutputDirectory);
                }
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("-{0}\n", String.Concat(Enumerable.Repeat(" ", 47).ToArray()));
                Console.ResetColor();
            }
            iLine++;
            Console.SetCursorPosition(iStartPrint, iLine);

            Console.Write("| Mode: ");
            if (SettingsMenu.szInjectMode != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("{0}  \n", SettingsMenu.szInjectMode);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("-{0}\n", String.Concat(Enumerable.Repeat(" ", 10).ToArray()));
                Console.ResetColor();
            }
            iLine++;
            Console.SetCursorPosition(iStartPrint, iLine);

            Console.Write("| Allocation: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("{0}\n", SettingsMenu.szMemAlloc);
            Console.ResetColor();
            iLine++;
            Console.SetCursorPosition(iStartPrint, iLine);

            Console.Write("| Invoke: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("{0}\n", SettingsMenu.szInvokeMethod);
            Console.ResetColor();
            iLine++;
            Console.SetCursorPosition(iStartPrint, iLine);

            Console.Write("| Version: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("{0}\n", SettingsMenu.SelectedCscVersion);
            Console.ResetColor();
            iLine++;
            Console.SetCursorPosition(iStartPrint, iLine);

            Console.Write("| Logging: ");
            if (SettingsMenu.UseLogging)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Enabled  \n");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Disabled\n");
                Console.ResetColor();
            }
            iLine++;
            Console.SetCursorPosition(iStartPrint, iLine);

            Console.Write("| Compile: ");
            if (SettingsMenu.CompileBinary)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Enabled  \n");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Disabled\n");
                Console.ResetColor();
            }
            iLine++;
            Console.SetCursorPosition(iStartPrint, iLine);
            if (TriggersMenu.SelectedTrigger == null)
            {
                Console.WriteLine("| Trigger: -         ");
            }
            else
            {
                Console.Write("| Trigger: ");
                if (TriggersMenu.TriggerBody != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                Console.WriteLine(TriggersMenu.SelectedTrigger.TriggerName);
                Console.ResetColor();
            }
            iLine++;
            Console.SetCursorPosition(iStartPrint, iLine);
            Console.Write("{0}{1}", "+", String.Concat(Enumerable.Repeat("-", 58).ToArray()));

        }
        public static void PrintCommandHelp(int iStartPrint, int iLine, string sMenu)
        {
            ClearCommandPanel(iStartPrint, iLine);

            List<string> AvailableCommands = new List<string>();
            AvailableCommands = GetMenuCommands(sMenu).ToList();

            Console.SetCursorPosition(iStartPrint, iLine);

            Console.Write("| Available Commands: ");
            iLine++;
            Console.SetCursorPosition(iStartPrint, iLine);
            for (int i = 0; i < AvailableCommands.Count; i++)
            {
                Console.Write("| ");
                if (i + 2 < AvailableCommands.Count)
                {
                    Console.Write("{0,-10}{1,-10}{2,-10}", AvailableCommands[i], AvailableCommands[i+1], AvailableCommands[i+2]);
                    i = i + 2;
                }
                else if (i + 1 < AvailableCommands.Count)
                {
                    Console.Write("{0,-10}{1,-10}", AvailableCommands[i], AvailableCommands[i+1]);
                    i++;
                }
                else if (i <= AvailableCommands.Count-1)
                {
                    Console.Write("{0,-10}", AvailableCommands[i]);
                }
                Console.Write("\n");
                iLine++;
                Console.SetCursorPosition(iStartPrint, iLine);
            }

            Console.Write("{0}{1}", "+", String.Concat(Enumerable.Repeat("-", 58).ToArray()));
            iLine++;
            Console.SetCursorPosition(iStartPrint, iLine);
            Console.Write("{0}", String.Concat(Enumerable.Repeat(" ", 59).ToArray()));



            //TODO
            SDConsole.PrintBuilds(Console.WindowWidth - 59, iLine);
        }

        public static void PrintBuilds(int iStartPrint, int iLine)
        {
            Console.SetCursorPosition(iStartPrint, iLine);
            Console.Write("| Previous Builds: ");
            iLine++;
            if (Build.CompiledFiles.Count > 0)
            {   
                foreach (string file in Build.CompiledFiles)
                {
                    Console.SetCursorPosition(iStartPrint, iLine);
                    Console.Write("| ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("{0}\n", file);
                    Console.ResetColor();
                    iLine++;
                }
            }
            else
            {
                Console.SetCursorPosition(iStartPrint, iLine);
                Console.Write("| ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("-\n");
                Console.ResetColor();
                iLine++;
            }
            Console.SetCursorPosition(iStartPrint, iLine);
            Console.Write("{0}{1}", "+", String.Concat(Enumerable.Repeat("-", 58).ToArray()));
        }

        public static void ClearCommandPanel(int iStartPrint, int iLine)
        {
            for(int i = 0; i < 15; i++)
            {
                Console.SetCursorPosition(iStartPrint, iLine + i);
                Console.Write("{0}", String.Concat(Enumerable.Repeat(" ", 59).ToArray()));
            }
        }
        public static void RefreshConfigPanel()
        {
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
        }
        public static void WriteError(string message)
        {
            SDLog.AddEntry("[ERROR] " + message);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("   [!] ");
            Console.ResetColor();
            Console.WriteLine(message);
            SDConsole.iConsoleLineNum += CountLines(message);
            return;
        }
        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("   [~] ");
            Console.ResetColor();
            Console.WriteLine(message);
            SDConsole.iConsoleLineNum += CountLines(message);
            return;
        }
        public static void WriteInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("   [*] ");
            Console.ResetColor();
            Console.WriteLine(message);
            SDConsole.iConsoleLineNum += CountLines(message);
            return;
        }
        public static void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("   [+] ");
            Console.ResetColor();
            Console.WriteLine(message);
            SDConsole.iConsoleLineNum += CountLines(message);
            return;
        }
        public static void Write(string message)
        {
            Console.Write("       ");
            Console.WriteLine(message);
            SDConsole.iConsoleLineNum += CountLines(message);
            return;
        }

        //Count the number of lines produced by a string
        public static int CountLines(string str)
        {
            int lines = 1;
            int index = 0;
            while (true)
            {
                index = str.IndexOf('\n', index);
                if (index < 0)
                    break;
                lines++;
                index++;
            }
            return lines;
        }

        
        public static string SpliceURLs(string text, int lineLength)
        {
            return Regex.Replace(text, "(.{" + lineLength + "})", "$1" + ("\n\t "));
        }


        public static string SpliceText(string inputText, int lineLength)
        {
            if (inputText.Length <= lineLength || inputText == null)
            {
                return inputText;
            }

            string[] stringSplit = inputText.Split(' ');
            int charCounter = 0;
            string finalString = "";

            for (int i = 0; i < stringSplit.Length; i++)
            {
                finalString += stringSplit[i] + " ";
                charCounter += stringSplit[i].Length + 1;

                if (charCounter > lineLength)
                {
                    finalString += "\n\t ";
                    charCounter = 0;
                }
            }
            return finalString;
        }

        public static string[] GetMenuCommands(string sMenu)
        {
            List<string> AvailableCommands = new List<string>();
            switch (sMenu)
            {
                case "Main":
                    AvailableCommands.AddRange(new string[] { "settings", "triggers", "build", "describe", "reconfig", "show", "load", "help", "clear", "save", "exit" });
                    break;
                case "Settings":
                    AvailableCommands.AddRange(new string[] { "mode", "output", "triggers", "compile", "version", "invoke", "memset", "clear", "help", "history", "log", "exit" });
                    break;
                case "Triggers":
                    AvailableCommands.AddRange(new string[] { "use", "reconfig", "load", "settings", "help", "clear", "exit" });
                    break;
                default:
                    break;
            }

            return AvailableCommands.ToArray();
        }
    }
}
