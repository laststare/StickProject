using System;
using System.Net.NetworkInformation;
using External.Framework;
using External.Reactive;

namespace CodeBase.Game.MainMenu
{
    public class MainMenuPm : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveEvent<MainMenuButton> menuButtonClicked;
            public ReactiveTrigger startLevel;
        } 
        
        private readonly Ctx _ctx;

        public MainMenuPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.menuButtonClicked.SubscribeWithSkip(ButtonClickReceiver));
        }

        private void ButtonClickReceiver(MainMenuButton button)
        {
            switch (button)
            {
                case MainMenuButton.StartGame:
                        _ctx.startLevel.Notify();
                    break;
                case MainMenuButton.RestartGame:
                        _ctx.startLevel.Notify();
                    break;
                case MainMenuButton.BackToStartScreen:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }
        }
    }
}