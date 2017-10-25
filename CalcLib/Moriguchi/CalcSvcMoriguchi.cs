using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    internal class CalcSvcMoriguchi : ICalcSvcEx
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

        class CalcContextMoriguchi : ICalcContext
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
            public string[] omikuji = { "大吉", "中吉", "小吉", "凶" };
        }

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

            if (ctx.Mode)
            {    //おみくじモード時
                OmikujiMethod(btn, ctx);
            }
            else
            {
                //電卓モード時
                CalcMethod(btn, ctx);
            }
        }

        /// <summary>
        /// おみくじモード時の動作
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="ctx"></param>
        private void OmikujiMethod(CalcButton btn, CalcContextMoriguchi ctx)
        {
            //押下ボタン判定
            switch (btn)
            {
                case CalcButton.Btn1:
                case CalcButton.Btn2:
                case CalcButton.Btn3:
                case CalcButton.Btn4:
                    OpenOmikuji(btn, ctx);
                    //おみくじを1回でも引いたら電卓モードへ
                    ctx.Mode = false;
                    break;

                //電卓モードへ戻る時
                case CalcButton.BtnClear:
                case CalcButton.BtnClearEnd:
                case CalcButton.BtnExt2:
                    ctx.Mode = false;
                    ctx.Buffer = "電卓モードへ移行";
                    ctx.Value = null;
                    ctx.Operation = null;
                    break;
                //関係ないボタン押下時
                default:
                    ctx.Value = "おみくじは1～4を選択:[C]or[CE]で電卓ﾓｰﾄﾞ";
                    break;
            }
        }

        /// <summary>
        /// おみくじの開示
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="ctx"></param>
        private void OpenOmikuji(CalcButton btn, CalcContextMoriguchi ctx)
        {
            //おみくじ配列のシャッフル
            var test = ctx.omikuji.OrderBy(x => Guid.NewGuid()).ToArray();

            //開示表示
            ctx.Value = $"本日の運勢は「{test[(int)btn - 1]}」です";
            ctx.Buffer = null;
            foreach (var kekka in test.Select(x => x)) { ctx.Buffer += kekka + " "; };
            //ctx.Buffer = $"{test[0] + test[1] + test[2] + test[3]}";

        }

        private void CalcMethod(CalcButton btn, CalcContextMoriguchi ctx)
        {
            if (!string.IsNullOrEmpty(ctx.Value) && ctx.Value.StartsWith("本"))
            {
                ctx.Value = null;
                ctx.Buffer = null;
            }

            switch (btn)
            {
                //演算子
                case CalcButton.BtnPlus:
                case CalcButton.BtnMinus:
                case CalcButton.BtnDivide:
                case CalcButton.BtnMultiple:
                    //小数点押下直後に演算子を押下すると小数点を削除する
                    if (ctx.Buffer.EndsWith("."))
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
                        var test = ctx.Buffer.Length;
                        ctx.Buffer = ctx.Buffer.Remove(ctx.Buffer.Length - 1);
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

                //おみくじモードボタン押下時
                case CalcButton.BtnExt2:
                    ctx.Mode = true;
                    ctx.Value = "おみくじを選択してください";
                    ctx.Operation = btn;
                    ctx.Buffer = "[1] [2] [3] [4]";
                    break;

                //小数点押下時
                case CalcButton.BtnDot:
                    if (!ctx.Buffer.EndsWith(".")) ctx.Buffer += ".";
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
                    ctx.Buffer += (int)btn;
                    break;
            }

            if (!string.IsNullOrEmpty(ctx.Buffer)) ctx.Buffer = ctx.Buffer.Replace("電卓モードへ移行", "");
        }

        /// <summary>
        /// 演算子ボタン押下時の処理
        /// </summary>
        /// <param name="ctx"></param>
        private void OnOpeButtonClick(CalcContextMoriguchi ctx, CalcButton btn)
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
        private string ExecCalcuration(CalcContextMoriguchi ctx, CalcButton operation)
        {
            string x;
            {
                x = string.Format("{0:#,0.#############}",Calc(ctx.Value, ctx.Buffer, operation));
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
