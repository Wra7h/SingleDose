using SingleDose.Menus;
using SingleDose.Misc;
using System;
using System.Linq;

namespace SingleDose
{
    internal class Program
    {
        public static string sCurrentMenu = "Main";
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.WindowWidth + Console.WindowWidth/5 + 5, Console.WindowHeight + Console.WindowHeight/5);
            Console.Clear();
            // Get csc.exe filepaths
            SettingsMenu.dAvailableCSCVersions = SettingsMenu.FetchCSCVersions();
            
            // Identify the triggers that are available
            Reflect.InitializeTriggers();

            // Identify the techniques that are available
            Reflect.InitializeTechniques();

            if (args.Contains("-t"))
                Tutorial.StartTutorial();
            else
                Start();
        }

        static void Start()
        {
            string Command = null;

            SDConsole.PrintHeader();
            SDConsole.PrintSettings(Console.WindowWidth - 59, SDConsole.iConsoleLineNum);
            SDConsole.PrintCommandHelp(Console.WindowWidth - 59, SDConsole.iConsoleLineNum+9, sCurrentMenu);
            SDConsole.iConsoleLineNum = 0;
            while (true)
            {
                Console.SetCursorPosition(0, SDConsole.iConsoleLineNum);
                Console.Write("["+sCurrentMenu+"]-> ");
                Command = Console.ReadLine();
                switch(sCurrentMenu)
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

                SDConsole.iConsoleLineNum++;
            }
        }
    }
}
