using SingleDose.ShellcodeFormats;
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
            Settings.dAvailableCSCVersions = Settings.FetchCSCVersions();

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

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("[*] Total CSC Versions Found: ");
            if (Settings.dAvailableCSCVersions.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("{0}", Settings.dAvailableCSCVersions.Count);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0}", Settings.dAvailableCSCVersions.Count);
            }
            Console.ResetColor();

            Console.WriteLine("\n       +---------------------------+\n ______|         MAIN MENU         |\n|      +---------------------------+");
            Console.WriteLine("|\n|\tsettings   triggers   build");
            Console.WriteLine("|\tdescribe   show       blurb");
            Console.WriteLine("|\thelp       clear      save");
            Console.WriteLine("|\texit");

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
            Console.WriteLine("|\t   Output Directory = {0}", Settings.szOutputDirectory);
            Console.WriteLine("|\t   CSC Version      = {0}", Settings.szSelectedCscVersion);
            if (Settings.szSelectedCscVersion.ToLower() == "custom")
            {
                Console.WriteLine("|\t   CSC Path         = {0}", Settings.szSelectedCompilerPath);
            }

            if (Settings.szCurrentLogFile != "")
            {
                Console.WriteLine("|\t   Log File         = {0}", Settings.szCurrentLogFile);
            }

            if (Settings.bvLogging)
            {
                Console.WriteLine("|\t   Logging          = Enabled");
            }
            else if (!Settings.bvLogging)
            {
                Console.WriteLine("|\t   Logging          = Disabled");
            }
            Console.WriteLine("|\t   Compile          = {0}", Settings.bvCompileBinary);
            Console.WriteLine("|\t   Mode             = {0}", Settings.szInjectMode);

            Console.WriteLine("|\n|\t              Triggers");
            Console.WriteLine("|\t          +--------------+");
            if (REQUIREDPROCESSDETAILS == "" && HIBERNATEPROCESSDETAILS == "" && AVOIDPROCESSDETAILS == "" && PERSISTPROCESSDETAILS == "" && TIMERSECONDS == "")
            {
                Console.WriteLine("|\t      No triggers configured.");
            }
            if (REQUIREDPROCESSDETAILS != "")
            {
                Console.WriteLine("|\t   Requirements     = {0}", REQUIREDPROCESSDETAILS.ToLower());
            }
            if (HIBERNATEPROCESSDETAILS != "")
            {
                Console.WriteLine("|\t   Hibernate Reqs   = {0}", HIBERNATEPROCESSDETAILS.ToLower());
            }
            if (AVOIDPROCESSDETAILS != "")
            {
                Console.WriteLine("|\t   Avoid Reqs   = {0}", AVOIDPROCESSDETAILS.ToLower());
            }
            if (PERSISTPROCESSDETAILS != "")
            {
                Console.WriteLine("|\t   Persist Reqs   = {0}", PERSISTPROCESSDETAILS.ToLower());
            }
            if (TIMERSECONDS != "")
            {
                Console.WriteLine("|\t   Timer = {0} seconds", TIMERSECONDS);
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
                    Console.WriteLine("|\n|             +------------+----------------------------------------------------------+----------------------+");
                    Console.WriteLine("|             |                                     MAIN HELP                                                |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+----------------------+");
                    Console.WriteLine("|             |  Command   |                       Description                        |     Example Usage    |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+----------------------+");
                    Console.WriteLine("|             |  Settings  | Enter the Settings submenu                               | > Settings           |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+----------------------+");
                    Console.WriteLine("|             |  Triggers  | Enter the Triggers submenu                               | > Triggers           |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+----------------------+");
                    Console.WriteLine("|             |  Build     | Build a loader/inject technique (See techniques below.)  | > build r1           |");
                    Console.WriteLine("|             |            |                                                          | > build CreateFiber  |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+----------------------+");
                    Console.WriteLine("|             |  Save      | Save an entry from history to a file.                    | > save  h1           |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+----------------------+");
                    Console.WriteLine("|             |  Show      | Display current config, techniques or history entries    | > show               |");
                    Console.WriteLine("|             |            |                                                          | > show history       |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+----------------------+");
                    Console.WriteLine("|             |  Describe  | See a description for a technique                        | > describe r5        |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+----------------------+");
                    Console.WriteLine("|             |  Clear     | Clear the terminal, settings, or triggers                | > clear              |");
                    Console.WriteLine("|             |            |                                                          | > clear triggers     |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+----------------------+");
                    Console.WriteLine("|             |  Blurb     | Display available commands when switching/clearing menus | > blurb              |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+----------------------+");
                    Console.WriteLine("|             |  Help      | Display this help                                        | > help               |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+----------------------+");
                    Console.WriteLine("|             |  Exit      | Exit Single Dose                                         | > exit               |");
                    Console.WriteLine("|             +------------+----------------------------------------------------------+----------------------+\n|\n|");
                    Console.WriteLine("|                          +-----------------------------------------------------------------+");
                    Console.WriteLine("|                          |                     AVAILABLE TECHNIQUES                        |");
                    Console.WriteLine("|                          +-----------------------------+-----------------------------------+");
                    Console.WriteLine("|                          |      Shellcode Loaders      |          Process Injects          |");
                    Console.WriteLine("|                          +-----------------------------+-----------------------------------+");
                    Console.WriteLine("|                          |  L1. Syscall_CreateThread   | R1. CreateRemoteThread-DLL [DLL]  |");
                    Console.WriteLine("|                          |  L2. SRDI-Loader            | R2. EarlyBird_QueueUserAPC        |");
                    Console.WriteLine("|                          |  L3. CreateFiber            | R3. Suspend_QueueUserAPC          |");
                    Console.WriteLine("|                          |  L4. EnumWindows            | R4. AddressOfEntryPoint           |");
                    Console.WriteLine("|                          |  L5. EnumChildWindows       | R5. KernelCallbackTable           |");
                    Console.WriteLine("|                          |  L6. EnumDateFormatsEx      | R6. NtCreateSection               |");
                    Console.WriteLine("|                          |  L7. EnumDesktops           |                                   |");
                    Console.WriteLine("|                          +-----------------------------+-----------------------------------+");
                    break;
                case "SETTINGS":
                    Settings.SettingsMenu();
                    Console.WriteLine("\n       +---------------------------+\n ______|         MAIN MENU         |\n|      +---------------------------+");
                    if (Settings.bvHelpBlurb)
                    {
                        Console.WriteLine("|\n|\tsettings   triggers   build");
                        Console.WriteLine("|\tdescribe   show       blurb");
                        Console.WriteLine("|\thelp       clear      save");
                        Console.WriteLine("|\texit");
                    }
                    break;
                case "TRIGGERS":
                    Triggers.TriggersMenu();
                    Console.WriteLine("\n       +---------------------------+\n ______|         MAIN MENU         |\n|      +---------------------------+");
                    if (Settings.bvHelpBlurb)
                    {
                        Console.WriteLine("|\n|\tsettings   triggers   build");
                        Console.WriteLine("|\tdescribe   show       blurb");
                        Console.WriteLine("|\thelp       clear      save");
                        Console.WriteLine("|\texit");
                    }
                    break;
                case "BLURB":
                    if (Settings.bvHelpBlurb)
                    {
                        Settings.bvHelpBlurb = false;
                        Console.WriteLine("|\n|   [~] Help blurbs have been disabled.");
                    }
                    else if (!Settings.bvHelpBlurb)
                    {
                        Settings.bvHelpBlurb = true;
                        Console.WriteLine("|\n|\tsettings   triggers   build");
                        Console.WriteLine("|\tdescribe   show       blurb");
                        Console.WriteLine("|\thelp       clear      save");
                        Console.WriteLine("|\texit");
                    }
                    break;
                case "BUILD":
                    if (Settings.szInjectMode != null && Settings.szOutputDirectory != null)
                    {
                        if (command.Split().Count() > 1)
                        {
                            switch (command.Split()[1].ToUpper())
                            {
                                case "CREATEREMOTETHREAD-DLL":
                                    Console.WriteLine("|\n|   [*] Building technique: DLL CreateRemoteThread");
                                    Settings.szSelectedTechnique = "CreateRemoteThread-DLL";

                                    //Memory Allocation Method
                                    Settings.listPInvokeRecipe.Add("VirtualAllocEx");

                                    //Remaining PInvokes needed
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "GetModuleHandle", "GetProcAddress", "WriteProcessMemory_ByteArray", "CreateRemoteThread" });

                                    break;
                                case "R1":
                                    goto case "CREATEREMOTETHREAD-DLL";
                                case "SRDI-LOADER":
                                    Console.WriteLine("|\n|   [*] Building technique: Shellcode Reflective DLL Injection (SRDI)");
                                    Settings.szSelectedTechnique = "SRDI-Loader";
                                    break;
                                case "L2":
                                    goto case "SRDI-LOADER";
                                case "EARLYBIRD_QUEUEUSERAPC":
                                    Console.WriteLine("|\n|   [*] Building technique: EarlyBird_QueueUserAPC");
                                    Settings.szSelectedTechnique = "EarlyBird_QueueUserAPC";

                                    //Memory Allocation Method
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "VirtualAllocEx", "VirtualProtectEx" });

                                    //Remaining PInvokes needed
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "CreateProcess", "WriteProcessMemory_ByteArray", "OpenThread", "QueueUserAPC", "ResumeThread" });
                                    break;
                                case "R2":
                                    goto case "EARLYBIRD_QUEUEUSERAPC";
                                case "SUSPEND_QUEUEUSERAPC":
                                    Console.WriteLine("|\n|   [*] Building technique: Suspend_QueueUserAPC");
                                    Settings.szSelectedTechnique = "Suspend_QueueUserAPC";

                                    //Memory Allocation Method
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "VirtualAllocEx", "VirtualProtectEx" });

                                    //Remaining PInvokes needed
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "WriteProcessMemory_ByteArray", "OpenThread", "QueueUserAPC" });

                                    break;
                                case "R3":
                                    goto case "SUSPEND_QUEUEUSERAPC";
                                case "SYSCALL_CREATETHREAD":
                                    Console.WriteLine("|\n|   [*] Building technique: Syscall_CreateThread");
                                    Settings.szSelectedTechnique = "Syscall_CreateThread";
                                    break;
                                case "L1":
                                    goto case "SYSCALL_CREATETHREAD";
                                case "CREATEFIBER":
                                    Console.WriteLine("|\n|   [*] Building technique: CREATEFIBER");
                                    Settings.szSelectedTechnique = "CreateFiber";

                                    //Memory Allocation Method
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "VirtualAlloc", "VirtualProtectEx" });

                                    //Remaining PInvokes needed
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "ConvertThreadToFiber", "CreateFiber", "SwitchToFiber" });

                                    break;
                                case "L3":
                                    goto case "CREATEFIBER";
                                case "ENUMWINDOWS":
                                    Console.WriteLine("|\n|   [*] Building technique: EnumWindows");
                                    Settings.szSelectedTechnique = "EnumWindows";

                                    //Memory Allocation Method
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "VirtualAlloc", "VirtualProtectEx" });

                                    //Remaining PInvokes needed
                                    Settings.listPInvokeRecipe.Add("EnumWindows");

                                    break;
                                case "L4":
                                    goto case "ENUMWINDOWS";
                                case "ENUMCHILDWINDOWS":
                                    Console.WriteLine("|\n|   [*] Building technique: EnumChildWindows");
                                    Settings.szSelectedTechnique = "EnumChildWindows";

                                    //Memory Allocation Method
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "VirtualAlloc", "VirtualProtectEx" });

                                    //Remaining PInvokes needed
                                    Settings.listPInvokeRecipe.Add("EnumChildWindows");
                                    break;
                                case "L5":
                                    goto case "ENUMCHILDWINDOWS";
                                case "ENUMDATEFORMATSEX":
                                    Console.WriteLine("|\n|   [*] Building technique: EnumDateFormatsEx");
                                    Settings.szSelectedTechnique = "EnumDateFormatsEx";

                                    //Memory Allocation Method
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "VirtualAlloc", "VirtualProtectEx" });

                                    //Remaining PInvokes needed
                                    Settings.listPInvokeRecipe.Add("EnumDateFormatsEx");
                                    break;
                                case "L6":
                                    goto case "ENUMDATEFORMATSEX";
                                case "ENUMDESKTOPS":
                                    Console.WriteLine("|\n|   [*] Building technique: EnumDesktops");
                                    Settings.szSelectedTechnique = "EnumDesktops";

                                    //Memory Allocation Method
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "VirtualAlloc", "VirtualProtectEx" });

                                    //Remaining PInvokes needed
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "GetProcessWindowStation", "EnumDesktops" });

                                    break;
                                case "L7":
                                    goto case "ENUMDESKTOPS";
                                case "ADDRESSOFENTRYPOINT":
                                    Console.WriteLine("|\n|   [*] Building technique: AddressOfEntryPoint");
                                    Settings.szSelectedTechnique = "AddressOfEntryPoint";

                                    //Memory Allocation Method
                                    Settings.listPInvokeRecipe.Add("VirtualAllocEx");

                                    //Remaining PInvokes needed
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "CreateProcess", "NtQueryInformationProcess", "ReadProcessMemory" , "WriteProcessMemory_ByteArray", "ResumeThread"});

                                    break;
                                case "R4":
                                    goto case "ADDRESSOFENTRYPOINT";
                                case "KERNELCALLBACKTABLE":
                                    Console.WriteLine("|\n|   [*] Building technique: KernelCallbackTable");
                                    Settings.szSelectedTechnique = "KernelCallbackTable";

                                    //Memory Allocation Method
                                    Settings.listPInvokeRecipe.Add("VirtualAllocEx");

                                    //Remaining PInvokes needed
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "NtQueryInformationProcess", "ReadProcessMemory", "WriteProcessMemory_IntPtr", "SendMessage" });

                                    break;
                                case "R5":
                                    goto case "KERNELCALLBACKTABLE";
                                case "NTCREATESECTION":
                                    Console.WriteLine("|\n|   [*] Building technique: NtCreateSection");
                                    Settings.szSelectedTechnique = "NtCreateSection";

                                    //Memory Allocation Method
                                    Settings.listPInvokeRecipe.Add("");

                                    //Remaining PInvokes needed
                                    Settings.listPInvokeRecipe.AddRange(new string[] { "NtCreateSection", "NtMapViewOfSection", "RtlCreateUserThread"});
                                    break;
                                case "R6":
                                    goto case "NTCREATESECTION";
                                default:
                                    Console.WriteLine("|\n|   [!] Unknown technique. Techniques can be found in help.");
                                    break;
                            }

                            
                            if (Settings.szSelectedTechnique != null)
                            {
                                //Build the .cs 
                                CSConstructor(Settings.szInjectMode, Settings.szSelectedTechnique);
                                //then clear the selected technique
                                Settings.szSelectedTechnique = null;
                                Settings.listPInvokeRecipe.Clear();
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
                                Settings.szInjectMode = null;
                                Settings.szOutputDirectory = null;
                                Settings.bvCompileBinary = true;
                                Console.WriteLine("|\n|   [~] Output and Mode have been cleared.");
                                break;
                            case "TRIGGERS":
                                HIBERNATEPROCESSDETAILS = "";
                                REQUIREDPROCESSDETAILS = "";
                                AVOIDPROCESSDETAILS = "";
                                PERSISTPROCESSDETAILS = "";
                                TIMERSECONDS = "";
                                TriggersToUse.Clear();
                                Console.WriteLine("|\n|   [~] Triggers have been cleared.");
                                break;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n       +---------------------------+\n ______|         MAIN MENU         |\n|      +---------------------------+");
                        if (Settings.bvHelpBlurb)
                        {
                            Console.WriteLine("|\n|\tsettings   triggers   build");
                            Console.WriteLine("|\tdescribe   show       blurb");
                            Console.WriteLine("|\thelp       clear      exit");
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
                            case "HISTORY":
                                Shellcode.DisplayHistory();
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        PrintSettings();
                    }
                    break;
                case "SAVE":
                    string historyItem = "";
                    if (command.Split().Count() > 1)
                    {
                        if (command.Split()[1].Length < 3 && shellcodeHistory.Count > 0)
                        {
                            historyItem = command.Split()[1];
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (shellcodeHistory.Count > 0)
                        {
                            Shellcode.DisplayHistory();
                            Console.WriteLine("|   [~] Enter selection: ");
                            do
                            {
                                Console.Write("|       > ");
                                historyItem = Console.ReadLine();
                            } while (historyItem.ToLower() != "exit" && !((historyItem.Length < 3 && int.Parse(historyItem[1].ToString()) <= shellcodeHistory.Count)));

                            if (historyItem.ToLower() == "exit")
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                    string payloadDirectory = Settings.szOutputDirectory + @"\Payloads";
                    if (!System.IO.Directory.Exists(payloadDirectory))
                    {
                        try
                        {
                            System.IO.Directory.CreateDirectory(payloadDirectory);
                        }
                        catch
                        {
                            Console.WriteLine("|\n|\t[!] Error creating payload directory. Please try again.");
                            break;
                        }
                    }

                    int entry = int.Parse(historyItem[1].ToString());
                    if (entry <= shellcodeHistory.Count && entry != 0)
                    {
                        string payloadFilename = System.IO.Path.GetRandomFileName().Split('.')[0] + ".bin";
                        string payloadFullPath = payloadDirectory + @"\" + payloadFilename;
                        System.IO.File.WriteAllBytes(payloadFullPath, shellcodeHistory[entry - 1].Shellcode);

                        if (System.IO.File.Exists(payloadFullPath))
                        {
                            Console.WriteLine("|\n|   [+] Payload saved as: {0}\n|", payloadFilename);
                        }
                        else
                        {
                            Console.WriteLine("|\n|   [!] Error saving payload.\n|");
                        }
                    }
                    break;
                case "DESCRIBE":
                    if (command.Split().Count() > 1)
                    {
                        switch (command.Split()[1].ToUpper())
                        {
                            case "CREATEREMOTETHREAD-DLL":
                                Console.WriteLine("|\n|\t CreateRemoteThread");
                                Console.WriteLine("|\t--------------------\n|");
                                Console.WriteLine("|   Inject Source: DLL");
                                Console.WriteLine("|   P/Invoke APIs: GetModuleHandle, GetProcAddress, VirtualAllocEx,");
                                Console.WriteLine("|                  WriteProcessMemory, CreateRemoteThread\n|");
                                Console.WriteLine("|   References: https://www.ired.team/offensive-security/code-injection-process-injection/dll-injection \n|");
                                break;
                            case "R1":
                                goto case "CREATEREMOTETHREAD-DLL";
                            case "SRDI-Loader":
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
                            case "L2":
                                goto case "SRDI-Loader";
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
                            case "R2":
                                goto case "EARLYBIRD_QUEUEUSERAPC";
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
                            case "R3":
                                goto case "SUSPEND_QUEUEUSERAPC";
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
                            case "L1":
                                goto case "SYSCALL_CREATETHREAD";
                            case "CREATEFIBER":
                                Console.WriteLine("|\n|\t   CreateFiber ");
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
                            case "L3":
                                goto case "CREATEFIBER";
                            case "ENUMWINDOWS":
                                Console.WriteLine("|\n|\t     EnumWindows");
                                Console.WriteLine("|\t  -----------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: VirtualAlloc, VirtualProtectEx, EnumWindows\n|");
                                Console.WriteLine("|   Summary: Unavailable at this time.\n|");
                                Console.WriteLine("|   References: https://vx-underground.org/papers/VXUG/Mirrors/Injection/callbackinjection/EnumWindows.cpp");
                                break;
                            case "L4":
                                goto case "ENUMWINDOWS";
                            case "ENUMCHILDWINDOWS":
                                Console.WriteLine("|\n|\t    EnumChildWindows");
                                Console.WriteLine("|\t  --------------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: VirtualAlloc, VirtualProtectEx, EnumChildWindows\n|");
                                Console.WriteLine("|   Summary: Unavailable at this time.\n|");
                                Console.WriteLine("|   References: https://vx-underground.org/papers/VXUG/Mirrors/Injection/callbackinjection/EnumChildWindows.cpp");
                                break;
                            case "L5":
                                goto case "ENUMCHILDWINDOWS";
                            case "ENUMDATEFORMATSEX":
                                Console.WriteLine("|\n|\t    EnumDateFormatsEx");
                                Console.WriteLine("|\t   -------------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: VirtualAlloc, VirtualProtectEx, EnumDateFormatsEx\n|");
                                Console.WriteLine("|   Summary: Unavailable at this time.\n|");
                                Console.WriteLine("|   References: https://vx-underground.org/papers/VXUG/Mirrors/Injection/callbackinjection/EnumDateFormatsA.cpp");
                                break;
                            case "L6":
                                goto case "ENUMDATEFORMATSEX";
                            case "ENUMDESKTOPS":
                                Console.WriteLine("|\n|\t     EnumDesktops");
                                Console.WriteLine("|\t  -----------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: VirtualAlloc, VirtualProtectEx, GetCurrentThreadId,");
                                Console.WriteLine("|                  GetThreadDesktop, EnumDesktopWindows\n|");
                                Console.WriteLine("|   Summary: Unavailable at this time.\n|");
                                Console.WriteLine("|   References: https://vx-underground.org/papers/VXUG/Mirrors/Injection/callbackinjection/EnumDesktopW.cpp");
                                break;
                            case "L7":
                                goto case "ENUMDESKTOPS";
                            case "ADDRESSOFENTRYPOINT":
                                Console.WriteLine("|\n|\t     AddressOfEntryPoint");
                                Console.WriteLine("|\t   -----------------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: CreateProcess, NtQueryInformationProcess, ReadProcessMemory,");
                                Console.WriteLine("|                  WriteProcessMemory, ResumeThread\n|");
                                Console.WriteLine("|   Summary: Writes your shellcode to the entrypoint of the suspended process.\n|");
                                Console.WriteLine("|   References: https://www.ired.team/offensive-security/code-injection-process-injection/addressofentrypoint-code-injection-without-virtualallocex-rwx");
                                break;
                            case "R4":
                                goto case "ADDRESSOFENTRYPOINT";
                            case "KERNELCALLBACKTABLE":
                                Console.WriteLine("|\n|\t     KernelCallbackTable");
                                Console.WriteLine("|\t   -----------------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: NtQueryInformationProcess, ReadProcessMemory, WriteProcessMemory");
                                Console.WriteLine("|                  SendMessage\n|");
                                Console.WriteLine("|   Summary: This technique was used by the FinFisher/FinSpy surveillance spyware.\n|");
                                Console.WriteLine("|   References: https://modexp.wordpress.com/2019/05/25/windows-injection-finspy/");
                                break;
                            case "R5":
                                goto case "KERNELCALLBACKTABLE";
                            case "NTCREATESECTION":
                                Console.WriteLine("|\n|\t     NtCreateSection");
                                Console.WriteLine("|\t   -------------------\n|");
                                Console.WriteLine("|   Inject Source: Shellcode");
                                Console.WriteLine("|   P/Invoke APIs: NtCreateSection, NtMapViewOfSection, RtlCreateUserThread\n|");
                                Console.WriteLine("|   Summary: Create a remote thread in the target process after mapping a new");
                                Console.WriteLine("|   section containing shellcode\n|");
                                Console.WriteLine("|   References: https://www.ired.team/offensive-security/code-injection-process-injection/ntcreatesection-+-ntmapviewofsection-code-injection");
                                break;
                            case "R6":
                                goto case "NTCREATESECTION";
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
