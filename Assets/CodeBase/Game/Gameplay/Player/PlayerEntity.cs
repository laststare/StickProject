using CodeBase.Data;
using External.Framework;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.Player
{
    public class PlayerEntity : BaseDisposable
    {
        public struct Ctx
        {
            public IContentProvider contentProvider; 
            public IReadOnlyReactiveTrigger startLevel;
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public IReadOnlyReactiveProperty<float> nextColumnXPosition;
            public IReadOnlyReactiveProperty<LevelFlowState> levelFlowState;
            public ReactiveProperty<float> stickLength;
            public ReactiveTrigger finishLevel;
            public ReactiveProperty<bool> columnIsReachable;
            public ReactiveEvent<LevelFlowState> changeLevelFlowState;
        }
        private readonly Ctx _ctx;
        private PlayerPm _pm;
        
        public PlayerEntity (Ctx ctx)
        {
            _ctx = ctx;
            CreatePm();
        }

        private void CreatePm()
        {
            var playerPmCtx = new PlayerPm.Ctx()
            {
                nextColumnXPosition = _ctx.nextColumnXPosition,
                levelFlowState = _ctx.levelFlowState,
                stickLength = _ctx.stickLength,
                actualColumnXPosition = _ctx.actualColumnXPosition,
                finishLevel = _ctx.finishLevel,
                columnIsReachable = _ctx.columnIsReachable,
                changeLevelFlowState = _ctx.changeLevelFlowState,
                contentProvider = _ctx.contentProvider,
                startLevel = _ctx.startLevel,
            };
            _pm = new PlayerPm(playerPmCtx);
            AddToDisposables(_pm);
        }
        
    }
}