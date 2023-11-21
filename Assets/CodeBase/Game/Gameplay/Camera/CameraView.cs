using DG.Tweening;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.Camera
{
    public class CameraView : MonoBehaviour
    {
        public struct Ctx
        {
            public IReadOnlyReactiveEvent<float> moveCameraToNextColumn;
            public ReactiveTrigger cameraFinishMoving;
            public IReadOnlyReactiveTrigger startLevel;
            public float cameraColumnXOffset;
        }
        
        private Ctx _ctx;
        private float _cameraColumnXOffset;
        
        public void Init(Ctx ctx)
        {
            _ctx = ctx;
            _cameraColumnXOffset = _ctx.cameraColumnXOffset;
            _ctx.moveCameraToNextColumn.SubscribeWithSkip(x => transform.DOMoveX(x, 1).OnComplete(() =>
            {
                _ctx.cameraFinishMoving.Notify();
            })).AddTo(this);

            _ctx.startLevel.Subscribe(() =>
            {
                transform.position = new Vector3(_cameraColumnXOffset, transform.position.y,
                    transform.position.z);
            });
        }
    }
}