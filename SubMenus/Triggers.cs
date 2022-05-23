using System;
using System.Linq;

namespace SingleDose
{
    class Triggers
    {
        public static void TriggersMenu()
        {
            string triggersInput = null;
            Console.WriteLine("\n       +---------------------------+\n ______|         TRIGGERS          |\n|      +---------------------------+");
            if (Settings.bvHelpBlurb)
            {
                Console.WriteLine("|\n|\tavoid      persist  require");
                Console.WriteLine("|\thibernate  timer     blurb");
                Console.WriteLine("|\tsettings   clear    help");
                Console.WriteLine("|\tshow       exit");
            }

            do
            {
                Console.Write("|\n+-->> ");
                triggersInput = Console.ReadLine();
                TriggersShellCommand(triggersInput);
                if (triggersInput.ToUpper() == "SETTINGS")
                {
                    return;
                }
            } while (triggersInput.ToUpper() != "EXIT");
        }

        public static void TriggersShellCommand(string command)
        {
            switch (command.ToUpper().Split()[0])
            {
                case "HELP":
                    Console.WriteLine("|\n|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |                                   TRIGGERS HELP                                               |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Command   |                       Description                        |     Example Usage     |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Settings  | Enter the Settings submenu                               | >> settings           |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Avoid     | Do not execute if condition is met. Binary will exit if  | >> Avoid iexplore.exe |");
                    Console.WriteLine("|             |            | the condition is met. Accepts PID, Module (DLL),         | >> avoid *            |");
                    Console.WriteLine("|             |            | or Process name (EXE). 'Avoid *' will enter a prompt     |                       |");
                    Console.WriteLine("|             |            | for multiple inputs.                                     |                       |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Require   | Only execute if condition is met. Binary will exit if    | >> require 1204       |");
                    Console.WriteLine("|             |            | the condition is not met. Accepts PID, Module (DLL),     | >> require excel.exe  |");
                    Console.WriteLine("|             |            | or Process name (EXE). 'Require *' will enter a prompt   |                       |");
                    Console.WriteLine("|             |            | for multiple inputs.                                     |                       |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Hibernate | Similar to the require trigger, but with sleeping while  | >> Hibernate 2408     |");
                    Console.WriteLine("|             |            | the condition is not met. Accepts PID, Module (DLL),     | >> hibernate *        |");
                    Console.WriteLine("|             |            | or Process name (EXE). 'Hibernate *' will enter a prompt |                       |");
                    Console.WriteLine("|             |            | for multiple inputs.                                     |                       |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Persist   | Similar to the avoid trigger, but with sleeping while    | >> persist 3612       |");
                    Console.WriteLine("|             |            | the condition is met. Accepts PID, Module (DLL),         | >> persist *          |");
                    Console.WriteLine("|             |            | or Process name (EXE). 'Persist *' will enter a prompt   |                       |");
                    Console.WriteLine("|             |            | for multiple inputs. This trigger is useful as a backup  |                       |");
                    Console.WriteLine("|             |            | since you can have it wait until previous techniques     |                       |");
                    Console.WriteLine("|             |            | used have been killed.                                   |                       |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+-----------------------+");
                    Console.WriteLine("|             |  Timer     | Set a timer to pause execution for a certain amount of   | >> timer 5            |");
                    Console.WriteLine("|             |            | seconds before executing the payload.                    |                       |");
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
                case "SETTINGS":
                    Settings.SettingsMenu();
                    break;
                case "TIMER":
                    if (command.Split().Count() > 1)
                    {
                        decimal intholder = 0;
                        if (Decimal.TryParse(command.Split()[1], out intholder))
                        {
                            Program.TIMERSECONDS = command.Split()[1];
                            Console.WriteLine("|\n|     [~] Timer set for {0} seconds.", Program.TIMERSECONDS);
                            if (!Program.TriggersToUse.Contains("TIMERTRIGGER"))
                            {
                                Program.TriggersToUse.Add("TIMERTRIGGER");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("|\n|   [~] Timer Usage: timer <seconds>");
                    }
                    break;
                case "REQUIRE":
                    if (command.Split().Count() > 1)
                    {
                        switch (command.ToUpper().Split()[1])
                        {
                            case "":
                                break;
                            case "*":
                                string procToadd;
                                Console.WriteLine("|\n|   [~] Enter your required PIDs/processes/modules. Enter \"Done!\" or \"Exit\" when finished.");
                                do
                                {
                                    Console.Write("|     > ");
                                    procToadd = Console.ReadLine();
                                    if (!procToadd.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) && !procToadd.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) && !int.TryParse(procToadd, out _) && procToadd.ToUpper() != "EXIT" && procToadd.ToUpper() != "DONE!")
                                    {
                                        Console.WriteLine("|        [!] Entry must end in .exe/.dll or be a PID.");
                                        continue;
                                    }
                                    if (!Program.TriggersToUse.Contains("REQUIRETRIGGER"))
                                    {
                                        Program.TriggersToUse.Add("REQUIRETRIGGER");
                                    }
                                    if (Program.REQUIREDPROCESSDETAILS == "" && procToadd.ToUpper() != "EXIT" && procToadd.ToUpper() != "DONE!")
                                    {
                                        Program.REQUIREDPROCESSDETAILS = "\"" + procToadd.ToUpper() + "\"";
                                    }
                                    else if (Program.REQUIREDPROCESSDETAILS != "" && procToadd.ToUpper() != "EXIT" && procToadd.ToUpper() != "DONE!")
                                    {
                                        if (!Program.REQUIREDPROCESSDETAILS.Contains(procToadd.ToUpper()))
                                        {
                                            Program.REQUIREDPROCESSDETAILS = Program.REQUIREDPROCESSDETAILS + ",\"" + procToadd.ToUpper() + "\"";
                                        }

                                    }
                                } while (procToadd.ToUpper() != "DONE!" && procToadd.ToUpper() != "EXIT");
                                break;
                            default:
                                if (!command.Split()[1].EndsWith(".exe", StringComparison.OrdinalIgnoreCase) && !command.Split()[1].EndsWith(".dll", StringComparison.OrdinalIgnoreCase) && !int.TryParse(command.Split()[1], out _))
                                {
                                    Console.WriteLine("|\n|     [!] Entry must end in .exe/.dll or be a PID.");
                                    break;
                                }

                                if (!Program.TriggersToUse.Contains("REQUIRETRIGGER"))
                                {
                                    Program.TriggersToUse.Add("REQUIRETRIGGER");
                                }

                                if (Program.REQUIREDPROCESSDETAILS == "")
                                {
                                    Program.REQUIREDPROCESSDETAILS = "\"" + command.Split()[1].ToUpper() + "\"";
                                }
                                else if (Program.REQUIREDPROCESSDETAILS != "")
                                {
                                    if (!Program.REQUIREDPROCESSDETAILS.Contains(command.Split()[1].ToUpper()))
                                    {
                                        Program.REQUIREDPROCESSDETAILS = Program.REQUIREDPROCESSDETAILS + ",\"" + command.Split()[1].ToUpper() + "\"";
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case "HIBERNATE":
                    if (command.Split().Count() > 1)
                    {
                        switch (command.ToUpper().Split()[1])
                        {
                            case "":
                                break;
                            case "*":
                                string procToadd;
                                Console.WriteLine("|\n|   [~] Enter your required PIDs/processes/modules. Enter \"Done!\" or \"Exit\" when finished.");
                                do
                                {
                                    Console.Write("|     > ");
                                    procToadd = Console.ReadLine();
                                    if (!procToadd.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) && !procToadd.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) && !int.TryParse(procToadd, out _) && procToadd.ToUpper() != "EXIT" && procToadd.ToUpper() != "DONE!")
                                    {
                                        Console.WriteLine("|        [!] Entry must end in .exe/.dll or be a PID.");
                                        continue;
                                    }
                                    if (!Program.TriggersToUse.Contains("HIBERNATETRIGGER"))
                                    {
                                        Program.TriggersToUse.Add("HIBERNATETRIGGER");
                                    }
                                    if (Program.HIBERNATEPROCESSDETAILS == "" && procToadd.ToUpper() != "EXIT" && procToadd.ToUpper() != "DONE!")
                                    {
                                        Program.HIBERNATEPROCESSDETAILS = "\"" + procToadd.ToUpper() + "\"";
                                    }
                                    else if (Program.HIBERNATEPROCESSDETAILS != "" && procToadd.ToUpper() != "EXIT" && procToadd.ToUpper() != "DONE!")
                                    {
                                        if (!Program.HIBERNATEPROCESSDETAILS.Contains(procToadd.ToUpper()))
                                        {
                                            Program.HIBERNATEPROCESSDETAILS = Program.HIBERNATEPROCESSDETAILS + ",\"" + procToadd.ToUpper() + "\"";
                                        }

                                    }
                                } while (procToadd.ToUpper() != "DONE!" && procToadd.ToUpper() != "EXIT");
                                break;
                            default:
                                if (!command.Split()[1].EndsWith(".exe", StringComparison.OrdinalIgnoreCase) && !command.Split()[1].EndsWith(".dll", StringComparison.OrdinalIgnoreCase) && !int.TryParse(command.Split()[1], out _))
                                {
                                    Console.WriteLine("|\n|     [!] Entry must end in .exe/.dll or be a PID.");
                                    break;
                                }

                                if (!Program.TriggersToUse.Contains("HIBERNATETRIGGER"))
                                {
                                    Program.TriggersToUse.Add("HIBERNATETRIGGER");
                                }

                                if (Program.HIBERNATEPROCESSDETAILS == "")
                                {
                                    Program.HIBERNATEPROCESSDETAILS = "\"" + command.Split()[1].ToUpper() + "\"";
                                }
                                else if (Program.HIBERNATEPROCESSDETAILS != "")
                                {
                                    if (!Program.HIBERNATEPROCESSDETAILS.Contains(command.Split()[1].ToUpper()))
                                    {
                                        Program.HIBERNATEPROCESSDETAILS = Program.HIBERNATEPROCESSDETAILS + ",\"" + command.Split()[1].ToUpper() + "\"";
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case "AVOID":
                    if (command.Split().Count() > 1)
                    {
                        switch (command.ToUpper().Split()[1])
                        {
                            case "":
                                break;
                            case "*":
                                string procToadd;
                                Console.WriteLine("|\n|   [~] Enter PIDs/processes/modules to avoid. Enter \"Done!\" or \"Exit\" when finished.");
                                do
                                {
                                    Console.Write("|     > ");
                                    procToadd = Console.ReadLine();
                                    if (!procToadd.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) && !procToadd.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) && !int.TryParse(procToadd, out _) && procToadd.ToUpper() != "EXIT" && procToadd.ToUpper() != "DONE!")
                                    {
                                        Console.WriteLine("|        [!] Entry must end in .exe/.dll or be a PID.");
                                        continue;
                                    }
                                    if (!Program.TriggersToUse.Contains("AVOIDTRIGGER"))
                                    {
                                        Program.TriggersToUse.Add("AVOIDTRIGGER");
                                    }
                                    if (Program.AVOIDPROCESSDETAILS == "" && procToadd.ToUpper() != "EXIT" && procToadd.ToUpper() != "DONE!")
                                    {
                                        Program.AVOIDPROCESSDETAILS = "\"" + procToadd.ToUpper() + "\"";
                                    }
                                    else if (Program.AVOIDPROCESSDETAILS != "" && procToadd.ToUpper() != "EXIT" && procToadd.ToUpper() != "DONE!")
                                    {
                                        if (!Program.AVOIDPROCESSDETAILS.Contains(procToadd.ToUpper()))
                                        {
                                            Program.AVOIDPROCESSDETAILS = Program.AVOIDPROCESSDETAILS + ",\"" + procToadd.ToUpper() + "\"";
                                        }

                                    }
                                } while (procToadd.ToUpper() != "DONE!" && procToadd.ToUpper() != "EXIT");
                                break;
                            default:
                                if (!command.Split()[1].EndsWith(".exe", StringComparison.OrdinalIgnoreCase) && !command.Split()[1].EndsWith(".dll", StringComparison.OrdinalIgnoreCase) && !int.TryParse(command.Split()[1], out _))
                                {
                                    Console.WriteLine("|\n|     [!] Entry must end in .exe/.dll or be a PID.");
                                    break;
                                }

                                if (!Program.TriggersToUse.Contains("AVOIDTRIGGER"))
                                {
                                    Program.TriggersToUse.Add("AVOIDTRIGGER");
                                }

                                if (Program.AVOIDPROCESSDETAILS == "")
                                {
                                    Program.AVOIDPROCESSDETAILS = "\"" + command.Split()[1].ToUpper() + "\"";
                                }
                                else if (Program.AVOIDPROCESSDETAILS != "")
                                {
                                    if (!Program.AVOIDPROCESSDETAILS.Contains(command.Split()[1].ToUpper()))
                                    {
                                        Program.AVOIDPROCESSDETAILS = Program.AVOIDPROCESSDETAILS + ",\"" + command.Split()[1].ToUpper() + "\"";
                                    }
                                }
                                break;
                        }
                    }
                    break;
                case "PERSIST":
                    if (command.Split().Count() > 1)
                    {
                        switch (command.ToUpper().Split()[1])
                        {
                            case "":
                                break;
                            case "*":
                                string procToadd;
                                Console.WriteLine("|\n|   [~] Enter your required PIDs/processes/modules. Enter \"Done!\" or \"Exit\" when finished.");
                                do
                                {
                                    Console.Write("|     > ");
                                    procToadd = Console.ReadLine();
                                    if (!procToadd.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) && !procToadd.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) && !int.TryParse(procToadd, out _) && procToadd.ToUpper() != "EXIT" && procToadd.ToUpper() != "DONE!")
                                    {
                                        Console.WriteLine("|        [!] Entry must end in .exe/.dll or be a PID.");
                                        continue;
                                    }
                                    if (!Program.TriggersToUse.Contains("PERSISTTRIGGER"))
                                    {
                                        Program.TriggersToUse.Add("PERSISTTRIGGER");
                                    }
                                    if (Program.PERSISTPROCESSDETAILS == "" && procToadd.ToUpper() != "EXIT" && procToadd.ToUpper() != "DONE!")
                                    {
                                        Program.PERSISTPROCESSDETAILS = "\"" + procToadd.ToUpper() + "\"";
                                    }
                                    else if (Program.PERSISTPROCESSDETAILS != "" && procToadd.ToUpper() != "EXIT" && procToadd.ToUpper() != "DONE!")
                                    {
                                        if (!Program.PERSISTPROCESSDETAILS.Contains(procToadd.ToUpper()))
                                        {
                                            Program.PERSISTPROCESSDETAILS = Program.PERSISTPROCESSDETAILS + ",\"" + procToadd.ToUpper() + "\"";
                                        }

                                    }
                                } while (procToadd.ToUpper() != "DONE!" && procToadd.ToUpper() != "EXIT");
                                break;
                            default:
                                if (!command.Split()[1].EndsWith(".exe", StringComparison.OrdinalIgnoreCase) && !command.Split()[1].EndsWith(".dll", StringComparison.OrdinalIgnoreCase) && !int.TryParse(command.Split()[1], out _))
                                {
                                    Console.WriteLine("|\n|     [!] Entry must end in .exe/.dll or be a PID.");
                                    break;
                                }

                                if (!Program.TriggersToUse.Contains("PERSISTTRIGGER"))
                                {
                                    Program.TriggersToUse.Add("PERSISTTRIGGER");
                                }

                                if (Program.PERSISTPROCESSDETAILS == "")
                                {
                                    Program.PERSISTPROCESSDETAILS = "\"" + command.Split()[1].ToUpper() + "\"";
                                }
                                else if (Program.PERSISTPROCESSDETAILS != "")
                                {
                                    if (!Program.PERSISTPROCESSDETAILS.Contains(command.Split()[1].ToUpper()))
                                    {
                                        Program.PERSISTPROCESSDETAILS = Program.PERSISTPROCESSDETAILS + ",\"" + command.Split()[1].ToUpper() + "\"";
                                    }
                                }
                                break;
                        }
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
                        }
                    }
                    else
                    {
                        Program.PrintSettings();
                    }
                    break;
                case "BLURB":
                    if (Settings.bvHelpBlurb)
                    {
                        Settings.bvHelpBlurb = false;
                        Console.WriteLine("|\n|   [~] Help blurbs has been disabled.");
                    }
                    else if (!Settings.bvHelpBlurb)
                    {
                        Settings.bvHelpBlurb = true;
                        Console.WriteLine("|\n|\tavoid      persist  require");
                        Console.WriteLine("|\thibernate  timer     blurb");
                        Console.WriteLine("|\tsettings   clear    help");
                        Console.WriteLine("|\tshow       exit");
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
                                Console.WriteLine("|   [~] Output and Mode have been cleared.");
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
                        Console.WriteLine("\n       +---------------------------+\n ______|         TRIGGERS          |\n|      +---------------------------+");
                        if (Settings.bvHelpBlurb)
                        {
                            Console.WriteLine("|\n|\tavoid      persist  require");
                            Console.WriteLine("|\thibernate  timer     blurb");
                            Console.WriteLine("|\tsettings   clear    help");
                            Console.WriteLine("|\tshow       exit");
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
    }
}