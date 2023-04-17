using SingleDose.Invokes;

namespace PoisonTendy.Invokes.Msacm32
{
    internal class acmDriverEnum : IInvoke
    {
        string IInvoke.Name => "AcmDriverEnum";

        string IInvoke.PInvoke => @"[DllImport(""Msacm32.dll"")]
        static extern IntPtr acmDriverEnum(IntPtr fnCallback, uint dwInstance, uint fdwEnum);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr acmDriverEnum(IntPtr fnCallback, uint dwInstance, uint fdwEnum)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(uint) , typeof(uint)};
            Object[] args = { fnCallback, dwInstance, fdwEnum };
            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Msacm32.dll"", ""acmDriverEnum"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
