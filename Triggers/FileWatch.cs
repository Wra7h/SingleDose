using System.Collections.Generic;

namespace SingleDose.Triggers
{
    internal class FileWatch : ITrigger
    {
        string ITrigger.TriggerName => "FileWatch";

        string ITrigger.TriggerDescription => "Avoids running the inject while a specified file exists on disk. The process will sleep before rechecking.";

        string ITrigger.Base => @"
            while (File.Exists(@""{{FILEPATH}}""))
            {
                Random sleepTime = new Random();
                int sleep = sleepTime.Next(90000, 300000);
                Console.WriteLine(""[{0} UTC]: Next check in {1} sec"", DateTime.UtcNow.ToString(), TimeSpan.FromMilliseconds(sleep).TotalSeconds.ToString());
                System.Threading.Thread.Sleep(sleep);
            }
";

        List<string> ITrigger.ReqQuestions => new List<string>()
        {
            "Enter a filepath to watch:" //{{FILEPATH}}
        };

        List<string> ITrigger.ReqPatterns => new List<string>()
        {
            "{{FILEPATH}}"
        };
    }
}
