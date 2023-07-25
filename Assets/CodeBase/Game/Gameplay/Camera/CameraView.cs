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
            public IReadOnlyReactiveEvent<float> moveCameraTo;
            public ReactiveTrigger cameraFinishMoving;
        }
        private Ctx _ctx;
        
        public void Init(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.moveCameraTo.SubscribeWithSkip(x => transform.DOMoveX(x, 1).OnComplete(() =>
            {
                _ctx.cameraFinishMoving.Notify();
            })).AddTo(this);
        }
    }
}