using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Takao
{
    internal class CalclatorSvc
    {
        public CalclatorSvc()
        {
        }

        public void HandleInput(CalcContext ctx, CalcButton btn)
        {
            // 数字っぽいボタンが押された場合
            // Operatorが押された場合
            // Equalが押された場合
            // 拡張機能ボタンが押された場合
            if (CalcButtonMap.NumberMap.ContainsKey(btn))
            {
                EnterNubmer(ctx, CalcButtonMap.Map[btn]);
            }
            else if (CalcButtonMap.OperatorMap.ContainsKey(btn))
            {
                ctx.SetOperator(CalcStrategyMap.OperatorMap[btn]);
                Console.WriteLine(ctx.operatorMode);
            }
            else if (CalcButtonMap.Map[btn].Equals("="))
            {
                HandleEqual(ctx);
            }
            else if (CalcButtonMap.Map[btn].Equals("clear"))
            {
                Clear(ctx);
            }
            else
            {
                // 何もしない
            }

            ctx.ApplyDisplayText();
        }

        public void EnterNubmer(CalcContext ctx, string num)
        {
            ctx.right += num;
        }

        public void Clear(CalcContext ctx)
        {
            ctx.Clear();
        }

        public void HandleEqual(CalcContext ctx)
        {
            ctx.operatorMode?.Execute(ctx);
        }
    }
}
