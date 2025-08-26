using CalcLibCore.Tomida2.Calc.Interpreter;
using System;

namespace CalcLibCore.Tomida2.Calc.implements
{
    /// <summary>
    /// DisplayTextの表示処理を担当するクラス
    /// </summary>
    internal class DisplayTextImpl
    {
        /// <summary>
        /// 表示用のテキストを生成します
        /// </summary>
        /// <param name="rowInput">現在の入力内容</param>
        /// <param name="isResultDisplayed">結果表示中かどうか</param>
        /// <param name="parseResult">パース結果</param>
        /// <returns>表示用テキスト</returns>
        public string ToDisplay(string rowInput, bool isResultDisplayed, IParseResult parseResult)
        {
            // 結果表示中はそのまま返す（既に計算済みの値）
            if (isResultDisplayed)
            {
                return FormatCalculationResult(rowInput);
            }
            
            // 空の入力の場合は"0"を返す
            if (string.IsNullOrEmpty(rowInput))
            {
                return "0";
            }
            
            try 
            {
                // 入力の最後の部分を分析
                var lastToken = GetLastToken(rowInput);
                
                // 小数点の重複処理（例：".."を"0."として扱う）
                if (lastToken.Contains(".."))
                {
                    return "0.";
                }
                
                // 最後がオペランド（数値）の場合はそのオペランドを表示
                if (IsNumericToken(lastToken) || lastToken.EndsWith(".") || lastToken == ".")
                {
                    return FormatInputNumber(lastToken);
                }
                
                // 最後が演算子の場合は中間結果を表示
                if (IsOperator(lastToken))
                {
                    // パーサーエラーが発生する可能性があるので、手動で中間結果を計算
                    var intermediateResult = CalculateIntermediateResult(rowInput);
                    return FormatCalculationResult(intermediateResult.ToString());
                }
                
                // その他の場合も評価結果を表示
                var evalResult = parseResult.Evaluate();
                return FormatCalculationResult(evalResult.ToString());
            }
            catch 
            {
                // パーサーエラーが発生した場合は、手動計算を試行
                try
                {
                    var fallbackResult = CalculateIntermediateResult(rowInput);
                    return FormatCalculationResult(fallbackResult.ToString());
                }
                catch
                {
                    // それでも失敗した場合は入力をそのまま表示
                    return string.IsNullOrEmpty(rowInput) ? "0" : rowInput;
                }
            }
        }

        /// <summary>
        /// 末尾が演算子の式から中間結果を計算します
        /// </summary>
        /// <param name="input">入力式</param>
        /// <returns>中間結果</returns>
        private decimal CalculateIntermediateResult(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0;

            // 末尾の演算子を除去
            string expressionWithoutLastOperator = input.TrimEnd();
            if (IsOperator(expressionWithoutLastOperator[expressionWithoutLastOperator.Length - 1].ToString()))
            {
                expressionWithoutLastOperator = expressionWithoutLastOperator.Substring(0, expressionWithoutLastOperator.Length - 1);
            }

            if (string.IsNullOrEmpty(expressionWithoutLastOperator))
                return 0;

            // 簡単な二項演算を手動で計算
            return ParseAndCalculateSimpleExpression(expressionWithoutLastOperator);
        }

        /// <summary>
        /// 簡単な式を手動で計算します
        /// </summary>
        /// <param name="expression">計算する式</param>
        /// <returns>計算結果</returns>
        private decimal ParseAndCalculateSimpleExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return 0;

            // 演算子で分割
            var operators = new[] { '+', '-', '*', '/' };
            foreach (var op in operators)
            {
                int lastOpIndex = expression.LastIndexOf(op);
                if (lastOpIndex > 0) // 先頭の負号は除く
                {
                    string leftPart = expression.Substring(0, lastOpIndex).Trim();
                    string rightPart = expression.Substring(lastOpIndex + 1).Trim();

                    if (decimal.TryParse(leftPart, out decimal left) && decimal.TryParse(rightPart, out decimal right))
                    {
                        return op switch
                        {
                            '+' => left + right,
                            '-' => left - right,
                            '*' => left * right,
                            '/' => right != 0 ? left / right : throw new DivideByZeroException(),
                            _ => 0
                        };
                    }
                    else
                    {
                        // 左側が複雑な式の場合は再帰的に計算
                        decimal leftResult = ParseAndCalculateSimpleExpression(leftPart);
                        if (decimal.TryParse(rightPart, out decimal rightValue))
                        {
                            return op switch
                            {
                                '+' => leftResult + rightValue,
                                '-' => leftResult - rightValue,
                                '*' => leftResult * rightValue,
                                '/' => rightValue != 0 ? leftResult / rightValue : throw new DivideByZeroException(),
                                _ => 0
                            };
                        }
                    }
                }
            }

            // 演算子がない場合は単一の数値
            if (decimal.TryParse(expression, out decimal result))
                return result;

            return 0;
        }

        /// <summary>
        /// 入力文字列の最後のトークンを取得します
        /// </summary>
        /// <param name="input">入力文字列</param>
        /// <returns>最後のトークン</returns>
        private string GetLastToken(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // 末尾から逆順に検索して最後のトークンを見つける
            int lastOperatorIndex = -1;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (IsOperator(input[i].ToString()))
                {
                    lastOperatorIndex = i;
                    break;
                }
            }

