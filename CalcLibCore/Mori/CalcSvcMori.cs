using System.Diagnostics;
using CalcLib.Mori;

namespace CalcLib
{
    internal class CalcSvcMori : ICalcSvcEx
    {
        public virtual ICalcContext CreateContext()
        {
            var ctx = new CalcContextExtend();
            ctx.Attach(new DisplayObserver(ctx));
            return ctx;
        }

        public string GetExtButtonText(int num)
        {
            return num switch
            {
                1 => "omikuji",
                2 => "Ex2",
                _ => null
            };
        }

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
