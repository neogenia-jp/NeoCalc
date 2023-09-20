namespace CalcLib.Takao
{
    internal class Substract : ICalcStrategy
    {
        public string Execute(CalcContext ctx)
        {
            var right = Decimal.Parse(ctx.digits.Pop());
            var left = Decimal.Parse(ctx.digits.Pop());
            return (left - right).ToString();
        }
    }
}
