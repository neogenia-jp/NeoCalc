using System;
namespace CalcLibCore.Tomida.Operators
{
	/// <summary>
	/// 演算子のコマンドインターフェース
	/// </summary>
	public interface ICalcOperator
	{
		public void Calclate(CalcContextTomida ctx);
	}
}

