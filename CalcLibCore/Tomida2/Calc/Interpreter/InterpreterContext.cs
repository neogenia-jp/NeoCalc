using System;
using System.Collections.Generic;
using System.Text;

namespace CalcLibCore.Tomida2.Calc.Interpreter
{
  /// <summary>
  /// Interpreterパターンで使用するコンテキストクラス
  /// </summary>
  public class InterpreterContext
  {
    private readonly string _input;
    private int _position;

    public InterpreterContext(string input)
    {
      _input = input ?? throw new ArgumentNullException(nameof(input));
      _position = 0;
    }

    /// <summary>
    /// 現在の入力位置
    /// </summary>
    public int Position => _position;

    /// <summary>
    /// 入力文字列の長さ
    /// </summary>
    public int Length => _input.Length;

    /// <summary>
    /// 現在位置の文字を取得
    /// </summary>
    public char CurrentChar => _position < _input.Length ? _input[_position] : '\0';

    /// <summary>
    /// 現在位置を次に進める
    /// </summary>
    public void Advance()
    {
      if (_position < _input.Length)
        _position++;
    }

    /// <summary>
    /// 現在位置から指定された文字数の文字列を取得
    /// </summary>
    public string Peek(int count)
    {
      int end = Math.Min(_position + count, _input.Length);
      return _input.Substring(_position, end - _position);
    }

    /// <summary>
    /// 入力の終端かどうかを確認
    /// </summary>
    public bool IsAtEnd => _position >= _input.Length;

    /// <summary>
    /// 空白文字をスキップ
    /// </summary>
    public void SkipWhitespace()
    {
      while (!IsAtEnd && char.IsWhiteSpace(CurrentChar))
      {
        Advance();
      }
    }

    /// <summary>
    /// 現在位置をリセット
    /// </summary>
    public void Reset()
    {
      _position = 0;
    }

    /// <summary>
    /// 現在位置を指定位置に設定
    /// </summary>
    public void SetPosition(int position)
    {
      _position = Math.Max(0, Math.Min(position, _input.Length));
    }
  }
}
