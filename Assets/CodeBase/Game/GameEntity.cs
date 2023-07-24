using CodeBase.Data;
using CodeBase.Game.Gameplay;
using CodeBase.Game.Level;
using CodeBase.Game.MainMenu;
using External.Framework;
using External.Reactive;
using UniRx;
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
        private MainMenuEntity _mainMenuEntity;

        private readonly ReactiveTrigger _startGame = new();
        private readonly ReactiveTrigger _startLevel = new();
        private readonly ReactiveTrigger _finishLevel = new();
        private readonly ReactiveProperty<LevelFlowState> _levelFlowState = new();
        private readonly ReactiveProperty<float> _actualColumnXPosition = new();
        public GameEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreateLevelBuilder();
            CreateGameplayEntity();
            CreateMainMenu();
        }

        private void CreateLevelBuilder()
        {
            var levelBuilderEntityCtx = new LevelBuilderEntity.Ctx
            {
                contentProvider = _ctx.contentProvider,
                startLevel = _startLevel,
                finishLevel = _finishLevel,
                actualColumnXPosition = _actualColumnXPosition
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
                finishLevel = _finishLevel,
                uiRoot = _ctx.uiRoot,
                startGame = _startGame,
                levelFlowState = _levelFlowState,
                actualColumnXPosition = _actualColumnXPosition
            };
            _gameplayEntity = new GameplayEntity(gameplayEntityCtx);
            AddUnsafe(_gameplayEntity);
        }

        private void CreateMainMenu()
        {
            var mainMenuEntityCtx = new MainMenuEntity.Ctx()
            {
                contentProvider = _ctx.contentProvider,
                uiRoot = _ctx.uiRoot,
                startLevel = _startLevel,
                startGame = _startGame
            };
            _mainMenuEntity = new MainMenuEntity(mainMenuEntityCtx);
            AddUnsafe(_mainMenuEntity);
        }
    }
}