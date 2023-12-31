using CodeBase.Data;
using External.Framework;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.MainMenu
{
    public class MainMenuEntity : BaseDisposable
    {
        public struct Ctx
        {
             public IContentProvider contentProvider; 
             public RectTransform uiRoot;
             public ReactiveTrigger startLevel;
             public IReadOnlyReactiveTrigger startGame;
             public IReadOnlyReactiveTrigger finishLevel;
             public ReactiveTrigger showStartMenu;
        } 
        
        private readonly Ctx _ctx;
        private MainMenuPm _pm;
        private MainMenuView _view;
        private readonly ReactiveEvent<MainMenuButton> _menuButtonClicked = new();

        public MainMenuEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreatePm();
            CrateView();
        }

        private void CreatePm()
        {
            var mainMenuPmCtx = new MainMenuPm.Ctx()
            {
                menuButtonClicked = _menuButtonClicked,
                startLevel = _ctx.startLevel,
                showStartMenu = _ctx.showStartMenu,
            };
            _pm = new MainMenuPm(mainMenuPmCtx);
            AddToDisposables(_pm);
        }

        private void CrateView()
        {
            _view = Object.Instantiate(_ctx.contentProvider.MainMenuView(), _ctx.uiRoot);
            _view.Init(new MainMenuView.Ctx()
            {
                menuButtonClicked = _menuButtonClicked,
                startGame = _ctx.startGame,
                finishLevel = _ctx.finishLevel,
                startLevel = _ctx.startLevel
            });
        }

        protected override void OnDispose()
        {
            if(_view != null)
                Object.Destroy(_view.gameObject);
            base.OnDispose();
        }
    }
}