using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalcLibCore.Tomida2.Calc.Interpreter;

namespace CalcLibCoreTest.Tomida2
{
    [TestClass]
    public class ParserExampleTest
    {
        [TestMethod]
        public void RunParserExamples()
        {
            // ParserExampleのRunExamplesメソッドを実行
            ParserExample.RunExamples();
        }
    }
}
