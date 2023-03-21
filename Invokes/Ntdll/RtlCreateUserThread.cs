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

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static IntPtr RtlCreateUserThread(IntPtr processHandle, IntPtr threadSecurity, bool createSuspended, Int32 stackZeroBits,
            IntPtr stackReserved, IntPtr stackCommit, IntPtr startAddress, IntPtr parameter, ref IntPtr threadHandle, IntPtr clientId)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(bool), typeof(Int32), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), Type.GetType(typeof(IntPtr) + ""&""), typeof(IntPtr) };
            Object[] args = { processHandle, threadSecurity, createSuspended, stackZeroBits, stackReserved, stackCommit, startAddress, parameter, threadHandle, clientId };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""ntdll.dll"", ""RtlCreateUserThread"", ref args, paramTypes);
            threadHandle = (IntPtr)args[8];
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
