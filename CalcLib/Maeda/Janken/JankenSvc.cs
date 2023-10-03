using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CalcLib.Maeda.Basis;
using CalcLib.Maeda.Dispatcher;

namespace CalcLib.Maeda.Janken
{
    internal enum JankenState
    {
        [DisplayString("ジャンケン...")]
        Init,

        [DisplayString("ジャンケン...")]
        PlayerThinking,  // 手を考え中

        [DisplayString("アイコデショ...")]
        Draw,  // あいこ

        [DisplayString("アナタノカチ！")]
        Won,   // プレイヤーの勝ち

        [DisplayString("アナタノマケ！")]
        Lost,  // プレイヤーの負け
    }

    public class JankenPlayerViaCalc : IJankenPlayer
    {
        public JankenHand Hand { get; set; } = JankenHand.Unknown;

        public JankenHand GetHand() => Hand;
    }

    /// <summary>
    /// おみくじサービスのためのコンテキスト
    /// </summary>
    internal class JankenContext : ICalcContext
    {
        JankenGame Game;

        JankenPlayerViaCalc Player;

        JankenResult Result;

        internal JankenState State { get; set; }

        JankenHand Hand1 => Player.Hand;

        JankenHand Hand2 => Result?.Hands[0] ?? JankenHand.Unknown;

        public string DisplayText => $"{Hand2.GetDisplayString().PadRight(3, '　')} vs {Hand1.GetDisplayString().PadLeft(3, '　')}";

        public string SubDisplayText => State.GetDisplayString();

        public void Init()
        {
            Game = new JankenGame();
            Player = new JankenPlayerViaCalc();
            Game.AddPlayer(new RandomJankenPlayer());
            Game.AddPlayer(Player);
            Result = null;
            State = JankenState.Init;
        }

        public void SetPlayerHand(JankenHand hand)
        {
            Player.Hand = hand;
            Result = null;  // あいこの時に、Resultがセットされているのでクリアしておく
        }

        public void Play()
        {
            Result = Game.Play();
            State = Result.IsDraw                  ? JankenState.Draw
                  : Result.WonPlayers[0] == Player ? JankenState.Won
                                                   : JankenState.Lost;
        }
    }

    /// <summary>
    /// おみくじサービス
    /// </summary>
    internal class JankenSvc : SvcBase<JankenContext>
    {
        public override string GetExtButtonText(int num) => num == 1 ? "✌️" : null;

        internal override JankenContext _CreateContext() => new JankenContext();

        public override void OnEnter(JankenContext ctx, SvcSwichedEventArg svcSwichedEventArg)
        {
            ctx.Init();
        }

        public override bool TryButtonClick(JankenContext ctx, CalcButton btn)
        {
            // 勝敗が決まっていたらこのサービスを抜ける
            if (ctx.State == JankenState.Lost || ctx.State == JankenState.Won) return false;

            switch (btn)
            {
                case CalcButton.BtnExt1:
                    if (ctx.State != JankenState.Init) return false;
                    break;
                case CalcButton.BtnEqual:
                    ctx.Play();
                    break;
                default:
                    var h = btn - CalcButton.Btn1 + 1;
                    if (1 <= h && h <= 3) ctx.SetPlayerHand((JankenHand)h);
                    break;
            }

            return true;
        }

        protected override void OnExitSvc(JankenContext ctx)
        {
        }
    }
}
