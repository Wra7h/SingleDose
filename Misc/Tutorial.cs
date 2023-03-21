using System;

namespace SingleDose.Misc
{
    internal class Tutorial
    {
        struct TutorialReqs
        {
            public string Message;
            public string Step;
            public string Answer;
        }

        public static void StartTutorial()
        {
            #region Requirements
            TutorialReqs[] Reqs = new TutorialReqs[12];
            Reqs[0].Message = @"  SingleDose has three different menus: Main, Settings & Triggers.";
            Reqs[1].Message = @"  The goal of this tutorial is to walk you through how to build a technique.";
            
            Reqs[2].Message = @"  The minimum requirements to build an loader or inject with SingleDose is a mode and an output directory. These are configured from the 'settings' menu. The current menu can be seen on each new line on the left side within the square brackets.";
            Reqs[2].Step = @"  Step 1: type 'settings'";
            Reqs[2].Answer = @"SETTINGS";

            Reqs[3].Message = @"  The Settings menu allows you to configure a few different aspects of the build process such as the version of .NET Framework to use when compiling, and the memory protections used when allocating payloads.";

            Reqs[4].Message = @"  The mode setting decides how the target process (if applicable) and the payload are provided to your inject. Static mode embeds all payload data into the executable which is gathered through questions on the console at build time. Dynamic/Download modes rely on flags provided at execution.";
            Reqs[4].Step = @"  Step 2: type 'mode static'";
            Reqs[4].Answer = @"MODE STATIC";

            Reqs[5].Message = @"  The output directory will be used to store builds, source files, saved payloads and logs. If the directory you specify doesn't exist, it will be created for you.";
            Reqs[5].Step = @"  Step 3: specify a directory with 'output <filepath>'";
            Reqs[5].Answer = @"OUTPUT";

            Reqs[6].Message = @"  Those two settings are the bare minimum for building. Let's go back to Main menu to start building.";
            Reqs[6].Step = @"  Step 4: type exit to return to Main menu";
            Reqs[6].Answer = @"EXIT";

            Reqs[7].Message = @"  You can view available techniques by typing 'show techniques'";
            Reqs[7].Step = @"  Step 5: type 'show techniques'";
            Reqs[7].Answer = @"SHOW TECH";

            Reqs[8].Message = @"  To select a build you can type the full name or the corresponding L# or R#. Let's build the first loader.";
            Reqs[8].Step = @"  Step 6: type 'build l1'";
            Reqs[8].Answer = @"BUILD L1";

            Reqs[9].Message = @"  Next, let's build an inject. When prompted, feel free to make up the target process id this time.";
            Reqs[9].Step = @"  Step 7: type 'build createremotethread'";
            Reqs[9].Answer = @"BUILD CREATEREMOTETHREAD";

            Reqs[10].Message = @"  At this point, the builds were hopefully successful. You can scroll up in the console and look at the pane on the right side.";
            
            Reqs[11].Message = @"  'Previous builds' shows a list of the past 5 succesful builds. Navigate to your output directory and you will see that there is a folder with the name of the technique, and inside is a folder called 'Source' and an random-named executable. This executable is ready to go!";
            
            #endregion

            string Command = null;
            
            SDConsole.PrintHeader();
            SDConsole.PrintSettings(Console.WindowWidth - 59, SDConsole.iConsoleLineNum);
            SDConsole.PrintCommandHelp(Console.WindowWidth - 59, SDConsole.iConsoleLineNum + 10, Program.sCurrentMenu);
            SDConsole.iConsoleLineNum = 0;

            foreach (TutorialReqs req in Reqs)
            {
                Console.SetCursorPosition(0, SDConsole.iConsoleLineNum);
                //Console.Write("[" + Program.sCurrentMenu + "]-> ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                SDConsole.Write(SDConsole.SpliceText(req.Message, 70));
                Console.ResetColor();
                if (req.Step != null)
                {
                    Command = TutorialGetUserInput(req.Step, req.Answer);
                    switch (Program.sCurrentMenu)
                    {
                        case "Main":
                            Menus.MainMenu.CommandHandler(Command);
                            break;
                        case "Settings":
                            Menus.SettingsMenu.CommandHandler(Command);
                            break;
                        case "Triggers":
                            Menus.TriggersMenu.CommandHandler(Command);
                            break;
                        default:
                            break;
                    }
                }

                SDConsole.iConsoleLineNum++;
            }

            SDConsole.WriteInfo("Press Enter to exit...");
            Console.ReadLine();
            return;
        }

        static string TutorialGetUserInput(string Step, string Ans)
        {
            string input = null;
            Console.ForegroundColor = ConsoleColor.Cyan;
            SDConsole.Write(Step);
            Console.ResetColor();
            do
            {
                Console.Write("[" + Program.sCurrentMenu + "]-> ");
                input = Console.ReadLine();
                SDConsole.iConsoleLineNum++;
            } while (!input.ToUpper().StartsWith(Ans));
            return input;
        }
    }
}
