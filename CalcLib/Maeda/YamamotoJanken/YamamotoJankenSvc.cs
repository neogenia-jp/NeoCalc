using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CalcLib.Maeda.Basis;

namespace CalcLib.Maeda.YamamotoJanken
{
    /// <summary>
    /// ジャンケンの手のベースクラス
    /// </summary>
    internal abstract class YamamotoJankenHandBase
    {
        public JankenHands Hand { get; set; } = JankenHands.Unknown;

        /// <summary>
        /// ジャンケンの手
        /// </summary>
        public enum JankenHands
        {
            Unknown = 0,    // 何も出していない状態
            Gu,             // グー
            Choki,          // チョキ
            Pa,             // パー
        }

        /// <summary>
        /// ジャンケンの手の名称
        /// </summary>
        public virtual string Name()
        {
            switch(Hand)
            {
                case JankenHands.Unknown:
                    return "？？？";
                case JankenHands.Gu:
                    return "グー";
                case JankenHands.Choki:
                    return "チョキ";
                case JankenHands.Pa:
                    return "パー";
                default:
                    return null;
            }
        }

        /// <summary>
        /// 手が決定しているかどうか
        /// </summary>
        public bool IsDecided() => Hand != JankenHands.Unknown;

        /// <summary>
        /// ジャンケンの手をリセットする
        /// </summary>
        public void Reset() => Hand = JankenHands.Unknown;

        /// <summary>
        /// ジャンケンの手をランダムで設定する
        /// </summary>
        public abstract void RandomSet();
    }

    /// <summary>
    /// ジャンケンの手クラス
    /// </summary>
    internal class YamamotoJankenHand : YamamotoJankenHandBase
    {
        /// <summary>
        /// ジャンケンの手をランダムで設定する
        /// </summary>
        public override void RandomSet()
        {
            var r = new Random();
            Hand = (JankenHands)(r.Next(Enum.GetNames(typeof(JankenHands)).Length));
        }
    }
    /// <summary>
    /// じゃんけんサービスのためのコンテキスト
    /// </summary>
    internal class YamamotoJankenContext : ICalcContext
    {
        public string DisplayText => NPC.Name() + " vs " + Player.Name();

        public string SubDisplayText { get; set; }

        public YamamotoJankenHandBase Player { get; set; } = new YamamotoJankenHand();

        public YamamotoJankenHandBase NPC { get; set; } = new YamamotoJankenHand();

        public SvcState State { get; set; }

        /// <summary>
        /// 判定結果
        /// </summary>
        public enum JudgeResult
        {
            Lose = -1,  // 負け
            Draw = 0,   // 引き分け
            Win = 1,    // 勝ち
            None = 9,   // 判定不可
        }

        /// <summary>
        /// 初期処理
        /// </summary>
        public void Init()
        {
            SubDisplayText = "ジャンケン...";
            State = SvcState.Initialized;
        }

        /// <summary>
        /// 自分の手を入力しているかの判定
        /// </summary>
        /// <returns></returns>
        public bool IsInputedYourHand()
        {
            return Player.IsDecided();
        }

        /// <summary>
        /// 勝負の判定をできるかどうか
        /// </summary>
        /// <returns></returns>
        public bool CanJudge()
        {
            // どちらも手を出していれば判定可能
            return NPC.IsDecided() && Player.IsDecided();
        }

        /// <summary>
        /// 結果判定
        /// </summary>
        /// <returns></returns>
        public JudgeResult Judge()
        {
            // 勝負の判定ができない場合は終了
            if (!CanJudge()) return JudgeResult.None;

            var playerIndex = 1;
            var result = ConfirmResult(new List<YamamotoJankenHandBase>() { NPC, Player });
            if (result == null)
            {
                // あいこの場合
                return JudgeResult.Draw;
            }
            else if(result[0] == playerIndex)
            {
                // プレイヤーの勝利
                return JudgeResult.Win;
            }
            else
            {
                // NPCの勝利
                return JudgeResult.Lose;
            }
        }

