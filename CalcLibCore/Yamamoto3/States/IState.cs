using System;
using CalcLib;
using CalcLib.Yamamoto3;

namespace CalcLib.Yamamoto3.States;

internal interface IState
{
    void InputNumber(CalcContextYamamoto3 ctx, CalcButton btn);
    void InputOperator(CalcContextYamamoto3 ctx, CalcButton btn);
    void InputEqual(CalcContextYamamoto3 ctx, CalcButton btn);
}
