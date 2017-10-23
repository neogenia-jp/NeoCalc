using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcLib.Maeda.Util;

namespace CalcLib.Maeda.Omikuji
{
    public struct Kuji
    {
        public string Name { get; }
        public int Worth { get; }
        public Kuji(int worth, string name) { Name = name; Worth = worth; }
        public override string ToString() => (Name+"　").Substring(0, 2);
    }

    [Serializable]
    public class OmikujiImpl
    {
        public IList<Kuji> Items { get; private set; }

        public int SelctedIdx { get; private set; } = -1;

        public void Init()
        {
            Items = new List<Kuji>
            {
                new Kuji(-10, "凶"),
                new Kuji(  3, "小吉"),
                new Kuji(  6, "中吉"),
                new Kuji( 10, "大吉"),
            }.Shuffle();
            SelctedIdx = -1;
        }

        public bool TryChoise(int num)
        {
            if (!IsFinished && 0 <= num && num < Items.Count)
            {
                SelctedIdx = num;
                return true;
            }
            return false;
        }

        public string ResultText => $"あなたの運勢は「{Items[SelctedIdx]}」です";

        public bool IsFinished => SelctedIdx >= 0;
    }
}
