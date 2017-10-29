using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Moriguchi
{
    public class CalcSvcMoriguchi : ICalcSvcEx
    {
        /// <summary>
        /// サービス名の列挙体
        /// </summary>
        public static Dictionary<int, string> SvcName = new Dictionary<int, string>
            {
                { 99,"CalcClass"},
                { 21,"OmikujiClass"},
                { 22,"StockSvc"},
            };

        /// <summary>
        /// 指定サービス番号
        /// </summary>
        public static int SvcNo;

        //共通部品のみに絞る。各機能の要素は移動する。
        public class ContextMoriguchi : ICalcContext
        {
            /// <summary>
            /// メインディスプレイに表示する文字列
            /// </summary>
            public string DisplayText => Buffer == null ? Value : Buffer;
            /// <summary>
            /// サブディスプレイに表示する文字列
            /// </summary>
            public virtual string SubDisplayText => Operation == null ? "" : Value + UtlClass.OpeNameHelper.Get(Operation);

            /// <summary>
            /// 左辺の値
            /// </summary>
            public string Value { get; set; }
            
            /// <summary>
            /// 計算対象とする演算子
            /// </summary>
            public CalcButton? Operation { get; set; }

            /// <summary>
            /// 入力中の値（Valueがセットされている時は右辺となる）
            /// </summary>
            public string Buffer { get; set; }

            //電卓にしか使ってない！邪魔
            public bool Reset { get; set; }

            public void Clear()
            {
                Buffer = "0";
                Value = null;
                Operation = null;
            }
        }
        
        /// <summary>
        /// サービスの入れ物
        /// </summary>
        static ISubSvc FaSvc;
        static ISubContext FaCtx;

        public virtual ICalcContext CreateContext() => new ContextMoriguchi();

        /// <summary>
        /// 拡張ボタンのテキストを返す
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string GetExtButtonText(int num)
        {
            if (num == 1) return "%";
            else if(num == 2) return "おみくじ";
            else if (num == 3) return "株価取得";
            return null;
        }

        /// <summary>
        /// まずここから始まる
        /// </summary>
        /// <param name="ctx0"></param>
        /// <param name="btn"></param>
        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            //ctx0(ディスプレイとサブディスプレイ)
            var ctx = ctx0 as ContextMoriguchi;
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");
            
            //defaultでは電卓モード
            if (FaSvc == null) SvcNo = 99;

            //機能別サービスNo取得
            SvcNo = (int)btn;

            //TODO:各機能のコンテキストとサービスを作成
            FaCtx = SvcFactory.CreateContext();
            FaSvc = SvcFactory.CreateService();

            var ret = FaSvc.OnClick(ctx, btn);

            if (!ret)
            {
                FaSvc = null;
            }
        }
    }
}
