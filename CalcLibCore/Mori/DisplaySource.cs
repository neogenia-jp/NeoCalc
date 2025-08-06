namespace CalcLib.Mori
{   
    // 表示モードのクラス
    public enum UIMode { CalcDefault, CalcInputting, Omikuji }
    public class DisplaySource
    {
        public string MainText { get; }
        public string SubText  { get; }
        public UIMode Mode     { get; }

        public DisplaySource(string main, string sub, UIMode mode)
        {
            MainText = main;
            SubText  = sub;
            Mode     = mode;
        }
    }
}   