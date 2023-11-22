using CodeBase.Data;
using CodeBase.Game.DataSave;
using External.Framework;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.ScoreCounter
{
    public class ScoreCounterEntity : BaseDisposable
    {
        public struct Ctx
        {
            public IContentProvider contentProvider;
            public RectTransform uiRoot;
            public IReadOnlyReactiveTrigger startGame;
            public IReadOnlyReactiveTrigger startLevel;
            public IReadOnlyReactiveTrigger finishLevel;
            public IReadOnlyReactiveTrigger showStartMenu;
            public ReactiveProperty<bool> columnIsReachable;
            public ReactiveProperty<float> nextColumnXPosition;
            public IReadOnlyReactiveProperty<IDataSave> dataSave;
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
                finishLevel = _ctx.finishLevel,
                columnIsReachable = _ctx.columnIsReachable,
                contentProvider = _ctx.contentProvider,
                dataSave = _ctx.dataSave,
                nextColumnXPosition = _ctx.nextColumnXPosition
            };
            _pm = new ScoreCounterPm(scoreCounterPmCtx);
            AddToDisposables(_pm);
        }

        private void CreateView()
        {
            _view = Object.Instantiate(_ctx.contentProvider.ScoreCounterView(), _ctx.uiRoot);
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