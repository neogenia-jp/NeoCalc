using System;
using CalcLib.Mori.Display;

namespace CalcLib.Mori
{
    // Subjectインターフェース
    internal interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    // Observerインターフェース
    internal interface IObserver
    {
        void Update(ISubject subject);
    }

    internal class DisplayObserver : IObserver
    {
        private readonly CalcContextExtend _context;
        private readonly IDisplayFormatter _formatter;

        public DisplayObserver(CalcContextExtend context, IDisplayFormatter formatter)
        {
            _context = context;
            _formatter = formatter;
        }

        public void Update(ISubject subject)
        {
            if (subject is CalcContextExtend)
            {
                var source = _context.DisplaySource;
                var view = _formatter.Format(source);
                _context.DisplayText = view.Main;
                _context.SubDisplayText = view.Sub;
            }
        }
    }
}
