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

            Debug.WriteLine($"Button Clicked {btn}, context={ctxEx.Current}");
            ctxEx.ButtonQueue.Enqueue(btn);
            _QueueButton(ctxEx);

        }

        private void _QueueButton(CalcContextTomidaEx ctx0)
        {
            // デキューできたらボタン処理する
            if(ctx0.ButtonQueue.Count != 0)
            {
                var button = ctx0.ButtonQueue.Dequeue();
                _OnButtonClickImpl(ctx0, button);
            }
        }
        private void _OnButtonClickImpl(CalcContextTomidaEx ctx0, CalcButton btn)
        {
            var ctxEx = ctx0;
            if (ctxEx is null) return;  // コンテキストがnullだったら何もしない



            Debug.WriteLine($"Button Clicked {btn}, context={ctxEx.Current}");

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
            {
                var command = ctx.Factory.Create(btn);
                if (command is null) return;    // ファクトリのcommand生成に失敗したら何もしない
                command.Execute(ctx);
            }
            // 最後に再起的にデキューを呼び出す
            _QueueButton(ctx0);
        }
    }
}
