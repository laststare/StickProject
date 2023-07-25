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
            public ContentProvider contentProvider; 
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public IReadOnlyReactiveProperty<LevelFlowState> levelFlowState;
            public ReactiveProperty<float> stickLength;
            public IReadOnlyReactiveTrigger startLevel;
            public IReadOnlyReactiveProperty<bool> columnIsReachable;
            public ReactiveEvent<LevelFlowState> changeLevelFlowState;
        }

        private readonly Ctx _ctx;
        private StickPm _pm;
        private readonly ReactiveTrigger _createView = new();
        private readonly ReactiveCollection<GameObject> _spawnedSticks = new();
        private readonly ReactiveTrigger _stickIsDown = new();
        private readonly ReactiveTrigger _startStickGrow = new();
        private readonly ReactiveTrigger _startStickRotation = new();
        
        public StickEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreatePm();
            AddUnsafe(_createView.Subscribe(CreateStickView));
        }

        private void CreatePm()
        {
            var stickPmCtx = new StickPm.Ctx()
            {
                levelFlowState = _ctx.levelFlowState,
                createView = _createView,
                startStickGrow = _startStickGrow,
                startStickRotation = _startStickRotation,
                stickIsDown = _stickIsDown,
                startLevel = _ctx.startLevel,
                spawnedSticks = _spawnedSticks,
                columnIsReachable = _ctx.columnIsReachable,
                changeLevelFlowState = _ctx.changeLevelFlowState
            };
            _pm = new StickPm(stickPmCtx);
            AddUnsafe(_pm);
        }

        private void CreateStickView()
        {
            var stick = Object.Instantiate(_ctx.contentProvider.Views.StickView,
                new Vector2(_ctx.actualColumnXPosition.Value + 1, Constant.PlayerYPosition - 0.5f),
                Quaternion.identity);
            stick.Init(new StickView.Ctx()
            {
                levelFlowState = _ctx.levelFlowState,
                stickLength = _ctx.stickLength,
                startStickGrow = _startStickGrow,
                startStickRotation = _startStickRotation,
                stickIsDown = _stickIsDown
            });
            _spawnedSticks.Add(stick.gameObject);
        }
        
    }
}