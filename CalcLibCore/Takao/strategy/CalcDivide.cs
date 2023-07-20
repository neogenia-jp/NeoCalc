namespace CalcLib.Takao
{
    internal class CalcDivide : ICalcStrategy
    {
        public void Execute(CalcContext ctx)
        {
            var (left, right) = ctx.ParseOperand();
            ctx.left = Decimal.Divide(left, right).ToString();
            ctx.right = "0";
        }
    }
}
