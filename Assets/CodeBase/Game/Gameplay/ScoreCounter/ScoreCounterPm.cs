using CodeBase.Data;
using CodeBase.Game.DataSave;
using External.Framework;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.ScoreCounter
{
    public class ScoreCounterPm : BaseDisposable
    {
        public struct Ctx
        {
            public IContentProvider contentProvider;
            public ReactiveEvent<string, string> showScore;
            public IReadOnlyReactiveTrigger startGame;
            public IReadOnlyReactiveTrigger finishLevel;
            public IReadOnlyReactiveProperty<bool> columnIsReachable;
            public ReactiveCollection<RewardView> spawnedRewardViews;
            public ReactiveTrigger spawnRewardView;
            public IReadOnlyReactiveProperty<IDataSave> dataSave;
        }
        
        private readonly Ctx _ctx;
        private int _currentScore, _bestScore;

        public ScoreCounterPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.startGame.Subscribe(GetSavedScore));
            AddUnsafe(_ctx.finishLevel.Subscribe(() =>
            {
                UpdateBestScore();
                SendScoreToView();
                ClearScore();
                DestroyRewardView();
            }));
            AddUnsafe(_ctx.columnIsReachable.Subscribe(x =>
            {
                if (!x) return;
                UpdateScore();
                RemoveOneView();
            }));
        }

        private void GetSavedScore()
        {
            _bestScore = _ctx.dataSave.Value.LoadBestScore();
            SendScoreToView();
        }

        private void SendScoreToView()
        {
            var bestText = $"Best score: {_bestScore}";
            var actualText =  $"Your score: {_currentScore}";
            _ctx.showScore.Notify(bestText, actualText);
        }

        private void UpdateScore()
        {
            _currentScore += _ctx.contentProvider.RewardConfig().OneColumnReward;
            _ctx.spawnRewardView.Notify();
        }

        private void UpdateBestScore()
        {
            if (_currentScore <= _bestScore)
                return;
            _bestScore = _currentScore;
            _ctx.dataSave.Value.SaveBestScore(_bestScore);
        }

        private void DestroyRewardView()
        {
            foreach (var view in _ctx.spawnedRewardViews) 
                Object.Destroy(view.gameObject);
            _ctx.spawnedRewardViews.Clear();
        }

        private void ClearScore() => _currentScore = 0;

        private void RemoveOneView()
        {
            if (_ctx.spawnedRewardViews.Count <= 2) return;
            Object.Destroy(_ctx.spawnedRewardViews[0].gameObject);
            _ctx.spawnedRewardViews.RemoveAt(0);
        }
    }
}