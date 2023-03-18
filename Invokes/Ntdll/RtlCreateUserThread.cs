using System;

namespace SingleDose.Invokes.Ntdll
{
    internal class RtlCreateUserThread : IInvoke
    {
        string IInvoke.Name => "RtlCreateUserThread";

        string IInvoke.PInvoke => @"[DllImport(""ntdll.dll"", SetLastError = true)]
        static extern IntPtr RtlCreateUserThread(
            IntPtr processHandle, IntPtr threadSecurity,
            bool createSuspended, Int32 stackZeroBits,
            IntPtr stackReserved, IntPtr stackCommit,
            IntPtr startAddress, IntPtr parameter,
            ref IntPtr threadHandle, IntPtr clientId);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
