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
        }
        private Ctx _ctx;
        [SerializeField] private TMP_Text bestScoreText, actualScoreText;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.showScore.SubscribeWithSkip(ShowScore).AddTo(this);
            _ctx.startLevel.Subscribe(() => gameObject.SetActive(false)).AddTo(this);
        }

        private void ShowScore(string best, string actual)
        {
            gameObject.SetActive(true);
            bestScoreText.text = best;
            actualScoreText.text = actual;
        }
    }
}