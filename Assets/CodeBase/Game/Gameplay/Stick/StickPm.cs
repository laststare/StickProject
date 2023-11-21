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
            public IContentProvider contentProvider; 
            public IReadOnlyReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public IReadOnlyReactiveTrigger startLevel;
            public IReadOnlyReactiveProperty<bool> columnIsReachable;
            public ReactiveEvent<LevelFlowState> changeLevelFlowState;
            public ReactiveProperty<float> stickLength;
        }

        private readonly Ctx _ctx;
      
        private CompositeDisposable _clickHandlers;
        private readonly List<GameObject> _spawnedSticks = new List<GameObject>();

        
        public StickPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerIdle) TmpClickDownSubscription();
            }));
            AddUnsafe(_ctx.startLevel.Subscribe(DestroySticks));

            AddUnsafe(_ctx.columnIsReachable.Subscribe(x =>
            {
                if(x) 
                    RemoveOneView();
            }));
        }
        
        private void CreateView()
        {
            var stick = Object.Instantiate(_ctx.contentProvider.Stick(),
                new Vector2(_ctx.actualColumnXPosition.Value + 1, Constant.PlayerYPosition - 0.5f),
                Quaternion.identity);
            _spawnedSticks.Add(stick.gameObject);
        }

        private void TmpClickDownSubscription()
        {
            _clickHandlers = new CompositeDisposable();
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0)).Subscribe(
                    (_) =>
                    {
                        CreateView();
                        GrowStickUp();
                        TmpClickUpSubscription();
                    }).AddTo(_clickHandlers);
        }

        private void TmpClickUpSubscription()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonUp(0)).Subscribe(
                    (_) =>
                    {
                        RotateStick();
                        _clickHandlers.Dispose();
                    })
                .AddTo(_clickHandlers); 
        }

        private async void GrowStickUp()
        {
            _ctx.changeLevelFlowState.Notify(LevelFlowState.StickGrowsUp);
            var stick = _spawnedSticks[^1];
            var stickHeight = 0f;
            var stickWidth = stick.transform.localScale.x;
            while (_ctx.levelFlowState.Value == LevelFlowState.StickGrowsUp)
            {
                stickHeight += Time.deltaTime * 6;
                stick.transform.localScale = new Vector3(stickWidth, stickHeight, 1);
                await UniTask.Yield();
            }
            _ctx.stickLength.Value = stickHeight;
        }
        
        private void RotateStick()
        {
            _ctx.changeLevelFlowState.Notify(LevelFlowState.StickFalls);
            var stick = _spawnedSticks[^1];
            stick.transform.DORotate(new Vector3(0, 0, -90f), 0.5f)
                .OnComplete(() => _ctx.changeLevelFlowState.Notify(LevelFlowState.PlayerRun));
        }
        
        private void DestroySticks()
        {
            foreach (var stick in _spawnedSticks) 
                Object.Destroy(stick);
            _spawnedSticks.Clear();
        }
        private void RemoveOneView()
        {
            if (_spawnedSticks.Count <= 2) return;
            Object.Destroy(_spawnedSticks[0].gameObject);
            _spawnedSticks.RemoveAt(0);
        }

    }
}