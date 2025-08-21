using System;

namespace CalcLib.Yamamoto3;

public class SubDisplayManager
{
    private Stack<string> _history;

    public SubDisplayManager()
    {
        _history = new Stack<string>();
    }

    public string GetText()
    {
        return string.Join(" ", _history.Reverse());
    }

    public void Append(string value)
    {
        _history.Push(value);
    }

    public void Clear()
    {
        _history.Clear();
    }

    public void ReplaceLast(string value)
    {
        if (_history.Count > 0)
        {
            _history.Pop();
        }
        _history.Push(value);
    }
}
