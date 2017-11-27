using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto
{
    /// <summary>
    /// 電卓のモード
    /// </summary>
    public enum AppMode
    {
        Calculator = 0,  // 電卓
        Omikuji,         // おみくじ
        Stock,           // 株価
        None,            // なし
    }
}
