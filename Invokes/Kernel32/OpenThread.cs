namespace SingleDose.Invokes.Kernel32
{
    internal class OpenThread : IInvoke
    {
        string IInvoke.Name => "OpenThread";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, int dwThreadId);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, int dwThreadId)
        {
            Type[] paramTypes = { typeof(int), typeof(bool), typeof(int) };
            Object[] args = { dwDesiredAccess, bInheritHandle, dwThreadId };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""OpenThread"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
