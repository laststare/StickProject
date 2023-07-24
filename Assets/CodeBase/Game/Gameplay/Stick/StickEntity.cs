using CodeBase.Data;
using External.Framework;
using UniRx;

namespace CodeBase.Game.Gameplay.Stick
{
    public class StickEntity : BaseDisposable
    {
        public struct Ctx
        {
            public ContentProvider contentProvider; 
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public ReactiveProperty<LevelFlowState> levelFlowState;
            public ReactiveProperty<float> stickLength;
        }

        private readonly Ctx _ctx;
        private StickPm _pm;
        
        public StickEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreatePm();
        }

        private void CreatePm()
        {
            var stickPmCtx = new StickPm.Ctx()
            {
                contentProvider = _ctx.contentProvider,
                actualColumnXPosition = _ctx.actualColumnXPosition,
                levelFlowState = _ctx.levelFlowState,
                stickLength = _ctx.stickLength
            };
            _pm = new StickPm(stickPmCtx);
            AddUnsafe(_pm);
        }
    }
}