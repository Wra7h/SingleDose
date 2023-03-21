namespace SingleDose.Invokes.Ntdll
{
    internal class NtMapViewOfSection : IInvoke
    {
        string IInvoke.Name => "NtMapViewOfSection";

        string IInvoke.PInvoke => @"[DllImport(""ntdll.dll"", SetLastError = true)]
        static extern uint NtMapViewOfSection(
            IntPtr SectionHandle, IntPtr ProcessHandle,
            ref IntPtr BaseAddress, UIntPtr ZeroBits,
            UIntPtr CommitSize, out ulong SectionOffset,
            out uint ViewSize, uint InheritDisposition,
            uint AllocationType, uint Win32Protect);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static uint NtMapViewOfSection(IntPtr SectionHandle, IntPtr ProcessHandle, ref IntPtr BaseAddress, UIntPtr ZeroBits,
            UIntPtr CommitSize, out ulong SectionOffset, out uint ViewSize, uint InheritDisposition, uint AllocationType, uint Win32Protect)
        {
            SectionOffset = 0x0;
            ViewSize = 0x0;
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), Type.GetType(typeof(IntPtr) + ""&""), typeof(UIntPtr), typeof(UIntPtr), Type.GetType(typeof(ulong) + ""&""), Type.GetType(typeof(uint) + ""&""), typeof(uint), typeof(uint), typeof(uint) };
            Object[] args = { SectionHandle, ProcessHandle, BaseAddress, ZeroBits, CommitSize, SectionOffset, ViewSize, InheritDisposition, AllocationType, Win32Protect };

            object res = DynamicPInvokeBuilder(typeof(uint), ""ntdll.dll"", ""NtMapViewOfSection"", ref args, paramTypes);

            BaseAddress = (IntPtr)args[2];
            SectionOffset = (ulong)args[5];
            ViewSize = (uint)args[6];

            return (uint)res;
        }

        {{INVOKE}}";
    }
}
