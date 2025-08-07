namespace CalcLib.Mori
{
    // おみくじのクラス
    internal class Omikuji
    {
        private static readonly string[] _kuji = { "大吉", "中吉", "小吉", "凶　" };
        private Dictionary<int, string> _map = new();

        internal int? _selected;
        // おみくじが選択済みかどうか
        internal bool HasSelected => _selected.HasValue;
        internal string _mainDisplay = "[1 ] [2 ] [3 ] [4 ]";
        internal string _subDisplay = "おみくじを選択して下さい";

        public Omikuji()
        {
            // 初期化とシャッフル
            Init();
        }

        // trueでおみくじ終了
        public bool Accept(CalcContextExtend context, CalcButton btn)
        {
            if (btn.IsClear() || btn.IsCE() || btn.IsOmikuji())
            {
                Init();
                return true;
            } 
            else if (HasSelected)
            {
                Init();
                return true;
            }
            else if (btn.IsOmikujiSelect())
            {
                _selected = (int)btn;
                return false;
            }  
            
            // その他のキーは無効
            return false;
        }

        internal void Shuffle()
        {
            _map = _kuji
                .OrderBy(_ => Random.Shared.Next())
                .Select((name, idx) => (idx + 1, name))
                .ToDictionary(t => t.Item1, t => t.name);
        }
        internal void Init()
        {
            _mainDisplay = "[1 ] [2 ] [3 ] [4 ]";
            _subDisplay = "おみくじを選択して下さい";
            _selected = null;
            Shuffle();
        }
        
        // 表示用のテキストとモードを返す
        internal DisplaySource RowDisplay()
        {
            UIMode mode = UIMode.Omikuji;
            if (_selected.HasValue)
            {
                _mainDisplay = string.Join(" ", _map.Values);
                _subDisplay = $"本日の運勢は「{_map[_selected.Value]}」です";
                mode = UIMode.OmikujiEnd;
            }
            else
            {
                _mainDisplay = "[1 ] [2 ] [3 ] [4 ]";
                _subDisplay = "おみくじを選択して下さい";
            }
            return new DisplaySource(_mainDisplay, _subDisplay, mode);
        }
    }
}