using System;
using System.Runtime.CompilerServices;

namespace CalcLib.Yamamoto3
{
	internal class CalcContextYamamoto3 : CalcContext
	{
		public IState State { get; set; } = new InitState();
		public string LeftSide { get; set; } = string.Empty;
		public string RightSide { get; set; } = string.Empty;
		public CalcButton? Operator { get; set; } = null;

		// TODO:State系のクラスは別ファイルに切り出し
		public interface IState
		{
			void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn);
			void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn);
			void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn);
		}

		private class InitState : IState
		{
			public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				if (ctx.LeftSide.Length < 10)
				{
					// TODO: Enumに拡張メソッド定義して1とか2とか返すようにしたい
					ctx.LeftSide += btn.ToString().Replace("Btn", string.Empty);
					ctx.DisplayText += btn.ToString().Replace("Btn", string.Empty);
				}
				ctx.State = new LeftSideState();
			}

			public void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				// 特に何もしない
			}

			public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				// 特に何もしない
			}
		}

		private class LeftSideState : IState
		{
			public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				if (ctx.LeftSide.Length < 10)
				{
					ctx.LeftSide += btn.ToString().Replace("Btn", string.Empty);
					ctx.DisplayText += btn.ToString().Replace("Btn", string.Empty);
				}
				ctx.State = new LeftSideState();
			}

			public void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				// subdisplayにleftを渡して、right側の入力を待つ
				ctx.Operator = btn;
				ctx.SubDisplayText = ctx.LeftSide;
				ctx.DisplayText = "";
				ctx.State = new OperatorState();
			}

			public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				// 特に何もしない
			}
		}

		public class OperatorState : IState
		{
			public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				if (ctx.RightSide.Length < 10)
				{
					ctx.RightSide += btn.ToString().Replace("Btn", string.Empty);
				}
				ctx.State = new RightSideState();
			}

			public void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				// subdisplayにleftを渡して、right側の入力を待つ
				ctx.Operator = btn;
				ctx.SubDisplayText = ctx.LeftSide;
				ctx.DisplayText = "";
				ctx.State = new RightSideState();
			}

			public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				// 特に何もしない
			}
		}

		private class RightSideState : IState
		{
			public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				if (ctx.RightSide.Length < 10)
				{
					ctx.RightSide += btn.ToString().Replace("Btn", string.Empty);
				}
				ctx.State = new RightSideState();
			}

			public void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				// まだ右側の入力がない場合はOperatorが変更されたとして、OperatorStateに戻る
				if (string.IsNullOrEmpty(ctx.RightSide))
				{
					ctx.Operator = btn;
					ctx.State = new OperatorState();
					return;
				}

				// 左側の入力とOperatorと右側の入力で計算を行い、SubDisplayに結果を表示
				// RightSideStateに戻る
				ctx.State = new OperatorState();
			}

			public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				// TODO:
				// ctx.Operator.Execute(left, right); みたいな感じでできると、条件分岐なくなる
				// 左側の入力とOperatorと右側の入力で計算を行い、MainDisplayに結果を表示
				if(ctx.Operator == CalcButton.BtnPlus)
				{
					ctx.DisplayText = (int.Parse(ctx.LeftSide) + int.Parse(ctx.RightSide)).ToString();
				}
				else if(ctx.Operator == CalcButton.BtnMinus)
				{
					ctx.DisplayText = (int.Parse(ctx.LeftSide) - int.Parse(ctx.RightSide)).ToString();
				}
				else if(ctx.Operator == CalcButton.BtnMultiple)
				{
					ctx.DisplayText = (int.Parse(ctx.LeftSide) * int.Parse(ctx.RightSide)).ToString();
				}
				else if(ctx.Operator == CalcButton.BtnDivide)
				{
					if(int.TryParse(ctx.RightSide, out var right) && right != 0)
					{
						ctx.DisplayText = (int.Parse(ctx.LeftSide) / right).ToString();
					}
					else
					{
						ctx.DisplayText = "Error";
					}
				}
				// SubDisplayをクリア
				ctx.State = new AnswerState();
			}
		}

		public class AnswerState : IState
		{
			public void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				// MainDisplayに数字を入力
				if (ctx.LeftSide.Length < 10)
				{
					ctx.LeftSide += btn.ToString().Replace("Btn", string.Empty);
				}
				ctx.State = new LeftSideState();
			}

			public void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				// MainDisplayの内容を左辺およびSubDisplayにセット
				ctx.LeftSide = ctx.DisplayText;
				ctx.SubDisplayText = ctx.DisplayText;
				ctx.State = new OperatorState();
			}

			public void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn)
			{
				// 特になし
			}
		}
    }
}

