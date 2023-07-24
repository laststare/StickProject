using External.Framework;
using External.Reactive;
using UniRx;

namespace CodeBase.Game.Gameplay.Camera
{
    public class CameraPm : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public ReactiveEvent<float> moveCameraTo;
            public IReadOnlyReactiveTrigger startLevel;
            public ReactiveTrigger cameraFinishMoving;
        }

        private readonly Ctx _ctx;
        private CameraPm _pm;

        public CameraPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.CameraRun)
                    SetCameraDestinationPoint();
            }));
            AddUnsafe(_ctx.startLevel.Subscribe(SetCameraDestinationPoint));
            AddUnsafe(_ctx.cameraFinishMoving.Subscribe(() =>
            {
                _ctx.levelFlowState.Value = LevelFlowState.PlayerIdle;
            }));
        }

        private void SetCameraDestinationPoint()
        {
            _ctx.moveCameraTo.Notify(_ctx.actualColumnXPosition.Value + Constant.CameraOnColumnXOffset);
        }
    }
}