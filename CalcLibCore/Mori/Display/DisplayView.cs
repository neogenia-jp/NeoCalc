using System;

namespace CalcLib.Mori.Display
{
    // 電卓表示部用のDTO
    internal sealed class DisplayView
    {
        // メインディスプレイに表示する文字列
        public string Main { get; }
        // サブディスプレイに表示する文字列
        public string Sub  { get; }

        public DisplayView(string main, string sub)
        {
            Main = main ?? string.Empty;
            Sub  = sub  ?? string.Empty;
        }
    }
}

