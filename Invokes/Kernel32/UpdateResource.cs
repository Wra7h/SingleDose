using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class UpdateResource : IInvoke
    {
        string IInvoke.Name => "UpdateResource";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern bool UpdateResource(
            IntPtr hUpdate, IntPtr lpType,
            IntPtr lpName, ushort wLanguage,
             byte[] lpData, uint cb);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