        /// <summary>
        /// 結果表示
        /// </summary>
        /// <returns></returns>
        public void PrintResult(JudgeResult result)
        {
            switch(result)
            {
                // 負けた場合
                case JudgeResult.Lose:
                    SubDisplayText = "アナタノマケ！";
                    break;

                // 引き分けた場合
                case JudgeResult.Draw:
                    SubDisplayText = "アイコデショ...";
                    break;

                // 勝った場合
                case JudgeResult.Win:
                    SubDisplayText = "アナタノカチ！";
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// ジャンケン結果確認
        /// </summary>
        /// <param name="handList">ジャンケンの手（複数人に対応するためリスト）</param>
        /// <return>
        /// ジャンケンに勝った人のインデックスリスト
        /// あいこの場合はnullで返す
        /// 出していない人がいた場合は例外を返す
        /// </return>
        public List<int> ConfirmResult(List<YamamotoJankenHandBase> handList)
        {
            // それぞれの手の数をカウントする
            Func<YamamotoJankenHandBase.JankenHands, int> handCount =
                (target) => handList.Where(h => h.Hand == target).Count();
            var cntDict = new Dictionary<YamamotoJankenHandBase.JankenHands, int>()
            {
                { YamamotoJankenHandBase.JankenHands.Unknown, handCount(YamamotoJankenHandBase.JankenHands.Unknown) },
                { YamamotoJankenHandBase.JankenHands.Gu, handCount(YamamotoJankenHandBase.JankenHands.Gu) },
                { YamamotoJankenHandBase.JankenHands.Choki, handCount(YamamotoJankenHandBase.JankenHands.Choki) },
                { YamamotoJankenHandBase.JankenHands.Pa, handCount(YamamotoJankenHandBase.JankenHands.Pa) },
            };

            if(cntDict[YamamotoJankenHandBase.JankenHands.Unknown] > 0)
            {
                // 手を出していない人がいれば例外
                throw new ApplicationException("手を出していない人がいます!");
            }

            if(cntDict[YamamotoJankenHandBase.JankenHands.Gu] > 0 && cntDict[YamamotoJankenHandBase.JankenHands.Choki] > 0 && cntDict[YamamotoJankenHandBase.JankenHands.Pa] > 0)
            {
                // 全部の手がある場合はあいこ
                return null;
            }
            if(cntDict.Where(handCnt => handCnt.Value > 0).Count() == 1)
            {
                // 手が１種類しかない場合はあいこ
                return null;
            }

            // ジャンケンの手から勝った人だけを抽出するメソッドを定義
            List<int> result;
            Func<YamamotoJankenHandBase.JankenHands, List<int>> f = (h) => {
                return handList.Select((handBase, index) => new { HandBase = handBase, Index = index }).
                    Where(c => c.HandBase.Hand == h).
                    Select(c => c.Index).ToList();
            };
            if(cntDict[YamamotoJankenHandBase.JankenHands.Gu] == 0)
            {
                // グーがいなければチョキの勝ち
                result = f(YamamotoJankenHandBase.JankenHands.Choki);
            }
            else if(cntDict[YamamotoJankenHandBase.JankenHands.Choki] == 0)
            {
                // チョキがいなければパーの勝ち
                result = f(YamamotoJankenHandBase.JankenHands.Pa);
            }
            else
            {
                // パーがいなければグーの勝ち
                result = f(YamamotoJankenHandBase.JankenHands.Gu);
            }

            return result;
        }
    }

    /// <summary>
    /// じゃんけんサービス
    /// </summary>
    internal class YamamotoJankenSvc : SvcBase<YamamotoJankenContext>
    {
        public override string GetExtButtonText(int num) => num == 1 ? "じゃんけん" : null;

        internal override YamamotoJankenContext _CreateContext() => new YamamotoJankenContext();

        public override bool TryButtonClick(YamamotoJankenContext ctx, CalcButton btn)
        {
            switch (btn)
            {
                case CalcButton.BtnExt1:
                    if (ctx.State != SvcState.Unknown) return false;
                    ctx.Init();
                    break;
                case CalcButton.Btn1:
                    if (ctx.State == SvcState.Finished) return false;
                    ctx.Player.Hand = YamamotoJankenHandBase.JankenHands.Gu;
                    break;
                case CalcButton.Btn2:
                    if (ctx.State == SvcState.Finished) return false;
                    ctx.Player.Hand = YamamotoJankenHandBase.JankenHands.Choki;
                    break;
                case CalcButton.Btn3:
                    if (ctx.State == SvcState.Finished) return false;
                    ctx.Player.Hand = YamamotoJankenHandBase.JankenHands.Pa;
                    break;
                case CalcButton.BtnEqual:
                    if (ctx.State == SvcState.Finished) return false;
                    ctx.NPC.RandomSet();
                    break;
                case CalcButton.BtnClear:
                case CalcButton.BtnClearEnd:
                    return false;
                default:
                    if (ctx.State == SvcState.Finished) return false;
                    break;
            }

            // 結果判定
            var result = ctx.Judge();

            // 結果表示
            ctx.PrintResult(result);

            // 状態遷移
            switch(result)
            {
                case YamamotoJankenContext.JudgeResult.Lose:
                case YamamotoJankenContext.JudgeResult.Win:
                    ctx.State = SvcState.Finished;
                    break;

                case YamamotoJankenContext.JudgeResult.Draw:
                    ctx.NPC.Hand = YamamotoJankenHandBase.JankenHands.Unknown;
                    ctx.Player.Hand = YamamotoJankenHandBase.JankenHands.Unknown;
                    ctx.State = SvcState.Running;
                    break;

                default:
                    ctx.State = SvcState.Running;
                    break;
            }

            return true;
        }

        protected override void OnExitSvc(YamamotoJankenContext ctx)
        {
            ctx.NPC.Reset();
            ctx.Player.Reset();
            ctx.State = SvcState.Unknown;
        }
    }
}
