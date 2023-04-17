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

        public static void InitializeInterfaces()
        {
            //https://stackoverflow.com/questions/10732933/can-i-use-activator-createinstance-with-an-interface
            Type[] Interfaces = (from t in Assembly.GetExecutingAssembly().GetTypes()
                                 where !t.IsInterface && !t.IsAbstract
                                 where typeof(ITrigger).IsAssignableFrom(t) || typeof(ITechnique).IsAssignableFrom(t) || typeof(IInvoke).IsAssignableFrom(t)
                                 select t).ToArray();

            TechniquesFound.AddRange(Interfaces.Where(t => typeof(ITechnique).IsAssignableFrom(t)).Select(t => (ITechnique)Activator.CreateInstance(t)).Distinct().ToArray());
            InvokesFound.AddRange(Interfaces.Where(t => typeof(IInvoke).IsAssignableFrom(t)).Select(t => (IInvoke)Activator.CreateInstance(t)).Distinct().ToArray());
            TriggersFound.AddRange(Interfaces.Where(t => typeof(ITrigger).IsAssignableFrom(t)).Select(t => (ITrigger)Activator.CreateInstance(t)).Distinct().ToArray());

            TriggersFound = TriggersFound.OrderBy(c => c.TriggerName).ToList();
            TechniquesFound = TechniquesFound.OrderBy(c => c.TechniqueName).ToList();
            InvokesFound = InvokesFound.OrderBy(c => c.Name).ToList();

            return;
        }

        public static bool LoadBoosterFromPath(string szPath)
        {
            try
            {


                if (String.IsNullOrEmpty(szPath))
                    return false;

                Assembly assembly = Assembly.LoadFrom(szPath);

                Type[] BoosterInterfaces = (from t in assembly.GetTypes()
                                            where !t.IsInterface && !t.IsAbstract
                                            where typeof(ITechnique).IsAssignableFrom(t) || typeof(ITrigger).IsAssignableFrom(t) || typeof(IInvoke).IsAssignableFrom(t)
                                            select t).ToArray();

                if (BoosterInterfaces.Length == 0)
                    return false;

                TechniquesFound.AddRange(BoosterInterfaces.Where(t => typeof(ITechnique).IsAssignableFrom(t)).Select(t => (ITechnique)Activator.CreateInstance(t)).Distinct().ToArray());
                InvokesFound.AddRange(BoosterInterfaces.Where(t => typeof(IInvoke).IsAssignableFrom(t)).Select(t => (IInvoke)Activator.CreateInstance(t)).Distinct().ToArray());
                TriggersFound.AddRange(BoosterInterfaces.Where(t => typeof(ITrigger).IsAssignableFrom(t)).Select(t => (ITrigger)Activator.CreateInstance(t)).Distinct().ToArray());

                TechniquesFound = TechniquesFound.GroupBy(x => x.TechniqueName).Select(y => y.First()).ToList();
                InvokesFound = InvokesFound.GroupBy(x => x.Name).Select(y => y.First()).ToList();
                TriggersFound = TriggersFound.GroupBy(x => x.TriggerName).Select(y => y.First()).ToList();

                return true;
            }
            catch (Exception ex)
            {
                SDConsole.WriteError(String.Format("Exception caught: \n         {0}", SDConsole.SpliceText(ex.Message, 45)));
                return false;
            }
        }
    }
}
