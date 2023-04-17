using SingleDose.Invokes;

namespace PoisonTendy.Invokes.KtmW32
{
    internal class CreateTransaction : IInvoke
    {
        string IInvoke.Name => "CreateTransaction";

        string IInvoke.PInvoke => @"[DllImport(""KtmW32.dll"")]
        static extern IntPtr CreateTransaction(IntPtr lpTransactionAttributes, IntPtr UOW, uint CreateOptions,
                uint IsolationLevel, uint IsolationFlags, uint Timeout, string Description);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr CreateTransaction(IntPtr lpTransactionAttributes, IntPtr UOW, uint CreateOptions,
                uint IsolationLevel, uint IsolationFlags, uint Timeout, string Description)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(uint), typeof(uint), typeof(uint), typeof(uint), typeof(string)};
            Object[] args = { lpTransactionAttributes, UOW, CreateOptions, IsolationLevel, IsolationFlags, Timeout, Description };
            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""KtmW32.dll"", ""CreateTransaction"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
