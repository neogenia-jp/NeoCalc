using System;
using CalcLib.Yamamoto2.Executors;

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

		public decimal? left;     // 左辺を入れておく
		public OperatorButtonExecutor ope;  // オペレータを入れておく

        public CalcContextYamamoto2()
		{
		}

        /// <summary>
        /// DisplayTextに対してdecimal型からセットする
        /// </summary>
        /// <remarks>
        /// decimal型に対して、ToStringすると、末尾の0がある状態で文字列になるため、
        /// そうならないようにセッターを用意した。
        /// </remarks>
        /// <param name="value"></param>
        public void SetDisplayText(decimal value)
        {
            var s = value.ToString();
            if (s.IndexOf('.') > -1)
            {
                // 小数点がある場合は末尾の0を削除する
                s = s.TrimEnd('0');
                if(s.Last() == '.')
                {
                    // 最後が点だった場合はそれも削除する
                    s = s.Remove(s.Length - 1, 1);
                }
            }
            DisplayText = s;
        }
    }
}

