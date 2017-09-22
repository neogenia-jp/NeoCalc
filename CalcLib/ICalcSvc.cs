namespace CalcLib
{
    public interface ICalcSvc
    {
        ICalcContext CreateContext();
        void OnButtonClick(ICalcContext ctx, CalcButton btn);
    }
}