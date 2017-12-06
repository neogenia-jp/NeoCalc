using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CalcLib.Maeda.Basis;

namespace CalcLib.Maeda.Janken
{
    /// <summary>
    /// おみくじサービスのためのコンテキスト
    /// </summary>
    internal class JankenContext : ICalcContext
    {
        public string DisplayText => Proxy.DisplayText;

        public string SubDisplayText => Proxy.SubDisplayText;

        internal Janken.JankenBase Proxy { get; set; } = new Janken.JankeniImpl();  // 実体はここ

        public void Init() => Proxy.Init();

        public bool Choise(int n) => Proxy.TryChoise(n);

        public SvcState State { get { return Proxy.State; } set { Proxy.State = value; } }
    }

    /// <summary>
    /// おみくじサービス
    /// </summary>
    internal class JankenSvc : SvcBase<JankenContext>
    {
        public override string GetExtButtonText(int num) => num == 1 ? "仮機能" : null;

        internal override JankenContext _CreateContext() => new JankenContext();

        public override bool TryButtonClick(JankenContext ctx, CalcButton btn)
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
        protected override void OnExitSvc(JankenContext ctx)
        {
            ctx.State = SvcState.Unknown;
        }
    }
}
