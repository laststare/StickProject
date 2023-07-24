using CodeBase.Data;
using External.Framework;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.Player
{
    public class PlayerEntity : BaseDisposable
    {
        public struct Ctx
        {
            public ContentProvider contentProvider; 
            public IReadOnlyReactiveTrigger startLevel;
            public ReactiveProperty<float> actualColumnXPosition;
        }
        private readonly Ctx _ctx;
        private PlayerPm _pm;
        private PlayerView _view;

        public PlayerEntity (Ctx ctx)
        {
            _ctx = ctx;
            CreatePm();
            CreateView();
        }

        private void CreatePm()
        {
            var playerPmCtx = new PlayerPm.Ctx()
            {

            };
            _pm = new PlayerPm(playerPmCtx);
            AddUnsafe(_pm);
        }

        private void CreateView()
        {
            _view = Object.Instantiate(_ctx.contentProvider.Views.PlayerView);
            _view.Init(new PlayerView.Ctx()
            {
                startLevel = _ctx.startLevel,
                actualColumnXPosition = _ctx.actualColumnXPosition
            });
        }
    }
}