            if (lastOperatorIndex == -1)
            {
                // 演算子がない場合は全体が数値
                return input.Trim();
            }
            else if (lastOperatorIndex == input.Length - 1)
            {
                // 末尾が演算子
                return input[lastOperatorIndex].ToString();
            }
            else
            {
                // 最後の演算子以降が数値
                return input.Substring(lastOperatorIndex + 1).Trim();
            }
        }

        /// <summary>
        /// トークンが数値かどうかを判定します
        /// </summary>
        /// <param name="token">判定するトークン</param>
        /// <returns>数値の場合true</returns>
        private bool IsNumericToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            return double.TryParse(token, out _);
        }

        /// <summary>
        /// 文字列が演算子かどうかを判定します
        /// </summary>
        /// <param name="token">判定する文字列</param>
        /// <returns>演算子の場合true</returns>
        private bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/";
        }

        /// <summary>
        /// 数値を適切な形式でフォーマットします
        /// </summary>
        /// <param name="value">フォーマットする値</param>
        /// <returns>フォーマットされた文字列</returns>
        private string FormatNumber(string value)
        {
            // 末尾が小数点のみの場合（例：1000000.）を先に処理
            if (value.EndsWith(".") && double.TryParse(value.TrimEnd('.'), out double dotNum))
            {
                return dotNum.ToString("N0") + ".";
            }
            
            if (double.TryParse(value, out double num))
            {
                // 小数点以下がある場合
                if (num != Math.Floor(num))
                {
                    var rounded = Math.Round(num, 13);
                    // 整数部分と小数部分を分離してフォーマット
                    var integerPart = Math.Floor(Math.Abs(rounded));
                    var decimalPart = Math.Abs(rounded) - integerPart;
                    
                    // 整数部分を3桁ごとにカンマ区切り
                    string formattedIntegerPart = integerPart.ToString("N0");
                    
                    // 負数の場合は符号を追加
                    if (rounded < 0)
                    {
                        formattedIntegerPart = "-" + formattedIntegerPart;
                    }
                    
                    // 小数部分を文字列に変換（先頭の"0."を除去）
                    string decimalString = decimalPart.ToString();
                    if (decimalString.StartsWith("0."))
                    {
                        decimalString = decimalString.Substring(1); // "."以降を取得
                    }
                    else
                    {
                        decimalString = "";
                    }
                    
                    return formattedIntegerPart + decimalString;
                }
                
                // 整数の場合は3桁ごとにカンマ区切り
                return num.ToString("N0");
            }
            
            return value;
        }

        /// <summary>
        /// 入力中の数値をフォーマットします（元の形式を保持）
        /// </summary>
        /// <param name="value">フォーマットする値</param>
        /// <returns>フォーマットされた文字列</returns>
        private string FormatInputNumber(string value)
        {
            // ".."などの連続小数点を"0."として処理
            if (value.Contains(".."))
            {
                return "0.";
            }

            // "."のみの場合は"0."として処理
            if (value == ".")
            {
                return "0.";
            }

            // 末尾が小数点のみの場合（例：1000.）
            if (value.EndsWith(".") && !value.StartsWith("."))
            {
                string integerPart = value.TrimEnd('.');
                if (double.TryParse(integerPart, out double dotNum))
                {
                    return dotNum.ToString("N0") + ".";
                }
                return value;
            }

            // 小数点を含む場合
            if (value.Contains("."))
            {
                string[] parts = value.Split('.');
                if (parts.Length == 2)
                {
                    // 整数部分が空の場合（例：".1"）
                    if (string.IsNullOrEmpty(parts[0]))
                    {
                        return "0." + parts[1];
                    }
                    
                    if (double.TryParse(parts[0], out double intPart))
                    {
                        string formattedIntPart = intPart.ToString("N0");
                        return formattedIntPart + "." + parts[1];
                    }
                }
                return value;
            }

            // 整数の場合
            if (double.TryParse(value, out double num))
            {
                return num.ToString("N0");
            }

            return value;
        }

        /// <summary>
        /// 計算結果をフォーマットします
        /// </summary>
        /// <param name="value">フォーマットする値</param>
        /// <returns>フォーマットされた文字列</returns>
        private string FormatCalculationResult(string value)
        {
            if (double.TryParse(value, out double num))
            {
                // 極小の値は0にする
                if (Math.Abs(num) < 1e-13)
                {
                    return "0";
                }

                // 小数点以下がある場合
                if (num != Math.Floor(num))
                {
                    var rounded = Math.Round(num, 13);
                    
                    // 小数点以下13桁まで表示（科学記数法を避ける）
                    string formattedValue = rounded.ToString("F13").TrimEnd('0');
                    if (formattedValue.EndsWith("."))
                    {
                        formattedValue = formattedValue.TrimEnd('.');
                    }
                    
                    // 整数部分と小数部分を分離
                    if (formattedValue.Contains("."))
                    {
                        string[] parts = formattedValue.Split('.');
                        if (double.TryParse(parts[0], out double intPart))
                        {
                            string formattedIntPart = intPart.ToString("N0");
                            return formattedIntPart + "." + parts[1];
                        }
                    }
                    else
                    {
                        // 小数部分がない場合（整数になった場合）
                        if (double.TryParse(formattedValue, out double integerValue))
                        {
                            return integerValue.ToString("N0");
                        }
                    }
                    
                    return formattedValue;
                }
                
                // 整数の場合は3桁ごとにカンマ区切り
                return num.ToString("N0");
            }
            
            return value;
        }
    }
}
