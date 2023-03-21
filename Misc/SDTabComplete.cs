using System;
using System.Collections.Generic;
using System.Linq;

namespace SingleDose.Misc
{
    internal class SDTabComplete
    {
        public static string UseTabComplete(string szCurrentMenu, List<string> listszCommandHistory)
        {
            string szCommand = "";
            bool bvEnterPressed = false;
            int iHistoryEntry = listszCommandHistory.Count();

            while (!bvEnterPressed)
            {
                Console.SetCursorPosition(0, SDConsole.iConsoleLineNum);
                Console.Write("[" + szCurrentMenu + "]-> ");
                Console.Write(szCommand);

                //input will hold the key press by user
                ConsoleKeyInfo input = Console.ReadKey();

                //Handle the key as necessary
                switch (input.Key)
                {
                    case ConsoleKey.Tab:
                        #region TabKey
                        // This will hold the commands/subcommands available
                        List<string> listszCommands = new List<string>() { };

                        // Commands that have subcommands available
                        if (szCurrentMenu == "Main")
                        {
                            if (szCommand.StartsWith("build ") || szCommand.StartsWith("describe "))
                            {
                                listszCommands = Reflect.TechniquesFound.Select(x => x.TechniqueName).ToList();
                            }
                            else if (szCommand.StartsWith("show "))
                            {
                                listszCommands = new List<string>() { "history", "techniques" };
                            }
                            else if (szCommand.StartsWith("clear "))
                            {
                                listszCommands = new List<string>() { "triggers", "settings" };
                            }
                        }
                        else if (szCurrentMenu == "Settings")
                        {
                            if (szCommand.StartsWith("mode "))
                            {
                                listszCommands = new List<string>() { "static", "dynamic", "download" };
                            }
                            else if (szCommand.StartsWith("version "))
                            {
                                listszCommands = Menus.SettingsMenu.dAvailableCSCVersions.Select(x => x.Key).ToList();
                                listszCommands.Add("custom");
                            }
                            else if (szCommand.StartsWith("memset "))
                            {
                                listszCommands = new List<string>() { "RWX", "RW/RX"};
                            }
                            else if (szCommand.StartsWith("clear "))
                            {
                                listszCommands = new List<string>() { "triggers", "settings" };
                            }
                            else if (szCommand.StartsWith("invoke "))
                            {
                                listszCommands = new List<string>() { "pinvoke", "dinvoke" };
                            }
                        }
                        else if (szCurrentMenu == "Triggers")
                        {
                            if (szCommand.StartsWith("use "))
                            {
                                listszCommands = Reflect.TriggersFound.Select(x => x.TriggerName.ToLower()).ToList();
                            }
                        }

                        // Default to the commands that match previously typed keys,
                        // or start iterating through the menu's commands
                        if (listszCommands.Count() == 0)
                        {
                            if (String.IsNullOrEmpty(szCommand))
                                listszCommands = SDConsole.GetMenuCommands(szCurrentMenu).ToList();
                            else
                                listszCommands = SDTabComplete.GetMatchingItems(szCommand, SDConsole.GetMenuCommands(szCurrentMenu)).ToList();
                        }

                        // The list is still empty, clear the command line and break
                        if (listszCommands.Count() == 0)
                        {
                            ResetCommandLine(szCommand);
                            break;
                        }

                        // We'll use this after getting the substring.
                        ConsoleKeyInfo SLastKey = new ConsoleKeyInfo();

                        //Actually get the subcommand now based on the supplied string array
                        if (szCommand.Split().Length < 2)
                        {
                            if (!String.IsNullOrEmpty(szCommand.Split()[0]))
                                listszCommands = GetMatchingItems(szCommand.Split()[0], listszCommands.ToArray()).ToList();

                            szCommand = GetBaseCommand(szCurrentMenu, listszCommands, ref SLastKey);
                        }
                        else if (szCommand.Split().Length == 2)
                        {
                            if (!String.IsNullOrEmpty(szCommand.Split()[1]))
                                listszCommands = GetMatchingItems(szCommand.Split()[1], listszCommands.ToArray()).ToList();
                            szCommand = AppendSubCommand(szCurrentMenu, szCommand, listszCommands, ref SLastKey);
                        }

                        // Handle the last key that was pressed when inside
                        // GetBaseCommand() && AppendSubCommand() functions
                        if (SLastKey.Key == ConsoleKey.Enter)
                        {
                            goto case ConsoleKey.Enter;
                        }
                        else if (SLastKey.Key == ConsoleKey.Backspace)
                        {
                            goto case ConsoleKey.Backspace;
                        }
                        else if (SLastKey.KeyChar != '\0')
                        {
                            input = SLastKey;
                            goto default;
                        }

                        #endregion
                        break;
                    case ConsoleKey.Enter:
                        //Break out of the while loop.
                        bvEnterPressed = true;
                        break;
                    case ConsoleKey.Backspace:
                        // Check to make sure the string is not empty.
                        if (!String.IsNullOrEmpty(szCommand))
                        {
                            //Clear what was typed
                            ResetCommandLine(szCommand);

                            //Remove the last char in the command
                            szCommand = szCommand.Remove(szCommand.Length - 1, 1);
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (listszCommandHistory.Count() != 0 && iHistoryEntry == 0)
                            iHistoryEntry = listszCommandHistory.Count();

                        if (iHistoryEntry != 0)
                        {
                            ResetCommandLine(szCommand);
                            szCommand = listszCommandHistory[--iHistoryEntry];
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (listszCommandHistory.Count() != 0 && iHistoryEntry == listszCommandHistory.Count())
                            iHistoryEntry = 0;

                        if (iHistoryEntry != listszCommandHistory.Count())
                        {
                            ResetCommandLine(szCommand);
                            szCommand = listszCommandHistory[iHistoryEntry++];
                        }
                        break;
                    case ConsoleKey.Delete:
                        break;
                    case ConsoleKey.LeftArrow:
                        break;
                    case ConsoleKey.RightArrow:
                        break;
                    default:
                        szCommand += input.KeyChar.ToString();
                        break;
                }
            }

            return szCommand;
        }

        public static string GetBaseCommand(string szCurrentMenu, List<string> listszCommands, ref ConsoleKeyInfo SLastKey)
        {
            int x = 0;
            string szCommand = "";
            
            if (listszCommands.Count() == 0)
                return szCommand;

            do
            {
                ResetCommandLine(listszCommands[x]);
                
                if (SLastKey.Key == ConsoleKey.Tab)
                    x++;

                if (x >= listszCommands.Count())
                    x = 0;

                Console.SetCursorPosition(0, SDConsole.iConsoleLineNum);
                Console.Write("[" + szCurrentMenu + "]-> ");
                Console.Write(listszCommands[x]);
            } while ((SLastKey = Console.ReadKey()).Key == ConsoleKey.Tab);

            szCommand = listszCommands[x];
            return szCommand;
        }

        public static string AppendSubCommand(string szCurrentMenu, string szCommand, List<string> listszSubCommands, ref ConsoleKeyInfo SLastKey)
        {
            // Validation
            if (listszSubCommands.Count() == 0)
                return szCommand;
            // End Validation 

            int x = 0;
            string szTemp = "";
            szCommand = szCommand.Split()[0] + " ";
            do
            {
                ResetCommandLine(szCommand + szTemp);
                szTemp = listszSubCommands[x++];

                Console.SetCursorPosition(0, SDConsole.iConsoleLineNum);
                Console.Write("[" + szCurrentMenu + "]-> ");
                Console.Write(szCommand);
                Console.Write(szTemp);

                if (x >= listszSubCommands.Count())
                    x = 0;
            } while ((SLastKey = Console.ReadKey()).Key == ConsoleKey.Tab);

            szCommand = szCommand + szTemp;
            return szCommand;
        }

        public static string[] GetMatchingItems(string szInput, string[] arrszCompareArr)
        {
            string[] arrszResults = new string[]{};
            
            if (szInput != null)
                arrszResults = arrszCompareArr.Where(x => x.ToLower().StartsWith(szInput.ToLower())).ToArray();
            else
                arrszResults = arrszCompareArr;

            return arrszResults;
        }

        public static void ResetCommandLine(string szCommand)
        {
            Console.SetCursorPosition(0, SDConsole.iConsoleLineNum);
            Console.Write(String.Concat(Enumerable.Repeat(" ", 15 + szCommand.Length).ToArray()));
        }
    }
}