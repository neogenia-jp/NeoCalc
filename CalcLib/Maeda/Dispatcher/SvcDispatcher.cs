using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalcLib.Maeda.Basis;

namespace CalcLib.Maeda.Dispatcher
{
    /// <summary>
    /// 複数のコンテキストを包含するマルチなコンテキストクラス
    /// </summary>
    public class MultiContext : ICalcContext
    {
        /// <summary>
        /// 包含しているコンテキスト
        /// </summary>
        public ICalcContext[] Items { get; }

        /// <summary>
        /// サービスクラス名の一覧
        /// </summary>
        public IList<string> SvcClassNames { get; }

        /// <summary>
        /// 現在のコンテキストID
        /// </summary>
        public int CurrentId { get; private set; } = 0;

        /// <summary>
        /// コンテキストスイッチを行う
        /// </summary>
        /// <param name="id"></param>
        public void Switch(int id) => CurrentId = id;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="svcClassNames"></param>
        public MultiContext(IEnumerable<string> svcClassNames)
        {
            SvcClassNames = svcClassNames.ToList();
            Items = new ICalcContext[SvcClassNames.Count];
        }

        /// <summary>
        /// 現在のコンテキストを取得する（コンテキストが未生成であれば内部で生成してくれる）
        /// </summary>
        /// <returns></returns>
        public ICalcContext Get()
        {
            if (Items[CurrentId] == null)
            {
                Items[CurrentId] = Util.SvcActivator.GetOrCreate(SvcClassNames[CurrentId]).CreateContext();
            }
            return Items[CurrentId];
        }

        public string DisplayText => Get().DisplayText;  // 現在のコンテキストに委譲


        public string SubDisplayText => Get().SubDisplayText;  // 現在のコンテキストに委譲
    }

    /// <summary>
    /// 複数のバックエンドサービスを切り替えながら処理を振り分けてくれるすごいやつ。
    /// </summary>
    internal class SvcDispatcher
    {
        /// <summary>
        /// バックエンドサービスの一覧を管理しているDB
        /// </summary>
        SvcTree Backends;

        SvcHolder DefaultSvc, CurrentSvc;

        public Func<MultiContext> CreateContext { get; }

        public string GetExtButtonText(int num) => Backends[num - 1]?.ButtonText;

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="serviceNames"></param>
        void Init(IEnumerable<string> serviceNames)
        {
            if (Backends != null) return;
            Backends = new SvcTree();
            int offset = 0;
            foreach (var name in serviceNames)
            {
                var svc = Util.SvcActivator.GetOrCreate(name);
                var adapter = new SvcBtnConvertAdapter(svc);
                foreach (var b in adapter.GetExtButtons(offset++))
                {
                    Backends.AddChildWithParent(b);
                }
            }
            DefaultSvc = CurrentSvc = new SvcHolder(0, Backends.First().GetSvc());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="serviceNames"></param>
        public SvcDispatcher(IEnumerable<string> serviceNames)
        {
            CreateContext = () => new MultiContext(serviceNames);
            Init(serviceNames);
        }

        /// <summary>
        /// 押下されたボタンに応じて、適切なサービスとコンテキストに処理を振り分ける
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        void _Dispatch(MultiContext ctx, CalcButton btn)
        {
            var s = Backends.Find(btn)?.Dup();  // 一覧より割り込み可能なサービスがあるか探す
            if (s != null)
            {
                // 割り込みがあれば、現在のサービスと差し替える。コンテキストも切り替える。
                var evt = new SvcSwichedEventArg(ctx.Get());
                CurrentSvc = s;
                ctx.Switch(CurrentSvc.Id);
                s.Svc.OnEnter(ctx.Get(), evt);
            }

            // 現在のサービスに対して処理を実行させる
            var ret = CurrentSvc.Svc.TryButtonClick(ctx.Get(), btn);  // false が返されたらデフォルトサービスに復帰という意味
            if (!ret)
            {
                // デフォルトサービスに復帰する。コンテキストも切り替える。
                var evt = new SvcSwichedEventArg(ctx.Get());
                CurrentSvc = DefaultSvc;
                ctx.Switch(CurrentSvc.Id);
                // イベント発行
                DefaultSvc.Svc.OnEnter(ctx.Get(), evt);
                // サービスを呼ぶ
                CurrentSvc.Svc.TryButtonClick(ctx.Get(), btn);
            }
        }

        /// <summary>
        /// ディスパッチ処理の外部に対するエントリポイント
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="btn"></param>
        public void Dispatch(ICalcContext ctx, CalcButton btn) => _Dispatch(ctx as MultiContext, btn);
    }
}
