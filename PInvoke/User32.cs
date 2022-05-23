namespace SingleDose.PInvoke
{
    internal class User32
    {
        public static string EnumChildWindows = @"[DllImport(""User32.dll"")]
        public static extern bool EnumChildWindows(
            IntPtr hWndParent,
            IntPtr lpEnumFunc,
            IntPtr lParam);
        
        {{PINVOKE}}";

        public static string EnumDesktops = @"[DllImport(""User32.dll"")]
        static extern bool EnumDesktops(
            IntPtr hwinsta,
            IntPtr lpEnumFunc,
            IntPtr lParam);

        {{PINVOKE}}";

        public static string EnumWindows = @"[DllImport(""User32.dll"")]
        static extern bool EnumWindows(
            IntPtr lpEnumFunc,
            IntPtr lParam);

        {{PINVOKE}}";

        public static string GetProcessWindowStation = @"[DllImport(""User32.dll"", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr GetProcessWindowStation();

        {{PINVOKE}}";

        public static string SendMessage = @"[DllImport(""user32.dll"")]
        static extern int SendMessage(
            IntPtr hWnd,
            uint Msg,
            IntPtr wParam,
            ref COPYDATASTRUCT lParam);

        {{PINVOKE}}";
    }
}
