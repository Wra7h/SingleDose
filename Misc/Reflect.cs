using SingleDose.Invokes;
using SingleDose.Techniques;
using SingleDose.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SingleDose.Misc
{
    internal class Reflect
    {
        public static List<ITechnique> TechniquesFound = new List<ITechnique>();
        public static List<ITrigger> TriggersFound = new List<ITrigger>();
        public static List<IInvoke> InvokesFound = new List<IInvoke>();

        public static void InitializeTriggers()
        {
            //https://stackoverflow.com/questions/10732933/can-i-use-activator-createinstance-with-an-interface
            Type[] TriggerInterfaces = (from t in Assembly.GetExecutingAssembly().GetTypes()
                                 where !t.IsInterface && !t.IsAbstract
                                 where typeof(ITrigger).IsAssignableFrom(t)
                                 select t).ToArray();

            TriggersFound.AddRange(TriggerInterfaces.Select(t => (ITrigger)Activator.CreateInstance(t)).ToArray());

            TriggersFound = TriggersFound.OrderBy(c => c.TriggerName).ToList();

            return;
        }

        public static void InitializeTechniques()
        {
            //https://stackoverflow.com/questions/10732933/can-i-use-activator-createinstance-with-an-interface
            Type[] TechniqueInterfaces = (from t in Assembly.GetExecutingAssembly().GetTypes()
                                        where !t.IsInterface && !t.IsAbstract
                                        where typeof(ITechnique).IsAssignableFrom(t)
                                        select t).ToArray();

            TechniquesFound.AddRange(TechniqueInterfaces.Select(t => (ITechnique)Activator.CreateInstance(t)).ToArray());

            TechniquesFound = TechniquesFound.OrderBy(c => c.TechniqueName).ToList();

            return;
        }

        public static void InitializeInvokes()
        {
            //https://stackoverflow.com/questions/10732933/can-i-use-activator-createinstance-with-an-interface
            Type[] InvokeInterfaces = (from t in Assembly.GetExecutingAssembly().GetTypes()
                                        where !t.IsInterface && !t.IsAbstract
                                        where typeof(IInvoke).IsAssignableFrom(t)
                                        select t).ToArray();

            InvokesFound.AddRange(InvokeInterfaces.Select(t => (IInvoke)Activator.CreateInstance(t)).ToArray());

            InvokesFound = InvokesFound.OrderBy(c => c.Name).ToList();

            return;
        }
    }
}
