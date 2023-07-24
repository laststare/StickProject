using Cysharp.Threading.Tasks;
using External.Framework;
using External.Reactive;
using UniRx;

namespace CodeBase.Game
{
    public class GameplayPm : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveTrigger startGame;
            public ReactiveProperty<LevelFlowState> levelFlowState;
        }

        private readonly Ctx _ctx;
      

        public GameplayPm(Ctx ctx)
        {
            _ctx = ctx;
            StartGame();
        }

        private async void StartGame()
        {
            await UniTask.DelayFrame(1); 
            _ctx.startGame.Notify();
            _ctx.levelFlowState.Value = LevelFlowState.PlayerIdle;
        }
    }
}