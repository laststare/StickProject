using CodeBase.Data;
using External.Framework;
using External.Reactive;
using UnityEngine;

namespace CodeBase.Game.Gameplay.ScoreCounter
{
    public class ScoreCounterEntity : BaseDisposable
    {
        public struct Ctx
        {
            public ContentProvider contentProvider;
            public RectTransform uiRoot;
            public IReadOnlyReactiveTrigger startGame;
            public IReadOnlyReactiveTrigger startLevel;
            public ReactiveEvent<int> addScore;
            public IReadOnlyReactiveTrigger finishLevel;
            public IReadOnlyReactiveTrigger showStartMenu;
        }
        private readonly Ctx _ctx;
        private ScoreCounterPm _pm;
        private ScoreCounterView _view;
        private readonly ReactiveEvent<string, string> _showScore = new();

        public ScoreCounterEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreatePm();
            CreateView();
        }

        private void CreatePm()
        {
            var scoreCounterPmCtx = new ScoreCounterPm.Ctx()
            {
                showScore = _showScore,
                startGame = _ctx.startGame,
                addScore = _ctx.addScore,
                finishLevel = _ctx.finishLevel
            };
            _pm = new ScoreCounterPm(scoreCounterPmCtx);
            AddUnsafe(_pm);
        }

        private void CreateView()
        {
            _view = Object.Instantiate(_ctx.contentProvider.UIViews.ScoreCounterView, _ctx.uiRoot);
            _view.Init(new ScoreCounterView.Ctx()
            {
                showScore = _showScore,
                startLevel = _ctx.startLevel,
                finishLevel = _ctx.finishLevel,
                startGame = _ctx.startGame,
                showStartMenu = _ctx.showStartMenu
            });
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            if(_view != null)
                Object.Destroy(_view.gameObject);
        }
    }
}