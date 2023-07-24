using System;
using CodeBase.Game.Gameplay.Stick;
using External.Framework;
using External.Reactive;
using UnityEngine;

namespace CodeBase.Game.Gameplay.ScoreCounter
{
    public class ScoreCounterPm : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveEvent<string, string> showScore;
            public IReadOnlyReactiveTrigger startGame;
            public ReactiveEvent<int> addScore;
            public IReadOnlyReactiveTrigger finishLevel;
        }
        private readonly Ctx _ctx;
        private int _currentScore, _bestScore;

        public ScoreCounterPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.startGame.Subscribe(GetSavedScore));
            AddUnsafe(_ctx.addScore.SubscribeWithSkip(UpdateScore));
            AddUnsafe(_ctx.finishLevel.Subscribe(SendScoreToView));
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

        private void UpdateScore(int points)
        {
            _currentScore += points;
            Debug.Log($"current score is {_currentScore}");
            if (_currentScore > _bestScore)
                UpdateBestScore();
        }

        private void UpdateBestScore()
        {
            _bestScore = _currentScore;
            PlayerPrefs.SetInt(Constant.SavedScore, _bestScore);
        }
    }
}