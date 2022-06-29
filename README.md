# Single Dose

Single Dose is a .NET console application containing collection of shellcode loaders and process injection techniques. Single Dose doesn't actually perform the load or inject, instead it takes your configuration and technique, then compiles an executable that only contains the technique you specified. This is the main difference between this tool and similar collections of injects like PInjectra where every technique is within the same binary doing the execution. The executables are C#, which gives users the flexibility to execute in-memory with `execute-assembly`. 

There are 3 different modes: Static, Dynamic, and Download.
 - Static: Embeds the shellcode and other target information (i.e. Process ID, Process Name) into the executable. 
 - Dynamic: This mode compiles the executable to accept flags on the commandline with `-PID` and `-DLL`/`-BIN`
 - Download: This mode compiles the executable to accept flags on the commandline with `-PID` and `URI`. (HTTP only)

\**Static mode allows users to specify a path on disk to Raw Shellcode OR a DLL. Using SRDI, Single Dose converts the DLL to shellcode before it is embedded into the executable.

There are 3 different menus: Main, Settings, and Triggers.
 - Main Menu: This menu contains a few commands, but the most important being `describe` and `build`.
 - Settings Menu: This menu is used to configure how the technique is built. The only required commands before building can be found here. They are `mode` and `output`. 
 - Triggers Menu: The triggers menu allows you to configure "checks" or conditions to meet before continuing the technique's execution. All of these are optional, may be useful in various scenarios.

You start in Main Menu, but to access the other menus you just type `settings` or `triggers`. To return back to Main Menu when you are ready to build, you just type `exit` to exit the Settings or Triggers menu.

I am not the original author for most of these techniques, I just rewrote or translated them into C# to make them compatible with Single Dose's building. Please check out the description for each technique for links to the original author or blog. This is done with `describe <technique>` in the Main Menu. To see the available techniques type `show techniques` from any menu.

## Available Techniques:  
| Technique       | Description             | Inject Data |
|:-------------:|:-------------:|:-------------:|
| CreateRemoteThread-DLL | Inject a DLL into a remote process and execute with CreateRemoteThread. | DLL |
| SRDI-Loader | Convert DLL into shellcode and inject. | DLL |
| EarlyBird_QueueUserAPC | Inject Shellcode into a new spawned process. | Shellcode |
| Suspend_QueueUserAPC | Inject Shellcode into a process currently running. | Shellcode |
| Syscall_CreateThread | Inject Shellcode using direct syscalls. | Shellcode |
| CreateFiber | Execute Shellcode via Fibers. | Shellcode |
| EnumWindows | Execute Shellcode via Callback. | Shellcode |
| EnumChildWindows | Execute Shellcode via Callback. | Shellcode |
| EnumDateFormatsEx | Execute Shellcode via Callback. | Shellcode |
| EnumDesktops | Execute Shellcode via Callback. | Shellcode |
| AddressOfEntryPoint | Inject Shellcode into a suspended process's entrypoint. | Shellcode |
| KernelCallbackTable | Used by the FinFisher/FinSpy surveillance spyware. | Shellcode |
| NtCreateSection | Create a remote thread in the target process after mapping a new section containing shellcode. | Shellcode |

## Basic Usage:  

Note: To see all available options, type `help` in each menu.

1. Enter the Settings menu with: `settings`  
2. Select the mode: `mode static`  
3. Set the output directory: `output C:\path\to\output\directory`  
4. If both of these modes have been successfully set, exit the settings menu to go back to Main Menu. Type `exit`  
5. Let's see what techniques are available. In Main Menu, type `show techniques`  
&nbsp;&nbsp;- The left column is for shellcode loaders, and the right column is for the process injection techniques.  
6. Start a build with: `build L3`
7. It will ask you for the path to your shellcode, so just type the path and Single Dose will format it and embed it as a byte array in the .cs file.

## Example using Triggers:
Triggers allow you to configure "checks" or conditions to meet before continuing the technique's execution. We'll use the `persist` trigger.  

1. Complete steps 1-4 of the Basic Usage.  
2. Enter the Triggers menu with: `triggers`
3. Type `help` to see the available triggers. The next few steps will walk through the "persist" trigger.
4. Type `persist notepad.exe`. This will prevent the technique from being executed while a "notepad.exe" is a process.  
5. If you type `show`, all the settings and triggers will be displayed. You can mix and match the triggers used, i.e. persist and require triggers can both be set before building a technique.
6. Once you see that notepad.exe is set, type `exit` in the Triggers Menu to return to Main Menu.
7. From here, you can build like step 6 in Basic Usage.
8. If you try to execute the technique with a notepad.exe process running, the technique will enter a "sleep" before checking if notepad.exe is running again. The sleep time is randomly generated between 90 - 300 seconds, and the exact time will be displayed in the console window.

Note: Triggers are not erased after a build. Type `clear triggers` to clear the triggers you've set.  

### References:
 - https://github.com/SafeBreach-Labs/pinjectra
 - https://github.com/monoxgas/sRDI
 - https://www.ired.team/offensive-security/code-injection-process-injection/
 - https://vx-underground.org/papers.html -> Windows VX -> Injection
