using System;

namespace CalcLib.Yamamoto2.Executors
{
	public class ExecutorFactory
	{
		private ExecutorFactory()
		{
		}

		internal static IExecutor Create(CalcContextYamamoto2 ctx, CalcButton btn)
		{
			switch (btn)
			{
				case CalcButton.Btn0:
				case CalcButton.Btn1:
				case CalcButton.Btn2:
				case CalcButton.Btn3:
				case CalcButton.Btn4:
				case CalcButton.Btn5:
				case CalcButton.Btn6:
				case CalcButton.Btn7:
				case CalcButton.Btn8:
				case CalcButton.Btn9:
					return new NumberButtonExecutor(ctx, btn);
				case CalcButton.BtnPlus:
					return new PlusButtonExecutor(ctx, btn);
				case CalcButton.BtnMinus:
                    return new MinusButtonExecutor(ctx, btn);
                case CalcButton.BtnMultiple:
                    return new MultipleButtonExecutor(ctx, btn);
                case CalcButton.BtnDivide:
                    return new DivideButtonExecutor(ctx, btn);
                case CalcButton.BtnEqual:
					return new EqualButtonExecutor(ctx, btn);
            }

            throw new NotImplementedException();
		}
    }
}

