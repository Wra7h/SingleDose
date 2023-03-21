namespace SingleDose.Invokes.User32
{
    internal class GetProcessWindowStation : IInvoke
    {
        string IInvoke.Name => "GetProcessWindowStation";

        string IInvoke.PInvoke => @"[DllImport(""User32.dll"", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetProcessWindowStation();

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr GetProcessWindowStation()
        {
            Type[] paramTypes = { };
            Object[] args = { };
            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""User32.dll"", ""GetProcessWindowStation"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
