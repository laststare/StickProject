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
        }
        private readonly Ctx _ctx;
        private int _currentScore, _bestScore;

        public ScoreCounterPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.startGame.Subscribe(GetSavedScore));
            AddUnsafe(_ctx.addScore.SubscribeWithSkip(UpdateScore));
        }

        private void GetSavedScore()
        {
            _bestScore = PlayerPrefs.GetInt(Constant.SavedScore);
            SendScoreToView(string.Empty);
        }

        private void SendScoreToView(string actual)
        {
            var bestText = $"Best score: {_bestScore}";
            var actualText = !string.IsNullOrEmpty(actual)? $"Your score: {actual}" : string.Empty;
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