namespace CalcLib
{
    public interface ICalcSvc
    {
        ICalcContext CreateContext();

        void OnButtonClick(ICalcContext ctx, CalcButton btn);
    }

    public interface ICalcSvcEx : ICalcSvc
    {
        string GetExtButtonText(int num);  // 1～4の数字が渡される。nullを返すとそのボタンは無効となる。
    }
}