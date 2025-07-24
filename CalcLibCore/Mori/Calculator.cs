using System.Text.RegularExpressions;
namespace CalcLib.Mori
{
    internal class CalcContextExtend : CalcContext
    {
        string _buffer = ""; // 数値入力時に詰めていくバッファ
        readonly Stack<ICalculable> _valueStack = new(); // 数値のスタック
        readonly Stack<CalcButton> _operatorStack = new(); // 演算子のスタック
        private readonly List<string> _displayHistory = new(); // ディスプレイ表示用の履歴
        ICalcState _state = NewNumberState.GetInstance();
        
        // イコール連続実行用の記憶
        private CalcButton? _lastOperator = null;
        private decimal? _lastRightOperand = null;

        // サブ表示部のテキストを更新する
        private void UpdateSubDisplayText()
        {
            SubDisplayText = string.Join(" ", _displayHistory);
        }

        // ButtonCommand から呼ばれるメソッド
        public void Accept(CalcButton btn)
        {
            _state = _state.AcceptInput(this, btn);
            UpdateSubDisplayText();
        }

        // 最初の数値 .の入力を受け付ける
        internal void StartNumber(CalcButton btn)
        {
            _buffer = (btn == CalcButton.BtnDot) ? "0." : btn.ToNumberString();
            RefreshDisplay();
            ClearLastOperation();
        }

        // 数値入力状態で足していく
        internal void AppendNumber(CalcButton btn)
        {
            if (btn == CalcButton.BtnDot && _buffer.Contains('.')) return;
            _buffer += btn.ToNumberString();
            RefreshDisplay();
        }

        // 数値を確定する
        internal void ConfirmNumber()
        {
            if (string.IsNullOrEmpty(_buffer)) return;
            _displayHistory.Add(_buffer);
            if (decimal.TryParse(_buffer, out var num))
            {
                _valueStack.Push(new ValueNode(num));
            }
            _buffer = "";
        }

        // 演算子を処理する
        internal void ProcessOperator(CalcButton op)
        {
            _displayHistory.Add(op.ToOperatorString());
            FixPending();
            _operatorStack.Push(op);
            ClearLastOperation();
        }
        
        // 直前の演算子を置き換える　連続押し対応
        internal void ReplaceLastOperator(CalcButton op)
        {
            if (_displayHistory.Any())
            {
                // 表示用入れ替え
                _displayHistory[_displayHistory.Count - 1] = op.ToOperatorString();
            }
            if (_operatorStack.Any())
            {
                // 演算子スタックを入れ替え
                _operatorStack.Pop();
                _operatorStack.Push(op);
            }
        }

        // イコールを押した時の処理
        internal void ProcessEqual()
        {
            if (_displayHistory.LastOrDefault() != "=")
            {
                _displayHistory.Add("=");
            }
            
            // 初回のイコール処理
            if (_lastOperator == null && _operatorStack.Count > 0 && _valueStack.Count >= 2)
            {
                // 前回の演算子と右辺を記憶
                _lastOperator = _operatorStack.Peek();
                _lastRightOperand = _valueStack.Peek().Evaluate();
            }
            
            // 連続イコール処理
            if (_lastOperator != null && _lastRightOperand != null && _valueStack.Count == 1)
            {
                // 前回の演算子と右辺で計算を繰り返し
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
                DisplayText = result.ToString("0.#############");
                _buffer = result.ToString();
                _valueStack.Clear();
                _valueStack.Push(new ValueNode(result));
                
                // サブディスプレイに連続計算式を表示
                _displayHistory.Clear();
                _displayHistory.Add(currentValue.ToString("0.#############"));
                _displayHistory.Add(_lastOperator.Value.ToOperatorString());
                _displayHistory.Add(_lastRightOperand.Value.ToString("0.#############"));
                _displayHistory.Add("=");
                return;
            }
            
            // 通常の計算処理
            FixPending();
            while (_operatorStack.Count > 0 && _valueStack.Count >= 2)
            {
                CreateExpressionNode();
            }
            if (_valueStack.TryPop(out var root))
            {
                var res = root.Evaluate();
                DisplayText = res.ToString("0.#############");
                _buffer = res.ToString();
                _operatorStack.Clear();
                _valueStack.Clear();
                _valueStack.Push(new ValueNode(res));
                _displayHistory.Clear();
            }
        }

        // 結果を左辺として新しい計算を開始する
        internal void StartResultAsLeftOperand()
        {
            _displayHistory.Clear();
            _displayHistory.Add(_buffer);
        }

        internal void Backspace()
        {
            if (_buffer.Length > 0)
            {
                _buffer = _buffer[..^1]; // C# 8.0 以降の構文, _buffer.Substring(0, _buffer.Length - 1)相当
            }
            if (_buffer.Length == 0)
            {
                _buffer = "0";
            }
            RefreshDisplay();
        }

        internal void ClearEntry()
        {
            _buffer = "0";
            DisplayText = "0";
        }

        internal void Reset()
        {
            DisplayText = "0";
            _displayHistory.Clear();
            _buffer = "";
            _valueStack.Clear();
            _operatorStack.Clear();
            ClearLastOperation();
        }
        
        // イコール連続実行用の記憶をクリア
        private void ClearLastOperation()
        {
            _lastOperator = null;
            _lastRightOperand = null;
        }

        // 未計算の演算子を解消する
        private void FixPending()
        {
            // 演算子と数値が足りない場合は何もしない
            if (_operatorStack.Count < 1 || _valueStack.Count < 2) return;
            CreateExpressionNode();
            var node = _valueStack.Peek();
            var result = node.Evaluate();
            _valueStack.Pop();
            _valueStack.Push(new ValueNode(result));
            DisplayText = result.ToString("0.#############");
        }

        // 演算子と数値を組み合わせて計算式を作成する
        private void CreateExpressionNode()
        {
            // 演算子と数値が足りない場合は何もしない
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
        private void RefreshDisplay()
        {
            if (string.IsNullOrEmpty(_buffer))
            {
                DisplayText = "0";
                return;
            }

            // マイナス符号は分離
            var isMinus = _buffer.StartsWith("-");
            var num     = isMinus ? _buffer[1..] : _buffer;

            // 小数点で分離
            var leftPart    = num.Split('.', 2);
            var rightPart  = leftPart[0];
            var fracPart = leftPart.Length > 1 ? "." + leftPart[1] : "";

            // 整数部に千区切りカンマを挿入
            var withComma = Regex.Replace(
                rightPart,
                "(?<=\\d)(?=(\\d{3})+$)",   // 右側から3桁ごとに
                ",");

            DisplayText = (isMinus ? "-" : "") + withComma + fracPart;
        }
    }


}