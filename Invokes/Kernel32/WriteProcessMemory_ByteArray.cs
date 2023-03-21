namespace SingleDose.Invokes.Kernel32
{
    internal class WriteProcessMemory_ByteArray : IInvoke
    {
        string IInvoke.Name => "WriteProcessMemory_ByteArray";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern bool WriteProcessMemory(
            IntPtr hProcess, IntPtr lpBaseAddress,
            byte[] lpBuffer, uint nSize,
            out IntPtr lpNumberOfBytesWritten);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten)
        {
            lpNumberOfBytesWritten = IntPtr.Zero;
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(byte[]), typeof(uint), Type.GetType(typeof(IntPtr) + ""&"") };
            Object[] args = { hProcess, lpBaseAddress, lpBuffer, nSize, lpNumberOfBytesWritten };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""WriteProcessMemory"", ref args, paramTypes);
            lpNumberOfBytesWritten = (IntPtr)args[4];

            return (bool)res;
        }

        {{INVOKE}}";
    }
}
