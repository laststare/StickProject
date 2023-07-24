using System;
using CodeBase.Data;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        private CompositeDisposable _clickHandlers;
      

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
                    MakeTemporarySubscription();
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

        private void MakeTemporarySubscription()
        {
            _clickHandlers = new CompositeDisposable();
            _clickHandlers = new CompositeDisposable();
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0)).Subscribe((_) =>
                {
                    GrowStickUp();
                }).AddTo(_clickHandlers);
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonUp(0)).Subscribe(
                    (_) => RotateStick())
                .AddTo(_clickHandlers); 
        }

        private async void GrowStickUp()
        {
            _ctx.levelFlowState.Value = LevelFlowState.StickGrowsUp;
            float stickHeight = 0;
            while (_ctx.levelFlowState.Value == LevelFlowState.StickGrowsUp)
            {
                stickHeight += Time.deltaTime * 6;
                _actualStick.transform.localScale = new Vector3(0.5f, stickHeight, 1);
                await UniTask.Yield();
            }
        }

        private void RotateStick()
        {
            _ctx.levelFlowState.Value = LevelFlowState.StickFalls;
            _actualStick.transform.DORotate(new Vector3(0, 0, -90f), 0.5f)
                .OnComplete(() => _ctx.levelFlowState.Value = LevelFlowState.PlayerRun);
            _clickHandlers.Dispose();
        }

    }
}