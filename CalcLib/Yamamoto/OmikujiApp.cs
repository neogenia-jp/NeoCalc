using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcLib.Yamamoto
{
    public class OmikujiApp : BaseApp
    {
        /// <summary>
        /// 入力状態
        /// </summary>
        public enum State
        {
            Init = 0,      // 初期化
            Start,         // おみくじ開始
            Opened,        // おみくじを開いた状態
            Fin,           // おみくじ終了
        }

        /// <summary>
        /// 入力状態
        /// </summary>
        public State InputState { get; set; } = State.Init;

        /// <summary>
        /// おみくじの種類
        /// </summary>
        public enum OmikujiKind
        {
            Daikiti = 0,    // 大吉
            Chukiti,        // 中吉
            Syokiti,        // 小吉
            Kyo,            // 凶
        }

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
        public override void Run(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcSvcYamamoto.CalcContextYamamoto;

            if(InputState == State.Init)
            {
                Init(ctx);
                return;
            }

            switch (btn)
            {
                // "C"
                // "CE"
                // "おみくじ"
                case CalcButton.BtnClear:
                case CalcButton.BtnClearEnd:
                case CalcButton.BtnExt2:
                    ToCaliculatorMode();
                    InputState = State.Fin;
                    break;

                // "株価取得"
                case CalcButton.BtnExt3:
                    ToStockMode();
                    InputState = State.Fin;
                    break;

                // "1"
                // "2"
                // "3"
                // "4"
                case CalcButton.Btn1:
                case CalcButton.Btn2:
                case CalcButton.Btn3:
                case CalcButton.Btn4:
                    if(InputState == State.Opened)
                    {
                        ToCaliculatorMode();
                        InputState = State.Fin;
                        break;
                    }
                    OpenOmikuji(ctx, btn);
                    break;

                default:
                    if(InputState == State.Opened)
                    {
                        ToCaliculatorMode();
                        InputState = State.Fin;
                        break;
                    }
                    break;
            }
        }

        /// <summary>
        /// おみくじの初期化
        /// </summary>
        /// <param name="ctx"></param>
        private void Init(CalcSvcYamamoto.CalcContextYamamoto ctx)
        {
            // おみくじ箱にくじを入れる
            foreach(var kind in Enum.GetValues(typeof(OmikujiKind)))
            {
                var omikuji = new Omikuji();
                omikuji.Result = ((OmikujiKind)kind).DispName();
                Box.Add(omikuji);
            }

            // おみくじ箱をシャッフル
            Box = Box.OrderBy(i => Guid.NewGuid()).ToList();

            // おみくじモードの初期表示テキストを設定
            ctx.SubDisplayText = "おみくじを選択して下さい";
            ctx.DisplayText = "";
            for(int i = 0; i < Enum.GetNames(typeof(OmikujiKind)).Length; i++)
            {
                ctx.DisplayText += $"[{i + 1} ] ";
            }
            ctx.DisplayText = ctx.DisplayText.TrimEnd();

            // 開始状態へ遷移
            InputState = State.Start;
        }

        /// <summary>
        /// おみくじを開く
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        /// <param name="index"></param>
        private void OpenOmikuji(CalcSvcYamamoto.CalcContextYamamoto ctx, CalcButton btn)
        {
            ctx.SubDisplayText = $"本日の運勢は「{Box[(int)btn - 1].Result}」です";
            ctx.DisplayText = string.Join(" ", Box.Select(x => x.Result).ToList());
            InputState = State.Opened;
        }
    }

    public static class OmikujiKindExt
    {
        public static string DispName(this OmikujiApp.OmikujiKind k)
        {
            string[] names = { "大吉", "中吉", "小吉", "凶　" };
            return names[(int)k];
        }

    }
}
