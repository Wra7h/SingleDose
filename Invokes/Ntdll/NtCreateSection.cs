namespace SingleDose.Invokes.Ntdll
{
    internal class NtCreateSection : IInvoke
    {
        string IInvoke.Name => "NtCreateSection";
        string IInvoke.PInvoke => @"[DllImport(""ntdll.dll"", SetLastError = true, ExactSpelling = true)]
        static extern uint NtCreateSection(
            ref IntPtr SectionHandle, uint DesiredAccess, IntPtr ObjectAttributes,
            ref uint MaximumSize, uint SectionPageProtection, uint AllocationAttributes,
            IntPtr FileHandle);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static uint NtCreateSection(ref IntPtr SectionHandle, uint DesiredAccess, IntPtr ObjectAttributes,
            ref uint MaximumSize, uint SectionPageProtection, uint AllocationAttributes, IntPtr FileHandle)
        {
            Type[] paramTypes = { Type.GetType(typeof(IntPtr) + ""&""), typeof(uint), typeof(IntPtr), Type.GetType(typeof(uint) + ""&""), typeof(uint), typeof(uint), typeof(IntPtr) };
            Object[] args = { SectionHandle, DesiredAccess, ObjectAttributes, MaximumSize, SectionPageProtection, AllocationAttributes, FileHandle };

            object res = DynamicPInvokeBuilder(typeof(uint), ""ntdll.dll"", ""NtCreateSection"", ref args, paramTypes);
            SectionHandle = (IntPtr)args[0];
            return (uint)res;
        }

        {{INVOKE}}";
    }
}
