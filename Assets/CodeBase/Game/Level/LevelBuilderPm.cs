using System;
using System.Collections.Generic;
using CodeBase.Data;
using External.Framework;
using External.Reactive;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeBase.Game.Level
{
    public class LevelBuilderPm : BaseDisposable
    {
        public struct Ctx 
        {
            public IContentProvider contentProvider;
            public IReadOnlyReactiveTrigger startLevel;
            public ReactiveProperty<float> actualColumnXPosition;
            public ReactiveProperty<float> nextColumnXPosition;
            public IReadOnlyReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveProperty<bool> columnIsReachable;
            public int minColumnDistance;
        }

        private readonly Ctx _ctx;
        private readonly List<GameObject> _levelColumns = new List<GameObject>();
        private readonly int _minColumnDistance;

        public LevelBuilderPm(Ctx ctx)
        {
            _ctx = ctx;
            _minColumnDistance = _ctx.minColumnDistance;
            AddToDisposables(_ctx.startLevel.Subscribe(CreateFirstColumns));
            AddToDisposables(_ctx.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.CameraRun) NextColumn();
            }));
            AddToDisposables(_ctx.columnIsReachable.Subscribe(x =>
            {
                if (x)
                    RemoveOneColumn();
            }));
        }

        private void CreateFirstColumns()
        {
            DestroyLevel();
            AddColumn(0);
            NextColumn();
        }
        private void NextColumn()
        {
            _ctx.actualColumnXPosition.Value = _ctx.nextColumnXPosition.Value;
            _ctx.nextColumnXPosition.Value = _ctx.actualColumnXPosition.Value + _minColumnDistance + UnityEngine.Random.Range(0, 4);
            AddColumn(_ctx.nextColumnXPosition.Value);
        }
        
        private void AddColumn(float xPosition)
        {
            var column = Object.Instantiate(_ctx.contentProvider.LevelColumn(),
                new Vector3(xPosition, 0, 0),
                Quaternion.identity);
            _levelColumns.Add(column); 
        }

        private void DestroyLevel()
        {
            foreach (var column in _levelColumns) 
                UnityEngine.Object.Destroy(column);
            _levelColumns.Clear();
            _ctx.actualColumnXPosition.Value = 0;
            _ctx.nextColumnXPosition.Value = 0;
        }
        
        private void RemoveOneColumn()
        {
            if (_levelColumns.Count <= 2) return;
            Object.Destroy(_levelColumns[0].gameObject);
            _levelColumns.RemoveAt(0);
        }
    }
}