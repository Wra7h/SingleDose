using System;

namespace SingleDose.Invokes.Setupapi
{
    internal class SetupOpenFileQueue : IInvoke
    {
        string IInvoke.Name => "SetupOpenFileQueue";

        string IInvoke.PInvoke => @"[DllImport(""Setupapi.dll"")]
        static extern IntPtr SetupOpenFileQueue();

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
