using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    class StockAcquisitionSvc : ISubSvc
    {
        public class StockContext
        {
            //証券コード
            private string shokenCode;

        }


        //クリック時
        public bool OnClick(ICalcContext ctx0, CalcButton btn)
        {
            string code = btn.ToString();


            return GetStock(ctx0);
        }

        //初期動作
        public void Init(ICalcContext ctx0)
        {
            var ctx = ctx0 as CalcSvcMoriguchi.CalcContextMoriguchi;
            //株価取得モードボタン押下時
            ctx.Value = "証券コードを入力して下さい";
            ctx.Operation = CalcButton.BtnExt2;
            ctx.Buffer ="";
        }

        /// <summary>
        /// 証券コードから株価を取得する
        /// </summary>
        /// <param name="KabukaCode"></param>
        /// <returns></returns>
        public bool GetStock(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = (CalcSvcMoriguchi.CalcContextMoriguchi)ctx0;
            var code = (int)btn;

            if (code < 10)
            {
                shokenCode += code.ToString();

            }

            ctx.Buffer = "1000";



            return true;
        }
    }
}
