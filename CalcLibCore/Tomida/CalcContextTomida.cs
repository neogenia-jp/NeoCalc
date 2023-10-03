using System;
using CalcLib;
using CalcLibCore.Tomida.Commands;
using CalcLibCore.Tomida.Domain;

namespace CalcLibCore.Tomida
{
    public class CalcContextTomida : ICalcContextEx
    {
        public Stack<CalcNumber> OperandStack { get; set; } = new Stack<CalcNumber>();
        public CalcButton? oper { get; set; } = null;
        public List<string> SubDisplayQueue { get; set; } = new List<string>();

        public CalcNumber buffer = CalcNumber.Empty;

        public string calcratedExpression = string.Empty;

        public string DisplayText => DisplayTextImpl();

        public string SubDisplayText => SubDisplayTextImpl();

        public CalcConstants.State State => GetState();

        /// <summary>
        /// 現在入力中の辺（右辺、左辺）が一度でも入力状態になったかどうかを判別する
        /// </summary>
        public bool isInputed = false;

        public IFactory Factory { get; }

        public ICalcContext? Parent { get; }

        public CalcContextTomida(ICalcContextEx parent)
        {
            Parent = parent;
            Factory = new CalcButtonCommandFactory();
        }

        /// <summary>
        /// コンテキストの内容を全て消去して初期状態に戻します。
        /// </summary>
        public void Clear()
        {
            buffer = CalcNumber.Empty;
            calcratedExpression = string.Empty;
            OperandStack.Clear();
            SubDisplayQueue.Clear();
            oper = null;
            isInputed = false;
        }

        private string DisplayTextImpl()
        {
            string result = string.Empty;
            switch (GetState())
            {
                case CalcConstants.State.InputEqual:
                    result = OperandStack.ElementAt(0).ToDisplayString();
                    // Equalを押した際に結果が実質ゼロの場合は"0"表記とする
                    if(Decimal.Parse(result) == Decimal.Zero)
                    {
                        result = "0";
                    }
                    break;
                case CalcConstants.State.InputOperator:
                    result = OperandStack.ElementAt(0).ToDisplayString();
                    break;
                default:
                    result = buffer.ToDisplayString();
                    break;
            }
            return result;
        }

        private string DisplayQueue()
        {
            return String.Join(" ", SubDisplayQueue.ToArray());
        }

        private string SubDisplayTextImpl()
        {
            string result = string.Empty;
            switch (GetState())
            {
                case CalcConstants.State.InputLeft:
                    //result = buffer.ToString();
                    result = String.Empty;
                    break;
                case CalcConstants.State.InputOperator:
                case CalcConstants.State.InputRight:
                    result = $"{DisplayQueue()}";
                    break;
                case CalcConstants.State.InputEqual:
                    result = string.Empty;
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
            else if (OperandStack.Count == 1 && oper != null && (buffer != CalcNumber.Empty || isInputed ))
            {
                return CalcConstants.State.InputRight;
            }
            else if (OperandStack.Count == 1 && oper != null && buffer == CalcNumber.Empty)
            {
                return CalcConstants.State.InputOperator;
            }
            else if (OperandStack.Count == 0)
            {
                return CalcConstants.State.InputLeft;
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

