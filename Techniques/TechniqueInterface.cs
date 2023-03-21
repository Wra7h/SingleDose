using System.Collections.Generic;

namespace SingleDose.Techniques
{
    public interface ITechnique
    {
        // Is the technique a shellcode loader?
        bool IsLoader { get; }

        // Does it need to be compiled with the "unsafe" flag?
        bool IsUnsafe { get; }

        // The name of the technique
        string TechniqueName { get; }

        // Description for the technique. How does it execute? What API triggers the execution?
        // Any other information such as "this will only execute when the OS does this", would be useful.
        string TechniqueDescription { get; }

        // The base template. This is the body of the code, with the various REGEX markers in correct places. i.e. {{NAMESPACE}}
        string Base { get; }
        // The technique's specific VirtualProtect/VirtualProtectEx to use for this technique. This is used depending on the setting of MemAlloc.(RWX or RW->RX)
        string VProtect { get; }

        // Are there any references where users can get more information on the technique?
        List<string> TechniqueReferences { get; }

        // What PInvokes are used? *Needs to be defined under PInvoke folder, and added to Build.AddInvokes() if necessary.
        List<string> Invokes { get; }

        //What additional information needs to be set before building? ProcessID? Executable path?
        List<string> Prerequisites { get; }
    }
}
