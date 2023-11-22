using CodeBase.Data;
using DG.Tweening;
using External.Framework;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.Camera
{
    public class CameraPm : BaseDisposable
    {
        public struct Ctx
        {
            public IContentProvider contentProvider;
            public IReadOnlyReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public IReadOnlyReactiveTrigger startLevel;
            public ReactiveEvent<LevelFlowState> changeLevelFlowState;
        }

        private readonly Ctx _ctx;
        private CameraPm _pm;
        private readonly Transform _camera;

        public CameraPm(Ctx ctx)
        {
            _ctx = ctx;
            _camera = Object.Instantiate(_ctx.contentProvider.Camera());
            
            AddToDisposables(_ctx.levelFlowState.Subscribe(x =>
            {
                if (x == LevelFlowState.CameraRun)
                {
                    var cameraPosition = _ctx.actualColumnXPosition.Value + _ctx.contentProvider.LevelConfig().GetCameraColumnXOffset;
                    _camera.DOMoveX(cameraPosition, 1).OnComplete(() =>
                    {
                        _ctx.changeLevelFlowState.Notify(LevelFlowState.PlayerIdle);
                    });
                }
            }));
            
            AddToDisposables(_ctx.startLevel.Subscribe(() =>
            {
                _camera.position = new Vector3(_ctx.contentProvider.LevelConfig().GetCameraColumnXOffset, _camera.position.y,
                    _camera.position.z);
            }));
        }
        
        protected override void OnDispose()
        {
            base.OnDispose();

            if (_camera != null)
            {
                Object.Destroy(_camera.gameObject);
            }
        }

    }
}