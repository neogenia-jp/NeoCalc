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
        public string DisplayText => $"{Enemy} VS {Player}";

        public string SubDisplayText { get; set; }

        public SvcState State { get; set; }

        /// <summary>
        /// プレイヤーの手
        /// </summary>
        public string Player { get; set; }

        /// <summary>
        /// 相手の手
        /// </summary>
        public string Enemy { get; set; }

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
            Player = "???";
            Enemy = "???";
            SubDisplayText = "ジャンケン...";
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
            if (State == SvcState.Initialized && 0 <= num && num < 3)
            {
                Player = Enum.GetName(typeof(Kuji.jankenhand), num);
                //State = SvcState.Running;
                return true;
            }
            //相手の手を決める番
            else if (Player != null && num == 10)
            {
                Random rnd = new Random();
                Enemy = Enum.GetName(typeof(Kuji.jankenhand), rnd.Next(3));

                //TODO:勝敗を決める 別関数「Judgement」
                return true;
            }
            return false;
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
