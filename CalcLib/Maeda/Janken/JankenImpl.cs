using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcLib.Maeda.Util;
using CalcLib.Maeda.Basis;

namespace CalcLib.Maeda.Janken
{
    /// <summary>
    /// くじを表すクラス
    /// </summary>
    public struct Kuji
    {
        /// <summary>
        /// ジャンケンの種類
        /// </summary>
        public enum JankenHand
        {
            unknown,
            グー,
            チョキ,
            パー
        }
    }

    /// <summary>
    /// おみくじのベースクラス
    /// </summary>
    [Serializable]
    public abstract class JankenBase
    {
        public string DisplayText => $"{ConvertJankenHand(Enemy)} VS {ConvertJankenHand(Player)}";

        public string SubDisplayText => GetMessageForState(State);

        public SvcState State { get; set; }

        /// <summary>
        /// プレイヤーの手
        /// </summary>
        public Kuji.JankenHand Player { get; set; }

        /// <summary>
        /// 相手の手
        /// </summary>
        public Kuji.JankenHand Enemy { get; set; }

        /// <summary>
        /// 勝敗を表す数値
        /// </summary>
        public int JudgeResult { get; set; }

        /// <summary>
        /// ジャンケンの内容
        /// </summary>
        public Kuji Items { get; protected set; }

        /// <summary>
        /// 選択したインデックス
        /// </summary>
        public int SelctedIdx { get; protected set; } = -2;

        /// <summary>
        /// くじの初期化を行う。
        /// </summary>
        /// <param name="initList">くじの中身</param>
        public virtual void Init()
        {
            Player = Kuji.JankenHand.unknown;
            Enemy = Kuji.JankenHand.unknown;
            State = SvcState.Initialized;
        }

        /// <summary>
        /// 選択を試みる
        /// </summary>
        /// <param name="num"></param>
        /// <returns>選択不可であればfalse</returns>
        public virtual bool TryChoise(int num)
        {
            //初期化後、自分の手を決める番
            if ((State == SvcState.Initialized || State == SvcState.Running) && 0 <= num && num < 3)
            {
                Player = (Kuji.JankenHand)num + 1;
                Enemy = Kuji.JankenHand.unknown;
                return true;
            }
            //相手の手を決める番
            else if (Player != Kuji.JankenHand.unknown && num == 10)
            {
                Random rnd = new Random();
                Enemy = (Kuji.JankenHand)rnd.Next(3) + 1 ;
                //ジャンケンの勝敗を決める
                JudgeResult = Judge(Player, Enemy);
                //勝敗によるStateの変更
                ChangeState(JudgeResult);
                return true;
            }
            return false;
        }

        public void ChangeState(int judge)
        {
            State = (judge == 0) ? SvcState.Running : State = SvcState.Finished;
        }

        /// <summary>
        /// stateによってサブディスプレイに表示するメッセージを取得する
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public string GetMessageForState(SvcState state)
        {
            if (state == SvcState.Initialized) return "ジャンケン...";
            if (state == SvcState.Running) return "アイコデショ...";           
            //勝敗メッセージを表示する関数
            return GetJudgeMesseage(JudgeResult);
        }

        /// <summary>
        /// 勝敗フラグによるメッセージの取得
        /// </summary>
        /// <param name="judge"></param>
        /// <returns></returns>
        public string GetJudgeMesseage(int judge)
        {
            return (judge == 1) ? "アナタノカチ！" : "アナタノマケ！";
        }

        /// <summary>
        /// ジャンケンの勝敗を決める
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemy"></param>
        public int Judge(Kuji.JankenHand player, Kuji.JankenHand enemy)
        {
            //勝ちパターン
            if    (player == Kuji.JankenHand.グー   && enemy == Kuji.JankenHand.チョキ
                || player == Kuji.JankenHand.チョキ && enemy == Kuji.JankenHand.パー
                || player == Kuji.JankenHand.パー   && enemy == Kuji.JankenHand.グー)
            {
                return 1;
            }
            //負けパターン
            if   (player == Kuji.JankenHand.グー   && enemy == Kuji.JankenHand.パー
               || player == Kuji.JankenHand.チョキ && enemy == Kuji.JankenHand.グー
               || player == Kuji.JankenHand.パー   && enemy == Kuji.JankenHand.チョキ)
            {
                return -1;
            }
            //あいこのパターン
            return 0;
        }

        /// <summary>
        /// 手がunknownの場合に"???"を返す関数
        /// </summary>
        /// <returns></returns>
        public string ConvertJankenHand(Kuji.JankenHand hand)
        {
            return (hand == Kuji.JankenHand.unknown) ? "???" : hand.ToString();
        }
    }

    /// <summary>
    /// おみくじの実装
    /// </summary>
    [Serializable]
    public class JankeniImpl : JankenBase
    {
        ///// <summary>
        ///// くじの初期化を行う。
        ///// </summary>
        //protected override IEnumerable<Kuji> InitKuji()
        //    => new List<Kuji>
        //    {
        //        new Kuji(-10, "凶"),
        //        new Kuji(  3, "小吉"),
        //        new Kuji(  6, "中吉"),
        //        new Kuji( 10, "大吉"),
        //    }.Shuffle();
    }
}
