using System.Collections.Generic;

namespace SingleDose.Triggers
{
    internal class Timer : ITrigger
    {
        string ITrigger.TriggerName => "Timer";

        string ITrigger.TriggerDescription => "Executes a countdown timer before continuing injection.";

        string ITrigger.Base => @"
            System.Threading.Thread.Sleep((int)({{TIME}}*1000));";

        List<string> ITrigger.ReqQuestions => new List<string>()
        {
            "Enter amount of time in seconds to sleep:" //{{TIME}}
        };

        List<string> ITrigger.ReqPatterns => new List<string>()
        {
            "{{TIME}}"
        };
    }
}
