using System;
using CalcLib;
using CalcLibCore.Tomida.Commands;

namespace CalcLibCore.Tomida
{
	/// <summary>
	/// コンテキストをスタックできるメディエーターコンテキスト
	/// 現在のコンテキストは Current プロパティで呼び出す。
	/// </summary>
	public class CalcContextTomidaEx: ICalcContextEx
	{
		private List<ICalcContextEx> _contextStack = new();

		public CalcContextTomidaEx()
		{
			_contextStack.Add(new CalcContextTomida());
		}

		/// <summary>
		/// 現在スタック上で一番上に存在するコンテキストを取得する
		/// </summary>
		public ICalcContextEx Current => _contextStack.Last();

		/// <summary>
		/// 新しいコンテキストスタックを積む
		/// </summary>
		/// <param name="ctx"></param>
		public void StackContext(ICalcContextEx ctx) => _contextStack.Add(ctx);

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

        public IFactory Factory => throw new NotImplementedException();
    }
}

