using External.Framework;
using External.Reactive;

namespace CodeBase.Game.Gameplay.Camera
{
    public class CameraPm : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveTrigger startLevel;
            public ReactiveTrigger finishLevel;
        }

        private readonly Ctx _ctx;
        private CameraPm _pm;
        private CameraView _view;

        public CameraPm(Ctx ctx)
        {
            _ctx = ctx;
        }
    }
}