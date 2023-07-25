using CodeBase.Data;
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
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public IReadOnlyReactiveProperty<float> nextColumnXPosition;
            public ReactiveEvent<float> movePlayerTo;
            public IReadOnlyReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveProperty<float> stickLength;
            public IReadOnlyReactiveTrigger playerFinishMoving;
            public ReactiveTrigger finishLevel;
            public ReactiveProperty<bool> columnIsReachable;
            public ReactiveEvent<LevelFlowState> changeLevelFlowState;
        }
        private readonly Ctx _ctx;

        public PlayerPm (Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerRun)
                    SetPlayerDestinationPoint();
            }));
            AddUnsafe(_ctx.playerFinishMoving.Subscribe(PlayerOnNextColumn));
        }

        private void SetPlayerDestinationPoint()
        {
            var moveDistance = _ctx.actualColumnXPosition.Value + 1 + _ctx.stickLength.Value;
            _ctx.columnIsReachable.SetValueAndForceNotify(moveDistance >= _ctx.nextColumnXPosition.Value - 1.25f &&
                                                          moveDistance <= _ctx.nextColumnXPosition.Value + 1.25f);
            var playerDestination = _ctx.columnIsReachable.Value
                ? _ctx.nextColumnXPosition.Value + Constant.PlayerOnColumnXOffset
                : moveDistance;
            _ctx.movePlayerTo.Notify(playerDestination);
        }

        private void PlayerOnNextColumn()
        {
            if (_ctx.columnIsReachable.Value)
                _ctx.changeLevelFlowState.Notify(LevelFlowState.CameraRun);
            else
                _ctx.finishLevel.Notify();
        }

    }
}