using System;
using System.Linq;

namespace SingleDose
{
    partial class Program
    {

        static void Main()
        {
            Run();
        }

        static void Run()
        {
            
            Console.WriteLine(@"
         _____ _____ _   _  _____ _      ______    _____   ____   _____ ______ 
        / ____|_   _| \ | |/ ____| |    |  ____|  |  __ \ / __ \ / ____|  ____|
       | (___   | | |  \| | |  __| |    | |__     | |  | | |  | | (___ | |__   
        \___ \  | | | . ` | | |_ | |    |  __|    | |  | | |  | |\___ \|  __|  
        ____) |_| |_| |\  | |__| | |____| |____   | |__| | |__| |____) | |____ 
       |_____/|_____|_| \_|\_____|______|______|  |_____/ \____/|_____/|______|
        _______________________________________________________________________ 
       |_______________________________________________________________________|
                                                                 [GitHub: Wra7h]");
            Console.WriteLine(@" 
          Basic usage from Main Menu: 
              1. Settings -> output [output directory] -> mode [mode] -> exit
              2. show techniques -> build [technique name/number]
          
          Descriptions for each technique with ""describe [technique]"".
          Additional options displayed with ""help"" command in each menu.");
            Console.WriteLine("\n");

            Console.WriteLine("\n  +-------------------+\n _|     MAIN MENU     |\n| +-------------------+");
            Console.WriteLine("|\n|\tsettings   triggers   build   describe   show");
            Console.WriteLine("|\texit       clear      help    blurb");
            string input;
            while (true)
            {
                Console.Write("|\n+--> ");
                input = Console.ReadLine();
                ShellCommands(input);
            }
        }



        /// <summary>
        /// Display current settings and triggers to console.
        /// </summary>
        public static void PrintSettings()
        {
            Console.WriteLine("|\n|\t      +----------------------+");
            Console.WriteLine("|\t      |   CURRENT SETTINGS   |   ");
            Console.WriteLine("|\t      +----------------------+  ");
            Console.WriteLine("|\t   Output Directory = {0}", Settings.OutputDirectory);
            Console.WriteLine("|\t   Compile          = {0}", Settings.CompileBinary);
            Console.WriteLine("|\t   Mode             = {0}", Settings.InjectMode);


            
            Console.WriteLine("|\n|\t              Triggers");
            Console.WriteLine("|\t          +--------------+");
            if (REQUIREDPROCESSDETAILS == "" && HIBERNATEPROCESSDETAILS == "" && AVOIDPROCESSDETAILS == "" && PERSISTPROCESSDETAILS == "")
            {
                Console.WriteLine("|\t      No triggers configured.");
            }

            if (REQUIREDPROCESSDETAILS != "")
            {
                Console.WriteLine("|\t   Requirements     = {0}", REQUIREDPROCESSDETAILS);
            }

            if (HIBERNATEPROCESSDETAILS != "")
            {
                Console.WriteLine("|\t   Hibernate Reqs   = {0}", HIBERNATEPROCESSDETAILS);
            }
            if (AVOIDPROCESSDETAILS != "")
            {
                Console.WriteLine("|\t   Avoid Reqs   = {0}", AVOIDPROCESSDETAILS);
            }
            if (PERSISTPROCESSDETAILS != "")
            {
                Console.WriteLine("|\t   Persist Reqs   = {0}", PERSISTPROCESSDETAILS);
            }

        }
        
        /// <summary>
        /// ShellCommands handles the commands for the Main Menu
        /// </summary>
        /// <param name="command"></param>
        static void ShellCommands(string command)
        {
            switch (command.ToUpper().Split()[0])
            {
                case "":
                    break;
                case "HELP":
                    Console.WriteLine("|\n|____________+-------------------+\n ____________|     Main Help     |\n|            +-------------------+");
                    Console.WriteLine("|\n|                 SUBMENUS");
                    Console.WriteLine("|            ------------------");
                    Console.WriteLine("|\tSettings :: Enter submenu for configuring settings");
                    Console.WriteLine("|\tTriggers :: Enter submenu for configuring execution conditions.");
                    Console.WriteLine("|\n|                  BUILD");
                    Console.WriteLine("|            ------------------");
                    Console.WriteLine("|\tBuild :: Build a binary using the configured settings and triggers.");
                    Console.WriteLine("|\t   > Example Usage: build <technique>");
                    Console.WriteLine("|\t   > Example Usage: build 3");
                    Console.WriteLine("|\n|                TECHNIQUES");
                    Console.WriteLine("|              --------------");
                    Console.WriteLine("|\t   1) CreateRemoteThread: Inject a DLL into a remote process and execute with CreateRemoteThread. [DLL]");
                    Console.WriteLine("|\t   2) SRDI: Convert DLL into shellcode and inject. [DLL]");
                    Console.WriteLine("|\t   3) EarlyBird_QueueUserAPC: Inject Shellcode into a newly spawned process. [Shellcode]");
                    Console.WriteLine("|\t   4) Suspend_QueueUserAPC: Inject Shellcode into a process currently running. [Shellcode]");
                    Console.WriteLine("|\t   5) Syscall_CreateThread: Inject Shellcode using direct syscalls. [Shellcode]");
                    Console.WriteLine("|\t   6) Fiber_Execution: Execute Shellcode via Fibers. [Shellcode]");
                    Console.WriteLine("|\n|              MISC. COMMANDS");
                    Console.WriteLine("|            ------------------");
                    Console.WriteLine("|\tShow :: Display current configuration, or techniques.");
                    Console.WriteLine("|\t   > Example Usage: show");
                    Console.WriteLine("|\t   > Example Usage: show techniques");
                    Console.WriteLine("|\tDescribe :: Describe techniques.");
                    Console.WriteLine("|\t   > Example Usage: describe <technique>");
                    Console.WriteLine("|\tClear :: Clear the terminal, settings or triggers.");
                    Console.WriteLine("|\t   > Example Usage: clear");
                    Console.WriteLine("|\t   > Example Usage: clear settings");
                    Console.WriteLine("|\tBlurb :: A switch to display a command blurb when switching between menus. (Default = true)");
                    Console.WriteLine("|\tHelp :: Display this help.");
                    Console.WriteLine("|\tExit :: Exit the program.");
                    break;
                case "SETTINGS":
                    Settings.SettingsMenu();
                    Console.WriteLine("\n  +-------------------+\n _|     MAIN MENU     |\n| +-------------------+");
                    if (Settings.helpBlurb)
                    {
                        Console.WriteLine("|\n|\tsettings   triggers   build   describe   show");
                        Console.WriteLine("|\texit       clear      help    blurb");
                    }
                    break;
                case "TRIGGERS":
                    Triggers.TriggersMenu();
                    Console.WriteLine("\n  +-------------------+\n _|     MAIN MENU     |\n| +-------------------+");
                    if (Settings.helpBlurb)
                    {
                        Console.WriteLine("|\n|\tsettings   triggers   build   describe   show");
                        Console.WriteLine("|\texit       clear      help    blurb");
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
                        Console.WriteLine("|\n|\tsettings   triggers   build   describe   show");
                        Console.WriteLine("|\texit       clear      help    blurb");
                    }
                    break;
                case "BUILD":
                    if (Settings.InjectMode != null && Settings.OutputDirectory != null)
                    {
                        if (command.Split().Count() > 1)
                        {
                            switch (command.Split()[1].ToUpper())
                            {
                                case "CREATEREMOTETHREAD":
                                    Console.WriteLine("|\n|   [*] Building technique: DLL CreateRemoteThread\n|");
                                    Settings.SelectedTechnique = "CreateRemoteThread";
                                    break;
                                case "1":
                                    Console.WriteLine("|\n|   [*] Building technique: DLL CreateRemoteThread\n|");
                                    Settings.SelectedTechnique = "CreateRemoteThread";
                                    break;
                                case "SRDI":
                                    Console.WriteLine("|\n|   [*] Building technique: Shellcode Reflective DLL Injection (SRDI)\n|");
                                    Settings.SelectedTechnique = "SRDI";
                                    break;
                                case "2":
                                    Console.WriteLine("|\n|   [*] Building technique: Shellcode Reflective DLL Injection (SRDI)\n|");
                                    Settings.SelectedTechnique = "SRDI";
                                    break;
                                case "EARLYBIRD_QUEUEUSERAPC":
                                    Console.WriteLine("|\n|   [*] Building technique: EarlyBird_QueueUserAPC\n|");
                                    Settings.SelectedTechnique = "EarlyBird_QueueUserAPC";
                                    break;
                                case "3":
                                    Console.WriteLine("|\n|   [*] Building technique: EarlyBird_QueueUserAPC\n|");
                                    Settings.SelectedTechnique = "EarlyBird_QueueUserAPC";
                                    break;
                                case "SUSPEND_QUEUEUSERAPC":
                                    Console.WriteLine("|\n|   [*] Building technique: Suspend_QueueUserAPC\n|");
                                    Settings.SelectedTechnique = "Suspend_QueueUserAPC";
                                    break;
                                case "4":
                                    Console.WriteLine("|\n|   [*] Building technique: Suspend_QueueUserAPC\n|");
                                    Settings.SelectedTechnique = "Suspend_QueueUserAPC";
                                    break;
                                case "SYSCALL_CREATETHREAD":
                                    Console.WriteLine("|\n|   [*] Building technique: Syscall_CreateThread\n|");
                                    Settings.SelectedTechnique = "Syscall_CreateThread";
                                    break;
                                case "5":
                                    Console.WriteLine("|\n|   [*] Building technique: Syscall_CreateThread\n|");
                                    Settings.SelectedTechnique = "Syscall_CreateThread";
                                    break;
                                case "FIBER_EXECUTION":
                                    Console.WriteLine("|\n|   [*] Building technique: Fiber_Execution\n|");
                                    Settings.SelectedTechnique = "Fiber_Execution";
                                    break;
                                case "6":
                                    Console.WriteLine("|\n|   [*] Building technique: Fiber_Execution\n|");
                                    Settings.SelectedTechnique = "Fiber_Execution";
                                    break;
                                default:
                                    Console.WriteLine("|\n|   [!] Unknown technique. Techniques can be found in help.");
                                    break;
                            }

                            //Build the .cs
                            if (CSConstructor(Settings.InjectMode, Settings.SelectedTechnique))
                            {
                                Console.WriteLine("|   [*] Construction Complete.");
                                Settings.SelectedTechnique = null;
                            }
                            else
                            {
                                if (Settings.CompileBinary) {
                                    Console.WriteLine("|\n|   [!] Error building binary. Check A/V, retry building, or build manually.");
                                } 
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("|\n|   [!] Please ensure a mode and output directory have been set.");
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
                                HIBERNATEPROCESSDETAILS = "";
                                REQUIREDPROCESSDETAILS = "";
                                AVOIDPROCESSDETAILS = "";
                                PERSISTPROCESSDETAILS = "";
                                Console.WriteLine("|\n|   [~] Triggers have been cleared.");
                                break;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n  +-------------------+\n _|     MAIN MENU     |\n| +-------------------+");
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
                        PrintSettings();
                    }
                    break;
                case "DESCRIBE":
                    if (command.Split().Count() > 1)
                    {
                        switch (command.Split()[1].ToUpper())
                        {
                            case "CREATEREMOTETHREAD":
                                Console.WriteLine("|\n|\t CreateRemoteThread");
                                Console.WriteLine("|\t--------------------\n|");
                                Console.WriteLine("|   Inject Source: DLL");
                                Console.WriteLine("|   P/Invoke APIs: GetModuleHandle, GetProcAddress, VirtualAllocEx,");
                                Console.WriteLine("|                  WriteProcessMemory, CreateRemoteThread\n|");
                                Console.WriteLine("|   References: https://www.ired.team/offensive-security/code-injection-process-injection/dll-injection \n|");
                                break;
                            case "1":
                                Console.WriteLine("|\n|\t CreateRemoteThread");
                                Console.WriteLine("|\t--------------------\n|");
                                Console.WriteLine("|   Inject Source: DLL");
                                Console.WriteLine("|   P/Invoke APIs: GetModuleHandle, GetProcAddress, VirtualAllocEx,");
                                Console.WriteLine("|                  WriteProcessMemory, CreateRemoteThread\n|");
                                Console.WriteLine("|   References: https://www.ired.team/offensive-security/code-injection-process-injection/dll-injection \n|");
                                break;
                            case "SRDI":
                                Console.WriteLine("|\n|\t Shellcode Reflective DLL Injection");
                                Console.WriteLine("|\t------------------------------------\n|");
                                Console.WriteLine("|   Inject Source: DLL");
                                Console.WriteLine("|   P/Invoke APIs: VirtualProtect\n|");
                                Console.WriteLine("|   Summary: This method takes the data from a DLL and converts it to shellcode");
                                Console.WriteLine("|            for injection. This technique is written slightly different than");
                                Console.WriteLine("|            the original PoC by @monoxgas. This technique mostly uses managed");
                                Console.WriteLine("|            code for injection.\n|");
                                Console.WriteLine("|   References: https://github.com/monoxgas/sRDI \n|");
                                break;
                            case "2":
                                Console.WriteLine("|\n|\t Shellcode Reflective DLL Injection");
                                Console.WriteLine("|\t------------------------------------\n|");
                                Console.WriteLine("|   Inject Source: DLL");
                                Console.WriteLine("|   P/Invoke APIs: VirtualProtect\n|");
                                Console.WriteLine("|   Summary: This method takes the data from a DLL and converts it to shellcode");
                                Console.WriteLine("|            for injection. This technique is written slightly different than");
                                Console.WriteLine("|            the original PoC by @monoxgas. This technique mostly uses managed");
                                Console.WriteLine("|            code for injection.\n|");
                                Console.WriteLine("|   References: https://github.com/monoxgas/sRDI \n|");
                                break;
                            case "EARLYBIRD_QUEUEUSERAPC":
                                Console.WriteLine("|\n|\t   EarlyBird QueueUserAPC");
                                Console.WriteLine("|\t  ------------------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: CreateProcess, VirtualAllocEx, WriteProcessMemory,");
                                Console.WriteLine("|                  OpenThread, VirtualProtectEx, QueueUserAPC, ResumeThread\n|");
                                Console.WriteLine("|   Summary: This method creates a new process in a suspended state. When the process resumes");
                                Console.WriteLine("|            the shellcode is executed. Has a higher potential of circumventing AV/EDR hooks.\n|");
                                Console.WriteLine("|   References: https://3xpl01tc0d3r.blogspot.com/2019/12/process-injection-part-v.html");
                                break;
                            case "3":
                                Console.WriteLine("|\n|\t   EarlyBird QueueUserAPC");
                                Console.WriteLine("|\t  ------------------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: CreateProcess, VirtualAllocEx, WriteProcessMemory,");
                                Console.WriteLine("|                  OpenThread, VirtualProtectEx, QueueUserAPC, ResumeThread\n|");
                                Console.WriteLine("|   Summary: This method creates a new process in a suspended state. When the process resumes");
                                Console.WriteLine("|            the shellcode is executed. Has a higher potential of circumventing AV/EDR hooks.\n|");
                                Console.WriteLine("|   References: https://3xpl01tc0d3r.blogspot.com/2019/12/process-injection-part-v.html");
                                break;
                            case "SUSPEND_QUEUEUSERAPC":
                                Console.WriteLine("|\n|\t   Suspend QueueUserAPC");
                                Console.WriteLine("|\t  ----------------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: VirtualAllocEx, WriteProcessMemory, VirtualProtectEx");
                                Console.WriteLine("|                  OpenThread, QueueUserAPC\n|");
                                Console.WriteLine("|   Summary: This method suspends each thread in a target process. The thread is resumed after");
                                Console.WriteLine("|            shellcode is queued. At the moment shellcode execution is not guaranteed for every thread,");
                                Console.WriteLine("|            so only 5 threads are suspended and resumed. Since 5 threads are targetted, it is possible");
                                Console.WriteLine("|            to have more than one successful execution. Feel free to edit or change the number of threads");
                                Console.WriteLine("|            in the .cs generated before compiling the final binary yourself.\n|");
                                Console.WriteLine("|   References: https://sevrosecurity.com/2020/04/13/process-injection-part-2-queueuserapc/");
                                break;
                            case "4":
                                Console.WriteLine("|\n|\t   Suspend QueueUserAPC");
                                Console.WriteLine("|\t  ----------------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: VirtualAllocEx, WriteProcessMemory, VirtualProtectEx");
                                Console.WriteLine("|                  OpenThread, QueueUserAPC\n|");
                                Console.WriteLine("|   Summary: This method suspends each thread in a target process. The thread is resumed after");
                                Console.WriteLine("|            shellcode is queued. At the moment shellcode execution is not guaranteed for every thread,");
                                Console.WriteLine("|            so only 5 threads are suspended and resumed. Since 5 threads are targetted, it is possible");
                                Console.WriteLine("|            to have more than one successful execution. Feel free to edit or change the number of threads");
                                Console.WriteLine("|            in the .cs generated before compiling the final binary yourself.\n|");
                                Console.WriteLine("|   References: https://sevrosecurity.com/2020/04/13/process-injection-part-2-queueuserapc/");
                                break;
                            case "SYSCALL_CREATETHREAD":
                                Console.WriteLine("|\n|\t   Syscall CreateThread");
                                Console.WriteLine("|\t  ----------------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: GetCurrentProcess, VirtualProtectEx\n|");
                                Console.WriteLine("|   Syscalls: NtAllocateVirtualMemory, NtCreateThreadEx, NtWaitForSingleObject\n|");
                                Console.WriteLine("|   Summary: Pretty complex technique to describe in a small amount of space.");
                                Console.WriteLine("|            Only certain OS builds are supported, which can be seen in the");
                                Console.WriteLine("|            .cs under FetchSyscallID(). This technique is useful when");
                                Console.WriteLine("|            bypassing AV/EDR hooks is a necessity.\n|");
                                Console.WriteLine("|   References: https://jhalon.github.io/utilizing-syscalls-in-csharp-1/");
                                Console.WriteLine("|               https://jhalon.github.io/utilizing-syscalls-in-csharp-2/");
                                Console.WriteLine("|               https://www.solomonsklash.io/syscalls-for-shellcode-injection.html");
                                break;
                            case "5":
                                Console.WriteLine("|\n|\t   Syscall CreateThread");
                                Console.WriteLine("|\t  ----------------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: GetCurrentProcess, VirtualProtectEx\n|");
                                Console.WriteLine("|   Syscalls: NtAllocateVirtualMemory, NtCreateThreadEx, NtWaitForSingleObject\n|");
                                Console.WriteLine("|   Summary: Pretty complex technique to describe in a small amount of space.");
                                Console.WriteLine("|            Only certain OS builds are supported, which can be seen in the");
                                Console.WriteLine("|            .cs under FetchSyscallID(). This technique is useful when");
                                Console.WriteLine("|            bypassing AV/EDR hooks is a necessity.\n|");
                                Console.WriteLine("|   References: https://jhalon.github.io/utilizing-syscalls-in-csharp-1/");
                                Console.WriteLine("|               https://jhalon.github.io/utilizing-syscalls-in-csharp-2/");
                                Console.WriteLine("|               https://www.solomonsklash.io/syscalls-for-shellcode-injection.html");
                                break;
                            case "FIBER_EXECUTION":
                                Console.WriteLine("|\n|\t   Fiber Execution");
                                Console.WriteLine("|\t  -----------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: ConvertThreadToFiber, VirtualAlloc, CreateFiber,");
                                Console.WriteLine("|                  VirtualProtectEx, SwitchToFiber\n|");
                                Console.WriteLine("|   Summary: Fibers are a lightweight thread of execution similar to OS threads.");
                                Console.WriteLine("|            However, unlike OS threads, they’re cooperatively scheduled as opposed ");
                                Console.WriteLine("|            to preemptively scheduled. A Fiber has it's own stack and instruction pointer.\n|");
                                Console.WriteLine("|   References: https://www.ired.team/offensive-security/code-injection-process-injection/executing-shellcode-with-createfiber");
                                Console.WriteLine("|               https://graphitemaster.github.io/fibers/");
                                Console.WriteLine("|               https://stackoverflow.com/questions/796217/what-is-the-difference-between-a-thread-and-a-fiber");
                                break;
                            case "6":
                                Console.WriteLine("|\n|\t   Fiber Execution");
                                Console.WriteLine("|\t  -----------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: ConvertThreadToFiber, VirtualAlloc, CreateFiber,");
                                Console.WriteLine("|                  VirtualProtectEx, SwitchToFiber\n|");
                                Console.WriteLine("|   Summary: Fibers are a lightweight thread of execution similar to OS threads.");
                                Console.WriteLine("|            However, unlike OS threads, they’re cooperatively scheduled as opposed ");
                                Console.WriteLine("|            to preemptively scheduled. A Fiber has it's own stack and instruction pointer.\n|");
                                Console.WriteLine("|   References: https://www.ired.team/offensive-security/code-injection-process-injection/executing-shellcode-with-createfiber");
                                Console.WriteLine("|               https://graphitemaster.github.io/fibers/");
                                Console.WriteLine("|               https://stackoverflow.com/questions/796217/what-is-the-difference-between-a-thread-and-a-fiber");
                                break;
                            default:
                                Console.WriteLine("|\n|   [!] Unknown technique. Techniques can be found with \"help\" or \"show techniques\".");
                                break;
                        }
                    }
                    break;
                case "EXIT":
                    if (ConfirmExit())
                    {
                        Environment.Exit(0);
                    }
                    Console.Clear();
                    Console.WriteLine("\n  +-------------------+\n _|     MAIN MENU     |\n| +-------------------+");
                    break;
                default:
                    Console.WriteLine("|\n|\t[!] Unknown Command: {0}", command);
                    break;
            }
        }

        /// <summary>
        /// This prompts this user to confirm whether or not they'd like to exit Single Dose.
        /// </summary>
        /// <returns></returns>
        static bool ConfirmExit()
        {
            Console.WriteLine("\nExit program? (Y/N)");
            string input = "";
            do
            {
                Console.Write("\t>");
                input = Console.ReadLine();
            } while (input != "Y" && input != "y" && input != "N" && input != "n");

            if (input == "Y" || input == "y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
