using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib
{
    internal class CalcContext : ICalcContext
    {
        /// <summary>
        /// ディスプレイに表示する文字列
        /// </summary>
        public virtual string DisplayText { get; set; }

        /// <summary>
        /// サブディスプレイに表示する文字列
        /// </summary>
        public virtual string SubDisplayText { get; set; }


        public override string ToString() => $"{DisplayText} ({SubDisplayText})";
    }
}
