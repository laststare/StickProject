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
            public ReactiveProperty<float> actualColumnXPosition;
            public ReactiveProperty<float> nextColumnXPosition;
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
                actualColumnXPosition = _ctx.actualColumnXPosition,
                nextColumnXPosition = _ctx.nextColumnXPosition
            };
            _pm = new LevelBuilderPm(levelBuilderPmCtx);
            AddUnsafe(_pm);
        }
    }
}