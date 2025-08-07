using CalcLibCore.Tomida2.Calc.Interpreter;
using System;

namespace CalcLibCoreTest.Tomida2
{
    /// <summary>
    /// 連続した演算のパーサーテスト
    /// </summary>
    public class ChainedExpressionTest
    {
        public static void RunTests()
        {
            Console.WriteLine("=== 連続演算パーサーテスト ===");
            
            var parser = new CalculatorParser();

            // 基本的な連続演算のテスト
            TestChainedExpression(parser, "1+2+3", 6);
            TestChainedExpression(parser, "10-5+2", 7);
            TestChainedExpression(parser, "2*3*4", 24);
            TestChainedExpression(parser, "100/4/5", 5);
            
            // 混合演算のテスト（左結合）
            TestChainedExpression(parser, "1+2*3", 9);  // (1+2)*3
            TestChainedExpression(parser, "10-5*2", 10); // (10-5)*2
            TestChainedExpression(parser, "8/2+3", 7);   // (8/2)+3
            TestChainedExpression(parser, "6+4/2", 5);   // (6+4)/2
            
            // より長い連続演算
            TestChainedExpression(parser, "1+2+3+4+5", 15);
            TestChainedExpression(parser, "20-5-3-2", 10);
            TestChainedExpression(parser, "2*3*2*2", 24);
            
            // 小数点を含む連続演算
            TestChainedExpression(parser, "1.5+2.5+3.0", 7.0m);
            TestChainedExpression(parser, "10.0/2.0/2.5", 2.0m);

            Console.WriteLine("=== テスト完了 ===");
        }

        private static void TestChainedExpression(CalculatorParser parser, string input, decimal expected)
        {
            try
            {
                // イコール記号付きでテスト
                string completeInput = input + "=";
                decimal result = parser.Evaluate(completeInput);
                
                if (result == expected)
                {
                    Console.WriteLine($"✓ {input} = {result}");
                }
                else
                {
                    Console.WriteLine($"✗ {input} = {result} (期待値: {expected})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ {input} エラー: {ex.Message}");
            }
        }
    }
}
