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
            public ReactiveProperty<float> nextColumnXPosition;
        }

        private readonly Ctx _ctx;
        private readonly List<GameObject> _levelColumns = new List<GameObject>();

        public LevelBuilderPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.startLevel.Subscribe(CreateFirstColumns));
            AddUnsafe(_ctx.finishLevel.Subscribe(DestroyLevel));
        }

        private void NextColumn()
        {
            //var tmpActual = _ctx.actualColumnXPosition.Value;
            _ctx.actualColumnXPosition.Value += _ctx.nextColumnXPosition.Value;
            _ctx.nextColumnXPosition.Value = _ctx.actualColumnXPosition.Value + 4 + UnityEngine.Random.Range(0, 4);
            AddColumn(_ctx.nextColumnXPosition.Value);
        }

        private void CreateFirstColumns()
        {
            AddColumn(0);
            NextColumn();
        }

        private void AddColumn(float xPosition)
        {
            var column = UnityEngine.Object.Instantiate(_ctx.contentProvider.Views.Levelcolumn,
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
        }
    }
}