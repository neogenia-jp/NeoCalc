using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Maeda.Util
{
    public static class TimeCop
    {
        internal static DateTimeOffset? _CurrentTime { get; set; }

        public static DateTimeOffset GetCurrentTime() => _CurrentTime ?? DateTimeOffset.Now;
    }
}
