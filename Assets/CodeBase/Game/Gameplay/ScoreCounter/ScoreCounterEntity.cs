using CodeBase.Data;
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
            public ContentProvider contentProvider;
            public RectTransform uiRoot;
            public IReadOnlyReactiveTrigger startGame;
            public IReadOnlyReactiveTrigger startLevel;
            public IReadOnlyReactiveTrigger finishLevel;
            public IReadOnlyReactiveTrigger showStartMenu;
            public ReactiveProperty<bool> columnIsReachable;
            public ReactiveProperty<float> nextColumnXPosition;
        }
        private readonly Ctx _ctx;
        private ScoreCounterPm _pm;
        private ScoreCounterView _view;
        private readonly ReactiveEvent<string, string> _showScore = new();
        private readonly ReactiveCollection<RewardView> _spawnedRewardViews = new();
        private readonly ReactiveTrigger _spawnRewardView = new();

        public ScoreCounterEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreatePm();
            CreateView();
            AddUnsafe(_spawnRewardView.Subscribe(CreateRewardView));
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
                spawnRewardView = _spawnRewardView,
                spawnedRewardViews = _spawnedRewardViews
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

        private void CreateRewardView()
        {
            var rewardView = Object.Instantiate(_ctx.contentProvider.Views.RewardView,
                new Vector3(_ctx.nextColumnXPosition.Value, Constant.PlayerYPosition, 0), Quaternion.identity);
            _spawnedRewardViews.Add(rewardView);
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            if(_view != null)
                Object.Destroy(_view.gameObject);
        }
    }
}