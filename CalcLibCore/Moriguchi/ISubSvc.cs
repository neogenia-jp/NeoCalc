using System;

namespace CalcLib.Moriguchi
{
    public interface ISubSvc
    {
        ISubContext CreateContext();

        bool OnClick(ISubContext ctx, CalcButton btn);

        void Init(ISubContext ctx, ICalcContext prevSvc);
    }
}
