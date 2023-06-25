using System;
namespace CalcLibCore.Tomida.Domain
{
	public class CalcNumber
	{
		const int DECIMAL_PLACES = 13;
		public static CalcNumber Empty = new CalcNumber("");

		private string _value = string.Empty;
		public CalcNumber(string value)
		{
			if (!_validate(value))
			{
				throw new ArgumentException("CalcNumberに適合しない文字列です。");
			}
			_value = value;
		}

		private bool _validate(string tryValue)
		{
			// 小数点チェック。文字列中に小数点が2個以上登場したら値として不適格
			if (tryValue.AsEnumerable().Count(c => c == '.') > 1)
			{
				return false;
			}
			// 小数点以下の桁数チェック
			if(tryValue.Take(tryValue.IndexOf(".")).Count() > DECIMAL_PLACES)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// 引数の文字列を追加して新しいCalcNumberオブジェクトを返します。
		/// CalcNumberとして不適切な文字列である場合は、thisがかえります。
		/// </summary>
		/// <param name="addString">末尾に追加したい文字列</param>
		/// <returns></returns>
		public CalcNumber Append(string addString)
		{
			string newString = this.ToString() + addString;
			if(_validate(newString))
			{
				return new CalcNumber(newString);
			}
			else
			{
				return this;
			}
		}

		public override string ToString()
		{
			return _value;
		}

		public Decimal ToDecimal()
		{
			return Decimal.Parse(_value);
		}

		/// <summary>
		/// Decimal型の数値をCalcNumberに変換します。
		/// CalcNumberの有効桁数を考慮します。
		/// </summary>
		/// <param name="d">Decimal型の数値</param>
		/// <returns>CalcNumber</returns>
		public static CalcNumber Parse(decimal d)
		{
			string format = "0." + String.Join("", Enumerable.Range(0, DECIMAL_PLACES).Select(i => "#"));
			return new CalcNumber(d.ToString(format));
		}

	}
}

