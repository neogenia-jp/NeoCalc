// IModeStateが扱う次の状態とボタンを保持するDTOクラス

namespace CalcLib.Mori
{
    internal class ModeResult
    {
        public IModeState? Next { get; }
        public CalcButton? ForwardButton { get; }

        private ModeResult(IModeState? next, CalcButton? forwardButton)
        {
            Next = next;
            ForwardButton = forwardButton;
        }

        public static ModeResult Continue(IModeState state)
        {
            return new(state, null);
        }

        public static ModeResult SwitchMode(IModeState? next, CalcButton? forwardButton = null)
        {
            return new(next, forwardButton);
        }
    }
}