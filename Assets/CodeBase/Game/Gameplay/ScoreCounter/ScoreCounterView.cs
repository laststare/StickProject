using External.Reactive;
using TMPro;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.ScoreCounter
{
    public class ScoreCounterView : MonoBehaviour
    {
        public struct Ctx
        {
            public IReadOnlyReactiveEvent<string, string> showScore;
            public IReadOnlyReactiveTrigger startLevel;
            public IReadOnlyReactiveTrigger finishLevel;
            public IReadOnlyReactiveTrigger startGame;
            public IReadOnlyReactiveTrigger showStartMenu;

        }
        private Ctx _ctx;
        [SerializeField] private TMP_Text bestScoreText, actualScoreText;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.showScore.SubscribeWithSkip(ShowScore).AddTo(this);
            _ctx.startLevel.Subscribe(HideAll).AddTo(this);
            _ctx.finishLevel.Subscribe(ShowFinish).AddTo(this);
            _ctx.startGame.Subscribe(ShowStart).AddTo(this);
            _ctx.showStartMenu.Subscribe(() =>
            {
                HideAll();
                ShowStart();
            }).AddTo(this);
        }

        private void ShowScore(string best, string actual)
        {
            gameObject.SetActive(true);
            bestScoreText.text = best;
            actualScoreText.text = actual;
        }

        private void ShowStart()
        {
            gameObject.SetActive(true);
            bestScoreText.gameObject.SetActive(true);
        }

        private void ShowFinish()
        {
            ShowStart();
            actualScoreText.gameObject.SetActive(true);
        }

        private void HideAll()
        {
            gameObject.SetActive(false);
            bestScoreText.gameObject.SetActive(false);
            actualScoreText.gameObject.SetActive(false);
        }
    }
}