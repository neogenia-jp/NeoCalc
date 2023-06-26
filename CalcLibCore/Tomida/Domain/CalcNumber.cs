using System;
namespace CalcLibCore.Tomida.Domain
{
	public class CalcNumber
	{
		const int DECIMAL_PLACES = 13;
		public static CalcNumber Empty = new CalcNumber(string.Empty);

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

		/// <summary>
		/// 末尾を1文字分削除したCaclNumberを返します。
		/// 末尾が削除できない場合はthisを返します。
		/// </summary>
		/// <returns></returns>
		public CalcNumber Backward()
		{
			return _value.Length == 0 ? this : new CalcNumber(_value.Substring(0, _value.Length - 1));
		}

		public override string ToString()
		{
			return _value;
		}

		// メインディスプレイ用の表示形式
		public string ToDisplayString()
		{
			/**
			 * NOTE:
			 * - bufferが小数点から始まっていたら"0"を補完する
			 * - 整数部はカンマ区切りで表示する
			 * - 小数部に入力されている"0"は消してはいけない→単純にFormatするだけではいけない
			 * - 整数部と小数部に分けてフォーマットする必要あり。
			 */
			string result = _value;
			// emptyの場合は"0"を返す
			if(result == string.Empty)
			{
				return "0";
			}
			// 入力が"."始まりだったら"0"を先頭に補完する
			if (_value.StartsWith("."))
			{
				result = "0" + _value;
			}
			// 整数位にカンマをつける
			var splits = result.Split(".");
			if(splits.Count() == 1)
			{
				// 整数位のみ
				result = Decimal.Parse(splits[0]).ToString("##,0");
            }
			else
			{
				// 小数点もある
				result = $"{Decimal.Parse(splits[0]).ToString("##,0")}.{splits[1]}";
			}

            return result;
		}

		// サブディスプレイ用の表示形式
		public string ToSubDisplayString()
		{
			try
			{
				return Parse(ToDecimal()).ToString();
			}
			catch (Exception ex)
			{
				return string.Empty;
			}
		}

		public Decimal ToDecimal()
		{
			return Decimal.Parse(_value == string.Empty ? "0" : _value);
		}

		/// <summary>
		/// Decimal型の数値をCalcNumberに変換します。
		/// CalcNumberの有効桁数を考慮します。
		/// </summary>
		/// <param name="d">Decimal型の数値</param>
		/// <returns>CalcNumber</returns>
		public static CalcNumber Parse(decimal d)
		{
			string format = "##,0." + String.Join("", Enumerable.Range(0, DECIMAL_PLACES).Select(i => "#"));
			return new CalcNumber(d.ToString(format));
		}

	}
}

