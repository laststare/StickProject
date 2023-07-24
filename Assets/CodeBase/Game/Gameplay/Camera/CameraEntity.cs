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
            public ReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveTrigger startLevel;
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
        }

        private readonly Ctx _ctx;
        private CameraPm _pm;
        private CameraView _view;
        private readonly ReactiveEvent<float> _moveCameraTo = new();
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
                moveCameraTo = _moveCameraTo,
                actualColumnXPosition = _ctx.actualColumnXPosition,
                levelFlowState = _ctx.levelFlowState,
                startLevel = _ctx.startLevel,
                cameraFinishMoving = _cameraFinishMoving
            };
            _pm = new CameraPm(cameraPmCtx);
            AddUnsafe(_pm);
        }

        private void CreateCameraView()
        {
            _view = Object.Instantiate(_ctx.contentProvider.Views.CameraView);
            _view.Init(new CameraView.Ctx()
            {
                moveCameraTo = _moveCameraTo,
                cameraFinishMoving = _cameraFinishMoving
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