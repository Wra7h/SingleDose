using SingleDose.Menus;
using SingleDose.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SingleDose
{
    internal class Program
    {
        public static string sCurrentMenu = "Main";
        static void Main(string[] args)
        {
            Console.SetWindowSize(149, 36);
            Console.Clear();
            // Get csc.exe filepaths
            SettingsMenu.dAvailableCSCVersions = SettingsMenu.FetchCSCVersions();
            
            // Identify the triggers that are available
            Reflect.InitializeTriggers();

            // Identify the techniques that are available
            Reflect.InitializeTechniques();

            // Identify the APIs that are available
            Reflect.InitializeInvokes();

            if (args.Contains("-t"))
                Tutorial.StartTutorial();
            else
                Start();
        }

        static void Start()
        {

            SDConsole.PrintHeader();
            SDConsole.PrintSettings(Console.WindowWidth - 59, SDConsole.iConsoleLineNum);
            SDConsole.PrintCommandHelp(Console.WindowWidth - 59, SDConsole.iConsoleLineNum+9, sCurrentMenu);
            SDConsole.iConsoleLineNum = 0;

            List<string> CommandHistory = new List<string>();

            while (true)
            {
                string Command = null;

                //Getting the command used to just be Console.ReadLine(), but tab complete seems
                //to need a significant amount of code. So it is being implemented from it's own
                //dedicated file: SDTabComplete.cs.
                Command = SDTabComplete.UseTabComplete(sCurrentMenu, CommandHistory);

                if (!String.IsNullOrEmpty(Command))
                    CommandHistory.Add(Command);

                if (!String.IsNullOrEmpty(Command))
                {
                    Console.SetCursorPosition(0, SDConsole.iConsoleLineNum + 1);
                    switch (sCurrentMenu)
                    {
                        case "Main":
                            MainMenu.CommandHandler(Command);
                            break;
                        case "Settings":
                            SettingsMenu.CommandHandler(Command);
                            break;
                        case "Triggers":
                            TriggersMenu.CommandHandler(Command);
                            break;
                        default:
                            break;
                    }
                }

                SDConsole.iConsoleLineNum++;
            }
        }
    }
}
