using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class CreateFile : IInvoke
    {
        string IInvoke.Name => "CreateFile";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateFile(
            string lpFileName, UInt32 dwDesiredAccess, 
            UInt32 dwShareMode, IntPtr lpSecurityAttributes, 
            UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes, 
            IntPtr hTemplateFile);


        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
