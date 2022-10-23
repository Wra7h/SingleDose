using System.Collections.Generic;

namespace SingleDose.Triggers
{
    internal class FileScan : ITrigger
    {
        string ITrigger.TriggerName => "FileScan";

        string ITrigger.TriggerDescription => "Checks for a file on disk. If the file exists, the process exits.";

        string ITrigger.Base => @"
            if (File.Exists(@""{{FILEPATH}}""))
            {
                Console.WriteLine(""File exists. Exiting..."");
                System.Environment.Exit(1);
            }";

        List<string> ITrigger.ReqQuestions => new List<string>()
        {
            "Enter a filepath:" //{{FILEPATH}}
        };

        List<string> ITrigger.ReqPatterns => new List<string>()
        {
            "{{FILEPATH}}"
        };
    }
}
