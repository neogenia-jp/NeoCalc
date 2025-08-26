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
            // 結果表示中はそのまま返す
            if (isResultDisplayed)
            {
                return rowInput;
            }
            
            // 空の入力の場合は"0"を返す
            if (string.IsNullOrEmpty(rowInput))
            {
                return "0";
            }
            
            try 
            {
                var result = parseResult.Evaluate();
                // 小数点以下の桁数を制限（13桁）
                if (result != Math.Floor(result))
                {
                    return Math.Round(result, 13).ToString();
                }
                return result.ToString();
            }
            catch 
            {
                // パーサーエラーが発生した場合は、現在の入力をそのまま表示
                return string.IsNullOrEmpty(rowInput) ? "0" : rowInput;
            }
        }
    }
}
