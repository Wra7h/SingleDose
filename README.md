# SingleDose

SingleDose is a framework to build shellcode load/process injection techniques. SingleDose doesn't actually perform the load or inject, but rather it takes your configuration and technique and will compile an `.exe` which will only contain the technique you specified. The executables are C#, which gives users the flexibility to execute in-memory with `execute-assembly` or the like.

<img src="/Images/Demo.gif" width="100%"/>

I am not the original author for most of these techniques, I just rewrote them using C# to make them compatible with SingleDose's build process. Please check out the description for each technique for links to the author or blog I referenced. This is done with `describe <technique>` from the Main menu. To see the available techniques type `show techniques` the main menu.

## Boosters

Boosters are just plugins for SingleDose. They implement additional techniques or triggers without overwhelming the basic framework. These can be loaded from the `Main` or `Triggers` menus.  

Usage:&nbsp;&nbsp;&nbsp;&nbsp;`[Main]-> load .\PoisonTendy.dll`  

## Basic Usage:  

A tutorial is included for basic building without triggers. Just run SingleDose with `-t` to start the tutorial mode.

Note: To see all available options, type `help` in each menu.

1. Enter the Settings menu with: `settings`  
2. Select the mode: `mode static`  
3. Set the output directory: `output C:\path\to\output\directory`  
4. If both of these modes have been successfully set, exit the settings menu to go back to Main Menu. Type `exit`  
5. To see what techniques are available. In Main Menu, type `show techniques`  
&nbsp;&nbsp;- The left column is for shellcode loaders, and the right column is for the process injection techniques.  
6. Start a build with: `build L3`
7. It will ask you for the path to your shellcode, so just type the path and SingleDose will format it and embed it as a byte array in the .cs file.

## Example using Triggers:
Triggers allow you to configure pre-execution checks or conditions to meet before continuing the technique's execution. We'll use the `ProcWatch` trigger.  

1. Complete steps 1-4 of the Basic Usage.  
2. Enter the Triggers menu with: `triggers`
3. Type `help` to see the available triggers. The next few steps will walk through the "procwatch" trigger.
4. Type `use procwatch` you will be prompted to enter a process id and/or name.  
5. Type `notepad.exe`. This will prevent the technique from being executed while a "notepad.exe" is a process. Procwatch supports CSV format when specifying multiple items.   
6. Once you see that `ProcWatch` is set in the panel as the trigger, type `exit` in the Triggers Menu to return to Main Menu.
7. From here, you can build like step 6 in Basic Usage.
8. If you try to execute the technique with a notepad.exe process running, the technique will enter a "sleep" before checking if notepad.exe is running again. The sleep time is randomly generated between 90 - 300 seconds, and the exact time will be displayed in the console window.

Note: Triggers are not erased after a build. Type `clear triggers` to clear the triggers you've set.  

## Compile
Open the .sln in Visual Studio, select Debug or Release at the top and build.

### References:
 - https://github.com/SafeBreach-Labs/pinjectra
 - https://github.com/monoxgas/sRDI
 - https://www.ired.team/offensive-security/code-injection-process-injection/
 - https://vx-underground.org/papers.html -> Windows VX -> Injection  
 - https://bohops.com/2022/04/02/unmanaged-code-execution-with-net-dynamic-pinvoke/
 - References per technique can be seen with `describe <technique>`
