using External.Framework;
using External.Reactive;
using UniRx;

namespace CodeBase.Game.Gameplay.Camera
{
    public class CameraPm : BaseDisposable
    {
        public struct Ctx
        {
            public IReadOnlyReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public ReactiveEvent<float> moveCameraToNextColumn;
            public ReactiveTrigger cameraFinishMoving;
            public ReactiveEvent<LevelFlowState> changeLevelFlowState;
            public float cameraColumnXOffset;
        }

        private readonly Ctx _ctx;
        private CameraPm _pm;
        private readonly float _cameraColumnXOffset;

        public CameraPm(Ctx ctx)
        {
            _ctx = ctx;
            _cameraColumnXOffset = _ctx.cameraColumnXOffset;
            AddToDisposables(_ctx.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.CameraRun)
                    SetCameraDestinationPointToColumn();
            }));
            AddToDisposables(_ctx.cameraFinishMoving.Subscribe(() =>
            {
                _ctx.changeLevelFlowState.Notify(LevelFlowState.PlayerIdle);
            }));
        }

        private void SetCameraDestinationPointToColumn()
        {
            _ctx.moveCameraToNextColumn.Notify(_ctx.actualColumnXPosition.Value + _cameraColumnXOffset);
        }
        
    }
}