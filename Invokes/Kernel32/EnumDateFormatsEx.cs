namespace SingleDose.Invokes.Kernel32
{
    internal class EnumDateFormatsEx : IInvoke
    {
        string IInvoke.Name => "EnumDateFormatsEx";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern bool EnumDateFormatsEx( IntPtr lpDateFmtEnumProcEx, uint Locale, uint dwFlags);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool EnumDateFormatsEx(IntPtr lpDateFmtEnumProcEx, uint Locale, uint dwFlags)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(uint), typeof(uint) };
            Object[] args = { lpDateFmtEnumProcEx, Locale, dwFlags };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""EnumDateFormatsEx"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
