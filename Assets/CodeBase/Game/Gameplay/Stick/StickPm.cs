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
            public ReactiveProperty<LevelFlowState> levelFlowState;
            public ReactiveTrigger createView;
            public ReactiveTrigger stickIsDown;
            public ReactiveTrigger startStickGrow ;
            public ReactiveTrigger startStickRotation;
            public ReactiveCollection<GameObject> spawnedSticks;
            public IReadOnlyReactiveTrigger startLevel;
            public IReadOnlyReactiveProperty<bool> columnIsReachable;
        }

        private readonly Ctx _ctx;
      
        private CompositeDisposable _clickHandlers;
        
        public StickPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.PlayerIdle) TmpClickDownSubscription();
            }));
            AddUnsafe(_ctx.startLevel.Subscribe(DestroySticks));
            AddUnsafe(_ctx.stickIsDown.Subscribe(() => _ctx.levelFlowState.Value = LevelFlowState.PlayerRun));
            AddUnsafe(_ctx.columnIsReachable.Subscribe(x =>
            {
                if(x) 
                    RemoveOneView();
            }));
        }

        private void TmpClickDownSubscription()
        {
            _clickHandlers = new CompositeDisposable();
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0)).Subscribe(
                    (_) =>
                    {
                        _ctx.createView.Notify();
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

        private void GrowStickUp()
        {
            _ctx.levelFlowState.Value = LevelFlowState.StickGrowsUp;
            _ctx.startStickGrow.Notify();
        }
        
        private void RotateStick()
        {
            _ctx.levelFlowState.Value = LevelFlowState.StickFalls;
            _ctx.startStickRotation.Notify();
        }
        
        private void DestroySticks()
        {
            foreach (var stick in _ctx.spawnedSticks) 
                Object.Destroy(stick);
            _ctx.spawnedSticks.Clear();
        }
        private void RemoveOneView()
        {
            if (_ctx.spawnedSticks.Count <= 2) return;
            Object.Destroy(_ctx.spawnedSticks[0].gameObject);
            _ctx.spawnedSticks.RemoveAt(0);
        }

    }
}