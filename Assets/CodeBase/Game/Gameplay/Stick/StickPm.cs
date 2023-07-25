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
        private readonly List<GameObject> _spawnedSticks = new();
        private CompositeDisposable _clickHandlers;
        private readonly ReactiveTrigger _stickIsDown = new();
        private readonly ReactiveTrigger _startStickGrow = new();
        private readonly ReactiveTrigger _startStickRotation = new();


        public StickPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerIdle)
                {
                    MakeTemporarySubscription();
                }
            }));
            AddUnsafe(_ctx.startLevel.Subscribe(DestroySticks));
            AddUnsafe(_stickIsDown.Subscribe(() => _ctx.levelFlowState.Value = LevelFlowState.PlayerRun));
        }

        private void MakeTemporarySubscription()
        {
            _clickHandlers = new CompositeDisposable();
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0)).Subscribe(
                    (_) =>
                    {
                        SpawnStick();
                        GrowStickUp();
                    }).AddTo(_clickHandlers);
            
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonUp(0)).Subscribe(
                    (_) =>
                    {
                        RotateStick();
                        _clickHandlers.Dispose();
                    })
                .AddTo(_clickHandlers); 
        }

        private void SpawnStick()
        {
            var stick = Object.Instantiate(_ctx.contentProvider.Views.StickView,
                new Vector2(_ctx.actualColumnXPosition.Value + 1, Constant.PlayerYPosition - 0.5f),
                Quaternion.identity);
            stick.Init(new StickView.Ctx()
            {
                levelFlowState = _ctx.levelFlowState,
                stickLength = _ctx.stickLength,
                startStickGrow = _startStickGrow,
                startStickRotation = _startStickRotation,
                stickIsDown = _stickIsDown
            });
            _spawnedSticks.Add(stick.gameObject);
        }

        private void GrowStickUp()
        {
            _ctx.levelFlowState.Value = LevelFlowState.StickGrowsUp;
            _startStickGrow.Notify();
        }
        
        private void RotateStick()
        {
            _ctx.levelFlowState.Value = LevelFlowState.StickFalls;
            _startStickRotation.Notify();
        }

        private void DestroySticks()
        {
            _spawnedSticks.ForEach(Object.Destroy);
            _spawnedSticks.Clear();
        }

    }
}