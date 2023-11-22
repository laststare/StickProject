using CodeBase.Data;
using External.Framework;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.Camera
{
    public class CameraEntity : BaseDisposable
    {
        public struct Ctx
        {
            public IContentProvider contentProvider;
            public IReadOnlyReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveTrigger startLevel;
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public ReactiveEvent<LevelFlowState> changeLevelFlowState;
        }

        private readonly Ctx _ctx;
        private CameraPm _pm;
        private CameraView _view;

        public CameraEntity(Ctx ctx)
        {
            _ctx = ctx;
            CreateCameraPm();
        }

        private void CreateCameraPm()
        {
            var cameraPmCtx = new CameraPm.Ctx()
            {
                actualColumnXPosition = _ctx.actualColumnXPosition,
                levelFlowState = _ctx.levelFlowState,
                changeLevelFlowState = _ctx.changeLevelFlowState,
                contentProvider = _ctx.contentProvider,
                startLevel = _ctx.startLevel,
            };
            _pm = new CameraPm(cameraPmCtx);
            AddToDisposables(_pm);
        }
        
      
    }
}