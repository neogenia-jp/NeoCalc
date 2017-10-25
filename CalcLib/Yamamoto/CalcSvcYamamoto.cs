using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto
{
    internal class CalcSvcYamamoto : ICalcSvcEx
    {
        public class CalcContextYamamoto : CalcContext
        {
            /// <summary>
            /// 電卓のモード
            /// </summary>
            public enum AppMode
            {
                Calculator = 0,  // 電卓
                Omikuji,         // おみくじ
            }

            /// <summary>
            /// 現在のモード
            /// </summary>
            public AppMode Mode { get; set; } = AppMode.Calculator;

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

            // モードに応じたアプリを作成
            var app = ApplicationFactory.CreateApp(ctx.Mode);

            // アプリを実行
            app.Run(ctx, btn);
        }

        /// <summary>
        /// 拡張ボタンのテキストを返す
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string GetExtButtonText(int num)
        {
            if (num == 1) return "%";
            return null;
        }


    }
}
