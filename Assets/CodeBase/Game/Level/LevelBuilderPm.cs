using System;
using CodeBase.Data;
using External.Framework;
using External.Reactive;
using UnityEngine;

namespace CodeBase.Game.Level
{
    public class LevelBuilderPm : BaseDisposable
    {
        public struct Ctx 
        {
            public ContentProvider contentProvider;
            public ReactiveEvent<int> startLevel;
            public ReactiveTrigger finishLevel;
        }

        private readonly Ctx _ctx;
        private GameObject[] _levelColumns;

        public LevelBuilderPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.startLevel.SubscribeWithSkip(CreateLevel));
            AddUnsafe(_ctx.finishLevel.Subscribe(DestroyLevel));
        }

        private void CreateLevel(int levelNum)
        {
            var startXPosition = 0;
            var columnsCount = _ctx.contentProvider.Settings.LevelConfig.GetColumnCountByLevel(levelNum);
            _levelColumns = new GameObject[columnsCount];
            for (var i = 0; i < columnsCount; i++)
            {
                var column = UnityEngine.Object.Instantiate(_ctx.contentProvider.Views.Levelcolumn,
                    new Vector3(startXPosition, 0, 0),
                    Quaternion.identity);
                _levelColumns[i] = column;
                startXPosition += 5 + UnityEngine.Random.Range(0, 5);
            }
        }

        private void DestroyLevel()
        {
            foreach (var column in _levelColumns)
            {
                UnityEngine.Object.Destroy(column);
            }
            _levelColumns = null;
        }
    }
}