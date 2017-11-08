using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto
{
    public class CalcSvcYamamoto : ICalcSvcEx
    {
        internal class CalcContextYamamoto : CalcContext
        {
            /// <summary>
            /// モード履歴
            /// </summary>
            public List<AppMode> ModeHistory { get; set; } = new List<AppMode>() { AppMode.Calculator };

            public CalcContextYamamoto()
            {
                ApplicationFactory.Init();
            }
        }
        
        public virtual ICalcContext CreateContext() => new CalcContextYamamoto();

        /// <summary>
        /// ボタンクリック時の動作（一番最初にここに入る）
        /// </summary>
        /// <param name="ctx0"></param>
        /// <param name="btn"></param>
        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContextYamamoto;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            while(true)
            {
                // モードに応じたアプリを作成
                var app = ApplicationFactory.CreateApp(ctx.ModeHistory.Last());

                // アプリを実行
                app.Run(ctx, btn);

                // モードが変わっていればもう一度実行
                if(app.NextMode != AppMode.None)
                {
                    ctx.ModeHistory.Add(app.NextMode);
                    continue;
                }
                break;
            }
        }

        /// <summary>
        /// 拡張ボタンのテキストを返す
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string GetExtButtonText(int num)
        {
            if (num == 1) return "%";
            if (num == 2) return "おみくじ";
            return null;
        }


    }
}
