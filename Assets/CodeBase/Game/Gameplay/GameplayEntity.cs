using CodeBase.Data;
using CodeBase.Game.Gameplay.Camera;
using CodeBase.Game.Gameplay.ScoreCounter;
using External.Framework;
using External.Reactive;
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
        }

        private readonly Ctx _ctx;
        private GameplayPm _pm;
        private CameraEntity _cameraEntity;
        private ScoreCounterEntity _scoreCounterEntity;

        public GameplayEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreatePm();
            CreateCameraEntity();
            CreateScoreCounter();
        }

        private void CreatePm()
        {
            var gameplayPmCtx = new GameplayPm.Ctx()
            {
                startGame = _ctx.startGame
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
    }
}