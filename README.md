```
         _____ _____ _   _  _____ _      ______    _____   ____   _____ ______
        / ____|_   _| \ | |/ ____| |    |  ____|  |  __ \ / __ \ / ____|  ____|
       | (___   | | |  \| | |  __| |    | |__     | |  | | |  | | (___ | |__
        \___ \  | | | . ` | | |_ | |    |  __|    | |  | | |  | |\___ \|  __|
        ____) |_| |_| |\  | |__| | |____| |____   | |__| | |__| |____) | |____
       |_____/|_____|_| \_|\_____|______|______|  |_____/ \____/|_____/|______|
        _______________________________________________________________________
       |_______________________________________________________________________|
                                                                 [GitHub: Wra7h]
```
Configure and create process injection binaries. 

Single Dose was built to run using .NET Framework v3.5. Binaries generated with Single Dose are also built for .NET Framework 3.5+. Available techniques will inject either DLL or Shellcode (raw format) depending on the technique.

Each technique has been tested with either :  
1. MessageBox64.dll: https://github.com/enigma0x3/MessageBox
2. Raw shellcode from MSFVenom that executes calc  

## Basic Usage:  
Creating the first binary can be accomplished using the commands below. Just enter the commands below and fill in the brackets as you see fit.  
```
  +--> settings
  +-->> output [output directory]
  +-->> mode [mode #] (1. Static, 2. Dynamic, or 3. Download)
  +-->> exit
  +--> show techniques
  +--> build [technique name/number]
```

### "What's going on here?"    
When you first execute Single Dose, you are at the Main Menu. Entering "settings" takes you to the settings submenu where you can configure how Single Dose builds your binary. 
The next 2 commands are from this submenu. "Output" tells Single Dose where to store your .cs and .exe, and the "mode" command will tell Single Dose whether you plan on embedding injection data or providing data at the time of execution. The "Static" mode will embed all injection data into the binary, "Dynamic" and "Download" will require the user specify flags at execution. **Important: Download mode only supports downloads over http, and not https at this time.** Entering "exit" from the Settings submenu will take you back to Main Menu. "Output" and "Mode" are the only required settings in order to successfully build a technique. Lastly, "show techniques" will display all of the techniques supported, and "build" will start the build process for a binary.  

## Main Menu Commands:  
| Command       | Action             | Example Usage  |
|:-------------:|:-------------:|:-------------:|
| Settings      | Enter Settings submenu | `settings`|
| Triggers      | Enter Triggers submenu | `triggers`|
| Show          | Display techniques, current settings | `show` or `show techniques` |
| Build         | Build a technique      | `build 2` or ` build SRDI` |
| Describe      | Describe a technique   | `describe 4` or `describe Suspend_QueueUserAPC` |
| Clear         | Clear the console, settings, or triggers | `clear`, `clear settings`, or `clear triggers` |
| Blurb         | Enable/Disable the display of available commands when switching between menus | `blurb` |
| Help          | Display the help for this menu | `help` |

## Available Techniques:  
| Technique       | Description             | Inject Data |
|:-------------:|:-------------:|:-------------:|
| CreateRemoteThread | Inject a DLL into a remote process and execute with CreateRemoteThread. | DLL |
| SRDI | Convert DLL into shellcode and inject. | DLL |
| EarlyBird_QueueUserAPC | Inject Shellcode into a new spawned process. | Shellcode |
| Suspend_QueueUserAPC | Inject Shellcode into a process currently running. | Shellcode |
| Syscall_CreateThread | Inject Shellcode using direct syscalls. | Shellcode |
| Fiber_Execution | Execute Shellcode via Fibers. | Shellcode |
| EnumWindows | Execute Shellcode via Callback. | Shellcode |
| EnumChildWindows | Execute Shellcode via Callback. | Shellcode |
| EnumDateFormatsEx | Execute Shellcode via Callback. | Shellcode |
| EnumDesktops | Execute Shellcode via Callback. | Shellcode |


## Settings Commands:  

| Command       | Action             | Example Usage  |
|:-------------:|:-------------:|:-------------:|
| Mode          | Required. Select the execution mode. \[Static, Dynamic or Download] | `mode 1` or `mode static` | 
| Output        | Required. Set the output directory for logs and binaries | `output C:\users\users\Desktop\Single_Dose` |
| Compile       | Enable/Disable compilation. If disabled, only a .cs will be generated \[Default: enabled]| `compile` |
| Log           | Enable/Disable logging. If enabled, a .log file will be in your output directory \[Default: enabled] | `log` |
| Blurb         | Enable/Disable the display of available commands when switching between menus | `blurb` |
| Show          | Display techniques, current settings | `show` or `show techniques` |
| Version       | Display/Set available versions of csc.exe on the host. This allows you to compile binaries for various versions of the .NET Framework. If the version is not displayed, the user can specify the absolute path with the "Custom" option. \[Default: v3.5] | `version`, `version #`, `version Roslyn`, or `version Custom` |
| Clear         | Clear the console, settings, or triggers | `clear`, `clear settings`, or `clear triggers` |
| Triggers      | Enter Triggers submenu | `triggers`|
| Exit          | Return to Main Menu | `exit` |
| Help          | Display the help for this menu | `help` |

| Mode          | Description   |
|:-------------:|:-------------:|
| Static        | Embed all data into the executable |
| Dynamic       | Specify data at runtime with flags in the commandline. `-PID, -DLL/-Bin` |
| Download      | Specify data at runtime with flags in the commandline. HTTP only. `-PID, -URI` |

## Triggers Commands:
The triggers tells the binary to only inject under certain conditions. Triggers are not required but need to be configured before building the binary if you choose to use them.  
 
| Command      | Action         | Example Usage |
|:-------------:|:-------------:|:-------------:|
| Avoid         | Do not execute if condition is met. Binary will exit if condition is met. Accepts PID, Module (DLL), or Process name (EXE). | `avoid notepad.exe`, `avoid *` |
| Require       | Only execute if all conditions are met. Binary will exit if conditions are not met. Accepts PID, Module (DLL), or Process name (EXE). | `require calc.exe`, `require *` |
| Hibernate     | Similar to the require trigger, but the binary will sleep instead of exit while the condition is not met. The sleep value is a random value between 90 seconds and 5 min. Accepts PID, Module (DLL), or Process name (EXE). | `hibernate ntdll.dll`, `hibernate *` |
| Persist       | Similar to the avoid trigger, but the binary will sleep instead of exit while the condition is met. The sleep value is a random value between 90 seconds and 5 min. Accepts PID, Module (DLL), or Process name (EXE). | `persist spoolsv.exe`, `persist *` |
| Show          | Display techniques, current settings | `show` or `show techniques` |
| Blurb         | Enable/Disable the display of available commands when switching between menus | `blurb` |
| Clear         | Clear the console, settings, or triggers | `clear`, `clear settings`, or `clear triggers` |
| Settings      | Enter Settings submenu | `settings`|
| Exit          | Return to Main Menu    | `exit` |
| Help          | Display the help for this menu | `help` |

Example Scenarios:
  1. "I want my binary to exit if notepad.exe is not currently running on the system."  
     - For this you would use the `avoid` trigger. Command: `avoid notepad.exe`  
     
  2. "My binary should _only_ execute if notepad.exe and pid 2048 are running on the system."  
     - This would be the `require` trigger. Command:
     ``` 
     require notepad.exe
     require 2048
     ```
     
  3. "I want my binary to pause execution, *but not exit* while notepad.exe is not running on the system."  
     - Check out the `hibernate` trigger. Hibernate will sleep until the condition is met. The sleep time is randomly generated.  
       The console will remain open during sleep, with status messages provided every time the binary checks the system for the conditions.  
       If you only want to inject when notepad is running, the command is: `hibernate notepad.exe`  
       
  4. "I want my binary to pause execution, *but not exit* while explorer.exe is running on the system."
     - This is the `persist` trigger. Persist will sleep while the condintion is met. The sleep time is randomly generated. Like, `hibernate`  
       the console will remain open during sleep, and status messages provided every time the binary checks the system for the conditions.  
       Command: `persist explorer.exe`. This means the binary will only inject, if explorer.exe is _not_ running on the system when the conditions
       are reevaluated.   

## What's next?
This has been a project built in my spare time. I plan on adding more techniques as time goes on. Constructive criticism is welcome and appreciated! Feel free to message me on Twitter @_Wra7h  

### Check these awesome references out:  
  - https://github.com/monoxgas/sRDI
  - https://3xpl01tc0d3r.blogspot.com/2019/12/process-injection-part-v.html
  - https://sevrosecurity.com/2020/04/13/process-injection-part-2-queueuserapc/
  - https://jhalon.github.io/utilizing-syscalls-in-csharp-1/
  - https://jhalon.github.io/utilizing-syscalls-in-csharp-2/
  - https://www.solomonsklash.io/syscalls-for-shellcode-injection.html  
  - https://www.ired.team/offensive-security/code-injection-process-injection/
  - https://graphitemaster.github.io/fibers/  
  - https://vx-underground.org/papers.html -> Windows VX -> Injection
  
