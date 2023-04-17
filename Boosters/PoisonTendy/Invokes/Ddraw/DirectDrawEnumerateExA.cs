using SingleDose.Invokes;

namespace PoisonTendy.Invokes.Ddraw
{
    internal class DirectDrawEnumerateExA : IInvoke
    {
        string IInvoke.Name => "DirectDrawEnumerateExA";

        string IInvoke.PInvoke => @"[DllImport(""Ddraw.dll"")]
        public extern static IntPtr DirectDrawEnumerateExA(
            IntPtr lpCallback, IntPtr lpContext, uint dwFlags);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr DirectDrawEnumerateExA(IntPtr lpCallback, IntPtr lpContext, uint dwFlags)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(uint) };
            Object[] args = { lpCallback, lpContext, dwFlags };
            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Ddraw.dll"", ""DirectDrawEnumerateExA"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
