using SingleDose.Invokes;

namespace PoisonTendy.Invokes.Imagehlp
{
    internal class BindImageEx : IInvoke
    {
        string IInvoke.Name => "BindImageEx";

        string IInvoke.PInvoke => @"[DllImport(""imagehlp.dll"", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool BindImageEx(
            uint dwFlags, IntPtr lpFileName,
            IntPtr lpDllPath, IntPtr lpReserved, IntPtr lpfnStatusCallback);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool BindImageEx(uint dwFlags, IntPtr lpFileName,
        IntPtr lpDllPath, IntPtr lpReserved, IntPtr lpfnStatusCallback)
        {
            Type[] paramTypes = { typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { dwFlags, lpFileName, lpDllPath, lpReserved, lpfnStatusCallback };
            object res = DynamicPInvokeBuilder(typeof(bool), ""imagehlp.dll"", ""BindImageEx"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
