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
            foreach (var i in Enumerable.Range(1, 4))
            {
                var text = svc?.GetExtButtonText(i);
                this.GetType().GetProperty("ButtonExt" + i).SetValue(this, text);
                this.GetType().GetProperty("IsEnabledButtonExt" + i).SetValue(this, text is not null);
                if (text is not null) btnDic.Add(text, (CalcButton)Enum.Parse(typeof(CalcButton), "BtnExt" + i));
            }
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

        private string buttonExt1;
        public string ButtonExt1 { get => buttonExt1; set => SetProperty(ref buttonExt1, value); }
        private string buttonExt2;
        public string ButtonExt2 { get => buttonExt2; set => SetProperty(ref buttonExt2, value); }
        private string buttonExt3;
        public string ButtonExt3 { get => buttonExt3; set => SetProperty(ref buttonExt3, value); }
        private string buttonExt4;
        public string ButtonExt4 { get => buttonExt4; set => SetProperty(ref buttonExt4, value); }

        private bool isEnabledButtonExt1;
        public bool IsEnabledButtonExt1 { get => isEnabledButtonExt1; set => SetProperty(ref isEnabledButtonExt1, value); }
        private bool isEnabledButtonExt2;
        public bool IsEnabledButtonExt2 { get => isEnabledButtonExt2; set => SetProperty(ref isEnabledButtonExt2, value); }
        private bool isEnabledButtonExt3;
        public bool IsEnabledButtonExt3 { get => isEnabledButtonExt3; set => SetProperty(ref isEnabledButtonExt3, value); }
        private bool isEnabledButtonExt4;
        public bool IsEnabledButtonExt4 { get => isEnabledButtonExt4; set => SetProperty(ref isEnabledButtonExt4, value); }
    }
}

