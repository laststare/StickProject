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
            public ReactiveEvent<float> moveCameraToNextColumn;
            public ReactiveTrigger cameraFinishMoving;
            public ReactiveEvent<LevelFlowState> changeLevelFlowState;
        }

        private readonly Ctx _ctx;
        private CameraPm _pm;

        public CameraPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.CameraRun)
                    SetCameraDestinationPointToColumn();
            }));
            AddUnsafe(_ctx.cameraFinishMoving.Subscribe(() =>
            {
                _ctx.changeLevelFlowState.Notify(LevelFlowState.PlayerIdle);
            }));
        }

        private void SetCameraDestinationPointToColumn()
        {
            _ctx.moveCameraToNextColumn.Notify(_ctx.actualColumnXPosition.Value + Constant.CameraOnColumnXOffset);
        }
        
    }
}