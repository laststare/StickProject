using CodeBase.Data;
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
            public ContentProvider contentProvider;
            public ReactiveEvent<string, string> showScore;
            public IReadOnlyReactiveTrigger startGame;
            public IReadOnlyReactiveTrigger finishLevel;
            public IReadOnlyReactiveProperty<bool> columnIsReachable;
            public ReactiveCollection<RewardView> spawnedRewardViews;
            public ReactiveTrigger spawnRewardView;
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
                if(x)
                    UpdateScore();
            }));
        }

        private void GetSavedScore()
        {
            _bestScore = PlayerPrefs.GetInt(Constant.SavedScore);
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
            _currentScore += _ctx.contentProvider.Settings.RewardConfig.OneColumnReward;
            _ctx.spawnRewardView.Notify();
        }

        private void UpdateBestScore()
        {
            if (_currentScore <= _bestScore)
                return;
            _bestScore = _currentScore;
            PlayerPrefs.SetInt(Constant.SavedScore, _bestScore);
        }

        private void DestroyRewardView()
        {
            foreach (var view in _ctx.spawnedRewardViews) 
                Object.Destroy(view.gameObject);
            _ctx.spawnedRewardViews.Clear();
        }

        private void ClearScore() => _currentScore = 0;
    }
}