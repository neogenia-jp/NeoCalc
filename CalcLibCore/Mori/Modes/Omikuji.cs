namespace CalcLib.Mori
{
    // おみくじのクラス
    internal class Omikuji
    {
        private static readonly string DefaultMainText = "[1 ] [2 ] [3 ] [4 ]";
        private static readonly string DefaultSubText  = "おみくじを選択して下さい";
        private static readonly string[] _kuji = { "大吉", "中吉", "小吉", "凶　" };
        private Dictionary<int, string> _map = new();

        internal int? _selected;
        // おみくじが選択済みかどうか
        internal bool HasSelected => _selected.HasValue;

        /// <summary>
        /// 入力ボタンを処理する。おみくじモードの継続/終了を判定する。
        /// </summary>
        /// <param name="context">電卓コンテキスト</param>
        /// <param name="btn">ボタン</param>
        /// <returns>モードを終了時 true、継続する場合は false</returns>
        public bool Accept(CalcButton btn)
        {
            if (btn.IsClear() || btn.IsCE() || btn.IsOmikuji()) return Exit();
            if (HasSelected) return Exit();
            if (btn.IsOmikujiSelect())
            {
                _selected = (int)btn;
                return false;
            }
            
            // その他のキーは無効
            return false;
        }

        private void Shuffle()
        {
            _map = _kuji
                .OrderBy(_ => Random.Shared.Next())
                .Select((name, idx) => (idx + 1, name))
                .ToDictionary(t => t.Item1, t => t.name);
        }

        /// <summary>
        /// おみくじの初期化
        /// </summary>
        internal void Init()
        {
            _selected = null;
            Shuffle();
        }

        // おみくじモードの終了条件で呼ばれる共通メソッド
        private bool Exit()
        {
            Init();
            return true;
        }

        // 表示用のテキストとモードを返す
        internal DisplaySource RowDisplay()
        {
            if (_selected.HasValue)
            {
                return new DisplaySource(string.Join(" ", _map.Values), $"本日の運勢は「{_map[_selected.Value]}」です", UIMode.OmikujiEnd);
            }
            return new DisplaySource(DefaultMainText, DefaultSubText, UIMode.Omikuji);
        }
    }
}

