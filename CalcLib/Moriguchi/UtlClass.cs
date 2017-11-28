using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    class UtlClass
    {
        //TODO:もっと汎用的に使える様に作ろう
        public static string Chomp(ISubContext ctx)
          => ctx.DisplayText = ctx.DisplayText.Remove(ctx.DisplayText.Length - 1);

        public class OpeNameHelper
        {
            static readonly Dictionary<CalcButton, string> OpeTextTable = new Dictionary<CalcButton, string>
            {
              { CalcButton.BtnPlus, "+" },
              { CalcButton.BtnMinus, "-" },
              { CalcButton.BtnMultiple, "×" },
              { CalcButton.BtnDivide, "÷"},
              { CalcButton.BtnExt2,""},
            };
            public static string Get(CalcButton? opeButton) => opeButton.HasValue ? OpeTextTable[opeButton.Value] : "";
        }
    }
}
