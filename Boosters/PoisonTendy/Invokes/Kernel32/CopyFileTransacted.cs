using SingleDose.Invokes;

namespace PoisonTendy.Invokes.Kernel32
{
    internal class CopyFileTransacted : IInvoke
    {
        string IInvoke.Name => "CopyFileTransacted";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern bool CopyFileTransacted(string lpExistingFileName, string lpNewFileName,
            IntPtr lpProgressRoutine, IntPtr lpData, Int32 pbCancel,
            uint dwCopyFlags, IntPtr hTransaction);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool CopyFileTransacted(string lpExistingFileName, string lpNewFileName,
            IntPtr lpProgressRoutine, IntPtr lpData, Int32 pbCancel, uint dwCopyFlags, IntPtr hTransaction)
        {
            Type[] paramTypes = { typeof(string), typeof(string), typeof(IntPtr), typeof(IntPtr), typeof(Int32), typeof(uint), typeof(IntPtr) };
            Object[] args = { lpExistingFileName, lpNewFileName, lpProgressRoutine, lpData, pbCancel, dwCopyFlags, hTransaction };
            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""CopyFileTransacted"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
