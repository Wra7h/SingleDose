using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class BeginUpdateResource : IInvoke
    {
        string IInvoke.Name => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern IntPtr BeginUpdateResource(string pFileName, bool bDeleteExistingResources);

        {{PINVOKE}}";

        string IInvoke.PInvoke => throw new NotImplementedException();

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
