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
    /// 電卓サービス
    /// </summary>
    internal class CalcuratorSvc : IBackendSvc
    {
        static class DecimalFormatter
        {
            public static string Format(decimal val) => val.ToString("0.################");
            public static string DisplayFormat(decimal val) => string.Format("{0:#,0.#############}", val);
        }

        class CalcBuffer 
        {
            StringBuilder _buff = new StringBuilder();

            public bool IsNull { get; private set; }

            public override string ToString() => _buff.ToString();
            public decimal ToDecimal() => IsEmpty ? 0m : decimal.Parse(_buff.ToString());

            public void Clear() => _buff.Clear();

            public void Null()
            {
                Clear();
                IsNull = true;
            }

            public bool IsEmpty => IsNull || _buff.Length == 0;

            public void Append(object str)
            {
                if (str.Equals("."))
                {
                    // ドット
                    if (IsEmpty) _buff.Append("0");
                    else if (_buff.ToString().Contains(".")) return;  // ドットの多重入力を回避
                }
                _buff.Append(str);
                IsNull = false;
            }

            public void Chomp()
            {
                if (IsEmpty) return;
                _buff.Length -= 1;
            }

            public CalcBuffer Replace(string str)
            {
                Clear();
                Append(str);
                return this;
            }

            public CalcBuffer Truncate() => Replace(DecimalFormatter.Format(ToDecimal()));

            public string Coalesce(string other) => IsNull ? other : ToString();

            public string DisplayText()
            {
                var str = DecimalFormatter.DisplayFormat(ToDecimal());
                var buff = _buff.ToString();
                if (buff.EndsWith("."))
                {
                    str += ".";  // ドットで終わっていればそれを付ける
                }
                else if (buff.Contains("."))
                {
                    // 途中にドットを含む場合
                    var m = Regex.Match(_buff.ToString(), @"\.?0*$");
                    if (m.Success) str += m.Value;  // 0 の繰り返しで終わっていればをそれを付ける
                }
                return str;
            }
        }

        interface IOperator
        { 
            void Exec(CalcContextMaeda ctx);
        }

        interface IBinaryOperator : IOperator
        { 
            void Calc(CalcContextMaeda ctx);
        }

        /// <summary>
        /// 四則演算等の汎用のオペレータ
        /// </summary>
        class ArithmeticOperator : IBinaryOperator
        {
            public string Label { get; }
            Func<decimal, decimal, decimal> _f;
            public ArithmeticOperator(string label, Func<decimal, decimal, decimal> f) { Label = label; _f = f; }
            public void Exec(CalcContextMaeda ctx)
            {
                if (ctx.Value1 != null && !ctx.Buffer.IsEmpty && ctx.Operator != null)
                {
                    // 履歴プッシュ
                    ctx.PushHistory(this);
                    // 次に計算する 
                    ctx.Operator.Calc(ctx);
                }
                else if (!ctx.Buffer.IsEmpty)
                {
                    // 履歴プッシュ
                    ctx.PushHistory(this);
                    // バッファに入力があれば Value1 に送る
                    ctx.Value1 = ctx.Buffer.Truncate().ToString();
                    ctx.Buffer.Clear();
                }
                ctx.Operator = this;
                ctx.OverrideLastHistory(this);
            }
            public void Calc(CalcContextMaeda ctx) {
                var v = _f(ctx.Value1Decimal, ctx.Value2Decimal);
                ctx.Value1 = v.ToString();
                ctx.Operator = null;
                ctx.Buffer.Clear();
            }
        }
        
        /// <summary>
        /// イコール
        /// </summary>
        class EqualOperator : IOperator
        {
            public void Exec(CalcContextMaeda ctx)
            {
                ctx.History.Clear();
                if (ctx.Operator == null)
                {
                    ctx.Buffer.Truncate();
                    return;
                }
                ctx.Operator.Exec(ctx);
                ctx.History.Clear();
                ctx.PushHistory();
                ctx.Operator = null;
            }
        }

        /// <summary>
        /// クリア
        /// </summary>
        class ClearOperator : IOperator
        {
            public void Exec(CalcContextMaeda ctx)
            {
                ctx.Value1 = null;
                ctx.Buffer.Null();
                ctx.Operator = null;
                ctx.History.Clear();
            }
        }

        /// <summary>
        /// クリアエンド
        /// </summary>
        class ClearEndOperator : IOperator
        {
            public void Exec(CalcContextMaeda ctx)
            {
                ctx.Buffer.Null();
            }
        }

        /// <summary>
        /// バックスペース
        /// </summary>
        class BackSpaceOperator : IOperator
        {
            public void Exec(CalcContextMaeda ctx)
            {
                ctx.Buffer.Chomp();
            }
        }

        /// <summary>
        /// パーセント
        /// </summary>
        class PercentOperator : IOperator
        {
            public void Exec(CalcContextMaeda ctx)
            {
                var v = decimal.Parse(ctx.Buffer.ToString());
                ctx.Buffer.Clear();
                ctx.Buffer.Append(ctx.Value1Decimal / v);
            }
        }

        Dictionary<CalcButton, IOperator> OperatorTable = new Dictionary<CalcButton, IOperator>
        {
            { CalcButton.BtnPlus, new ArithmeticOperator("+", (val1, val2) => val1 + val2)},
            { CalcButton.BtnMinus, new ArithmeticOperator("-", (val1, val2) => val1 - val2)},
            { CalcButton.BtnMultiple, new ArithmeticOperator("×", (val1, val2) => val1 * val2)},
            { CalcButton.BtnDivide, new ArithmeticOperator("÷", (val1, val2) => val1 / val2)},
            { CalcButton.BtnClear, new ClearOperator() },
            { CalcButton.BtnClearEnd, new ClearEndOperator() },
            { CalcButton.BtnBS, new BackSpaceOperator() },
            { CalcButton.BtnEqual, new EqualOperator() },
            { CalcButton.BtnExt1, new PercentOperator() },
        };



        class CalcContextMaeda : ICalcContext
        {
            public CalcBuffer Buffer { get; } = new CalcBuffer();

            public List<string> History { get; } = new List<string>();

            public string Value1;
            public decimal Value1Decimal => string.IsNullOrWhiteSpace(Value1) ? 0m : decimal.Parse(Value1);

            public IBinaryOperator Operator;

            public decimal Value2Decimal => Buffer.IsEmpty ? 0m : decimal.Parse(Buffer.ToString());

            public string DisplayText
            {
                get
                {
                    if (Buffer.IsNull) return "0";
                    return Buffer.IsEmpty ? string.Format("{0:#,0.#############}", Value1Decimal) : Buffer.DisplayText();
                }
            }

            public string SubDisplayText => Operator!=null ? string.Join(" ", History) : "";

            internal void AppendNum(string num)
            {
                Buffer.Append(num);
                if (Operator == null) Value1 = null;
            }

            internal void PushHistory(ArithmeticOperator ope = null)
            {
                History.Add(Buffer.IsEmpty ? Value1 : DecimalFormatter.Format(Buffer.ToDecimal()));
                ope = ope ?? Operator as ArithmeticOperator;
                if (ope != null) History.Add(ope.Label);
            }
            internal void OverrideLastHistory(ArithmeticOperator ope)
            {
                if (History.Count < 2)
                {
                    History.Add("0");
                }
                else
                {
                    History.RemoveAt(History.Count - 1);
                }
                History.Add(ope.Label);
            }
        }

        /// <summary>
        /// コンテキストの新規生成
        /// </summary>
        /// <returns></returns>
        public ICalcContext CreateContext() => new CalcContextMaeda();

        /// <summary>
        /// 拡張ボタンのテキストを返す
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string GetExtButtonText(int num)
        {
            if (num == 1) return "%";
            return null;
        }

        /// <summary>
        /// ボタン押下時の処理
        /// </summary>
        /// <param name="ctx0"></param>
        /// <param name="btn"></param>
        public bool TryButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContextMaeda;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            if (OperatorTable.ContainsKey(btn))
            {
                OperatorTable[btn].Exec(ctx);
            }
            else if (btn < CalcButton.BtnExt1)
            {
                ctx.AppendNum(btn == CalcButton.BtnDot ? "." : $"{(int)btn}");
            }
            return true;
        }

    }
}
