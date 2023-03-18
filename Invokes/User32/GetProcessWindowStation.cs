using System;

namespace SingleDose.Invokes.User32
{
    internal class GetProcessWindowStation : IInvoke
    {
        string IInvoke.Name => "GetProcessWindowStation";

        string IInvoke.PInvoke => @"[DllImport(""User32.dll"", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetProcessWindowStation();

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
