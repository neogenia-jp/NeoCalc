using System.Diagnostics;
using CalcLib.Mori;

namespace CalcLib
{
    internal class CalcSvcMori : ICalcSvc
    {
        public virtual ICalcContext CreateContext() => new CalcContextExtend();

        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            if (ctx0 is not CalcContextExtend ctx)
            {
                Debug.WriteLine("Context is not CalcContext type");
                return;
            }
            var cmd = ButtonCommandFactory.Create(btn);
            cmd.Execute(ctx);
        }
    }
}
