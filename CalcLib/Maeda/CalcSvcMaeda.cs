using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CalcLib.Maeda.Basis;
using CalcLib.Maeda.Dispatcher;

namespace CalcLib.Maeda
{
    /// <summary>
    /// 前田実装分のサービスフロント
    /// </summary>
    public class CalcSvcMaeda : ICalcSvcEx
    {
        /// <summary>
        /// バックエンドサービス一覧
        /// </summary>
        static readonly string[] _services = {
            typeof(CalcuratorSvc).FullName,
            typeof(OmikujiSvc).FullName,
        };

        SvcDispatcher Dispatcher = new SvcDispatcher(_services);

        /// <summary>
        /// コンテキストの新規生成
        /// </summary>
        /// <returns></returns>
        public virtual ICalcContext CreateContext() => Dispatcher.CreateContext();

        /// <summary>
        /// 拡張ボタンのテキストを返す
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public string GetExtButtonText(int num) => Dispatcher.GetExtButtonText(num);

        /// <summary>
        /// ボタン押下時の処理
        /// </summary>
        /// <param name="ctx0"></param>
        /// <param name="btn"></param>
        public virtual void OnButtonClick(ICalcContext ctx, CalcButton btn) => Dispatcher.Dispatch(ctx, btn); 
    }
}
