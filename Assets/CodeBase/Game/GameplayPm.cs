using CodeBase.Game.Gameplay.Camera;
using CodeBase.Game.Gameplay.ScoreCounter;
using Cysharp.Threading.Tasks;
using External.Framework;
using External.Reactive;

namespace CodeBase.Game
{
    public class GameplayPm : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveTrigger startGame;
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
        }
    }
}