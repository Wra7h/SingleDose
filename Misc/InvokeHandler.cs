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
            Regex regPattern = new Regex("{{PINVOKE}}");

            List<string> PInvokes = technique.PInvokeRecipe;

            if (SettingsMenu.szMemAlloc == "RW/RX" && !technique.PInvokeRecipe.Contains("VirtualProtectEx"))
            {
                PInvokes.Add("VirtualProtectEx");
            }

            foreach (string szRecipeItem in PInvokes)
            {
                IInvoke invoke = Reflect.InvokesFound.First(x => x.Name.ToLower() == szRecipeItem.ToLower());
                CSContents = regPattern.Replace(CSContents, invoke.PInvoke);
            }

            //Clear the last remaining "{{PINVOKE}}"
            CSContents = regPattern.Replace(CSContents, "");
            return CSContents;
        }
    }
}
