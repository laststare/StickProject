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
        private PlayerView _view;
        private readonly ReactiveEvent<float> _movePlayerTo = new();
        private readonly ReactiveTrigger _playerFinishMoving = new ();
        
        public PlayerEntity (Ctx ctx)
        {
            _ctx = ctx;
            CreatePm();
            CreateView();
        }

        private void CreatePm()
        {
            var playerPmCtx = new PlayerPm.Ctx()
            {
                nextColumnXPosition = _ctx.nextColumnXPosition,
                movePlayerTo = _movePlayerTo,
                levelFlowState = _ctx.levelFlowState,
                stickLength = _ctx.stickLength,
                actualColumnXPosition = _ctx.actualColumnXPosition,
                playerFinishMoving = _playerFinishMoving,
                finishLevel = _ctx.finishLevel,
                columnIsReachable = _ctx.columnIsReachable,
                changeLevelFlowState = _ctx.changeLevelFlowState,
                columnOffset = _ctx.contentProvider.LevelConfig().GetColumnOffset,
                playerOnColumnXOffset = _ctx.contentProvider.LevelConfig().GetPlayerOnColumnXOffset
            };
            _pm = new PlayerPm(playerPmCtx);
            AddToDisposables(_pm);
        }

        private void CreateView()
        {
            _view = Object.Instantiate(_ctx.contentProvider.PlayerView());
            _view.Init(new PlayerView.Ctx()
            {
                startLevel = _ctx.startLevel,
                movePlayerTo = _movePlayerTo,
                playerFinishMoving = _playerFinishMoving,
                playerYPosition = _ctx.contentProvider.LevelConfig().GetPlayerYPosition,
                playerOnColumnXOffset = _ctx.contentProvider.LevelConfig().GetPlayerOnColumnXOffset
            });
        }
    }
}