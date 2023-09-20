namespace CalcLib.Takao
{
    internal class CalclatorSvc
    {

        /// <summary>
        /// ボタンごとの振り分け担当
        /// </summary>
        public void HandleInput(CalcContext ctx, CalcButton btn)
        {
            if (CalcButtonMap.NumberMap.ContainsKey(btn))
            {
                HandleNumber(ctx, CalcButtonMap.Map[btn]);
            }
            else if (CalcButtonMap.OperatorMap.ContainsKey(btn))
            {
                HandleOperator(ctx, CalcButtonMap.Map[btn]);
            }
            else if (CalcButtonMap.Map[btn].Equals("="))
            {
                HandleEqual(ctx);
            }
            else if (CalcButtonMap.Map[btn].Equals("clear"))
            {
                HandleClear(ctx);
            }
            else
            {
            }
        }

        // 数字が入力されたときの処理
        public void HandleNumber(CalcContext ctx, string num)
        {
            // Equalを押した後は計算を１から行うため初期化
            if (ctx.state == CalcContext.State.Equal)
            {
                HandleClear(ctx);
            }

            // operatorが入力された後は２つ目のoperandを入力したいため0をpush
            if (ctx.operatorMode != null && ctx.digits.Count < 2)
            {
                ctx.state = CalcContext.State.Second;
                ctx.digits.Push("0");
            }

            ctx.digits.Push(ctx.digits.Pop() + num);
        }

        // operatorが入力されたときの処理
        public void HandleOperator(CalcContext ctx, string op)
        {
            ctx.operatorMode = StrategyFactory.Create(op.ToString());
            if (ctx.digits.Count > 1)
            {
                Execute(ctx);
            }
        }

        // クリアが入力されたときの処理
        public void HandleClear(CalcContext ctx)
        {
            ctx.digits.Clear();
            ctx.digits.Push("0");
            ctx.operatorMode = null;
            ctx.state = CalcContext.State.First;
        }

        // =が入力されたときの処理
        public void HandleEqual(CalcContext ctx)
        {
            ctx.state = CalcContext.State.Equal;
            Execute(ctx);
        }

        // 計算の実行
        public void Execute(CalcContext ctx)
        {
            string? result = ctx.operatorMode?.Execute(ctx);

            if (result == null)
            {
                return;
            }

            ctx.digits.Push(result);
        }
    }
}
