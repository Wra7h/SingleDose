using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleDose.Invokes.Comdlg32
{
    internal class ChooseColor : IInvoke
    {
        string IInvoke.Name => "ChooseColor";

        string IInvoke.PInvoke => @"[DllImport(""comdlg32.dll"", SetLastError = true, CharSet = CharSet.Auto)]
        public extern static bool ChooseColor(ref CHOOSECOLOR lpcc);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
