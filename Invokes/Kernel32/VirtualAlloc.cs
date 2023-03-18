using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class VirtualAlloc : IInvoke
    {
        string IInvoke.Name => "VirtualAlloc";

        string IInvoke.PInvoke => @"[DllImport(""kernel32"")]
        public static extern IntPtr VirtualAlloc(
            IntPtr lpAddress, uint dwSize, 
            uint flAllocationType, uint flProtect);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
