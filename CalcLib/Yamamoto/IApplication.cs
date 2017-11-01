using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto
{
    public interface IApplication
    {
        /// <summary>
        /// アプリ実行
        /// </summary>
        /// <param name="ctx0"></param>
        /// <param name="btn"></param>
        void Run(ICalcContext ctx0, CalcButton btn);
    }
}
