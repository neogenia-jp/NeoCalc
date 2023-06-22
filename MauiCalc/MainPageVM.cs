using Microsoft.Maui.Controls;
using System;
using CalcLib;
using System.Windows.Input;

namespace MauiCalc
{
	public class MainPageVM : BindableBase
	{
        CalcLib.ICalcSvc svc;
        CalcLib.ICalcContext ctx;

        Dictionary<string, CalcButton> btnDic = new()
        {
            {"0", CalcButton.Btn0},
            {"1", CalcButton.Btn1},
            {"2", CalcButton.Btn2},
            {"3", CalcButton.Btn3},
            {"4", CalcButton.Btn4},
            {"5", CalcButton.Btn5},
            {"6", CalcButton.Btn6},
            {"7", CalcButton.Btn7},
            {"8", CalcButton.Btn8},
            {"9", CalcButton.Btn9},
            {"BS", CalcButton.BtnBS},
            {"C", CalcButton.BtnClear},
            {"CE", CalcButton.BtnClearEnd},
            {"÷", CalcButton.BtnDivide},
            {".", CalcButton.BtnDot},
            {"=", CalcButton.BtnEqual},
            {"Ext1", CalcButton.BtnExt1},
            {"Ext2", CalcButton.BtnExt2},
            {"Ext3", CalcButton.BtnExt3},
            {"Ext4", CalcButton.BtnExt4},
            {"-", CalcButton.BtnMinus},
            {"×", CalcButton.BtnMultiple},
            {"+", CalcButton.BtnPlus},
            {"±", CalcButton.BtnPlusMinus},
        };

        /// <summary>
        /// コンテキストの内容を画面に反映する
        /// </summary>
        void UpdateContext()
        {
            DisplayText = ctx.DisplayText;
            SubDisplayText = ctx.SubDisplayText;
        }

        /// <summary>
        /// 拡張ボタンのセットアップ
        /// </summary>
        /// <param name="svc"></param>
        private void SetupExtButtons(ICalcSvcEx svc)
        {
            //TODO:拡張ボタンの処理を追加
        }

        /// <summary>
        /// ボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnButtonClick(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn is null) return;
            if (btn.Text is null) return;
            if (!btnDic.ContainsKey(btn.Text)) throw new ApplicationException($"ボタン'{btn.Text}'が解釈できません。");

            svc.OnButtonClick(ctx, btnDic[btn.Text]);
            UpdateContext();
        }

        internal void OnLoad(object sender, EventArgs e)
        {
            ctx = CalcLib.Factory.CreateContext();
            svc = CalcLib.Factory.CreateService();
            Title = $" ({svc.GetType().Name})";
            SetupExtButtons(svc as ICalcSvcEx);
            UpdateContext();
        }

        private string title;
        public string Title { get => title; set => SetProperty(ref title, value); }

        private string displayText;
        public string DisplayText { get => displayText; set => SetProperty(ref displayText, value); }

        private string subDisplayText;
        public string SubDisplayText { get => subDisplayText; set => SetProperty(ref subDisplayText, value); }
    }
}

