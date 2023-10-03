namespace CalcLib.Takao
{
    internal class StrategyFactory
    {
        public static ICalcStrategy Create(string op)
        {
            switch (op)
            {
                case "+":
                    return new Add();
                case "-":
                    return new Substract();
                case "x":
                    return new Multiply();
                case "/":
                    return new Divide();
                default:
                    throw new Exception("Invalid operator");
            }
        }
    }
}
