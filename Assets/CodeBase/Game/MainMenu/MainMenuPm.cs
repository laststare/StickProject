using System;
using System.Net.NetworkInformation;
using Cysharp.Threading.Tasks;
using External.Framework;
using External.Reactive;
using UniRx;

namespace CodeBase.Game.MainMenu
{
    public class MainMenuPm : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveEvent<MainMenuButton> menuButtonClicked;
            public ReactiveTrigger startLevel;
            public ReactiveTrigger showStartMenu;
        } 
        
        private readonly Ctx _ctx;

        public MainMenuPm(Ctx ctx)
        {
            _ctx = ctx;
            AddToDisposables(_ctx.menuButtonClicked.SubscribeWithSkip(ButtonClickReceiver));
        }

        private void ButtonClickReceiver(MainMenuButton button)
        {
            switch (button)
            {
                case MainMenuButton.StartGame:
                    StartLevel();
                    break;
                case MainMenuButton.RestartGame:
                    StartLevel();
                    break;
                case MainMenuButton.BackToStartScreen:
                        _ctx.showStartMenu.Notify();
                    break;
            }
        }

        private void StartLevel()
        {
            _ctx.startLevel.Notify();
        }
    }
}