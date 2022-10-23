using SingleDose.Menus;
using SingleDose.PInvoke;
using SingleDose.Techniques;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SingleDose.Misc
{
    internal class PInvokeHandler
    {
        public static string AddPInvokes(ITechnique technique, string CSContents)
        {
            Regex regPattern = new Regex("{{PINVOKE}}");

            List<string> PInvokes = technique.PInvokeRecipe;

            if (SettingsMenu.szMemAlloc == "RW/RX" && !technique.PInvokeRecipe.Contains("VirtualProtectEx"))
            {
                PInvokes.Add("VirtualProtectEx");
            }

            foreach (string szRecipeItem in PInvokes)
            {
                switch (szRecipeItem)
                {
                    case "BeginUpdateResource":
                        CSContents = regPattern.Replace(CSContents, Kernel32.BeginUpdateResource);
                        break;
                    case "CertEnumSystemStore":
                        CSContents = regPattern.Replace(CSContents, Crypt32.CertEnumSystemStore);
                        break;
                    case "ChooseColor":
                        CSContents = regPattern.Replace(CSContents, Comdlg32.ChooseColor);
                        break;
                    case "ClusWorkerCreate":
                        CSContents = regPattern.Replace(CSContents, ResUtils.ClusWorkerCreate);
                        break;
                    case "ClusWorkerTerminateEx":
                        CSContents = regPattern.Replace(CSContents, ResUtils.ClusWorkerTerminateEx);
                        break;
                    case "CloseHandle":
                        CSContents = regPattern.Replace(CSContents, Kernel32.CloseHandle);
                        break;
                    case "CloseThreadpoolTimer":
                        CSContents = regPattern.Replace(CSContents, Kernel32.CloseThreadpoolTimer);
                        break;
                    case "CloseThreadpoolWork":
                        CSContents = regPattern.Replace(CSContents, Kernel32.CloseThreadpoolWork);
                        break;
                    case "ConvertThreadToFiber":
                        CSContents = regPattern.Replace(CSContents, Kernel32.ConvertThreadToFiber);
                        break;
                    case "CreateEvent":
                        CSContents = regPattern.Replace(CSContents, Kernel32.CreateEvent);
                        break;
                    case "CreateFiber":
                        CSContents = regPattern.Replace(CSContents, Kernel32.CreateFiber);
                        break;
                    case "CreateFile":
                        CSContents = regPattern.Replace(CSContents, Kernel32.CreateFile);
                        break;
                    case "CreateProcess":
                        CSContents = regPattern.Replace(CSContents, Kernel32.CreateProcess);
                        break;
                    case "CreateRemoteThread":
                        CSContents = regPattern.Replace(CSContents, Kernel32.CreateRemoteThread);
                        break;
                    case "CreateThread":
                        CSContents = regPattern.Replace(CSContents, Kernel32.CreateThread);
                        break;
                    case "CreateThreadpoolTimer":
                        CSContents = regPattern.Replace(CSContents, Kernel32.CreateThreadpoolTimer);
                        break;
                    case "CreateThreadpoolWait":
                        CSContents = regPattern.Replace(CSContents, Kernel32.CreateThreadpoolWait);
                        break;
                    case "CreateThreadpoolWork":
                        CSContents = regPattern.Replace(CSContents, Kernel32.CreateThreadpoolWork);
                        break;
                    case "CreateWaitableTimer":
                        CSContents = regPattern.Replace(CSContents, Kernel32.CreateWaitableTimer);
                        break;
                    case "DispatchMessage":
                        CSContents = regPattern.Replace(CSContents, User32.DispatchMessage);
                        break;
                    case "EndUpdateResource":
                        CSContents = regPattern.Replace(CSContents, Kernel32.EndUpdateResource);
                        break;
                    case "EnumChildWindows":
                        CSContents = regPattern.Replace(CSContents, User32.EnumChildWindows);
                        break;
                    case "EnumDateFormatsEx":
                        CSContents = regPattern.Replace(CSContents, Kernel32.EnumDateFormatsEx);
                        break;
                    case "EnumDesktops":
                        CSContents = regPattern.Replace(CSContents, User32.EnumDesktops);
                        break;
                    case "EnumWindows":
                        CSContents = regPattern.Replace(CSContents, User32.EnumWindows);
                        break;
                    case "FlsAlloc":
                        CSContents = regPattern.Replace(CSContents, Kernel32.FlsAlloc);
                        break;
                    case "FlsSetValue":
                        CSContents = regPattern.Replace(CSContents, Kernel32.FlsSetValue);
                        break;
                    case "GetCurrentThread":
                        CSContents = regPattern.Replace(CSContents, Kernel32.GetCurrentThread);
                        break;
                    case "GetMessage":
                        CSContents = regPattern.Replace(CSContents, User32.GetMessage);
                        break;
                    case "GetModuleHandle":
                        CSContents = regPattern.Replace(CSContents, Kernel32.GetModuleHandle);
                        break;
                    case "GetOpenFileName":
                        CSContents = regPattern.Replace(CSContents, Comdlg32.GetOpenFileName);
                        break;
                    case "GetProcAddress":
                        CSContents = regPattern.Replace(CSContents, Kernel32.GetProcAddress);
                        break;
                    case "GetProcessWindowStation":
                        CSContents = regPattern.Replace(CSContents, User32.GetProcessWindowStation);
                        break;
                    case "GetThreadContext":
                        CSContents = regPattern.Replace(CSContents, Kernel32.GetThreadContext);
                        break;
                    case "GetTopWindow":
                        CSContents = regPattern.Replace(CSContents, User32.GetTopWindow);
                        break;
                    case "ImageGetDigestStream":
                        CSContents = regPattern.Replace(CSContents, Imagehlp.ImageGetDigestStream);
                        break;
                    case "NtCreateSection":
                        CSContents = regPattern.Replace(CSContents, Ntdll.NtCreateSection);
                        break;
                    case "NtMapViewOfSection":
                        CSContents = regPattern.Replace(CSContents, Ntdll.NtMapViewOfSection);
                        break;
                    case "NtQueryInformationProcess":
                        CSContents = regPattern.Replace(CSContents, Ntdll.NtQueryInformationProcess);
                        break;
                    case "NtTestAlert":
                        CSContents = regPattern.Replace(CSContents, Ntdll.NtTestAlert);
                        break;
                    case "OpenThread":
                        CSContents = regPattern.Replace(CSContents, Kernel32.OpenThread);
                        break;
                    case "QueueUserAPC":
                        CSContents = regPattern.Replace(CSContents, Kernel32.QueueUserAPC);
                        break;
                    case "ReadProcessMemory":
                        CSContents = regPattern.Replace(CSContents, Kernel32.ReadProcessMemory);
                        break;
                    case "ResumeThread":
                        CSContents = regPattern.Replace(CSContents, Kernel32.ResumeThread);
                        break;
                    case "RtlCreateUserThread":
                        CSContents = regPattern.Replace(CSContents, Ntdll.RtlCreateUserThread);
                        break;
                    case "SendMessage":
                        CSContents = regPattern.Replace(CSContents, User32.SendMessage);
                        break;
                    case "SetThreadpoolTimer":
                        CSContents = regPattern.Replace(CSContents, Kernel32.SetThreadpoolTimer);
                        break;
                    case "SetThreadContext":
                        CSContents = regPattern.Replace(CSContents, Kernel32.SetThreadContext);
                        break;
                    case "SetThreadpoolWait":
                        CSContents = regPattern.Replace(CSContents, Kernel32.SetThreadpoolWait);
                        break;
                    case "SetTimer":
                        CSContents = regPattern.Replace(CSContents, User32.SetTimer);
                        break;
                    case "SetupCommitFileQueue":
                        CSContents = regPattern.Replace(CSContents, Setupapi.SetupCommitFileQueue);
                        break;
                    case "SetupOpenFileQueue":
                        CSContents = regPattern.Replace(CSContents, Setupapi.SetupOpenFileQueue);
                        break;
                    case "SetupQueueCopy":
                        CSContents = regPattern.Replace(CSContents, Setupapi.SetupQueueCopy);
                        break;
                    case "SetWaitableTimer":
                        CSContents = regPattern.Replace(CSContents, Kernel32.SetWaitableTimer);
                        break;
                    case "SleepEx":
                        CSContents = regPattern.Replace(CSContents, Kernel32.SleepEx);
                        break;
                    case "SubmitThreadpoolWork":
                        CSContents = regPattern.Replace(CSContents, Kernel32.SubmitThreadpoolWork);
                        break;
                    case "SuspendThread":
                        CSContents = regPattern.Replace(CSContents, Kernel32.SuspendThread);
                        break;
                    case "SwitchToFiber":
                        CSContents = regPattern.Replace(CSContents, Kernel32.SwitchToFiber);
                        break;
                    case "UpdateResource":
                        CSContents = regPattern.Replace(CSContents, Kernel32.UpdateResource);
                        break;
                    case "VerifierEnumerateResource":
                        CSContents = regPattern.Replace(CSContents, Verifier.VerifierEnumerateResource);
                        break;
                    case "VirtualAlloc":
                        CSContents = regPattern.Replace(CSContents, Kernel32.VirtualAlloc);
                        break;
                    case "VirtualAllocEx":
                        CSContents = regPattern.Replace(CSContents, Kernel32.VirtualAllocEx);
                        break;
                    case "VirtualProtectEx":
                        CSContents = regPattern.Replace(CSContents, Kernel32.VirtualProtectEx);
                        break;
                    case "WaitForSingleObject":
                        CSContents = regPattern.Replace(CSContents, Kernel32.WaitForSingleObject);
                        break;
                    case "WaitForThreadpoolTimerCallbacks":
                        CSContents = regPattern.Replace(CSContents, Kernel32.WaitForThreadpoolTimerCallbacks);
                        break;
                    case "WaitForThreadpoolWorkCallbacks":
                        CSContents = regPattern.Replace(CSContents, Kernel32.WaitForThreadpoolWorkCallbacks);
                        break;
                    case "WriteProcessMemory_ByteArray":
                        CSContents = regPattern.Replace(CSContents, Kernel32.WriteProcessMemory_ByteArray);
                        break;
                    case "WriteProcessMemory_IntPtr":
                        CSContents = regPattern.Replace(CSContents, Kernel32.WriteProcessMemory_IntPtr);
                        break;
                    default:
                        break;
                }
            }

            //Clear the last remaining "{{PINVOKE}}"
            CSContents = regPattern.Replace(CSContents, "");
            return CSContents;
        }
    }
}
