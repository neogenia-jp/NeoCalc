namespace CalcLib.Mori
{
    // おみくじのクラス
    internal class Omikuji
    {
        public void Accept(CalcContextExtend context, CalcButton btn)
        {
            if (btn.IsClear() || btn.IsCE())
            {
                return;
            }
            
        }
        
        // 表示用のテキストとモードを返す
        internal DisplaySource RowDisplay()
        {
            return new DisplaySource("[1 ] [2 ] [3 ] [4 ]", "おみくじを選択して下さい", UIMode.Omikuji);
        }
    }
}