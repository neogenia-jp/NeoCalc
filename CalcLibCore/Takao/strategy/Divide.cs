namespace CalcLib.Takao
{
    internal class Divide : ICalcStrategy
    {
        // 計算の実行
        public string Execute(CalcContext ctx)
        {
            var right = Decimal.Parse(ctx.digits.Pop());
            var left = Decimal.Parse(ctx.digits.Pop());
            return (left / right).ToString();
        }
    }
}
