
using System.Collections.Generic;

namespace SingleDose
{
    partial class Program
    {
        public class SRDIClass
        {
            public string HashFunction { get; set; } 
            public string AdditionalData { get; set; }
            public bool ObfuscateImports { get; set; }
            public int ImportDelay { get; set; }
            public bool ClearHeader { get; set; }
            public string DLLFilepath { get; set; }
            public byte[] DLLData { get; set; }
        }

        //Triggers//

        //Require
        public static List<string> TriggersToUse = new List<string> { };
        public static string REQUIREDPROCESSDETAILS = "";
        public static string REQUIRETRIGGER = @"
            System.Collections.Generic.List<string> requirements = new System.Collections.Generic.List<string> { {{REQUIREMENTS}} };
            bool continueInject = false;
            System.Collections.Generic.List<string> matches = new System.Collections.Generic.List<string> {};
            foreach (var p in System.Diagnostics.Process.GetProcesses())
            {
                try
                {
                    if (requirements.Contains(p.MainModule.ModuleName.ToUpper()))
                    {
                        if (!matches.Contains(p.MainModule.ModuleName.ToUpper()))
                        {
                            matches.Add(p.MainModule.ModuleName.ToUpper());
                        }
                    }
                    
                    if (requirements.Contains(p.Id.ToString()))
                    {
                        matches.Add(p.Id.ToString());
                    }

                    foreach (System.Diagnostics.ProcessModule m in p.Modules)
                    {
                        if (requirements.Contains(m.ModuleName.ToUpper()))
                        {
                            if (!matches.Contains(m.ModuleName.ToUpper()))
                            {
                                matches.Add(m.ModuleName.ToUpper());
                            }
                        }
                    }   
                }
                catch
                {
                    continue;
                }
            }
            if (requirements.Count() == matches.Count()) {
                continueInject = true;
            }

            if (!continueInject)
            {
                Console.WriteLine(""[!] Requirements not met."");
                System.Environment.Exit(0);
            }
            {{TRIGGER}}
";

        //Hibernate
        //SLEEP UNTIL ALL CONDITIONS ARE MET
        public static string HIBERNATEPROCESSDETAILS = "";
        public static string HIBERNATETRIGGER = @"
            System.Collections.Generic.List<string> hibernateRequirements = new System.Collections.Generic.List<string> { {{REQUIREMENTS}} };
            System.Collections.Generic.List<string> hibernateMatches = new System.Collections.Generic.List<string> { };
            do
            {
                foreach (var p in System.Diagnostics.Process.GetProcesses())
                {
                    try
                    {
                        if (hibernateRequirements.Contains(p.MainModule.ModuleName))
                        {
                            if (!hibernateMatches.Contains(p.MainModule.ModuleName.ToUpper()))
                            {
                                hibernateMatches.Add(p.MainModule.ModuleName);
                            }
                        }

                        if (hibernateRequirements.Contains(p.Id.ToString()))
                        {
                            hibernateMatches.Add(p.Id.ToString());
                        }

                        foreach (System.Diagnostics.ProcessModule m in p.Modules)
                        {
                            if (hibernateRequirements.Contains(m.ModuleName.ToUpper()))
                            {
                                if (!hibernateMatches.Contains(m.ModuleName.ToUpper()))
                                {
                                    hibernateMatches.Add(m.ModuleName.ToUpper());
                                }
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (hibernateRequirements.Count() != hibernateMatches.Count())
                {
                    Random sleepTime = new Random();
                    int sleep = sleepTime.Next(90000, 300000);
                    Console.Write(""[ "" + DateTime.UtcNow.ToString() + "" UTC ]"");
                    Console.WriteLine("": -  Next check in {0} sec"",TimeSpan.FromMilliseconds(sleep).TotalSeconds.ToString());
                    System.Threading.Thread.Sleep(sleep);
                }
                else
                {
                    Console.Write(""[ "" + DateTime.UtcNow.ToString() + "" UTC ]"");
                    Console.WriteLine("": +"");
                }

            } while (hibernateRequirements.Count() != hibernateMatches.Count());
            
            {{TRIGGER}}
";

        //Avoid
        //AVOID EXECUTION IF ANY REQUIREMENT IS MET
        public static string AVOIDPROCESSDETAILS="";
        public static string AVOIDTRIGGER = @"
            System.Collections.Generic.List<string> avoid = new System.Collections.Generic.List<string> { {{REQUIREMENTS}} };
            foreach (var p in System.Diagnostics.Process.GetProcesses())
            {
                try
                {
                    if (avoid.Contains(p.MainModule.ModuleName.ToUpper()) || avoid.Contains(p.Id.ToString()))
                    {
                        Console.WriteLine(""[!] Execution not permitted."");
                        Environment.Exit(1);
                    }

                    foreach (System.Diagnostics.ProcessModule m in p.Modules)
                    {
                        if (avoid.Contains(m.ModuleName.ToUpper()))
                        {
                            Console.WriteLine(""[!] Execution not permitted."");
                            Environment.Exit(1);
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
            {{TRIGGER}}";

        //Persist
        //SLEEP UNTIL CONDITIONS ARE NOT MET
        public static string PERSISTPROCESSDETAILS = "";
        public static string PERSISTTRIGGER = @"
            System.Collections.Generic.List<string> persistRequirements = new System.Collections.Generic.List<string> { {{REQUIREMENTS}} };
            int conditionsMet;
            do
            {
                conditionsMet = 0;
                foreach (var p in System.Diagnostics.Process.GetProcesses())
                {
                    try
                    {
                        if (persistRequirements.Contains(p.MainModule.ModuleName))
                        {
                            conditionsMet++;
                            continue;
                        }

                        if (persistRequirements.Contains(p.Id.ToString()))
                        {
                            conditionsMet++;
                            continue;
                        }

                        foreach (System.Diagnostics.ProcessModule m in p.Modules)
                        {
                            if (persistRequirements.Contains(m.ModuleName.ToUpper()))
                            {
                                conditionsMet++;
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (conditionsMet > 0)
                {
                    Random sleepTime = new Random();
                    int sleep = sleepTime.Next(90000, 300000);
                    Console.Write(""[ "" + DateTime.UtcNow.ToString() + "" UTC ]"");
                    Console.WriteLine("": -  Next check in {0} sec"",TimeSpan.FromMilliseconds(sleep).TotalSeconds.ToString());
                    System.Threading.Thread.Sleep(sleep);
                }
                else
                {
                    Console.Write(""[ "" + DateTime.UtcNow.ToString() + "" UTC ]"");
                    Console.WriteLine("": +"");
                }

            } while (conditionsMet > 0); 
            {{TRIGGER}}";
    }
}
