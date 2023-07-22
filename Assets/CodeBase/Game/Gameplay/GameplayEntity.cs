using CodeBase.Data;
using CodeBase.Game.Gameplay.Camera;
using External.Framework;
using External.Reactive;

namespace CodeBase.Game.Gameplay
{
    public class GameplayEntity : BaseDisposable
    {
        public struct Ctx
        {
            public ContentProvider contentProvider;
            public ReactiveEvent<int> startLevel;
            public ReactiveTrigger finishLevel;
        }

        private readonly Ctx _ctx;
        private CameraEntity _cameraEntity;

        public GameplayEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreateCameraEntity();
        }

        private void CreateCameraEntity()
        {
            var cameraEntityCtx = new CameraEntity.Ctx()
            {
                contentProvider = _ctx.contentProvider,
                startLevel = _ctx.startLevel,
                finishLevel = _ctx.finishLevel
            };
            _cameraEntity = new CameraEntity(cameraEntityCtx);
            AddUnsafe(_cameraEntity);
        }
    }
}