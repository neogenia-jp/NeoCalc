using System;
namespace CalcLib.Yamamoto2
{
	internal class CalcContextYamamoto2 : CalcContext
	{
		public enum StateEnum {
            InputedNumber,
            InputedOperator,
            InputedEqual,
        }
        public StateEnum State { get; set; }

		public decimal? w1;
		public CalcButton ope;

		public CalcContextYamamoto2()
		{
		}
	}
}

