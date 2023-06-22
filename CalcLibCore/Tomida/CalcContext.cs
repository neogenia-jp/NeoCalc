using System;
using CalcLib;

namespace CalcLibCore.Tomida
{
    public class CalcContextTomida : ICalcContext
    {
        public Stack<string> OperandStack { get; set; } = new Stack<string>();
        public CalcButton? oper { get; set; } = null;

        public string buffer = string.Empty;

        public string calcratedExpression = string.Empty;

        public string DisplayText => DisplayTextImpl();

        public string SubDisplayText => SubDisplayTextImpl();

        public void Clear()
        {
            buffer = string.Empty;
            calcratedExpression = string.Empty;
            OperandStack.Clear();
            oper = null;
        }

        private string DisplayTextImpl()
        {
            string result = string.Empty;
            switch (GetState())
            {
                case CalcConstants.State.InputEqual:
                    var d = Decimal.Parse($"{OperandStack.ElementAt(0)}");
                    result = d.ToString("0.#############");
                    break;
                default:
                    result = buffer;
                    break;
            }
            return result;
        }
        private string SubDisplayTextImpl()
        {
            string result = string.Empty;
            switch (GetState())
            {
                case CalcConstants.State.InputLeft:
                    result = buffer;
                    break;
                case CalcConstants.State.InputOperator:
                    result = $"{OperandStack.ElementAt(0)} {CalcConstants.DisplayStringDic[oper.Value]}";
                    break;
                case CalcConstants.State.InputRight:
                    result = $"{OperandStack.ElementAt(0)} {CalcConstants.DisplayStringDic[oper.Value]} {buffer}";
                    break;
                case CalcConstants.State.InputEqual:
                    result = $"{calcratedExpression}";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return result;
        }
        public CalcConstants.State GetState()
        {
            if (oper == CalcButton.BtnEqual)
            {
                return CalcConstants.State.InputEqual;
            }
            else if (OperandStack.Count == 0)
            {
                return CalcConstants.State.InputLeft;
            }
            else if (OperandStack.Count == 1 && oper == null)
            {
                return CalcConstants.State.InputOperator;
            }
            else if (OperandStack.Count == 1 && oper != null)
            {
                return CalcConstants.State.InputRight;
            }
            else if (OperandStack.Count == 2 && oper != null)
            {
                return CalcConstants.State.InputComplete;
            }
            else
            {
                throw new InvalidOperationException("存在しない状態です");
            }
        }
    }
}

