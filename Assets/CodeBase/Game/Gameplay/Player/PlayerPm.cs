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
            public ReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveProperty<float> stickLength;
            public IReadOnlyReactiveTrigger playerFinishMoving;
            public ReactiveTrigger finishLevel;
            public ReactiveEvent<int> addScore;
        }
        private readonly Ctx _ctx;
        private bool _isStickLengthCorrect;

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
            _isStickLengthCorrect = moveDistance >= _ctx.nextColumnXPosition.Value - 1 &&
                                    moveDistance <= _ctx.nextColumnXPosition.Value + 1;
            var playerDestination = _isStickLengthCorrect
                ? _ctx.nextColumnXPosition.Value + Constant.PlayerOnColumnXOffset
                : moveDistance;
            _ctx.movePlayerTo.Notify(playerDestination);
        }

        private void PlayerOnNextColumn()
        {
            if (_isStickLengthCorrect)
            {
                _ctx.addScore.Notify(10);
                _ctx.levelFlowState.Value = LevelFlowState.CameraRun;
            }
            else 
                _ctx.finishLevel.Notify();
        }

    }
}