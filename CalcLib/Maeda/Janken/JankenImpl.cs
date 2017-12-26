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
        public enum jankenhand
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
        public string DisplayText => $"{JyankenHandConverter(Enemy)} VS {JyankenHandConverter(Player)}";

        public string SubDisplayText => ForSubDisplay(State);

        public SvcState State { get; set; }

        /// <summary>
        /// プレイヤーの手
        /// </summary>
        public Kuji.jankenhand Player { get; set; }

        /// <summary>
        /// 相手の手
        /// </summary>
        public Kuji.jankenhand Enemy { get; set; }

        /// <summary>
        /// 勝敗を表す数値
        /// </summary>
        public int judge { get; set; }

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
            Player = Kuji.jankenhand.unknown;
            Enemy = Kuji.jankenhand.unknown;
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
                Player = (Kuji.jankenhand)num + 1;
                Enemy = Kuji.jankenhand.unknown;
                return true;
            }
            //相手の手を決める番
            else if (Player != Kuji.jankenhand.unknown && num == 10)
            {
                Random rnd = new Random();
                Enemy = (Kuji.jankenhand)rnd.Next(3) + 1 ;
                //ジャンケンの勝敗を決める
                judge = Judgement(Player, Enemy);
                //勝敗によるStateの変更
                StateChange(judge);
                return true;
            }
            return false;
        }

        public void StateChange(int judge)
        {
            if (judge == 0)
            {
                State = SvcState.Running;
            }
            else
            {
                State = SvcState.Finished;
            }
        }

        /// <summary>
        /// stateによってサブディスプレイに表示するメッセージを取得する
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public string ForSubDisplay(SvcState state)
        {
            if (state == SvcState.Initialized)
            {
                return "ジャンケン...";
            }
            else if (state == SvcState.Running)
            {
                return "アイコデショ...";
            }
            //勝敗メッセージを表示する関数
            return JudgeMesseage(judge);
        }

        /// <summary>
        /// 勝敗フラグによるメッセージの取得
        /// </summary>
        /// <param name="judge"></param>
        /// <returns></returns>
        public string JudgeMesseage(int judge)
        {
            if (judge == 1)
            {
                return "アナタノカチ！";
            }
            return "アナタノマケ！";
        }

        /// <summary>
        /// ジャンケンの勝敗を決める
        /// </summary>
        /// <param name="Player"></param>
        /// <param name="enemy"></param>
        public int Judgement(Kuji.jankenhand player, Kuji.jankenhand enemy)
        {
            //勝ちパターン
            if    (player == Kuji.jankenhand.グー   && enemy == Kuji.jankenhand.チョキ
                || player == Kuji.jankenhand.チョキ && enemy == Kuji.jankenhand.パー
                || player == Kuji.jankenhand.パー   && enemy == Kuji.jankenhand.グー)
            {
                return 1;
            }
            //負けパターン
            else if   (player == Kuji.jankenhand.グー   && enemy == Kuji.jankenhand.パー
                    || player == Kuji.jankenhand.チョキ && enemy == Kuji.jankenhand.グー
                    || player == Kuji.jankenhand.パー   && enemy == Kuji.jankenhand.チョキ)
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
        public string JyankenHandConverter(Kuji.jankenhand hand)
        {
            if (hand == Kuji.jankenhand.unknown)
            {
                return "???";
            }
            return hand.ToString();
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
