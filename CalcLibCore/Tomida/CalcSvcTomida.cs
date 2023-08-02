using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalcLibCore.Tomida;
using CalcLibCore.Tomida.Domain;
using CalcLibCore.Tomida.Commands;

namespace CalcLib.Tomida
{
    internal class CalcSvcTomida : ICalcSvc
    {
        public virtual ICalcContext CreateContext() => new CalcContextTomidaEx();

        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctxEx = ctx0 as CalcContextTomidaEx;
            if (ctxEx is null) return;  // コンテキストがnullだったら何もしない

            // 親コンテキストからコマンドを生成
            // コマンドがnullだったら現在コンテキストの処理へ移行
            // そうでないなら生成されたコマンドを実行して終了
            var switchCommand = ctxEx.Factory.Create(btn);
            if(switchCommand != null)
            {
                switchCommand.Execute(ctxEx);
                return;
            }

            // 現在コンテキストのコマンド生成と実行
            var ctx = ctxEx.Current;
            if (ctx is null) return;    // コンテキストがnullだったら何もしない
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");
            {
                var command = ctx.Factory.Create(btn);
                if (command is null) return;    // ファクトリのcommand生成に失敗したら何もしない
                command.Execute(ctx);
            }
        }
    }
}
