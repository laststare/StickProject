using CodeBase.Data;
using CodeBase.Game.Gameplay.Camera;
using CodeBase.Game.Gameplay.Player;
using CodeBase.Game.Gameplay.ScoreCounter;
using CodeBase.Game.Gameplay.Stick;
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
        }

        private readonly Ctx _ctx;
        private GameplayPm _pm;
        private CameraEntity _cameraEntity;
        private ScoreCounterEntity _scoreCounterEntity;
        private PlayerEntity _playerEntity;
        private StickEntity _stickEntity;
        private readonly ReactiveProperty<float> _stickLength = new();

        public GameplayEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreatePm();
            CreateCameraEntity();
            CreateScoreCounter();
            CreatePlayerEntity();
            CreateStickEntity();
        }

        private void CreatePm()
        {
            var gameplayPmCtx = new GameplayPm.Ctx()
            {
                startGame = _ctx.startGame,
                levelFlowState = _ctx.levelFlowState,
                startLevel = _ctx.startLevel
            };
            _pm = new GameplayPm(gameplayPmCtx);
            AddUnsafe(_pm);
        }

        private void CreateCameraEntity()
        {
            var cameraEntityCtx = new CameraEntity.Ctx()
            {
                contentProvider = _ctx.contentProvider,
                startLevel = _ctx.startLevel,
                finishLevel = _ctx.finishLevel
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
                startLevel = _ctx.startLevel
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
                stickLength = _stickLength
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
                stickLength = _stickLength
            };
            _stickEntity = new StickEntity(stickEntityCtx);
            AddUnsafe(_stickEntity);
        }
    }
}