using CodeBase.Data;
using External.Framework;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.Stick
{
    public class StickEntity : BaseDisposable
    {
        public struct Ctx
        {
            public IContentProvider contentProvider; 
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public IReadOnlyReactiveProperty<LevelFlowState> levelFlowState;
            public ReactiveProperty<float> stickLength;
            public IReadOnlyReactiveTrigger startLevel;
            public IReadOnlyReactiveProperty<bool> columnIsReachable;
            public ReactiveEvent<LevelFlowState> changeLevelFlowState;
        }

        private readonly Ctx _ctx;
        private StickPm _pm;

        public StickEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreatePm();
        }

        private void CreatePm()
        {
            var stickPmCtx = new StickPm.Ctx()
            {
                levelFlowState = _ctx.levelFlowState,
                startLevel = _ctx.startLevel,
                columnIsReachable = _ctx.columnIsReachable,
                changeLevelFlowState = _ctx.changeLevelFlowState,
                stickLength = _ctx.stickLength,
                contentProvider = _ctx.contentProvider,
                actualColumnXPosition = _ctx.actualColumnXPosition
            };
            _pm = new StickPm(stickPmCtx);
            AddToDisposables(_pm);
        }
        
        
    }
}