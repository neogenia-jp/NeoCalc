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

            var ctx = ctxEx.Current;

            // 拡張ボタンの処理
            // おみくじボタンを押された場合、現在のコンテキストを変更する処理を行う
            if(btn == CalcButton.BtnExt2)
            {
                if(ctx.GetType() == typeof(OmikujiContext)){
                    ctxEx.UnstackContext();
                }
                else
                {
                    ctxEx.StackContext(new OmikujiContext());
                }
                return;
            }

            if (ctx is null) return;    // コンテキストがnullだったら何もしない
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");
            if(ctx.GetType() == typeof(CalcContextTomida))
            {
                // 押されたボタンに対応するコマンドオブジェクトをファクトリーで生成
                // Executeする
                var command = ctx.Factory.Create(btn);
                if (command is null) return;    // ファクトリのcommand生成に失敗したら何もしない
                command.Execute((CalcContextTomida)ctx);
            }

        }
    }
}
