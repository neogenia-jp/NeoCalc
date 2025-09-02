// IModeStateが扱う次の状態とボタンを保持するDTOクラス

namespace CalcLib.Mori
{
    internal class ModeResult
    {
        // 暫定 型ではなく文字列でモードを指定
        public string? NextKey { get; }
        public CalcButton? ForwardButton { get; }

        private ModeResult(string? next, CalcButton? forwardButton)
        {
            NextKey = next;
            ForwardButton = forwardButton;
        }

        public static ModeResult Continue()
        {
            return new(null, null);
        }

        public static ModeResult SwitchMode(string key, CalcButton? forwardButton = null)
        {
            return new(key, forwardButton);
        }
    }
}

