using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Takao
{
    internal class CalcContext : ICalcContext
    {
        public string DisplayText { get; set; } = "0";
        public string SubDisplayText { get; set; } = "0";
        public string left = "0";
        public string right = "0";
        public string right_memo = "0";

        public ICalcStrategy? operatorMode;

        public override string ToString() => $"display: {DisplayText} subdispplay: {SubDisplayText}";

        // mainの操作
        public void ApplyDisplayText()
        {
            FormatOperand();
            DisplayText = (right == "0") ? left : right;
            debugContext();
        }

        public void FormatOperand()
        {
            right = Decimal.Parse(right).ToString();
            left = Decimal.Parse(left).ToString();
        }

        public (decimal, decimal) ParseOperand()
        {
            return (Decimal.Parse(left), Decimal.Parse(right));
        }

        public void SetOperator(ICalcStrategy mode)
        {
            operatorMode = mode;

            // 1+1+1+=3のために実行する。
            if (operatorMode != null)
            {
                right_memo = right;
                operatorMode.Execute(this);
            }
            else
            {
                left = right;
                right = "0";
            }
        }

        public void debugContext()
        {
            Console.WriteLine(ToString());
            Console.WriteLine($"left: {left}, right: {right}, right_memo: {right_memo}, operator: {operatorMode?.ToString()}");
        }

        // subの操作
        // public void ApplySubDisplayText(CalclatorSvc svc)
        // {
        //     SubDisplayText = "";
        //     // ButtonHistory.ForEach(btn => svc.ExecuteSub(btn));
        // }

        // display 初期化
        public void Clear()
        {
            left = "0";
            right = "0";
            DisplayText = "0";
            SubDisplayText = "0";
            operatorMode = null;
        }
    }
}


