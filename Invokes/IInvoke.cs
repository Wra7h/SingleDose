using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleDose.Invokes
{
    public interface IInvoke
    {
        string Name { get; }

        string PInvoke { get; }

        string DInvoke { get; }
    }
}
