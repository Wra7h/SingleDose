using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleDose.Invokes.Comdlg32
{
    internal class GetOpenFileName : IInvoke
    {
        string IInvoke.Name => "GetOpenFileName";

        string IInvoke.PInvoke => @"[DllImport(""comdlg32.dll"", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();

    }
}
