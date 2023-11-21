using Cysharp.Threading.Tasks;
using External.Framework;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay
{
    public class GameFlowPm: BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveEvent<LevelFlowState> changeLevelFlowState;
            public ReactiveTrigger startLevel;
            public ReactiveTrigger startGame;
        }

        private readonly Ctx _ctx;
        
        public GameFlowPm(Ctx ctx)
        {
            _ctx = ctx;
            AddToDisposables(_ctx.startLevel.Subscribe(() =>_ctx.levelFlowState.Value = LevelFlowState.PlayerIdle));
            AddToDisposables(_ctx.changeLevelFlowState.SubscribeWithSkip(x => _ctx.levelFlowState.Value = x));
            StartGame();
        }
        
        private async void StartGame()
        {
            await UniTask.DelayFrame(1); 
            _ctx.startGame.Notify(); 
        }
    }
}