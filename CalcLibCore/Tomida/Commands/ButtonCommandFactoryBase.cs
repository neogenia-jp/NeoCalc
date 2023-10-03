using System;
using CalcLib;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace CalcLibCore.Tomida.Commands
{
    public abstract class ButtonCommandFactoryBase : IFactory
    {
        public abstract ButtonCommandBase? Create(CalcButton btn);

        /// <summary>
        /// 引数のbtnに対応するCommandインスタンスを返します。
        /// </summary>
        /// <param name="btn">クリックされたボタン</param>
        /// <returns>ボタンに対応するCommandインスタンス</returns>
        /// <exception cref="ArgumentException">対応するボタンがない場合</exception>
        public ButtonCommandBase? Create<I>(CalcButton btn)
        {
            try
            {
                // ICalcCommandインタフェースを実装しているクラス情報をすべて取得する
                var commandClasses =
                    Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(c => c.GetInterfaces()
                        .Any(t => t == typeof(I))
                    ).ToArray();
                // 対象のbtnをattributeに持つクラスを特定する
                // attributeは各command同士で重複指定されていないことが前提
                var targetClass =
                    commandClasses
                    .Where(c => c.GetCustomAttributes<ButtonCommandAttribute>()
                        .Any(attr => attr.DependencyButton == btn)
                    ).FirstOrDefault();
                // 特定したクラスのTypeからインスタンスを生成して返す
                if(targetClass == null)
                {
                    return null;
                }
                else
                {
                    var args = new Object[] { btn };
                    return Activator.CreateInstance(targetClass, args) as ButtonCommandBase;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new ArgumentException("ファクトリーに登録されていないボタンです");
            }

        }
    }
}

