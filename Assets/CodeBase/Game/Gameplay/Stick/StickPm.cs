using System;
using System.Collections.Generic;
using CodeBase.Data;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using External.Framework;
using External.Reactive;
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
            public ReactiveProperty<float> stickLength;
            public IReadOnlyReactiveTrigger startLevel;
        }

        private readonly Ctx _ctx;
        private Transform _actualStick;
        private List<GameObject> _spawnedSticks = new List<GameObject>();
        private CompositeDisposable _clickHandlers;
      

        public StickPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.levelFlowState.Subscribe(LevelFlowReceiver));
            AddUnsafe(_ctx.startLevel.Subscribe(DestroySticks));
        }

        private void LevelFlowReceiver(LevelFlowState state)
        {
            switch (state)
            {
                case LevelFlowState.PlayerIdle:
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

        private Transform SpawnStick()
        {
            var stick = Object.Instantiate(_ctx.contentProvider.Views.StickView,
                new Vector2(_ctx.actualColumnXPosition.Value + 1, Constant.PlayerYPosition - 0.5f),Quaternion.identity);
            _spawnedSticks.Add(stick.gameObject);
            return stick.transform;
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
            _actualStick = SpawnStick();
            _ctx.levelFlowState.Value = LevelFlowState.StickGrowsUp;
            var stickHeight = 0f;
            var stickWidth = _actualStick.transform.localScale.x;
            while (_ctx.levelFlowState.Value == LevelFlowState.StickGrowsUp)
            {
                stickHeight += Time.deltaTime * 6;
                _actualStick.transform.localScale = new Vector3(stickWidth, stickHeight, 1);
                await UniTask.Yield();
            }
            _ctx.stickLength.Value = stickHeight;
        }
        
        private void RotateStick()
        {
            _ctx.levelFlowState.Value = LevelFlowState.StickFalls;
            _actualStick.transform.DORotate(new Vector3(0, 0, -90f), 0.5f)
                .OnComplete(() => _ctx.levelFlowState.Value = LevelFlowState.PlayerRun);
            _clickHandlers.Dispose();
        }

        private void DestroySticks()
        {
            _spawnedSticks.ForEach(x => Object.Destroy(x));
            _spawnedSticks.Clear();
        }

    }
}