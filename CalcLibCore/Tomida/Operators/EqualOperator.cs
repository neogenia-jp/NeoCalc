using System;
namespace CalcLibCore.Tomida.Operators
{
	public class EqualOperator : ICalcOperator
	{
		public EqualOperator()
		{
		}

        public void Calclate(CalcContextTomida ctx)
        {
            var command = CalcConstants.OperatorCommandDic[ctx.oper.Value];
            command.Calclate(ctx);
            ctx.SubDisplayQueue.Clear();
        }
    }
}

