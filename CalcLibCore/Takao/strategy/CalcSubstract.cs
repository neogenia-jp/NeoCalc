namespace CalcLib.Takao
{
    internal class CalcSubstract : ICalcStrategy
    {
        public void Execute(CalcContext ctx)
        {
            var (left, right) = ctx.ParseOperand();
            ctx.left = Decimal.Subtract(left, right).ToString();
            ctx.right = "0";
        }
    }
}
