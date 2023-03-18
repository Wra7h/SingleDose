using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class GetModuleHandle : IInvoke
    {
        string IInvoke.Name => "GetModuleHandle";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
