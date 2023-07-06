using System;
namespace CalcLibCore.Tomida.Extensions
{
	public static class Extensions
	{
		/// <summary>
		/// 拡張メソッド。対象のボタンに対応するディスプレイ用文字列表現を取得します。
		/// </summary>
		/// <param name="btn"></param>
		/// <returns></returns>
		public static string ToDisplayString(this CalcLib.CalcButton btn)
		{
			return CalcConstants.DisplayStringDic[btn];
		}
	}
}

