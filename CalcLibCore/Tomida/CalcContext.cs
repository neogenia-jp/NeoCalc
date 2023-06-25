using System;
using CalcLib;
using CalcLibCore.Tomida.Domain;

namespace CalcLibCore.Tomida
{
    public class CalcContextTomida : ICalcContext
    {
        public Stack<CalcNumber> OperandStack { get; set; } = new Stack<CalcNumber>();
        public CalcButton? oper { get; set; } = null;

        public CalcNumber buffer = CalcNumber.Empty;

        public string calcratedExpression = string.Empty;

        public string DisplayText => DisplayTextImpl();

        public string SubDisplayText => SubDisplayTextImpl();

        public void Clear()
        {
            buffer = CalcNumber.Empty;
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
                    result = OperandStack.ElementAt(0).ToString();
                    break;
                default:
                    result = buffer.ToString();
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
                    result = buffer.ToString();
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

