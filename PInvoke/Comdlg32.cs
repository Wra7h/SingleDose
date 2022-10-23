namespace SingleDose.PInvoke
{
    internal class Comdlg32
    {
        public static string GetOpenFileName = @"[DllImport(""comdlg32.dll"", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

        {{PINVOKE}}";

        public static string ChooseColor = @"[DllImport(""comdlg32.dll"", SetLastError = true, CharSet = CharSet.Auto)]
        public extern static bool ChooseColor(ref CHOOSECOLOR lpcc);

        {{PINVOKE}}";
    }
}
