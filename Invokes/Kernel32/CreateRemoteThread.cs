namespace SingleDose.Invokes.Kernel32
{
    internal class CreateRemoteThread : IInvoke
    {
        string IInvoke.Name => "CreateRemoteThread";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateRemoteThread(
            IntPtr hProcess, IntPtr lpThreadAttributes,
            uint dwStackSize, IntPtr lpStartAddress,
            IntPtr lpParameter, uint dwCreationFlags,
            IntPtr lpThreadId);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static IntPtr CreateRemoteThread(
            IntPtr hProcess, IntPtr lpThreadAttributes,
            uint dwStackSize, IntPtr lpStartAddress,
            IntPtr lpParameter, uint dwCreationFlags,
            IntPtr lpThreadId)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(uint), typeof(IntPtr) };
            Object[] args = { hProcess, lpThreadAttributes, dwStackSize, lpStartAddress, lpParameter, dwCreationFlags, lpThreadId };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""CreateRemoteThread"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
