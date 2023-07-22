using CodeBase.Data;
using CodeBase.Game.Gameplay;
using CodeBase.Game.Level;
using External.Framework;
using External.Reactive;
using UnityEngine;

namespace CodeBase.Game
{
    public class GameEntity : BaseDisposable
    {
        public struct Ctx
        {
            public ContentProvider contentProvider;
            public RectTransform uiRoot;
        }
        private readonly Ctx _ctx;
        private LevelBuilderEntity _levelBuilderEntity;
        private GameplayEntity _gameplayEntity;

        private readonly ReactiveEvent<int> _startLevel = new();
        private readonly ReactiveTrigger _finishLevel = new();
        public GameEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreateLevelBuilder();
            CreateGameplayEntity();
            _startLevel.Notify(0);
        }

        private void CreateLevelBuilder()
        {
            var levelBuilderEntityCtx = new LevelBuilderEntity.Ctx
            {
                contentProvider = _ctx.contentProvider,
                startLevel = _startLevel,
                finishLevel = _finishLevel
            };
            _levelBuilderEntity = new LevelBuilderEntity(levelBuilderEntityCtx);
            AddUnsafe(_levelBuilderEntity);
        }

        private void CreateGameplayEntity()
        {
            var gameplayEntityCtx = new GameplayEntity.Ctx()
            {
                contentProvider = _ctx.contentProvider,
                startLevel = _startLevel,
                finishLevel = _finishLevel
            };
            _gameplayEntity = new GameplayEntity(gameplayEntityCtx);
            AddUnsafe(_gameplayEntity);
        }
    }
}