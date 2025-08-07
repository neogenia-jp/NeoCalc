using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalcLib.Mori
{
    // 計算機処理の中心になるクラス 旧CalcContext
    internal class Calculator
    {
        internal string Buffer { get; private set; } = ""; // 数値入力時に詰めていくバッファ
        internal IReadOnlyList<string> DisplayHistory => _displayHistory; // ディスプレイ表示用の履歴
        internal ICalcState State { get; private set; } = NewNumberState.GetInstance();
        
        private readonly Stack<ICalculable> _valueStack = new(); // 数値のスタック
        private readonly Stack<CalcButton> _operatorStack = new(); // 演算子のスタック
        private readonly List<string> _displayHistory = new();

        // イコール連続実行用の記憶
        private CalcButton? _lastOperator = null;
        private decimal? _lastRightOperand = null;
        
        // 表示用のテキストとモードを返す
        internal DisplaySource RowDisplay()
        {
            var mode = State switch
            {
                NumberState => UIMode.CalcInputting,
                _ => UIMode.CalcDefault
            };
            return new DisplaySource(Buffer, string.Join(" ", _displayHistory), mode);
        }

        // 起点のメソッド
        public void Accept(CalcButton btn)
        {
            State = State.AcceptInput(this, btn);
        }

        // 計算系の処理
        internal void StartNumber(CalcButton btn)
        {
            Buffer = (btn == CalcButton.BtnDot) ? "0." : btn.ToNumberString();
            ClearLastOperation();
        }

        internal void AppendNumber(CalcButton btn)
        {
            if (btn == CalcButton.BtnDot && Buffer.Contains('.')) return;
            Buffer += btn.ToNumberString();
        }

        internal void ConfirmNumber()
        {
            if (string.IsNullOrEmpty(Buffer)) return;
            _displayHistory.Add(Buffer);
            if (decimal.TryParse(Buffer, out var num))
            {
                _valueStack.Push(new ValueNode(num));
            }
        }

        // 演算子の処理
        internal void ProcessOperator(CalcButton op)
        {
            _displayHistory.Add(op.ToOperatorString());
            FixPending();
            _operatorStack.Push(op);
            ClearLastOperation();
        }

        // 最後の演算子を置き換える
        internal void ReplaceLastOperator(CalcButton op)
        {
            if (_displayHistory.Any())
            {
                _displayHistory[_displayHistory.Count - 1] = op.ToOperatorString();
            }
            if (_operatorStack.Any())
            {
                _operatorStack.Pop();
                _operatorStack.Push(op);
            }
        }

        internal void ProcessEqual()
        {
            if (_displayHistory.LastOrDefault() != "=")
            {
                _displayHistory.Add("=");
            }
            
            if (_lastOperator == null && _operatorStack.Count > 0 && _valueStack.Count >= 2)
            {
                _lastOperator = _operatorStack.Peek();
                _lastRightOperand = _valueStack.Peek().Evaluate();
            }
            
            if (_lastOperator != null && _lastRightOperand != null && _valueStack.Count == 1)
            {
                var currentValue = _valueStack.Peek().Evaluate();
                ICalculable node = _lastOperator switch
                {
                    CalcButton.BtnPlus => new AdditionNode(new ValueNode(currentValue), new ValueNode(_lastRightOperand.Value)),
                    CalcButton.BtnMinus => new SubtractionNode(new ValueNode(currentValue), new ValueNode(_lastRightOperand.Value)),
                    CalcButton.BtnMultiple => new MultiplicationNode(new ValueNode(currentValue), new ValueNode(_lastRightOperand.Value)),
                    CalcButton.BtnDivide => new DivisionNode(new ValueNode(currentValue), new ValueNode(_lastRightOperand.Value)),
                    _ => new ValueNode(currentValue)
                };
                var result = node.Evaluate();
                Buffer = result.ToString();
                _valueStack.Clear();
                _valueStack.Push(new ValueNode(result));
                
                _displayHistory.Clear();
                _displayHistory.Add(currentValue.ToString("0.#############"));
                _displayHistory.Add(_lastOperator.Value.ToOperatorString());
                _displayHistory.Add(_lastRightOperand.Value.ToString("0.#############"));
                _displayHistory.Add("=");
                return;
            }
            
            FixPending();
            while (_operatorStack.Count > 0 && _valueStack.Count >= 2)
            {
                CreateExpressionNode();
            }
            if (_valueStack.TryPop(out var root))
            {
                var res = Math.Round(root.Evaluate(), 13);
                Buffer = res.ToString();

                _operatorStack.Clear();
                _valueStack.Clear();
                _valueStack.Push(new ValueNode(res));
                _displayHistory.Clear();
            }
        }

        internal void StartResultAsLeftOperand()
        {
            _displayHistory.Clear();
            _displayHistory.Add(Buffer);
        }

        internal void Backspace()
        {
            if (Buffer.Length > 0)
            {
                Buffer = Buffer[..^1];
            }
            if (Buffer.Length == 0)
            {
                Buffer = "0";
            }
        }

        internal void ClearEntry()
        {
            Buffer = "0";
        }

        internal void Reset()
        {
            _displayHistory.Clear();
            Buffer = "";
            _valueStack.Clear();
            _operatorStack.Clear();
            ClearLastOperation();
            State = NewNumberState.GetInstance();
        }
        
        private void ClearLastOperation()
        {
            _lastOperator = null;
            _lastRightOperand = null;
        }

        private void FixPending()
        {
            if (_operatorStack.Count < 1 || _valueStack.Count < 2) return;
            CreateExpressionNode();
            var node = _valueStack.Peek();
            var result = node.Evaluate();
            _valueStack.Pop();
            _valueStack.Push(new ValueNode(result));
            Buffer = result.ToString(); // 中途式の処理はバッファも更新する
        }

        private void CreateExpressionNode()
        {
            if (_operatorStack.Count == 0 || _valueStack.Count < 2) return;

            var op = _operatorStack.Pop();
            var right = _valueStack.Pop();
            var left = _valueStack.Pop();

            ICalculable node = op switch
            {
                CalcButton.BtnPlus => new AdditionNode(left, right),
                CalcButton.BtnMinus => new SubtractionNode(left, right),
                CalcButton.BtnMultiple => new MultiplicationNode(left, right),
                CalcButton.BtnDivide => new DivisionNode(left, right),
                _ => throw new System.InvalidOperationException("Unknown operator")
            };

            _valueStack.Push(node);
        }
    }
}
