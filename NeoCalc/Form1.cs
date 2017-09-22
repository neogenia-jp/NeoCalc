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

        private void button9_Click(object sender, EventArgs e)
        {
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn1);
            UpdateContext();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn2);
            UpdateContext();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            svc.OnButtonClick(ctx, CalcLib.CalcButton.Btn3);
            UpdateContext();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnPlus);
            UpdateContext();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            svc.OnButtonClick(ctx, CalcLib.CalcButton.BtnEqual);
            UpdateContext();
        }
    }
}
