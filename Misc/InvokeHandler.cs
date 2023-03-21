using SingleDose.Invokes;
using SingleDose.Menus;
using SingleDose.Techniques;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SingleDose.Misc
{
    internal class InvokeHandler
    {
        public static string AddInvokes(ITechnique technique, string CSContents)
        {
            Regex regPattern = new Regex("{{INVOKE}}");

            List<string> PInvokes = technique.Invokes;

            if (SettingsMenu.szMemAlloc == "RW/RX" && !technique.Invokes.Contains("VirtualProtectEx"))
            {
                PInvokes.Add("VirtualProtectEx");
            }

            if (SettingsMenu.szInvokeMethod.ToUpper() == "DINVOKE")
            {
                string DInvokeMethod = DInvoke.DynamicPInvokeBuilder;
                regPattern = new Regex("{{ASSEMBLYNAME}}");
                DInvokeMethod = regPattern.Replace(DInvokeMethod, Build.GenRandomString());
                regPattern = new Regex("{{DYNAMICMODULE}}");
                DInvokeMethod = regPattern.Replace(DInvokeMethod, Build.GenRandomString());

                regPattern = new Regex("{{INVOKE}}");
                CSContents = regPattern.Replace(CSContents, DInvokeMethod);
            }

            foreach (string szRecipeItem in PInvokes)
            {
                IInvoke invoke = Reflect.InvokesFound.First(x => x.Name.ToLower() == szRecipeItem.ToLower());

                if (invoke != null)
                {
                    switch (SettingsMenu.szInvokeMethod.ToUpper())
                    {
                        case "PINVOKE":
                            CSContents = regPattern.Replace(CSContents, invoke.PInvoke);
                            break;
                        case "DINVOKE":
                            CSContents = regPattern.Replace(CSContents, invoke.DInvoke);
                            break;
                        default:
                            break;
                    }
                }
            }

            //Clear the last remaining "{{INVOKE}}"
            CSContents = regPattern.Replace(CSContents, "");
            return CSContents;
        }
    }
}
