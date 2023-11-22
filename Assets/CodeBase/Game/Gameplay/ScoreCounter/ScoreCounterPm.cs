using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Game.DataSave;
using DG.Tweening;
using External.Framework;
using External.Reactive;
using TMPro;
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
            public IReadOnlyReactiveProperty<IDataSave> dataSave;
            public ReactiveProperty<float> nextColumnXPosition;
        }
        
        private readonly Ctx _ctx;
        private int _currentScore, _bestScore;
        private readonly float _playerYPosition;

        public ScoreCounterPm(Ctx ctx)
        {
            _ctx = ctx;
            _playerYPosition = _ctx.contentProvider.LevelConfig().GetPlayerYPosition;
            AddToDisposables(_ctx.startGame.Subscribe(GetSavedScore));
            AddToDisposables(_ctx.finishLevel.Subscribe(() =>
            {
                UpdateBestScore();
                SendScoreToView();
                ClearScore();
            }));
            AddToDisposables(_ctx.columnIsReachable.Subscribe(x =>
            {
                if (!x) return;
                UpdateScore();
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
            SpawnRewardItem();
        }
        
        private void SpawnRewardItem()
        {
            var reward = Object.Instantiate(_ctx.contentProvider.Reward(),
                new Vector3(_ctx.nextColumnXPosition.Value, _playerYPosition, 0), Quaternion.identity);
            
            reward.DOMoveY(reward.position.y + 3, 2);
            var rewardText = reward.transform.GetChild(0).GetComponent<TMP_Text>();
            rewardText.DOFade(0, 2).OnComplete(() => { Object.Destroy(reward.gameObject);});
        }

        private void UpdateBestScore()
        {
            if (_currentScore <= _bestScore)
                return;
            _bestScore = _currentScore;
            _ctx.dataSave.Value.SaveBestScore(_bestScore);
        }

        private void ClearScore() => _currentScore = 0;
        
    }
    
}