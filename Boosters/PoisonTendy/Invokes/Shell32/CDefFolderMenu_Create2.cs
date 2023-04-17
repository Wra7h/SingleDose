using SingleDose.Invokes;

namespace PoisonTendy.Invokes.Shell32
{
    internal class CDefFolderMenu_Create2 : IInvoke
    {
        string IInvoke.Name => "CDefFolderMenu_Create2";

        string IInvoke.PInvoke => @"[DllImport(""Shell32.dll"")]
        static extern IntPtr CDefFolderMenu_Create2(
            IntPtr pidlFolder, IntPtr hwnd, uint cidl,
            IntPtr apidl, IntPtr psf, IntPtr pfn,
            uint nKeys, IntPtr ahkeys, out IntPtr ppcm);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr CDefFolderMenu_Create2(
            IntPtr pidlFolder, IntPtr hwnd, uint cidl,
            IntPtr apidl, IntPtr psf, IntPtr pfn,
            uint nKeys, IntPtr ahkeys, out IntPtr ppcm)
        {
            ppcm = IntPtr.Zero;
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(uint), typeof(IntPtr), Type.GetType(typeof(IntPtr) + ""&"")};
            Object[] args = { pidlFolder, hwnd, cidl, apidl, psf, pfn, nKeys, ahkeys, ppcm};
            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Shell32.dll"", ""CDefFolderMenu_Create2"", ref args, paramTypes);
            ppcm = (IntPtr)args[8];
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
