using CodeBase.Data;
using CodeBase.Game.DataSave;
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
            public IContentProvider contentProvider;
            public RectTransform uiRoot;
        }
        private readonly Ctx _ctx;
        private LevelBuilderEntity _levelBuilderEntity;
        private GameplayEntity _gameplayEntity;
        private MainMenuEntity _mainMenuEntity;
        private DataSaveEntity _dataSaveEntity;

        private readonly ReactiveTrigger _startGame = new();
        private readonly ReactiveTrigger _startLevel = new();
        private readonly ReactiveTrigger _finishLevel = new();
        private readonly ReactiveProperty<LevelFlowState> _levelFlowState = new();
        private readonly ReactiveProperty<float> _actualColumnXPosition = new();
        private readonly ReactiveProperty<float> _nextColumnXPosition = new();
        private readonly ReactiveTrigger _showStartMenu = new();
        private readonly ReactiveProperty<bool> _columnIsReachable = new();
        private readonly ReactiveProperty<IDataSave> _dataSave = new();

        public GameEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreateDataSave();
            CreateLevelBuilder();
            CreateGameplayEntity();
            CreateMainMenu();
        }
        
        private void CreateDataSave()
        {
            var dataSAveEntityCtx = new DataSaveEntity.Ctx()
            {
                dataSave = _dataSave
            };
            _dataSaveEntity = new DataSaveEntity(dataSAveEntityCtx);
            AddToDisposables(_dataSaveEntity);
        }

        private void CreateLevelBuilder()
        {
            var levelBuilderEntityCtx = new LevelBuilderEntity.Ctx
            {
                contentProvider = _ctx.contentProvider,
                startLevel = _startLevel,
                actualColumnXPosition = _actualColumnXPosition,
                nextColumnXPosition = _nextColumnXPosition,
                levelFlowState = _levelFlowState,
                columnIsReachable = _columnIsReachable
            };
            _levelBuilderEntity = new LevelBuilderEntity(levelBuilderEntityCtx);
            AddToDisposables(_levelBuilderEntity);
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
                actualColumnXPosition = _actualColumnXPosition,
                nextColumnXPosition = _nextColumnXPosition,
                showStartMenu = _showStartMenu,
                columnIsReachable = _columnIsReachable,
                dataSave = _dataSave
            };
            _gameplayEntity = new GameplayEntity(gameplayEntityCtx);
            AddToDisposables(_gameplayEntity);
        }

        private void CreateMainMenu()
        {
            var mainMenuEntityCtx = new MainMenuEntity.Ctx()
            {
                contentProvider = _ctx.contentProvider,
                uiRoot = _ctx.uiRoot,
                startLevel = _startLevel,
                startGame = _startGame,
                finishLevel = _finishLevel,
                showStartMenu = _showStartMenu,
            };
            _mainMenuEntity = new MainMenuEntity(mainMenuEntityCtx);
            AddToDisposables(_mainMenuEntity);
        }
        
    }
}