using SingleDose.Menus;
using System;
using System.Collections.Generic;
using System.IO;

namespace SingleDose.Misc
{
    internal class SDLog
    {
        public static List<string> LogEntryHold = new List<string>();
        public static void AddEntry(string message)
        {
            string LogFile = null;

            if (!SettingsMenu.UseLogging)
            {
                return;
            }

            if (SettingsMenu.OutputDirectory == null)
            { 
                LogEntryHold.Add(String.Format("{0}: {1}", DateTime.UtcNow, message));
                return;
            }

            LogFile = Path.GetFullPath(SettingsMenu.OutputDirectory) + "\\SingleDose[" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() + "].log";

            if (!File.Exists(LogFile))
            {
                File.Create(LogFile).Dispose();
            }

            if (LogEntryHold.Count != 0)
            {
                try
                {
                    foreach (string entry in LogEntryHold)
                    {
                        File.WriteAllText(LogFile, entry);
                        LogEntryHold.Remove(entry);
                    }
                }
                catch { }
            }

            File.AppendAllText(LogFile, String.Format("{0}: {1}\n", DateTime.UtcNow, message));

        }
    }
}
