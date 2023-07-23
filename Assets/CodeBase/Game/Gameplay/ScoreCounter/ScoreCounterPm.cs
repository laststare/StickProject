using System;
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
        }
        private readonly Ctx _ctx;
        private string _bestScore = "0";

        public ScoreCounterPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.startGame.Subscribe(GetSavedScore));
        }

        private void GetSavedScore()
        {
            var savedBestScore = PlayerPrefs.GetString(Constant.savedScore);
            if (!string.IsNullOrEmpty(savedBestScore))
                _bestScore = savedBestScore;
            SendScoreToView(string.Empty);
        }

        private void SendScoreToView(string actual)
        {
            var bestText = $"Best score: {_bestScore}";
            var actualText = !string.IsNullOrEmpty(actual)? $"Your score: {actual}" : string.Empty;
            _ctx.showScore.Notify(bestText, actualText);
        }

        private void UpdateBestScore(int updatesBestScore)
        {
            var updatesBestScoreText = updatesBestScore.ToString();
            PlayerPrefs.SetString(Constant.savedScore, updatesBestScoreText);
            _bestScore = updatesBestScoreText;
        }
    }
}