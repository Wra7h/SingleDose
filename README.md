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

### References:
 - https://github.com/SafeBreach-Labs/pinjectra
 - https://github.com/monoxgas/sRDI
 - https://www.ired.team/offensive-security/code-injection-process-injection/
 - https://vx-underground.org/papers.html -> Windows VX -> Injection
