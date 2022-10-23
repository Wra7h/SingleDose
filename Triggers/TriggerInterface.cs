using System.Collections.Generic;

namespace SingleDose.Triggers
{
    public interface ITrigger
    {
        // The name of the trigger
        string TriggerName { get; }

        // Description for the technique. How does it execute? What API triggers the execution?
        // Any other information such as "this will only execute when the OS does this", would be useful.
        string TriggerDescription { get; }

        // The base template. This is the body of the trigger code, with the various REGEX markers in correct places. i.e. {{NAMESPACE}}
        string Base { get; }

        List<string> ReqQuestions { get; }

        List<string> ReqPatterns { get; }
    }
}
