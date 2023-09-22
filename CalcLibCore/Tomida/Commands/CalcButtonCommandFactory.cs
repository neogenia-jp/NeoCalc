using System;
using CalcLib;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace CalcLibCore.Tomida.Commands
{
    public class CalcButtonCommandFactory : ButtonCommandFactoryBase
    {
        /// <summary>
        /// 引数のbtnに対応するCommandインスタンスを返します。
        /// </summary>
        /// <param name="btn">クリックされたボタン</param>
        /// <returns>ボタンに対応するCommandインスタンス</returns>
        /// <exception cref="ArgumentException">対応するボタンがない場合</exception>
        public override ButtonCommandBase? Create(CalcButton btn) => base.Create<ICalcCommand>(btn);
    }
}

