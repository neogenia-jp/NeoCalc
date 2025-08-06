// 電卓以外のextraモード用State

namespace CalcLib.Mori
{
internal interface IModeState
{
    IModeState Accept(CalcContextExtend context, CalcButton btn);
    DisplaySource RowDisplay();
}

internal class CalcMode : IModeState
{
    private static readonly IModeState singleton = new CalcMode();
    private CalcMode() { }
    public static IModeState GetInstance() => singleton;
    private readonly Calculator _calculator = new();

    public IModeState Accept(CalcContextExtend context, CalcButton btn)
    {
       if (btn.IsOmikuji())
       {
          return OmikujiState.GetInstance();
       }
       _calculator.Accept(btn);
       return this;
    }

    public DisplaySource RowDisplay()
    {
        return _calculator.RowDisplay();
    }
}

internal class OmikujiState : IModeState
{
    private readonly Omikuji _omikuji = new();
    private static readonly IModeState singleton = new OmikujiState();
    private OmikujiState() { }
    public static IModeState GetInstance() => singleton;

    public IModeState Accept(CalcContextExtend context, CalcButton btn)
    {
        if (btn.IsClear() || btn.IsCE())
        {
            return CalcMode.GetInstance();
        }
        _omikuji.Accept(context, btn);
        return this;
    }

    public DisplaySource RowDisplay()
    {
        return _omikuji.RowDisplay();
    }
}
}