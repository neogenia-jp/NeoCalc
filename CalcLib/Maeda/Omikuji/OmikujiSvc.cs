using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CalcLib.Maeda.Basis;

namespace CalcLib.Maeda.Omikuji
{
    /// <summary>
    /// おみくじサービスのためのコンテキスト
    /// </summary>
    internal class OmikujiContext : ICalcContext
    {
        public string DisplayText => Proxy.DisplayText;

        public string SubDisplayText => Proxy.SubDisplayText;

        internal Omikuji.OmikujiBase Proxy { get; set; } = new Omikuji.OmikujiImpl();  // 実体はここ

        public void Init() => Proxy.Init();

        public bool Choise(int n) => Proxy.TryChoise(n);

        public SvcState State { get { return Proxy.State; } set { Proxy.State = value; } }
    }

    /// <summary>
    /// おみくじサービス
    /// </summary>
    internal class OmikujiSvc : SvcBase<OmikujiContext>
    {
        public override string GetExtButtonText(int num) => num == 1 ? "*" : null;

        internal override OmikujiContext _CreateContext() => new OmikujiContext();

        public override bool TryButtonClick(OmikujiContext ctx, CalcButton btn)
        {
            switch (btn)
            {
                case CalcButton.BtnExt1:
                    if (ctx.State != SvcState.Unknown) return false;
                    ctx.Init();
                    break;
                default:
                    if (ctx.State == SvcState.Finished) return false;
                    ctx.Choise(btn - CalcButton.Btn1);
                    break;
            }

            return true;
        }
        protected override void OnExitSvc(OmikujiContext ctx)
        {
            ctx.State = SvcState.Unknown;
        }
    }
}
