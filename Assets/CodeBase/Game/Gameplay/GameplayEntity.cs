using CodeBase.Data;
using CodeBase.Game.Gameplay.Camera;
using CodeBase.Game.Gameplay.Player;
using CodeBase.Game.Gameplay.ScoreCounter;
using CodeBase.Game.Gameplay.Stick;
using Cysharp.Threading.Tasks;
using External.Framework;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay
{
    public class GameplayEntity : BaseDisposable
    {
        public struct Ctx
        {
            public IContentProvider contentProvider;
            public RectTransform uiRoot;
            public ReactiveTrigger startLevel;
            public ReactiveTrigger finishLevel;
            public ReactiveTrigger startGame;
            public ReactiveProperty<LevelFlowState> levelFlowState;
            public ReactiveProperty<float> actualColumnXPosition;
            public ReactiveProperty<float> nextColumnXPosition;
            public IReadOnlyReactiveTrigger showStartMenu;
            public ReactiveProperty<bool> columnIsReachable;
        }

        private readonly Ctx _ctx;
        private GameFlowPm _gameFlowPm;
        private CameraEntity _cameraEntity;
        private ScoreCounterEntity _scoreCounterEntity;
        private PlayerEntity _playerEntity;
        private StickEntity _stickEntity;
        private readonly ReactiveProperty<float> _stickLength = new();
        private readonly ReactiveEvent<LevelFlowState> _changeLevelFlowState = new();
        
        public GameplayEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreateGameFlowPm();
            CreateCameraEntity();
            CreateScoreCounter();
            CreatePlayerEntity();
            CreateStickEntity();
        }
        
        /// <summary>
        /// ниже, для всех классов переменная levelFlowState в состоянии readonly
        /// переключание игровых состяний происходит вызовом changeLevelFlowState
        /// </summary>
        private void CreateGameFlowPm()
        {
            var gameFlowPmCtx = new GameFlowPm.Ctx()
            {
                levelFlowState = _ctx.levelFlowState,
                changeLevelFlowState = _changeLevelFlowState,
                startLevel = _ctx.startLevel,
                startGame = _ctx.startGame
            };
            _gameFlowPm = new GameFlowPm(gameFlowPmCtx);
            AddUnsafe(_gameFlowPm);
        }

        private void CreateCameraEntity()
        {
            var cameraEntityCtx = new CameraEntity.Ctx()
            {
                contentProvider = _ctx.contentProvider,
                startLevel = _ctx.startLevel,
                levelFlowState = _ctx.levelFlowState,
                actualColumnXPosition = _ctx.actualColumnXPosition,
                changeLevelFlowState = _changeLevelFlowState
            };
            _cameraEntity = new CameraEntity(cameraEntityCtx);
            AddUnsafe(_cameraEntity);
        }

        private void CreateScoreCounter()
        {
            var scoreCounterEntityCtx = new ScoreCounterEntity.Ctx()
            {
                contentProvider = _ctx.contentProvider,
                uiRoot = _ctx.uiRoot,
                startGame = _ctx.startGame,
                startLevel = _ctx.startLevel,
                finishLevel = _ctx.finishLevel,
                showStartMenu = _ctx.showStartMenu,
                columnIsReachable = _ctx.columnIsReachable,
                nextColumnXPosition = _ctx.nextColumnXPosition
            };
            _scoreCounterEntity = new ScoreCounterEntity(scoreCounterEntityCtx);
            AddUnsafe(_scoreCounterEntity);
        }

        private void CreatePlayerEntity()
        {
            var playerEntityCtx = new PlayerEntity.Ctx()
            {
                contentProvider = _ctx.contentProvider,
                startLevel = _ctx.startLevel,
                actualColumnXPosition = _ctx.actualColumnXPosition,
                nextColumnXPosition = _ctx.nextColumnXPosition,
                levelFlowState = _ctx.levelFlowState,
                stickLength = _stickLength,
                finishLevel = _ctx.finishLevel,
                columnIsReachable = _ctx.columnIsReachable,
                changeLevelFlowState = _changeLevelFlowState
            };
            _playerEntity = new PlayerEntity(playerEntityCtx);
            AddUnsafe(_playerEntity);
        }

        private void CreateStickEntity()
        {
            var stickEntityCtx = new StickEntity.Ctx()
            {
                contentProvider = _ctx.contentProvider,
                actualColumnXPosition = _ctx.actualColumnXPosition,
                levelFlowState = _ctx.levelFlowState,
                stickLength = _stickLength,
                startLevel = _ctx.startLevel,
                columnIsReachable = _ctx.columnIsReachable,
                changeLevelFlowState = _changeLevelFlowState
            };
            _stickEntity = new StickEntity(stickEntityCtx);
            AddUnsafe(_stickEntity);
        }
    }
}