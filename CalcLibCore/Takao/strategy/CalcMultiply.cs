namespace CalcLib.Takao
{
    internal class CalcMltiply : ICalcStrategy
    {
        public void Execute(CalcContext ctx)
        {
            var (left, right) = ctx.ParseOperand();
            ctx.left = Decimal.Multiply(left, right).ToString();
            ctx.right = "0";
        }
    }
}
