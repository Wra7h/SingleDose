namespace SingleDose.Invokes.Kernel32
{
    internal class SuspendThread : IInvoke
    {
        string IInvoke.Name => "SuspendThread";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern uint SuspendThread(IntPtr hThread);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static uint SuspendThread(IntPtr hThread)
        {
            Type[] paramTypes = { typeof(IntPtr) };
            Object[] args = { hThread };

            object res = DynamicPInvokeBuilder(typeof(uint), ""Kernel32.dll"", ""SuspendThread"", ref args, paramTypes);
            return (uint)res;
        }

        {{INVOKE}}";
    }
}
