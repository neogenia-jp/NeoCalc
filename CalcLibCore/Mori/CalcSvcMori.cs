using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib
{
    internal class CalcSvcMori : ICalcSvc
    {

        // 電卓の表示部
        public virtual ICalcContext CreateContext() => new CalcContextExtend();


        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            if (ctx0 is not CalcContextExtend ctx)
            {
                Debug.WriteLine("Context is not CalcContext type");
                return;
            }

            // 表示部と入力を処理
            ctx.ProcessInput(btn);
        }
    }
}
