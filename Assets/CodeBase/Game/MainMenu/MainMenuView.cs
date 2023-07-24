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
            public IReadOnlyReactiveTrigger finishLevel;
            public IReadOnlyReactiveTrigger startLevel;
        }
        private Ctx _ctx;

        [SerializeField] private Button startGameBtn, restartGameBtn, backStartScreenBtn;

        public void Init(Ctx ctx)
        {
            _ctx = ctx;

            _ctx.startGame.Subscribe(() => startGameBtn.gameObject.SetActive(true)).AddTo(this);

            _ctx.finishLevel.Subscribe(ShowFinishScreen).AddTo(this);

            _ctx.startLevel.Subscribe(() => { 
                HideStartScreen();
                HideFinishScreen();
            });
            
            startGameBtn.onClick.AddListener(() =>
            {
                _ctx.menuButtonClicked.Notify(MainMenuButton.StartGame);
            });
            
            restartGameBtn.onClick.AddListener(() =>
            {
                _ctx.menuButtonClicked.Notify(MainMenuButton.RestartGame);
            });
            
            backStartScreenBtn.onClick.AddListener(() =>
            {
                _ctx.menuButtonClicked.Notify(MainMenuButton.BackToStartScreen);
                ShowStartScreen();
            });
        }

        private void ShowStartScreen()
        {
            startGameBtn.gameObject.SetActive(true);
            HideFinishScreen();
        }

        private void ShowFinishScreen()
        {
            restartGameBtn.gameObject.SetActive(true); 
            backStartScreenBtn.gameObject.SetActive(true);
        }
        
        private void HideStartScreen() => startGameBtn.gameObject.SetActive(false);

        private void HideFinishScreen()
        {
            restartGameBtn.gameObject.SetActive(false); 
            backStartScreenBtn.gameObject.SetActive(false);
        }
    }
}