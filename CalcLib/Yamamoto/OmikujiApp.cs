using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto
{
    public class OmikujiApp : IApplication
    {
        /// <summary>
        /// 入力状態
        /// </summary>
        public enum State
        {
            Start = 0,     // 演算子入力後
            Fin,           // おみくじ終了
        }

        /// <summary>
        /// 入力状態
        /// </summary>
        public State InputState { get; set; }

        /// <summary>
        /// おみくじ
        /// </summary>
        public class Omikuji
        {
            public string Result { get; set; }
        }

        /// <summary>
        /// おみくじ箱
        /// </summary>
        private List<Omikuji> Box { get; set; } = new List<Omikuji>();

        /// <summary>
        /// アプリ実行
        /// </summary>
        /// <param name="ctx0"></param>
        /// <param name="btn"></param>
        public void Run(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcSvcYamamoto.CalcContextYamamoto;

            switch (btn)
            {
                // "C"
                // "CE"
                // "おみくじ"
                case CalcButton.BtnClear:
                case CalcButton.BtnClearEnd:
                case CalcButton.BtnExt2:
                    ToCaliculatorMode(ctx, btn);
                    break;

                // "1"
                // "2"
                // "3"
                // "4"
                case CalcButton.Btn1:
                case CalcButton.Btn2:
                case CalcButton.Btn3:
                case CalcButton.Btn4:
                    OpenOmikuji(ctx, btn);
                    break;

                default:
                    if(InputState == State.Fin)
                    {
                        ToCaliculatorMode(ctx, btn);
                    }
                    break;
            }
        }

        /// <summary>
        /// おみくじを開く
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void OpenOmikuji(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
        }

        /// <summary>
        /// 電卓モードへ移行
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        private void ToCaliculatorMode(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
        }

    }
}
