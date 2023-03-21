namespace SingleDose.Invokes.Kernel32
{
    internal class WaitForSingleObject : IInvoke
    {
        string IInvoke.Name => "WaitForSingleObject";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern UInt32 WaitForSingleObject( IntPtr hHandle, UInt32 dwMilliseconds);

        {{INVOKE}}";
        string IInvoke.DInvoke => @"public static UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(UInt32) };
            Object[] args = { hHandle, dwMilliseconds };

            object res = DynamicPInvokeBuilder(typeof(UInt32), ""Kernel32.dll"", ""WaitForSingleObject"", ref args, paramTypes);
            return (UInt32)res;
        }

        {{INVOKE}}";
    }
}
