using CodeBase.Data;
using External.Framework;
using External.Reactive;
using UniRx;

namespace CodeBase.Game.Level
{
    public class LevelBuilderEntity : BaseDisposable
    {
        public struct Ctx
        {
            public ContentProvider contentProvider;
            public ReactiveTrigger startLevel;
            public ReactiveTrigger finishLevel;
            public ReactiveProperty<float> actualColumnXPosition;
        }

        private readonly Ctx _ctx;
        private LevelBuilderPm _pm;

        public LevelBuilderEntity(Ctx ctx)
        {
            _ctx = ctx;
            var levelBuilderPmCtx = new LevelBuilderPm.Ctx()
            {
                contentProvider = _ctx.contentProvider,
                startLevel = _ctx.startLevel,
                finishLevel = _ctx.finishLevel,
                actualColumnXPosition = _ctx.actualColumnXPosition
            };
            _pm = new LevelBuilderPm(levelBuilderPmCtx);
            AddUnsafe(_pm);
        }
    }
}