using CalcLibCore.Tomida2.Calc.Interpreter;
using System;
using System.Text;

namespace CalcLibCore.Tomida2.Calc.implements
{
    /// <summary>
    /// SubDisplayTextの表示処理を担当するクラス
    /// </summary>
    internal class SubDisplayTextImpl
    {
        /// <summary>
        /// サブ表示用のテキストを生成します
        /// 式の末尾がオペランドの場合はそのオペランドを表示せず、
        /// 末尾が=の場合は空文字列を返します
        /// </summary>
        /// <param name="rowInput">現在の入力内容</param>
        /// <param name="isResultDisplayed">結果表示中かどうか</param>
        /// <param name="parseResult">パース結果</param>
        /// <returns>サブ表示用テキスト</returns>
        public string ToDisplay(string rowInput, bool isResultDisplayed, IParseResult parseResult)
        {
            // 結果表示中は空文字を返す
            if (isResultDisplayed)
            {
                return string.Empty;
            }
            
            // 空の入力の場合は空文字を返す
            if (string.IsNullOrEmpty(rowInput))
            {
                return string.Empty;
            }
            
            // 末尾が=の場合は空文字を返す
            if (rowInput.TrimEnd().EndsWith("="))
            {
                return string.Empty;
            }
            
            try 
            {
                // 入力文字列をフォーマット（末尾のオペランドを除去し、オペランドとオペレーターの間に空白を挿入）
                return FormatExpressionWithoutTrailingOperand(rowInput);
            }
            catch 
            {
                // エラーが発生した場合は元の入力をそのまま返す
                return rowInput;
            }
        }

        /// <summary>
        /// 式をフォーマットして、末尾のオペランドを除去し、オペランドとオペレーターの間に空白を挿入します
        /// </summary>
        /// <param name="input">入力式</param>
        /// <returns>フォーマットされた式（末尾のオペランドを除く）</returns>
        private string FormatExpressionWithoutTrailingOperand(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // 末尾から演算子以外の文字（オペランド）を取り除く
            string processedInput = RemoveTrailingOperand(input);
            
            if (string.IsNullOrEmpty(processedInput))
                return string.Empty;

            var result = new StringBuilder();
            
            for (int i = 0; i < processedInput.Length; i++)
            {
                char currentChar = processedInput[i];
                
                // 演算子の場合は前後に空白を追加し、表示用に変換
                if (IsOperator(currentChar))
                {
                    // 前に空白を追加（ただし、先頭に空白が既にある場合は追加しない）
                    if (result.Length > 0 && result[result.Length - 1] != ' ')
                    {
                        result.Append(' ');
                    }
                    
                    // 演算子を表示用に変換
                    result.Append(ConvertOperatorForDisplay(currentChar));
                    
                    // 後ろに空白を追加（ただし、次の文字が既に空白の場合は追加しない）
                    if (i + 1 < processedInput.Length && processedInput[i + 1] != ' ')
                    {
                        result.Append(' ');
                    }
                }
                else
                {
                    result.Append(currentChar);
                }
            }
            
            return result.ToString();
        }

        /// <summary>
        /// 演算子を表示用の文字に変換します
        /// </summary>
        /// <param name="operatorChar">内部演算子文字</param>
        /// <returns>表示用演算子文字</returns>
        private char ConvertOperatorForDisplay(char operatorChar)
        {
            return operatorChar switch
            {
                '*' => '×',
                '/' => '÷',
                _ => operatorChar
            };
        }

        /// <summary>
        /// 入力文字列の末尾から、最後の演算子以降のオペランド部分を取り除きます
        /// </summary>
        /// <param name="input">入力文字列</param>
        /// <returns>末尾のオペランドを除いた文字列</returns>
        private string RemoveTrailingOperand(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // 末尾から逆順に検索して、最後の演算子の位置を見つける
            int lastOperatorIndex = -1;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (IsOperator(input[i]))
                {
                    lastOperatorIndex = i;
                    break;
                }
            }

            // 演算子が見つからない場合（入力が単なるオペランド）は空文字を返す
            if (lastOperatorIndex == -1)
            {
                return string.Empty;
            }

            // 最後の演算子までの部分を返す（演算子を含む）
            return input.Substring(0, lastOperatorIndex + 1);
        }

        /// <summary>
        /// 指定された文字が演算子かどうかを判定します
        /// </summary>
        /// <param name="c">判定する文字</param>
        /// <returns>演算子の場合true</returns>
        private bool IsOperator(char c)
        {
            return c == '+' || c == '-' || c == '*' || c == '/';
        }
    }
}
