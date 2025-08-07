using CalcLib;

namespace CalcLibCore.Tomida2.Calc.Strategy
{
    internal interface IButtonStrategy
    {
        void OnButtonClick(CalcContextTomida2 ctx, CalcButton btn);
    }
}