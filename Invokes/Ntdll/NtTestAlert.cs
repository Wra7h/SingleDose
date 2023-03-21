namespace SingleDose.Invokes.Ntdll
{
    internal class NtTestAlert : IInvoke
    {
        string IInvoke.Name => "NtTestAlert";

        string IInvoke.PInvoke => @"[DllImport(""ntdll.dll"", SetLastError = true)]
        public static extern uint NtTestAlert();

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static uint NtTestAlert()
        {
            Type[] paramTypes = { };
            Object[] args = { };

            object res = DynamicPInvokeBuilder(typeof(uint), ""ntdll.dll"", ""NtTestAlert"", ref args, paramTypes);
            return (uint)res;
        }

        {{INVOKE}}";
    }
}
