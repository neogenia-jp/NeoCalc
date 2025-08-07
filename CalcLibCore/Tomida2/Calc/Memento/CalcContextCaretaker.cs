using System.Collections.Generic;
using System.Linq;

namespace CalcLibCore.Tomida2.Calc.Memento
{
    /// <summary>
    /// Mementoを管理するCaretakerクラス
    /// </summary>
    internal class CalcContextCaretaker
    {
        private readonly List<CalcContextMemento> _mementos = new();
        private const int MaxHistorySize = int.MaxValue; // 最大履歴サイズ

        /// <summary>
        /// Mementoを保存します
        /// </summary>
        /// <param name="memento">保存するMemento</param>
        public void SaveMemento(CalcContextMemento memento)
        {
            _mementos.Add(memento);
            
            // 履歴サイズを制限
            if (_mementos.Count > MaxHistorySize)
            {
                _mementos.RemoveAt(0);
            }
        }

        /// <summary>
        /// 最後に保存されたMementoを取得し、履歴から削除します
        /// </summary>
        /// <returns>最後に保存されたMemento、存在しない場合はnull</returns>
        public CalcContextMemento? GetLastMemento()
        {
            if (_mementos.Count == 0)
                return null;

            var lastMemento = _mementos.Last();
            _mementos.RemoveAt(_mementos.Count - 1);
            return lastMemento;
        }

        /// <summary>
        /// Undo可能かどうかを確認します
        /// </summary>
        /// <returns>Undo可能な場合はtrue</returns>
        public bool CanUndo => _mementos.Count > 0;

        /// <summary>
        /// 履歴をクリアします
        /// </summary>
        public void ClearHistory()
        {
            _mementos.Clear();
        }
    }
}
