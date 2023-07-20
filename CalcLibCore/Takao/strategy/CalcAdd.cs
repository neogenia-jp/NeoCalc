namespace CalcLib.Takao
{
    internal class CalcAdd : ICalcStrategy
    {
        public void Execute(CalcContext ctx)
        {
            if (ctx.right.Equals("0")) ctx.right = ctx.right_memo;
            var (left, right) = ctx.ParseOperand();
            ctx.left = Decimal.Add(left, right).ToString();
            ctx.right = "0";
        }
    }
}
