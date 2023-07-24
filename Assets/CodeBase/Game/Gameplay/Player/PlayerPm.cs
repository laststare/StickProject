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
        }

        private void SetPlayerDestinationPoint()
        {
            var distance = _ctx.actualColumnXPosition.Value + 1 + _ctx.stickLength.Value;
            Debug.Log($"length {_ctx.stickLength.Value} distance {distance}");
            var correct = distance >= _ctx.nextColumnXPosition.Value - 1 &&
                          distance <= _ctx.nextColumnXPosition.Value + 1;
            if (correct)
                _ctx.movePlayerTo.Notify(_ctx.nextColumnXPosition.Value + Constant.PlayerOnColumnXOffset);
            else
                _ctx.movePlayerTo.Notify(distance);
            
        }

    }
}