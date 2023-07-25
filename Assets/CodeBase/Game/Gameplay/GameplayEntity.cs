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
            public ContentProvider contentProvider;
            public RectTransform uiRoot;
            public IReadOnlyReactiveTrigger startLevel;
            public ReactiveTrigger finishLevel;
            public ReactiveTrigger startGame;
            public ReactiveProperty<LevelFlowState> levelFlowState;
            public ReactiveProperty<float> actualColumnXPosition;
            public ReactiveProperty<float> nextColumnXPosition;
            public IReadOnlyReactiveTrigger showStartMenu;
        }

        private readonly Ctx _ctx;
        private CameraEntity _cameraEntity;
        private ScoreCounterEntity _scoreCounterEntity;
        private PlayerEntity _playerEntity;
        private StickEntity _stickEntity;
        private readonly ReactiveProperty<float> _stickLength = new();
        private readonly ReactiveEvent<int> _addScore = new();


        public GameplayEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreateCameraEntity();
            CreateScoreCounter();
            CreatePlayerEntity();
            CreateStickEntity();
            StartGame();
        }

        private async void StartGame()
        {
            await UniTask.DelayFrame(1); 
            _ctx.startGame.Notify(); 
        }

        private void CreateCameraEntity()
        {
            var cameraEntityCtx = new CameraEntity.Ctx()
            {
                contentProvider = _ctx.contentProvider,
                startLevel = _ctx.startLevel,
                levelFlowState = _ctx.levelFlowState,
                actualColumnXPosition = _ctx.actualColumnXPosition
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
                addScore = _addScore,
                finishLevel = _ctx.finishLevel,
                showStartMenu = _ctx.showStartMenu
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
                addScore = _addScore,
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
            };
            _stickEntity = new StickEntity(stickEntityCtx);
            AddUnsafe(_stickEntity);
        }
    }
}