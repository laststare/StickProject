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
            public IReadOnlyReactiveTrigger startLevel;
            public ReactiveProperty<float> actualColumnXPosition;
            public ReactiveProperty<float> nextColumnXPosition;
            public IReadOnlyReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveProperty<bool> columnIsReachable;
        }

        private readonly Ctx _ctx;
        private readonly LevelBuilderPm _pm;

        public LevelBuilderEntity(Ctx ctx)
        {
            _ctx = ctx;
            var levelBuilderPmCtx = new LevelBuilderPm.Ctx()
            {
                contentProvider = _ctx.contentProvider,
                startLevel = _ctx.startLevel,
                actualColumnXPosition = _ctx.actualColumnXPosition,
                nextColumnXPosition = _ctx.nextColumnXPosition,
                levelFlowState = _ctx.levelFlowState,
                columnIsReachable = _ctx.columnIsReachable
            };
            _pm = new LevelBuilderPm(levelBuilderPmCtx);
            AddUnsafe(_pm);
        }
    }
}