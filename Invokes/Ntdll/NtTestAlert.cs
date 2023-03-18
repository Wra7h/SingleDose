using System;

namespace SingleDose.Invokes.Ntdll
{
    internal class NtTestAlert : IInvoke
    {
        string IInvoke.Name => "NtTestAlert";

        string IInvoke.PInvoke => @"[DllImport(""ntdll.dll"", SetLastError = true)]
        public static extern uint NtTestAlert();

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
