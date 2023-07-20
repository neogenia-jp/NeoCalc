namespace CalcLib.Takao
{
    internal static class CalcStrategyMap
    {
        public static Dictionary<CalcButton, ICalcStrategy> OperatorMap = new Dictionary<CalcButton, ICalcStrategy>()
        {
            { CalcButton.BtnPlus, new CalcAdd() },
            { CalcButton.BtnMinus, new CalcSubstract() },
            { CalcButton.BtnMultiple, new CalcMltiply() },
            { CalcButton.BtnDivide, new CalcDivide() },
        };
    }
};
