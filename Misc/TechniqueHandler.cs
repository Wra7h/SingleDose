using SingleDose.Techniques;
using SingleDose.Techniques.Injects;
using SingleDose.Techniques.Loaders;

namespace SingleDose.Misc
{
    internal class TechniqueHandler
    {
        public static ITechnique GetTechnique(string szName)
        {
            ITechnique technique = null;
            switch (szName)
            {
                #region Loaders
                case "FLSSETVALUE":
                    technique = new FlsSetValue();
                    break;
                case "L1":
                    goto case "FLSSETVALUE";
                case "IMAGEGETDIGEST":
                    technique = new ImageGetDigest();
                    break;
                case "L2":
                    goto case "IMAGEGETDIGEST";
                case "CREATEFIBER":
                    technique = new CreateFiber();
                    break;
                case "L3":
                    goto case "CREATEFIBER";
                case "NTTESTALERT":
                    technique = new NtTestAlert();
                    break;
                case "L4":
                    goto case "NTTESTALERT";
                case "THREADPOOLWAIT":
                    technique = new ThreadpoolWait();
                    break;
                case "L5":
                    goto case "THREADPOOLWAIT";
                case "CREATETHREAD":
                    technique = new CreateThread();
                    break;
                case "L6":
                    goto case "CREATETHREAD";
                case "ENUMDESKTOPS":
                    technique = new EnumDesktops();
                    break;
                case "L7":
                    goto case "ENUMDESKTOPS";
                case "SETTIMER":
                    technique = new SetTimer();
                    break;
                case "L8":
                    goto case "SETTIMER";
                case "SETUPCOMMITFILEQUEUE":
                    technique = new SetupCommitFileQueue();
                    break;
                case "L9":
                    goto case "SETUPCOMMITFILEQUEUE";
                case "CERTENUMSYSTEMSTORE":
                    technique = new CertEnumSystemStore();
                    break;
                case "L10":
                    goto case "CERTENUMSYSTEMSTORE";
                case "ENUMCHILDWINDOWS":
                    technique = new EnumChildWindows();
                    break;
                case "L11":
                    goto case "ENUMCHILDWINDOWS";
                case "ENUMDATEFORMATSEX":
                    technique = new EnumDateFormatsEx();
                    break;
                case "L12":
                    goto case "ENUMDATEFORMATSEX";
                case "ENUMWINDOWS":
                    technique = new EnumWindows();
                    break;
                case "L13":
                    goto case "ENUMWINDOWS";
                case "GETOPENFILENAME":
                    technique = new GetOpenFilename();
                    break;
                case "L14":
                    goto case "GETOPENFILENAME";
                case "VERIFIERENUMERATERESOURCE":
                    technique = new VerifierEnumerateResource();
                    break;
                case "L15":
                    goto case "VERIFIERENUMERATERESOURCE";
                case "THREADPOOLTIMER":
                    technique = new ThreadpoolTimer();
                    break;
                case "L16":
                    goto case "THREADPOOLTIMER";
                case "THREADPOOLWORK":
                    technique = new ThreadpoolWork();
                    break;
                case "L17":
                    goto case "THREADPOOLWORK";
                #endregion

                #region Injects
                case "CREATEREMOTETHREAD":
                    technique = new CreateRemoteThread();
                    break;
                case "R1":
                    goto case "CREATEREMOTETHREAD";
                case "EARLYBIRDQUEUEUSERAPC":
                    technique = new EarlyBirdQueueUserAPC();
                    break;
                case "R2":
                    goto case "EARLYBIRDQUEUEUSERAPC";
                case "SUSPENDQUEUEUSERAPC":
                    technique = new SuspendQueueUserAPC();
                    break;
                case "R3":
                    goto case "SUSPENDQUEUEUSERAPC";
                case "ADDRESSOFENTRYPOINT":
                    technique = new AddressOfEntryPoint();
                    break;
                case "R4":
                    goto case "ADDRESSOFENTRYPOINT";
                case "KERNELCALLBACKTABLE":
                    technique = new KernelCallbackTable();
                    break;
                case "R5":
                    goto case "KERNELCALLBACKTABLE";
                case "NTCREATESECTION":
                    technique = new NtCreateSection();
                    break;
                case "R6":
                    goto case "NTCREATESECTION";
                case "PERESOURCE":
                    technique = new PEResource();
                    break;
                case "R7":
                    goto case "PERESOURCE";
                case "SIR":
                    technique = new SIR();
                    break;
                case "R8":
                    goto case "SIR";
                case "SPAWNTHREADHIJACK":
                    technique = new SpawnThreadHijack();
                    break;
                case "R9":
                    goto case "SPAWNTHREADHIJACK";
                #endregion
                default:
                    SDConsole.WriteError("Unknown technique. Techniques can be found in help.");
                    break;
            }
            
            return technique;
        }
    }
}
