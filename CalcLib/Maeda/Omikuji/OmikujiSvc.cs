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
        public string DisplayText { get; set; }

        public string SubDisplayText { get; set; }

        public Omikuji.OmikujiImpl omikuji { get; set; } = new Omikuji.OmikujiImpl();

        public bool finished;

        public void Init()
        {
            DisplayText = "[1 ] [2 ] [3 ] [4 ]";
            SubDisplayText = "";
            omikuji.Init();
        }

        public bool Choise(int n)
        {
            if (!omikuji.TryChoise(n)) return false;  // 選択不可なら何もせずfalseを返す

            DisplayText = string.Join(" ", omikuji.Items);
            SubDisplayText = omikuji.ResultText;
            return true;
        }

        public bool Finished => omikuji.IsFinished;
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
            switch (btn)
            {
                case CalcButton.BtnExt1:
                    ctx.Init();
                    break;
                default:
                    if (ctx.Finished) return false;
                    ctx.Choise(btn - CalcButton.Btn1);
                    break;
            }

            return true;
        }
    }
}
