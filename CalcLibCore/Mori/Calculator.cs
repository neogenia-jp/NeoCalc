namespace CalcLib
{

    internal class CalcContextExtend : CalcContext 
    {
        // 電卓の状態を全体的に管理するクラス
        // stateパターン
        internal IState State { get; set; }
        // 計算のロジックを管理するクラス
        internal ICalculationStrategy? Strategy { get; set; }
        // 現在の値
        internal decimal CurrentValue { get; set; }
        // 左辺の値
        internal decimal Operand { get; set; }
        // 左辺の演算子
        internal string OperatorString { get; set; } = "";

        // コンストラクタ
        public CalcContextExtend()
        {
            State = NewNumberState.GetInstance();
            Strategy = new NoneStrategy(); //　初期状態の計算しないストラテジー
            CurrentValue = 0;
            Operand = 0;
            OperatorString = "";
            DisplayText = "0";
            SubDisplayText = "";
        }
        
        // 入力を受け付ける
        public void ProcessInput(CalcButton btn)
        {
            State.AcceptInput(this, btn);
        }

        // 数字桁数を増やす
        // public void AppendNumber(CalcButton btn)
        // {
                //各Stateに移動
        // }

        // 計算完了後のリセット
        public void Reset()
        {
            CurrentValue = 0;
            Operand = 0;
            Strategy = new NoneStrategy();
            OperatorString = "";
            DisplayText = "0";
            SubDisplayText = "";
            State = NewNumberState.GetInstance();
        }

        internal void UpdateDisplay(decimal value)
        {
            // 小数点以下の桁数を制限する
            DisplayText = value.ToString("0.#############");
        }
        // 演算子ボタンの文字列を取得
        // 電卓表示部用の演算子の文字列を取得する
        // nullを渡すと現在のストラテジーから表示する演算子文字列を取得する
        internal string GetOperatorString(CalcButton? btn)
        {
            var targetBtn = btn ?? GetButtonFromStrategy();
            return targetBtn?.ToOperatorString() ?? "";
        }

        // 演算子ボタンを取得
        private CalcButton? GetButtonFromStrategy()
        {
            if (Strategy is AdditionStrategy) return CalcButton.BtnPlus;
            if (Strategy is SubtractionStrategy) return CalcButton.BtnMinus;
            if (Strategy is MultiplicationStrategy) return CalcButton.BtnMultiple;
            if (Strategy is DivisionStrategy) return CalcButton.BtnDivide;
            return null;
        }



        // 演算子ボタンによってストラテジーを変更する
        public ICalculationStrategy ChangeStrategy(CalcButton btn) => btn switch
        {
            CalcButton.BtnPlus => new AdditionStrategy(),
            CalcButton.BtnMinus => new SubtractionStrategy(),
            CalcButton.BtnMultiple => new MultiplicationStrategy(),
            CalcButton.BtnDivide => new DivisionStrategy(),
        };
    }

    /// <summary>
    /// 状態インターフェース
    /// </summary>
    internal interface IState
    {
        void AcceptInput(CalcContextExtend context, CalcButton btn);
    }

    internal class NewNumberState : IState
    {
        private static readonly IState singleton = new NewNumberState();
        private NewNumberState() { }
        public static IState GetInstance()
        {
            return singleton;
        }

        public void AcceptInput(CalcContextExtend context, CalcButton btn)
        {
            // 数字ボタンの場合 数字を追加してすぐNumberStateに遷移
            if (btn.IsNumber())
            {
                context.DisplayText = btn.ToNumberString();
                context.CurrentValue = decimal.Parse(context.DisplayText);
                context.State = NumberState.GetInstance(); // 数字入力状態に遷移
            }
            // 演算子ボタン
            else if (btn.IsOperator())
            {
                context.Operand = context.CurrentValue;
                context.Strategy = context.ChangeStrategy(btn);
                context.OperatorString = context.GetOperatorString(btn); // 演算子文字列を保存
                context.SubDisplayText = $"{context.CurrentValue} {context.OperatorString}";
                context.State = OperatorState.GetInstance(); // 演算子入力状態に遷移
            }
            // イコールボタン
            else if (btn.IsEqual())
            {
                // なにもしない
            }
            // クリアボタン
            else if (btn.IsClear())
            {
                context.Reset();
            }
        }        
    }
    /// <summary>
    /// 数字入力状態
    /// </summary>
    internal class NumberState : IState
    {
        private static IState singleton = new NumberState();
        private NumberState() { }
        public static IState GetInstance()
        {
            return singleton;
        }

        public void AcceptInput(CalcContextExtend context, CalcButton btn)
        {
            // 数字ボタンの場合ただ数字を追加する
            if (btn.IsNumber())
            {
                context.DisplayText += btn.ToNumberString();
                context.CurrentValue = decimal.Parse(context.DisplayText);
                context.State = NumberState.GetInstance(); // 数字入力状態に遷移
            }
            // 演算子ボタン
            else if (btn.IsOperator())
            {
                // まだ計算が始まっていない（NoneStrategy）場合は計算を実行せずにそのまま演算子を設定する
                if (context.Strategy is NoneStrategy)
                {
                    context.Operand = context.CurrentValue;
                    context.Strategy = context.ChangeStrategy(btn);
                    context.OperatorString = context.GetOperatorString(btn); // 演算子文字列保存
                    context.SubDisplayText = $"{context.Operand} {context.OperatorString}";
                    context.State = OperatorState.GetInstance(); // 演算子入力状態に遷移
                    return;
                }

                // 計算中の場合の演算子はまず計算を実行する
                var rightOperand = context.CurrentValue;
                
                // 既存のSubDisplayTextから計算履歴を構築
                var tempExpression = context.SubDisplayText;
                
                context.CurrentValue = context.Strategy.Execute(context.Operand, context.CurrentValue);
                context.UpdateDisplay(context.CurrentValue);

                context.Operand = context.CurrentValue;
                context.Strategy = context.ChangeStrategy(btn);
                context.OperatorString = context.GetOperatorString(btn); // 新しい演算子を保存
                
                // 計算履歴を保持しながら新しい演算子を追加
                context.SubDisplayText = $"{tempExpression} {rightOperand} {context.OperatorString}";
                context.State = OperatorState.GetInstance(); // 演算子入力状態に遷移
            }
            // イコールボタン
            else if (btn.IsEqual())
            {
                // 計算中の場合のイコールはまず計算を実行する
                var rightOperand = context.CurrentValue;
                context.CurrentValue = context.Strategy.Execute(context.Operand, context.CurrentValue);
                context.UpdateDisplay(context.CurrentValue);
                // 計算のサマリーを表示
                context.SubDisplayText = $"{context.Operand} {context.OperatorString} {rightOperand} =";
                context.Operand = rightOperand; // イコールの繰り返し計算のために右辺を保存
                context.State = EqualState.GetInstance();
            }
            // クリアボタン
            else if (btn.IsClear())
            {
                context.Reset();
            }
        }
    }

    /// <summary>
    /// 演算子入力状態
    /// </summary>
    internal class OperatorState : IState
    {
        private static IState singleton = new OperatorState();
        private OperatorState() { }
        public static IState GetInstance()
        {
            return singleton;
        }
        
        public void AcceptInput(CalcContextExtend context, CalcButton btn)
        {
            // 数字ボタンの場合は数字入力状態に戻す
            if (btn.IsNumber())
            {
                context.DisplayText = btn.ToNumberString();
                context.CurrentValue = decimal.Parse(context.DisplayText);
                context.State = NumberState.GetInstance();
            }
            // 続けて演算子ボタンが押されると置き換える
            else if (btn.IsOperator())
            {
                context.Strategy = context.ChangeStrategy(btn);
                context.OperatorString = context.GetOperatorString(btn); // 演算子文字列を更新
                context.SubDisplayText = $"{context.Operand} {context.OperatorString}";
            }
            // イコールボタンは計算を実行する
            else if (btn.IsEqual())
            {
                var rightOperand = context.CurrentValue;
                context.CurrentValue = context.Strategy.Execute(context.Operand, context.CurrentValue);
                context.DisplayText = context.CurrentValue.ToString();
                // 現在のストラテジーを適用して計算結果を表示する
                context.SubDisplayText = $"{context.Operand} {context.GetOperatorString(null)} {rightOperand} =";
                context.Operand = rightOperand; // イコールの繰り返し計算のために右辺を保存
                context.State = EqualState.GetInstance(); // イコール入力状態に遷移
            }
            // クリアボタン
            else if (btn.IsClear())
            {
                context.Reset();
            }
        }
    }

    /// <summary>
    /// イコール入力状態
    /// </summary>
    internal class EqualState : IState
    {
        private static IState singleton = new EqualState();
        private EqualState() { }
        public static IState GetInstance()
        {
            return singleton;
        }

        public void AcceptInput(CalcContextExtend context, CalcButton btn)
        {
            // 数字ボタンの場合は数字入力状態に戻す
            if (btn.IsNumber())
            {
                context.Reset();
                context.State.AcceptInput(context, btn);
            }
            // イコールのあとに演算子がくると計算結果が左辺になり演算子ストラテジーを変更する
            else if (btn.IsOperator())
            {
                context.Operand = context.CurrentValue;
                context.Strategy = context.ChangeStrategy(btn); // 演算子を変更する
                context.OperatorString = context.GetOperatorString(btn); // 演算子文字列を保存
                context.SubDisplayText = $"{context.CurrentValue} {context.OperatorString}";
                context.State = OperatorState.GetInstance(); // 演算子入力状態に遷移
            }
            // イコールボタンは同じ計算を再実行する
            else if (btn.IsEqual())
            {
                var leftOperand = context.CurrentValue;
                context.CurrentValue = context.Strategy.Execute(context.CurrentValue, context.Operand);
                context.DisplayText = context.CurrentValue.ToString();
                context.SubDisplayText = $"{leftOperand} {context.OperatorString} {context.Operand} =";
            }
            // クリアボタン
            else if (btn.IsClear())
            {
                context.Reset();
            }
        }
    }


    // 計算のロジックを管理するクラス
    internal interface ICalculationStrategy
    {
        decimal Execute(decimal operand1, decimal operand2);
    }

    internal class NoneStrategy : ICalculationStrategy
    {
        public decimal Execute(decimal operand1, decimal operand2)
        {
            return operand2;
        }
    }
    // 足し算
    internal class AdditionStrategy : ICalculationStrategy
    {
        public decimal Execute(decimal operand1, decimal operand2)
        {
            return operand1 + operand2;
        }
    }

    // 引き算
    internal class SubtractionStrategy : ICalculationStrategy
    {
        public decimal Execute(decimal operand1, decimal operand2)
        {
            return operand1 - operand2;
        }
    }

    // 掛け算
    internal class MultiplicationStrategy : ICalculationStrategy
    {
        public decimal Execute(decimal operand1, decimal operand2)
        {
            return operand1 * operand2;
        }
    }

    // 割り算
    internal class DivisionStrategy : ICalculationStrategy
    {
        public decimal Execute(decimal operand1, decimal operand2)
        {
            if (operand2 == 0m) return 0m; // mはdecimalのリテラル
            return operand1 / operand2;
        }
    }


}