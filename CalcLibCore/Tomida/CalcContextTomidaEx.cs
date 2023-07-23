using System;
using CalcLib;

namespace CalcLibCore.Tomida
{
	/// <summary>
	/// コンテキストをスタックできるメディエーターコンテキスト
	/// 現在のコンテキストは Current プロパティで呼び出す。
	/// </summary>
	public class CalcContextTomidaEx: ICalcContext
	{
		private List<ICalcContext> _contextStack = new();

		public CalcContextTomidaEx()
		{
			_contextStack.Add(new CalcContextTomida());
		}

		/// <summary>
		/// 現在スタック上で一番上に存在するコンテキストを取得する
		/// </summary>
		public ICalcContext Current => _contextStack.Last();

		/// <summary>
		/// 新しいコンテキストスタックを積む
		/// </summary>
		/// <param name="ctx"></param>
		public void StackContext(ICalcContext ctx) => _contextStack.Add(ctx);

		/// <summary>
		/// Currentコンテキストを除去する
		/// </summary>
		public void UnstackContext() => _contextStack.Remove(Current);

		/// <summary>
		/// DisplayTextはCurrentコンテキストに委譲する
		/// </summary>
		public string DisplayText => Current.DisplayText;

		/// <summary>
		/// SubDisplayTextはCurrentコンテキストに委譲する
		/// </summary>
		public string SubDisplayText => Current.SubDisplayText;
    }
}

