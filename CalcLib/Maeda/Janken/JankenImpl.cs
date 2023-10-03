using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcLib.Maeda.Util;
using CalcLib.Maeda.Basis;

namespace CalcLib.Maeda.Janken
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal class DisplayStringAttribute : Attribute
    {
        public string Text { get; }
        internal DisplayStringAttribute(string text) { Text = text; }
    }

    internal static class DisplayStringAttributeExt
    {
        /// <summary>
        /// [DisplayString("xxx")] の "xxx" の部分を取得する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        internal static string GetDisplayString(this Enum enumObj)
        {
            var m = enumObj.GetType().GetField(enumObj.ToString());
            if (m == null) return null;

            var attr = (DisplayStringAttribute)m.GetCustomAttributes(typeof(DisplayStringAttribute), false).FirstOrDefault();
            if (attr == null) return null;

            return attr.Text;
        }
    }

    /// <summary>
    /// じゃんけんの手を表す列挙型
    /// </summary>
    public enum JankenHand
    {
        [DisplayString("？？？")]
        Unknown,

        [DisplayString("グー")]
        Goo,

        [DisplayString("チョキ")]
        Choki,

        [DisplayString("パー")]
        Paa
    }

    public interface IJankenPlayer
    {
        JankenHand GetHand();
    }

    public class RandomJankenPlayer : IJankenPlayer
    {
        Random r = new Random();

        public virtual JankenHand GetHand() => (JankenHand)(r.Next(2) + 1);
    }

    public class JankenResult
    {
        Dictionary<int, IJankenPlayer> _list;
        public IReadOnlyList<int> WonPlayerNum { get; private set; }
        public IReadOnlyList<IJankenPlayer> WonPlayers { get; private set; }
        public IReadOnlyList<JankenHand> Hands { get; private set; }

        private JankenResult() { }

        public bool IsDraw => _list.Count == 0;

        public static JankenResult Finish(Dictionary<int, IJankenPlayer> wonPlayers, JankenHand[] hands)
            => new JankenResult {
                _list = wonPlayers,
                WonPlayerNum = wonPlayers.Keys.ToList(),
                WonPlayers = wonPlayers.Keys.Select(n=>wonPlayers[n]).ToList(),
                Hands = hands,
            };

        public static JankenResult Draw(JankenHand[] hands)
            => Finish(new Dictionary<int, IJankenPlayer>(), hands);
    }

    public class JankenGame
    {
        List<IJankenPlayer> _Players = new List<IJankenPlayer>();
        IReadOnlyList<IJankenPlayer> Players => _Players;

        public void AddPlayer(IJankenPlayer p) => _Players.Add(p);

        public JankenResult Play()
        {
            var hands = _Players.Select(x => x.GetHand()).ToArray();    // プレイヤーにじゃんけんの手を出させて配列に詰める
            var dHands = hands.Distinct().ToList();                     // 手を重複排除
            if (dHands.Count != 2) return JankenResult.Draw(hands);     // 1種類または3種類の手がある場合は、あいこ

            // 勝敗判定
            dHands.Sort();                        // 手をソートする。2種類の手がグーチョキパーの順に並ぶ
            JankenHand winner = dHands[1];        // とりあえず後の方の手が勝ちとする
            if (dHands[0] == JankenHand.Goo && dHands[1] == JankenHand.Choki
                || dHands[0] == JankenHand.Choki && dHands[1] == JankenHand.Paa
                || dHands[0] == JankenHand.Paa && dHands[1] == JankenHand.Goo)
            {
                winner = dHands[0];               // 前の方の手が勝ちの場合
            }

            // LINQ式で 勝者を抽出し、 { プレイヤー番号 => プレーヤー } な連想配列を作って JankenResult を返す
            return JankenResult.Finish(
                hands.Select((hand, i) => new { hand, i }).Where(x => x.hand == winner).ToDictionary(x => x.i, x => _Players[x.i]),
                hands);
        }
    }
}
