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
        public virtual ICalcContext CreateContext() => new CalcContextTomida();

        public virtual void OnButtonClick(ICalcContext ctx0, CalcButton btn)
        {
            var ctx = ctx0 as CalcContextTomida;
            if (ctx is null) return;    // コンテキストがnullだったら何もしない
            Debug.WriteLine($"Button Clicked {btn}, context={ctx}");

            // 押されたボタンに対応するコマンドオブジェクトをファクトリーで生成
            // Executeする
            var command = ButtonCommandFactory.Create(btn);
            if (command is null) return;    // ファクトリのcommand生成に失敗したら何もしない
            command.Execute(ctx);
        }
    }
}
