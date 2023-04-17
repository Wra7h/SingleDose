using SingleDose.Invokes;

namespace PoisonTendy.Invokes.Wscapi
{
    internal class WscRegisterForChanges : IInvoke
    {
        string IInvoke.Name => "WscRegisterForChanges";

        string IInvoke.PInvoke => @"[DllImport(""wscapi.dll"", CharSet = CharSet.Unicode)]
        public static extern int WscRegisterForChanges(
            IntPtr hWnd, ref IntPtr callback, IntPtr context,
            IntPtr waitHandle);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static int WscRegisterForChanges(
            IntPtr hWnd, ref IntPtr callback, IntPtr context, IntPtr waitHandle)
        {
            Type[] paramTypes = { typeof(IntPtr), Type.GetType(typeof(IntPtr) + ""&""), typeof(IntPtr), typeof(IntPtr)};
            Object[] args = { hWnd, callback, context, waitHandle};
            object res = DynamicPInvokeBuilder(typeof(int), ""wscapi.dll"", ""WscRegisterForChanges"", ref args, paramTypes);
            callback = (IntPtr)args[1];
            return (int)res;
        }

        {{INVOKE}}";
    }
}
