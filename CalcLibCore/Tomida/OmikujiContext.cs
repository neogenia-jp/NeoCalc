using System;
using CalcLib;
using CalcLibCore.Tomida.Commands;
using CalcLibCore.Tomida.Commands.Omikuji;
using CalcLibCore.Tomida.Extensions;

namespace CalcLibCore.Tomida
{
	public class OmikujiContext: ICalcContextEx
	{
        private static string[] _SUITS_DEFAULT = new string[] { "大吉", "中吉", "小吉", "凶" };

        private string[] _suits;

		public OmikujiContext()
		{
            Factory = new OmikujiCommandFactory();
            _suits = _SUITS_DEFAULT.Shuffle();
		}

        public string DisplayText => DisplayTextImpl();

        private string DisplayTextImpl()
        {
            throw new NotImplementedException();
        }

        public string SubDisplayText => SubDisplayTextImpl();

        private string SubDisplayTextImpl()
        {
            throw new NotImplementedException();
        }

        public CalcConstants.State State => GetState();

        public IFactory Factory { get; }

        private CalcConstants.State GetState()
        {
            throw new NotImplementedException();
        }
    }
}

