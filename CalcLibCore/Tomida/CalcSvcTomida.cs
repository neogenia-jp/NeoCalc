using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcLibCore.Tomida;
using CalcLibCore.Tomida.Domain;
using CalcLibCore.Tomida.Operators;

namespace CalcLib.Tomida
{
    internal class CalcSvcTomida : ICalcSvc
    {
        public virtual ICalcContext CreateContext() => new CalcContextTomida();

        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContextTomida;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            // operがEqualの場合、クリア処理を行う
            if(ctx.oper == CalcButton.BtnEqual)
            {
                ctx.Clear();
            }

            var command = ButtonCommandFactory.Create(btn);
            command.Execute(ctx);

            /*
            if (CalcConstants.operators.Contains(btn))
            {
                ctx.OperandStack.Push(ctx.buffer);
                if(ctx.GetState() == CalcConstants.State.InputOperator)
                {
                    ctx.oper = btn;
                }else if(ctx.GetState() == CalcConstants.State.InputComplete)
                {
                    var command = CalcConstants.OperatorCommandDic[ctx.oper.Value];
                    command.Calclate(ctx);
                    ctx.oper = btn;
                }
                ctx.buffer = CalcNumber.Empty;
            }
            else if (CalcConstants.numbers.Contains(btn))
            {
                var command = ButtonCommandFactory.Create(btn);
                //ctx.buffer = ctx.buffer.Append(CalcConstants.DisplayStringDic[btn]);
                command.Execute(ctx);
            }
            else
            {
                throw new InvalidOperationException("不正なボタンです");
            }
            */
        }
    }
}
