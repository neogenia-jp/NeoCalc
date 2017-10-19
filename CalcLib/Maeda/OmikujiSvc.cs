using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CalcLib.Maeda.Basis;

namespace CalcLib.Maeda
{
    /// <summary>
    /// おみくじサービスのためのコンテキスト
    /// </summary>
    internal class OmikujiContext : ICalcContext
    {
        public string DisplayText => "[1 ] [2 ] [3 ] [4 ] ";

        public string SubDisplayText => "omikuji";
    }

    /// <summary>
    /// おみくじサービス
    /// </summary>
    internal class OmikujiSvc : SvcBase<OmikujiContext>
    {
        public override string GetExtButtonText(int num) => num == 1 ? "*" : null;

        protected override OmikujiContext _CreateContext() => new OmikujiContext();

        public override bool TryButtonClick(OmikujiContext ctx, CalcButton btn)
        {
            // FIXME

            return true;
        }
    }
}
