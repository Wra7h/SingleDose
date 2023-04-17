using System.Collections.Generic;

namespace SingleDose.Triggers
{
    internal class ProcScan : ITrigger
    {
        string ITrigger.TriggerName => "ProcScan";

        string ITrigger.TriggerDescription => "Check running process names and ids once. If your specified criteria is found, the inject exits.";

        string ITrigger.Base => @"
            int match = 0;
            Process[] processes = System.Diagnostics.Process.GetProcesses();
            foreach (string item in (""{{DETAILS}}"").Split(','))
            {
                if (Array.Exists(processes, proc => proc.Id.ToString() == item.Trim() || item.ToUpper().Trim().StartsWith(proc.ProcessName.ToUpper())))
                {
                    match++;
                    continue;
                }
            }

            if (match > 0)
            {
                Console.WriteLine(""Exiting..."");
                System.Environment.Exit(0);
            }
";

        List<string> ITrigger.ReqQuestions => new List<string>()
        {
            "What PID or process name to look for? [Supports CSV]" //{{DETAILS}}
        };

        List<string> ITrigger.ReqPatterns => new List<string>()
        {
            "{{DETAILS}}"
        };
    }
}
