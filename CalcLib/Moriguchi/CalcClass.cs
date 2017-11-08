using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    class CalcClass : ISubSvc
    {
        /// <summary>
        /// 電卓モード用コンテキスト
        /// </summary>
        public class CalcContext : ISubContext
        {
            /// <summary>
            /// メインディスプレイに表示する文字列
            /// </summary>
            public string DisplayText
            {
                get
                {
                    return Buffer == null ? Value : Buffer;
                }
                set
                {

                }
            }
            /// <summary>
            /// サブディスプレイに表示する文字列
            /// </summary>
            public string SubDisplayText
            {
                get
                {
                    return Operation == null ? "" : Value + UtlClass.OpeNameHelper.Get(Operation);
                }
                set
                {

                }
            }
            //TODO:ゆくゆくはコッチを使う↓

            ///// <summary>
            ///// メインディスプレイに表示する文字列
            ///// </summary>
            //public string DisplayText { get; set; }

            ///// <summary>
            ///// サブディスプレイに表示する文字列
            ///// </summary>
            //public string SubDisplayText { get; set; }

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

            //電卓にしか使ってない！邪魔
            public bool Reset { get; set; }

            public void Clear()
            {
                DisplayText = null;
                SubDisplayText = null;
                Operation = null;
            }
        }

        public virtual ISubContext CreateContext() => new CalcContext();

        /// <summary>
        /// 初期動作
        /// </summary>
        /// <param name="ctx0"></param>
        public void Init(ISubContext Factx, ICalcContext prevCtx)
        {
            var factx = Factx as CalcContext;
            factx.Clear();
        }

        /// <summary>
        /// クリック時動作
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        /// <returns></returns>
        public bool OnClick(ISubContext ctx, CalcButton btn)
        {
            CalcMethod(btn, (CalcContext)ctx);
            return true;
        }

        private void CalcMethod(CalcButton btn, CalcContext ctx)
        {
            switch (btn)
            {
                //演算子
                case CalcButton.BtnPlus:
                case CalcButton.BtnMinus:
                case CalcButton.BtnDivide:
                case CalcButton.BtnMultiple:
                    //小数点押下直後に演算子を押下すると小数点を削除する
                    if (ctx.Buffer?.EndsWith(".") == true)
                    {
                        char[] dot = { '.' };
                        ctx.Buffer = ctx.Buffer.TrimEnd(dot);
                    }
                    OnOpeButtonClick(ctx, btn);   // 演算子ボタン押下時の処理
                    break;

                //クリア
                case CalcButton.BtnClear:
                    ctx.Buffer = null;
                    ctx.Value = null;
                    ctx.Operation = null;
                    //ctx.SubDisplayText = null;
                    break;
                case CalcButton.BtnClearEnd:
                    break;
                //バックスペース
                case CalcButton.BtnBS:
                    if (!string.IsNullOrEmpty(ctx.Buffer))
                    {
                        UtlClass.Chomp(ctx);
                    }
                    break;

                //パーセント(BtnExt1)押下時
                case CalcButton.BtnExt1:
                    //Valueがnullでない時、valueのbuffer%をbufferに入れる
                    //TODO:Valueに値が入っていると%ボタンを押下する度に計算してしまう
                    if (!string.IsNullOrEmpty(ctx.Value))
                    {
                        var val = double.Parse(ctx.Value);
                        var buf = double.Parse(ctx.Buffer);
                        ctx.Buffer = (val * (buf / 100)).ToString();
                    }
                    break;

                //小数点押下時
                case CalcButton.BtnDot:
                    if (!ctx.Buffer?.EndsWith(".") == true) ctx.Buffer += ".";
                    break;

                //計算
                case CalcButton.BtnEqual:
                    if (!string.IsNullOrEmpty(ctx.Buffer) && !string.IsNullOrEmpty(ctx.Value) && ctx.Operation != null)
                    {
                        ExecCalcuration(ctx, ctx.Operation.Value);
                        ctx.Operation = null;
                        //ctx.SubDisplayText = null;
                    }
                    break;

                //入力数値取得
                default:
                    if (ctx.Reset)
                    {
                        ctx.Buffer = null;
                        ctx.Reset = false;
                    }
                    if (ctx.Buffer?.StartsWith("0") == true) ctx.Buffer = "";
                    ctx.Buffer += (int)btn;
                    break;
            }
        }

        /// <summary>
        /// 演算子ボタン押下時の処理
        /// </summary>
        /// <param name="ctx"></param>
        private void OnOpeButtonClick(CalcContext ctx, CalcButton btn)
        {
            ctx.Operation = btn;
            //ctx.SubDisplayText += ctx.Buffer;

            if (string.IsNullOrEmpty(ctx.Value))
            {
                // 左辺が未入力の時、Bufferの値を左辺とする
                ctx.Value = ctx.Buffer;
                ctx.Buffer = null;
                //ctx.SubDisplayText += ctx.Buffer + OpeNameHelper.Get(btn);
            }
            else if (!string.IsNullOrEmpty(ctx.Buffer))
            {
                // 左辺が入力済みで、Bufferが入力済みの時、計算処理を実行する
                var x = ExecCalcuration(ctx, btn);

                // 続けて計算できるよう実行結果を左辺にセットする。
                ctx.Value = x;
                ctx.Buffer = null;
                //ctx.SubDisplayText += OpeNameHelper.Get(btn);
            }
        }

        /// <summary>
        /// 計算処理を実行します
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private string ExecCalcuration(CalcContext ctx, CalcButton operation)
        {
            string x;
            {
                x = string.Format("{0:#,0.#############}", Calc(ctx.Value, ctx.Buffer, operation));
                ctx.Reset = true;
                ctx.Buffer = x;
                ctx.Value = null;
            }
            return x;
        }

        /// <summary>
        /// 計算処理
        /// </summary>
        /// <param name="Value1"></param>
        /// <param name="Value2"></param>
        /// <param name="Ope"></param>
        /// <returns></returns>
        public double Calc(string Value, string Buffer, CalcButton Ope)
        {
            double answer = 0;

            var val1 = double.Parse(Value);
            var val2 = double.Parse(Buffer);

            if (Ope == CalcButton.BtnPlus)
            {
                answer = val1 + val2;
            }
            else if (Ope == CalcButton.BtnMinus)
            {
                answer = val1 - val2;
            }
            else if (Ope == CalcButton.BtnMultiple)
            {
                answer = val1 * val2;
            }
            else if (Ope == CalcButton.BtnDivide)
            {
                answer = val1 / val2;
            }
            return answer;
        }
    }
}
