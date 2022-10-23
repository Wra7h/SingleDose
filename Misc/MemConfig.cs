using SingleDose.Menus;
using SingleDose.Techniques;
using System;
using System.Text.RegularExpressions;

namespace SingleDose.Misc
{
    public class MemConfig
    {
        public static string SetMem(string sContents)
        {
            Regex rpProtect = new Regex("{{flProtect}}");
            string sRWX = "0x40";
            string sRW = "0x04";

            switch (SettingsMenu.szMemAlloc)
            {
                case "RWX":
                    sContents = rpProtect.Replace(sContents, sRWX);
                    SDLog.AddEntry(String.Format("Memory allocation set to RWX"));
                    break;
                case "RW/RX":
                    sContents = rpProtect.Replace(sContents, sRW);
                    SDLog.AddEntry(String.Format("Memory allocation set to RW"));
                    break;
                default:
                    break;
            }

            return sContents;
        }

        public static string SetProtect(ITechnique technique, string szContents)
        {
            Regex rpMemProtect = new Regex("{{PROTECT}}");

            switch (SettingsMenu.szMemAlloc)
            {
                case "RWX":
                    szContents = rpMemProtect.Replace(szContents, "");
                    break;
                case "RW/RX":
                    if (technique.VProtect == null)
                        goto case "RWX";
                    szContents = rpMemProtect.Replace(szContents, technique.VProtect);
                    SDLog.AddEntry(String.Format("Memory protection set to RX"));
                    break;
                default:
                    break;
            }

            return szContents;
        }
    }
}
