using CalcLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeoCalc
{
    public partial class Form1 : Form
    {

        CalcLib.ICalcSvc svc;
        CalcLib.ICalcContext ctx;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 0 ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn0);
        }

        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            ctx = CalcLib.Factory.CreateContext();
            svc = CalcLib.Factory.CreateService();
            SetupExtButtons(svc as ICalcSvcEx);
            UpdateContext();
        }

        /// <summary>
        /// コンテキストの内容を画面に反映する
        /// </summary>
        private void UpdateContext()
        {
            label1.Text = ctx.DisplayText;
            label2.Text = ctx.SubDisplayText;
        }

        /// <summary>
        /// 拡張ボタンのセットアップ
        /// </summary>
        /// <param name="svc"></param>
        private void SetupExtButtons(ICalcSvcEx svc)
        {
            var btns = new[] { button21, button22, button23, button24 };
            for (int i = 0; i < btns.Length; i++)
            {
                var txt = svc?.GetExtButtonText(i+1);
                btns[i].Text = txt;
                btns[i].Enabled = txt != null;
            }
        }

        /// <summary>
        /// ボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn == null) return;

            // ボタンの Tag より CalcButton enum を決定する（Tagが設定されていなければTextを見る）
            var tag = btn.Tag?.ToString();
            if (string.IsNullOrEmpty(tag)) tag = btn.Text;

            var btnName = $"Btn{tag}";
            CalcButton cb;
            if (!Enum.TryParse(btnName, out cb))
            {
                throw new ApplicationException($"ボタン'{btnName}'が解釈できません。");
            }

            svc.OnButtonClick(ctx, cb);
            UpdateContext();
        }
    }
}
