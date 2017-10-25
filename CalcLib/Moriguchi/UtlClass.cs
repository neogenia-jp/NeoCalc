using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    class UtlClass
    {
        public static void Chomp(CalcSvcMoriguchi.CalcContextMoriguchi ctx)
          => ctx.Buffer = ctx.Buffer.Remove(ctx.Buffer.Length - 1);
    }
}
