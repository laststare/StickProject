using CodeBase.Data;
using External.Framework;
using External.Reactive;
using UnityEngine;

namespace CodeBase.Game.Gameplay.Camera
{
    public class CameraEntity : BaseDisposable
    {
        public struct Ctx
        {
            public ContentProvider contentProvider;
            public ReactiveEvent<int> startLevel;
            public ReactiveTrigger finishLevel;
        }

        private readonly Ctx _ctx;
        private CameraPm _pm;
        private CameraView _view;

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

            };
            _pm = new CameraPm(cameraPmCtx);
            AddUnsafe(_pm);
        }

        private void CreateCameraView()
        {
            _view = Object.Instantiate(_ctx.contentProvider.Views.CameraView);
            _view.Init(new CameraView.Ctx()
            {
                
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