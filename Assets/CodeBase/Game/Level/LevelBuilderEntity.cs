using CodeBase.Data;
using External.Framework;
using External.Reactive;

namespace CodeBase.Game.Level
{
    public class LevelBuilderEntity : BaseDisposable
    {
        public struct Ctx
        {
            public ContentProvider contentProvider;
            public ReactiveEvent<int> startLevel;
            public ReactiveTrigger finishLevel;
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
                finishLevel = _ctx.finishLevel
            };
            _pm = new LevelBuilderPm(levelBuilderPmCtx);
            AddUnsafe(_pm);
        }
    }
}