using System;
using CalcLib;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace CalcLibCore.Tomida.Commands
{
    public class ButtonCommandFactory
    {
        /// <summary>
        /// 引数のbtnに対応するCommandインスタンスを返します。
        /// </summary>
        /// <param name="btn">クリックされたボタン</param>
        /// <returns>ボタンに対応するCommandインスタンス</returns>
        /// <exception cref="ArgumentException">対応するボタンがない場合</exception>
        public static ButtonCommandBase? Create(CalcButton btn)
        {
            try
            {
                // ICalcCommandインタフェースを実装しているクラス情報をすべて取得する
                var commandClasses =
                    Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(c => c.GetInterfaces()
                        .Any(t => t == typeof(ICalcCommand))
                    ).ToArray();
                // 対象のbtnをattributeに持つクラスを特定する
                // attributeは各command同士で重複指定されていないことが前提
                var targetClass =
                    commandClasses
                    .Where(c => c.GetCustomAttributes<ButtonCommandAttribute>()
                        .Any(attr => attr.DependencyButton == btn)
                    ).First();
                // 特定したクラスのTypeからインスタンスを生成して返す
                var args = new Object[1] { btn };
                return Activator.CreateInstance(targetClass, args) as ButtonCommandBase;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new ArgumentException("ファクトリーに登録されていないボタンです");
            }

        }
    }
}

