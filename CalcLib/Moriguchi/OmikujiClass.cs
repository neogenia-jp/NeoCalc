using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    class OmikujiClass : ISubSvc
    {
        public bool OnClick(ICalcContext ctx, CalcButton btn) => OmikujiMethod(btn,(CalcSvcMoriguchi.CalcContextMoriguchi)ctx);

        public void Init(ICalcContext ctx0)
        {
            var ctx = ctx0 as CalcSvcMoriguchi.CalcContextMoriguchi;
            //おみくじモードボタン押下時
            ctx.Mode = true;
            ctx.Value = "おみくじを選択して下さい";
            ctx.Operation = CalcButton.BtnExt2;
            ctx.Buffer = "[1 ] [2 ] [3 ] [4 ]";
        }

        //TODO:Contextの中に「おみくじモード」終了のフラグが必要

        /// <summary>
        /// おみくじモード時の動作
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="ctx"></param>
        private bool OmikujiMethod(CalcButton btn, CalcSvcMoriguchi.CalcContextMoriguchi ctx)
        {
            //押下ボタン判定
            switch (btn)
            {
                case CalcButton.Btn1:
                case CalcButton.Btn2:
                case CalcButton.Btn3:
                case CalcButton.Btn4:
                    OpenOmikuji(btn, ctx);
                    return false;

                //電卓モードへ戻る時
                case CalcButton.BtnClear:
                case CalcButton.BtnClearEnd:
                //case CalcButton.BtnExt2:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// おみくじの開示
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="ctx"></param>
        private void OpenOmikuji(CalcButton btn, CalcSvcMoriguchi.CalcContextMoriguchi ctx)
        {
            //おみくじ配列のシャッフル
            var test = ctx.omikuji.OrderBy(x => Guid.NewGuid()).ToArray();

            //開示表示
            ctx.Value = $"本日の運勢は「{test[(int)btn - 1]}」です";
            ctx.Buffer = null;
            foreach (var kekka in test.Select(x => x))
            {
                ctx.Buffer += kekka + " ";
            };
            UtlClass.Chomp(ctx);
        }
    }
}
