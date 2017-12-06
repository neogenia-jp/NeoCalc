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
        /// 名称。"大吉" など。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// くじの価値。大吉が価値が高く、凶は価値が低い。
        /// </summary>
        public int Worth { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <returns></returns>
        public Kuji(int worth, string name) { Name = name; Worth = worth; }

        public override string ToString() => (Name+"　").Substring(0, 2);
    }

    /// <summary>
    /// おみくじのベースクラス
    /// </summary>
    [Serializable]
    public abstract class JankenBase
    {
        public string DisplayText { get; set; }

        public string SubDisplayText { get; set; }

        public SvcState State { get; set; }

        /// <summary>
        /// くじの内容
        /// </summary>
        public IList<Kuji> Items { get; protected set; }

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
            DisplayText = "[1 ] [2 ] [3 ] [4 ]";
            SubDisplayText = "おみくじを選択して下さい";
            Items = InitKuji().ToList();
            SelctedIdx = -1;
            State = SvcState.Initialized;
        }

        protected abstract IEnumerable<Kuji> InitKuji();

        /// <summary>
        /// 選択を試みる
        /// </summary>
        /// <param name="num"></param>
        /// <returns>選択不可であればfalse</returns>
        public virtual bool TryChoise(int num)
        {
            if (State == SvcState.Initialized && 0 <= num && num < Items.Count)
            {
                SelctedIdx = num;
                DisplayText = string.Join(" ", Items);
                SubDisplayText = ResultText;
                State = SvcState.Finished;
                return true;
            }
            return false;
        }

        public string ResultText => $"本日の運勢は「{Items[SelctedIdx]}」です";
    }

    /// <summary>
    /// おみくじの実装
    /// </summary>
    [Serializable]
    public class JankeniImpl : JankenBase
    {
        /// <summary>
        /// くじの初期化を行う。
        /// </summary>
        protected override IEnumerable<Kuji> InitKuji()
            => new List<Kuji>
            {
                new Kuji(-10, "凶"),
                new Kuji(  3, "小吉"),
                new Kuji(  6, "中吉"),
                new Kuji( 10, "大吉"),
            }.Shuffle();
    }
}
