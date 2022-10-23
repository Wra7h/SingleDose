using System.Collections.Generic;

namespace SingleDose.Triggers
{
    internal class ProcWatch: ITrigger
    {
        string ITrigger.TriggerName => "ProcWatch";

        string ITrigger.TriggerDescription => "Continual scanning for specified process names or pids. The trigger will sleep before rechecking.";

        string ITrigger.Base => @"
            START:
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
                Random sleepTime = new Random();
                int sleep = sleepTime.Next(90000, 300000);
                Console.WriteLine(""[{0} UTC]: Next check in {1} sec"", DateTime.UtcNow.ToString(), TimeSpan.FromMilliseconds(sleep).TotalSeconds.ToString());
                System.Threading.Thread.Sleep(sleep);
                goto START;
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
