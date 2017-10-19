using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalcLib.Maeda.Basis
{
    /// <summary>
    /// サービスツリーのための親子ノードを表すクラス
    /// 複数の子を持つことは出来ないが、孫を持つことは出来る。
    /// </summary>
    internal class SvcNode
    {
        /// <summary>
        /// 末端のサービスが対応している拡張ボタンのボタンテキスト
        /// </summary>
        public string ButtonText { get; }

        /// <summary>
        /// サービスインスタンス（末端のノードでない限り null）
        /// </summary>
        IBackendSvc _Svc;

        /// <summary>
        /// 子ノード
        /// </summary>
        SvcNode Child { get; set; }

        /// <summary>
        /// 子孫を末端までたどる（ただし今のところ親子関係までしか使っておらず、孫関係までは存在しない）
        /// </summary>
        public SvcNode Descendant => Child?.Descendant ?? this;  // 再帰

        /// <summary>
        /// サービスインスタンス
        /// </summary>
        /// <returns></returns>
        public IBackendSvc GetSvc() => Descendant._Svc;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="btnNum"></param>
        /// <param name="text"></param>
        /// <param name="svc"></param>
        public SvcNode(int btnNum, string text, IBackendSvc svc)
        {
            ButtonText = text;
            _Svc = svc;
        }

        /// <summary>
        /// 親子関係を作る
        /// </summary>
        /// <param name="child">子とするノード</param>
        /// <returns>子を内包する親ノード</returns>
        public static SvcNode CreateParentChild(SvcNode child)
            => new SvcNode(-1, child.ButtonText, null)
            {
                Child = child
            };
    }

    /// <summary>
    /// サービスを親子構造で管理できる、めちゃめちゃゴイスーなやつ
    /// </summary>
    internal class SvcTree : IEnumerable, IEnumerable<SvcNode>
    {
        /// <summary>
        /// 親ノードのリスト
        /// </summary>
        List<SvcNode> Items { get; } = new List<SvcNode>();

        public IEnumerator GetEnumerator() => Items.GetEnumerator();

        IEnumerator<SvcNode> IEnumerable<SvcNode>.GetEnumerator() => Items.GetEnumerator();

        /// <summary>
        /// 親ノードの個数
        /// </summary>
        public int Count => Items.Count;

        /// <summary>
        /// 親ノードにアクセスするためのインデクサ
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public SvcNode this[int idx] => Items.Count <= idx ? null : Items[idx];

        /// <summary>
        /// 子ノードを追加する。その親を作って親子関係にして、親をItemsに追加してくれる。
        /// </summary>
        /// <param name="eb"></param>
        public void AddChildWithParent(SvcNode eb) => Items.Add(SvcNode.CreateParentChild(eb));
    }

    /// <summary>
    /// SvcTreeと他のBasisクラスを組み合わせて使うための拡張メソッド定義
    /// </summary>
    internal static class SvcTreeExt
    {
        /// <summary>
        /// GetExtButtonTextをラップしてSvcTreeNodeのリストに変換して返してくれる便利なやつ
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IEnumerable<SvcNode> GetExtButtons(this IBackendSvc self)
        {
            string t;
            for (int i = 1; (t = self.GetExtButtonText(i)) != null; i++)
            {
                yield return new SvcNode(i, t, self);
            }
        }

        /// <summary>
        /// 検索処理
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        public static SvcHolder Find(this SvcTree tree, CalcButton btn)
        {
            var idx = btn - CalcButton.BtnExt1;
            if (idx < 0 || tree.Count <= idx) return null;
            return new SvcHolder(idx, tree[idx].Descendant.GetSvc());
        }
    }
}

