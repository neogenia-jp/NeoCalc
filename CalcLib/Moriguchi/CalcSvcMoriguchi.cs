using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    public class CalcSvcMoriguchi : ICalcSvcEx
    {
        class OpeNameHelper
        {
            static readonly Dictionary<CalcButton, string> OpeTextTable = new Dictionary<CalcButton, string>
            {
              { CalcButton.BtnPlus, "+" },
              { CalcButton.BtnMinus, "-" },
              { CalcButton.BtnMultiple, "×" },
              { CalcButton.BtnDivide, "÷"},
              { CalcButton.BtnExt2,""},
            };
            public static string Get(CalcButton? opeButton) => opeButton.HasValue ? OpeTextTable[opeButton.Value] : "";
        }

        public class CalcContextMoriguchi : ICalcContext
        {
            /// <summary>
            /// 左辺の値
            /// </summary>
            public string Value { get; set; }
            
            /// <summary>
            /// 計算対象とする演算子
            /// </summary>
            public CalcButton? Operation { get; set; }

            /// <summary>
            /// 入力中の値（Valueがセットされている時は右辺となる）
            /// </summary>
            public string Buffer { get; set; }

            public bool Reset { get; set; }

            public string DisplayText => Buffer == null ? Value : Buffer;

            /// <summary>
            /// サブディスプレイに表示する文字列
            /// </summary>
            public virtual string SubDisplayText => Operation == null ? "" : Value + OpeNameHelper.Get(Operation);

            /// <summary>
            /// モードの状態(trueならおみくじモード)
            /// </summary>
            public bool Mode;

            /// <summary>
            /// おみくじ
            /// </summary>
            public string[] omikuji = { "大吉", "中吉", "小吉", "凶　" };

            public void Clear()
            {
                Buffer = "0";
                Value = null;
                Operation = null;
            }

        }

        /// <summary>
        /// サービスの入れ物
        /// </summary>
        static ISubSvc svc;

        public virtual ICalcContext CreateContext() => new CalcContextMoriguchi();

        /// <summary>
        /// 拡張ボタンのテキストを返す
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string GetExtButtonText(int num)
        {
            if (num == 1)
            {
                return "%";
            }
            else if(num == 2)
            {
                return "おみくじ";
            }
            return null;
        }

        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContextMoriguchi;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            //defaultでは電卓モード
            if (svc == null)
            {
                svc = new CalcClass();
                svc.Init(ctx);
            }

            switch (btn)
            {
                case CalcButton.BtnExt2:
                    svc = new OmikujiClass();
                    svc.Init(ctx);
                    break;
                default:
                    break;
            }

            var ret = svc.OnClick(ctx, btn);

            if (!ret)
            {
                svc = null;
            }
        }
    }
}
