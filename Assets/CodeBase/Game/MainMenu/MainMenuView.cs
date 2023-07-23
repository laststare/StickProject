using External.Reactive;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Game.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        public struct Ctx
        {
            public ReactiveEvent<MainMenuButton> menuButtonClicked;
            public IReadOnlyReactiveTrigger startGame;
        }
        private Ctx _ctx;

        [SerializeField] private Button startGameBtn;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            _ctx.startGame.Subscribe(() => startGameBtn.gameObject.SetActive(true)).AddTo(this);
            
            startGameBtn.onClick.AddListener(() =>
            {
                _ctx.menuButtonClicked.Notify(MainMenuButton.StartGame);
                startGameBtn.gameObject.SetActive(false);
            });
        }
    }
}