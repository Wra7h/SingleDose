using System;
using System.Linq;

namespace SingleDose
{
    class Triggers
    {
        public static void TriggersMenu()
        {
            string triggersInput = null;
            Console.WriteLine("\n  +--------------+\n _|   TRIGGERS   |\n| +--------------+");
            if (Settings.helpBlurb)
            {
                Console.WriteLine("|\n|\tavoid   persist   require   hibernate   show");
                Console.WriteLine("|\texit    clear     help      settings    blurb");
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
                    Console.WriteLine("|\n|\t\t\t\t Triggers Commands");
                    Console.WriteLine("|\t\t\t\t-------------------");
                    Console.WriteLine("|\t1) Avoid :: Do not execute if condition is met. Binary will exit if condition is met.");
                    Console.WriteLine("|\t           Accepts PID, Module (DLL), or Process name (EXE).");
                    Console.WriteLine("|\t          'Avoid *' will enter an interface allowing you to add multiple conditions.");
                    Console.WriteLine("|\t   > Example Usage: avoid *");
                    Console.WriteLine("|\t   > Example Usage: avoid iexplore.exe");
                    Console.WriteLine("|");
                    Console.WriteLine("|\t2) Require :: Only execute if all conditions are met. Binary will exit if conditions are not met.");
                    Console.WriteLine("|\t           Accepts PID, Module (DLL), or Process name (EXE).");
                    Console.WriteLine("|\t          'Require *' will enter an interface allowing you to add multiple conditions.");
                    Console.WriteLine("|\t   > Example Usage: require *");
                    Console.WriteLine("|\t   > Example Usage: require notepad.exe");
                    Console.WriteLine("|");
                    Console.WriteLine("|\t3) Hibernate :: Similar to the require trigger, but with sleeping while the condition is not met.");
                    Console.WriteLine("|\t             The sleep value is a random value between 90 seconds and 5 min.");
                    Console.WriteLine("|\t             Accepts PID, Module (DLL), or Process name (EXE).");
                    Console.WriteLine("|\t            'Hibernate *' will enter an interface allowing you to add multiple conditions.");
                    Console.WriteLine("|\t   > Example Usage: hibernate *");
                    Console.WriteLine("|\t   > Example Usage: hibernate notepad.exe");
                    Console.WriteLine("|");
                    Console.WriteLine("|\t4) Persist :: Similar to the avoid trigger, but with sleeping while the condition is met.");
                    Console.WriteLine("|\t              Accepts PID, Module (DLL), or Process name (EXE).");
                    Console.WriteLine("|\t   > Example Usage: persist *");
                    Console.WriteLine("|\t   > Example Usage: persist notepad.exe");
                    Console.WriteLine("|\n|\n|\tClear :: Clear the terminal, settings or triggers.");
                    Console.WriteLine("|\t   > Example Usage: clear");
                    Console.WriteLine("|\t   > Example Usage: clear settings");
                    Console.WriteLine("|\tShow :: Display current configuration.");
                    Console.WriteLine("|\tBlurb :: A switch to display a command blurb when switching between menus. (Default = true)");
                    Console.WriteLine("|\tExit :: Return to Main Menu");
                    break;
                case "SETTINGS":
                    Settings.SettingsMenu();
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
                                Console.WriteLine("|\n|                TECHNIQUES");
                                Console.WriteLine("|              --------------");
                                Console.WriteLine("|\t   1) CreateRemoteThread: Inject a DLL into a remote process and execute with CreateRemoteThread. [DLL]");
                                Console.WriteLine("|\t   2) SRDI: Convert DLL into shellcode and inject. [DLL]");
                                Console.WriteLine("|\t   3) EarlyBird_QueueUserAPC: Inject Shellcode into a newly spawned process. [Shellcode]");
                                Console.WriteLine("|\t   4) Suspend_QueueUserAPC: Inject Shellcode into a process currently running. [Shellcode]");
                                Console.WriteLine("|\t   5) Syscall_CreateThread: Inject Shellcode using direct syscalls. [Shellcode]");
                                Console.WriteLine("|\t   6) Fiber_Execution: Execute Shellcode via Fibers. [Shellcode]");
                                break;
                        }
                    }
                    else
                    {
                        Program.PrintSettings();
                    }
                    break;
                case "BLURB":
                    if (Settings.helpBlurb)
                    {
                        Settings.helpBlurb = false;
                        Console.WriteLine("|\n|   [~] Help blurbs has been disabled.");
                    }
                    else if (!Settings.helpBlurb)
                    {
                        Settings.helpBlurb = true;
                        Console.WriteLine("|\n|\tavoid   persist   require   hibernate   show");
                        Console.WriteLine("|\texit    clear     help      settings    blurb");
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
                                Console.WriteLine("|   [~] Output and Mode have been cleared.");
                                break;
                            case "TRIGGERS":
                                Program.HIBERNATEPROCESSDETAILS = "";
                                Program.REQUIREDPROCESSDETAILS = "";
                                Console.WriteLine("|   [~] Triggers have been cleared.");
                                break;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n  +--------------+\n _|   TRIGGERS   |\n| +--------------+");
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
