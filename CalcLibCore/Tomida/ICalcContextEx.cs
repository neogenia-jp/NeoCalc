using System;
using CalcLib;
using CalcLibCore.Tomida.Commands;

namespace CalcLibCore.Tomida
{
    public interface ICalcContextEx : ICalcContext
    {
        IFactory Factory { get; }
    }
}

