using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    public interface ISubContext
    {
        string DisplayText { get; }

        string SubDisplayText { get; }
    }
}
