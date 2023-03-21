namespace SingleDose.Invokes.Kernel32
{
    internal class FlsAlloc : IInvoke
    {
        string IInvoke.Name => "FlsAlloc";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern uint FlsAlloc(IntPtr callback);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static uint FlsAlloc(IntPtr callback)
        {
            Type[] paramTypes = { typeof(IntPtr) };
            Object[] args = { callback };

            object res = DynamicPInvokeBuilder(typeof(uint), ""Kernel32.dll"", ""FlsAlloc"", ref args, paramTypes);
            return (uint)res;
        }

        {{INVOKE}}";
    }
}
