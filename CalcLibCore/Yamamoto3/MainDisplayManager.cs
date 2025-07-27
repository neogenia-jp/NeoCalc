using System;

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
        return _text.ToString();
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
}
