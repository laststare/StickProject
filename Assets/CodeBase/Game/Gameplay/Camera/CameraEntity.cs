using CodeBase.Data;
using External.Framework;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.Camera
{
    public class CameraEntity : BaseDisposable
    {
        public struct Ctx
        {
            public ContentProvider contentProvider;
            public IReadOnlyReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveTrigger startLevel;
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public ReactiveEvent<LevelFlowState> changeLevelFlowState;
        }

        private readonly Ctx _ctx;
        private CameraPm _pm;
        private CameraView _view;
        private readonly ReactiveEvent<float> _moveCameraToNextColumn = new();
        private readonly ReactiveTrigger _cameraFinishMoving = new();

        public CameraEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreateCameraPm();
            CreateCameraView();
        }

        private void CreateCameraPm()
        {
            var cameraPmCtx = new CameraPm.Ctx()
            {
                moveCameraToNextColumn = _moveCameraToNextColumn,
                actualColumnXPosition = _ctx.actualColumnXPosition,
                levelFlowState = _ctx.levelFlowState,
                cameraFinishMoving = _cameraFinishMoving,
                changeLevelFlowState = _ctx.changeLevelFlowState
            };
            _pm = new CameraPm(cameraPmCtx);
            AddUnsafe(_pm);
        }

        private void CreateCameraView()
        {
            _view = Object.Instantiate(_ctx.contentProvider.Views.CameraView);
            _view.Init(new CameraView.Ctx()
            {
                moveCameraToNextColumn = _moveCameraToNextColumn,
                cameraFinishMoving = _cameraFinishMoving,
                startLevel = _ctx.startLevel,
            });
        }
        
        protected override void OnDispose()
        {
            base.OnDispose();

            if (_view != null)
            {
              Object.Destroy(_view.gameObject);
            }
        }
    }
}