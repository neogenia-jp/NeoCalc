using System;
using CalcLib;
using CalcLibCore.Tomida.Commands;

namespace CalcLibCore.Tomida
{
    public interface ICalcContextEx : ICalcContext
    {
        /// <summary>
        /// このコンテキストが持つFactoryインスタンス
        /// </summary>
        IFactory Factory { get; }
        /// <summary>
        /// 親コンテキスト
        /// </summary>
        ICalcContext Parent { get;}
    }
}

