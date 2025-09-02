using System.Diagnostics;
using CalcLib.Mori;
using CalcLib.Mori.Display;

namespace CalcLib
{
    internal class CalcSvcMori : ICalcSvcEx
    {
        public virtual ICalcContext CreateContext()
        {
            var ctx = new CalcContextExtend();
            var formatter = new DefaultDisplayFormatter();
            ctx.Attach(new DisplayObserver(ctx, formatter));
            return ctx;
        }

        public string GetExtButtonText(int num)
        {
            return num switch
            {
                2 => "omikuji",
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
