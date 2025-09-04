// IModeStateが扱う次の状態とボタンを保持するDTOクラス

namespace CalcLib.Mori
{
    internal class ModeResult
    {
        // 遷移先モード（型安全化）
        public ModeKey? Next { get; }
        public CalcButton? ForwardButton { get; }

        private ModeResult(ModeKey? next, CalcButton? forwardButton)
        {
            Next = next;
            ForwardButton = forwardButton;
        }

        public static ModeResult Continue()
        {
            return new(null, null);
        }

        public static ModeResult SwitchMode(ModeKey key, CalcButton? forwardButton = null)
        {
            return new(key, forwardButton);
        }
    }
}
