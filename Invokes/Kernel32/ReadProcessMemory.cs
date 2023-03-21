namespace SingleDose.Invokes.Kernel32
{
    internal class ReadProcessMemory : IInvoke
    {
        string IInvoke.Name => "ReadProcessMemory";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess, IntPtr lpBaseAddress,
            byte[] lpBuffer, Int32 nSize,
            out IntPtr lpNumberOfBytesRead);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesRead)
        {
            lpNumberOfBytesRead = IntPtr.Zero;
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(byte[]), typeof(Int32), Type.GetType(typeof(IntPtr) + ""&"") };
            Object[] args = { hProcess, lpBaseAddress, lpBuffer, nSize, lpNumberOfBytesRead };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""ReadProcessMemory"", ref args, paramTypes);
            lpNumberOfBytesRead = (IntPtr)args[4];
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
