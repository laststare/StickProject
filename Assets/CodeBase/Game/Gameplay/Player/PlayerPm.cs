using CodeBase.Data;
using DG.Tweening;
using External.Framework;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.Player
{
    public class PlayerPm : BaseDisposable
    {
        public struct Ctx
        {
            public IContentProvider contentProvider; 
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public IReadOnlyReactiveProperty<float> nextColumnXPosition;
            public IReadOnlyReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveProperty<float> stickLength;
            public IReadOnlyReactiveTrigger startLevel;
            public ReactiveTrigger finishLevel;
            public ReactiveProperty<bool> columnIsReachable;
            public ReactiveEvent<LevelFlowState> changeLevelFlowState;
        }
        
        private readonly Ctx _ctx;
        private readonly int _columnOffset;
        private readonly float _playerOnColumnXOffset, _destinationOffset;
        private readonly Transform _player;

        public PlayerPm (Ctx ctx)
        {
            _ctx = ctx;
            _columnOffset = _ctx.contentProvider.LevelConfig().GetColumnOffset;
            _playerOnColumnXOffset = _ctx.contentProvider.LevelConfig().GetPlayerOnColumnXOffset;
            _destinationOffset = _ctx.contentProvider.LevelConfig().GetDestinationOffset;
            _player = Object.Instantiate(_ctx.contentProvider.Player());

            AddToDisposables(_ctx.startLevel.Subscribe(() =>
            {
                _player.position = new Vector2(_playerOnColumnXOffset, _ctx.contentProvider.LevelConfig().GetPlayerYPosition);
                _player.gameObject.SetActive(true);
            }));
            
            AddToDisposables(_ctx.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerRun)
                    SetPlayerDestinationPoint();
            }));
        }

        private void SetPlayerDestinationPoint()
        {
            var moveDistance = _ctx.actualColumnXPosition.Value + _destinationOffset + _ctx.stickLength.Value;
            _ctx.columnIsReachable.SetValueAndForceNotify(moveDistance >= _ctx.nextColumnXPosition.Value - _columnOffset &&
                                                          moveDistance <= _ctx.nextColumnXPosition.Value + _columnOffset);
            var playerDestination = _ctx.columnIsReachable.Value
                ? _ctx.nextColumnXPosition.Value + _playerOnColumnXOffset
                : moveDistance;

            _player.DOMoveX(playerDestination, 2).OnComplete(PlayerOnNextColumn);
        }

        private void PlayerOnNextColumn()
        {
            if (_ctx.columnIsReachable.Value)
                _ctx.changeLevelFlowState.Notify(LevelFlowState.CameraRun);
            else
                _ctx.finishLevel.Notify();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            if(_player != null)
                Object.Destroy(_player.gameObject);
                
        }
    }
}