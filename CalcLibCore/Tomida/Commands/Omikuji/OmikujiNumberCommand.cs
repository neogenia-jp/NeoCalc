using System;
using CalcLib;


namespace CalcLibCore.Tomida.Commands.Omikuji
{
    [ButtonCommand(CalcButton.Btn1)]
    [ButtonCommand(CalcButton.Btn2)]
    [ButtonCommand(CalcButton.Btn3)]
    [ButtonCommand(CalcButton.Btn4)]
    public class OmikujiNumberCommand : OmikujiButtonCommandBase
    {
        public OmikujiNumberCommand(CalcButton btn) : base(btn)
        {
        }

        public override void Execute(OmikujiContext ctx)
        {
            switch (ctx.State)
            {
                case OmikujiState.BeforeLotted:
                    int idx = 0;
                    switch (Btn)
                    {
                        case CalcButton.Btn1:
                            idx = 0;
                            break;
                        case CalcButton.Btn2:
                            idx = 1;
                            break;
                        case CalcButton.Btn3:
                            idx = 2;
                            break;
                        case CalcButton.Btn4:
                            idx = 3;
                            break;

                    }
                    ctx.SelectedIndex = idx;
                    break;
                case OmikujiState.AfterLotted:
                    var ctxEx = ctx.Parent as CalcContextTomidaEx;
                    ctxEx?.UnstackContext();
                    ctxEx?.ButtonQueue.Enqueue(Btn);
                    break;

            }
        }
    }
}

