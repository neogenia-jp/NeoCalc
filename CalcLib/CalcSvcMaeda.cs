using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib
{
    internal class CalcSvcMaeda : ICalcSvcEx
    {
        class CalcBuffer 
        {
            StringBuilder _buff = new StringBuilder();

            public bool IsNull { get; private set; }

            public override string ToString() => _buff.ToString();

            public void Clear() => _buff.Clear();

            public void Null()
            {
                Clear();
                IsNull = true;
            }

            public bool IsEmpty => IsNull || _buff.Length == 0;

            public void Append(object str)
            {
                _buff.Append(str);
                IsNull = false;
            }

            public void Chomp()
            {
                if (IsEmpty) return;
                _buff.Length -= 1;
            }

            public void Replace(string str)
            {
                Clear();
                Append(str);
            }

            public string Coaesce(string other) => IsNull ? other : ToString();

        }

        interface IOperator
        { 
            void Exec(CalcContextMaeda ctx);
        }

        /// <summary>
        /// 四則演算等の汎用のオペレータ
        /// </summary>
        class ArithmeticOperator : IOperator
        {
            public string Label { get; }
            Func<decimal, decimal, decimal> _f;
            public ArithmeticOperator(string label, Func<decimal, decimal, decimal> f) { Label = label; _f = f; }
            public void Exec(CalcContextMaeda ctx)
            {
                if (ctx.Value1 != null && !ctx.Buffer.IsEmpty)
                {
                    // まず計算する 
                    Calc(ctx);
                }
                else if (!ctx.Buffer.IsEmpty)
                {
                    // バッファに入力があれば Value1 に送る
                    ctx.Value1 = ctx.Buffer.ToString();
                    ctx.Buffer.Clear();
                }
                
                ctx.Operator = this;
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
                if (ctx.Operator == null) return;
                ctx.Operator.Exec(ctx);
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
                ctx.Buffer.Append(ctx.Value1Decimal * 100m / v);
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
        };



        class CalcContextMaeda : ICalcContext
        {
            public CalcBuffer Buffer { get; } = new CalcBuffer();

            public string Value1;
            public decimal Value1Decimal => decimal.Parse(Value1);

            public IOperator Operator;

            public decimal Value2Decimal => decimal.Parse(Buffer.ToString());

            public string DisplayText => Buffer.IsNull ? null : string.Format("{0:#,0.############}", Buffer.IsEmpty ? Value1Decimal: Value2Decimal);

            public string SubDisplayText => Operator!=null ? $"{Value1} {(Operator as ArithmeticOperator)?.Label}" : null;

            internal void AppendNum(string num)
            {
                Buffer.Append(num);
                if (Operator == null) Value1 = null;
            }

        }

        /// <summary>
        /// コンテキストの新規生成
        /// </summary>
        /// <returns></returns>
        public virtual ICalcContext CreateContext() => new CalcContextMaeda();

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
        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContextMaeda;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            if (OperatorTable.ContainsKey(btn))
            {
                OperatorTable[btn].Exec(ctx);
            }
            else
            {
                ctx.AppendNum(btn == CalcButton.BtnDot ? "." : $"{(int)btn}");
            }
        }

    }
}
