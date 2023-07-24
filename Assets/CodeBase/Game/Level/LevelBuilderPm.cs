using System;
using System.Collections.Generic;
using CodeBase.Data;
using External.Framework;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Level
{
    public class LevelBuilderPm : BaseDisposable
    {
        public struct Ctx 
        {
            public ContentProvider contentProvider;
            public IReadOnlyReactiveTrigger startLevel;
            public IReadOnlyReactiveTrigger finishLevel;
            public ReactiveProperty<float> actualColumnXPosition;
        }

        private readonly Ctx _ctx;
        private readonly List<GameObject> _levelColumns = new List<GameObject>();
        private float _startXPosition;

        public LevelBuilderPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.startLevel.Subscribe(CreateFirstColumns));
            AddUnsafe(_ctx.finishLevel.Subscribe(DestroyLevel));
        }

        private void NextColumn()
        {
            
        }

        private void CreateFirstColumns()
        {
            for (var i = 0; i < 2; i++)
            {
                var column = UnityEngine.Object.Instantiate(_ctx.contentProvider.Views.Levelcolumn,
                    new Vector3(_startXPosition, 0, 0),
                    Quaternion.identity);
                _levelColumns.Add(column);
                _startXPosition += 5 + UnityEngine.Random.Range(0, 4);
            }
        }

        private void DestroyLevel()
        {
            foreach (var column in _levelColumns) 
                UnityEngine.Object.Destroy(column);
            _levelColumns.Clear();
            _startXPosition = 0;
        }
    }
}