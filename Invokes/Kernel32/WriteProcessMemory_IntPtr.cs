namespace SingleDose.Invokes.Kernel32
{
    internal class WriteProcessMemory_IntPtr : IInvoke
    {
        string IInvoke.Name => "WriteProcessMemory_IntPtr";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern bool WriteProcessMemory(
             IntPtr hProcess, IntPtr lpBaseAddress,
             IntPtr lpBuffer, Int32 nSize,
             out IntPtr lpNumberOfBytesWritten);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten)
        {
            lpNumberOfBytesWritten = IntPtr.Zero;
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(Int32), Type.GetType(typeof(IntPtr) + ""&"") };
            Object[] args = { hProcess, lpBaseAddress, lpBuffer, nSize, lpNumberOfBytesWritten };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""WriteProcessMemory"", ref args, paramTypes);
            lpNumberOfBytesWritten = (IntPtr)args[4];

            return (bool)res;
        }

        {{INVOKE}}";
    }
}
