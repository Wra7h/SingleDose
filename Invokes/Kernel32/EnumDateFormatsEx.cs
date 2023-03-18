using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class EnumDateFormatsEx : IInvoke
    {
        string IInvoke.Name => "EnumDateFormatsEx";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern bool EnumDateFormatsEx(
            IntPtr lpDateFmtEnumProcEx, uint Locale, uint dwFlags);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
