namespace SingleDose.Invokes.Kernel32
{
    internal class ResumeThread : IInvoke
    {
        string IInvoke.Name => "ResumeThread";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern uint ResumeThread(IntPtr hThread);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static uint ResumeThread(IntPtr hThread)
        {
            Type[] paramTypes = { typeof(IntPtr) };
            Object[] args = { hThread };

            object res = DynamicPInvokeBuilder(typeof(uint), ""Kernel32.dll"", ""ResumeThread"", ref args, paramTypes);
            return (uint)res;
        }

        {{INVOKE}}";
    }
}
