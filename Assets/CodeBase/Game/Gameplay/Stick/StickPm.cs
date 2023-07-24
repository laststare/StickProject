using System;
using CodeBase.Data;
using Cysharp.Threading.Tasks;
using External.Framework;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Game.Gameplay.Stick
{
    public class StickPm : BaseDisposable
    {
        public struct Ctx
        {
            public ContentProvider contentProvider; 
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public ReactiveProperty<LevelFlowState> levelFlowState;
        }

        private readonly Ctx _ctx;
        private StickView _actualStick;
        private CompositeDisposable _touchHandlers;
      

        public StickPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.levelFlowState.Subscribe(LevelFlowReceiver));
        }

        private void LevelFlowReceiver(LevelFlowState state)
        {
            switch (state)
            {
                case LevelFlowState.PlayerIdle:
                    SpawnStick();
                    _touchHandlers = new CompositeDisposable();
                    _touchHandlers = new CompositeDisposable();
                    Observable.EveryUpdate()
                        .Where(_ => Input.GetMouseButtonDown(0)).Subscribe((_) =>
                        {
                            HandleClickDown();
                        }).AddTo(_touchHandlers);
                    Observable.EveryUpdate()
                        .Where(_ => Input.GetMouseButtonUp(0)).Subscribe(
                            (_) => _ctx.levelFlowState.Value = LevelFlowState.StickFalls)
                        .AddTo(_touchHandlers);
                    break;
                case LevelFlowState.StickGrowsUp:
                    break;
                case LevelFlowState.StickFalls:
                    break;
                case LevelFlowState.PlayerRun:
                    break;
                case LevelFlowState.CameraRun:
                    break;
            }
        }

        private void SpawnStick()
        {
            _actualStick = Object.Instantiate(_ctx.contentProvider.Views.StickView,
                new Vector2(_ctx.actualColumnXPosition.Value + 1, Constant.PlayerYPosition - 0.5f), Quaternion.identity);
        }

        private async void HandleClickDown()
        {
            _ctx.levelFlowState.Value = LevelFlowState.StickGrowsUp;
            float stickHeight = 0;
            while (_ctx.levelFlowState.Value == LevelFlowState.StickGrowsUp)
            {
                stickHeight += Time.deltaTime * 3;
                _actualStick.transform.localScale = new Vector3(0.5f, stickHeight, 1);
                await UniTask.Yield();
            }

            
        }

    }
}