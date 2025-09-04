using System;
using CalcLib.Yamamoto3.Extensions;

namespace CalcLib.Yamamoto3;

public class MainDisplayManager
{
    private string _text;

    public MainDisplayManager()
    {
        _text = string.Empty;
    }

    public string GetText()
    {
        // カンマ付きの表示を返す
        return decimal.Parse(GetRawText()).ToString("#,0.#############");
    }


    public string GetRawText()
    {
        // 生の表示を返す
        return _text;
    }

    public void Update(decimal value)
    {
        // DisplayTextを更新
        Update(value.ToString("0.#############"));
    }

    public void Update(string value)
    {
        // DisplayTextを更新
        _text = value;
    }

    public void Concat(string value)
    {
        // DisplayTextを更新
        _text += value;
    }

    public void Clear()
    {
        _text = "";
    }

    public void DeleteLastLetter()
    {
        Update(_text.DeleteLastLetter());
    }
}
