using Cysharp.Threading.Tasks;
using DG.Tweening;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.Stick
{
    public class StickView : MonoBehaviour
    {
        public struct Ctx
        {
            public IReadOnlyReactiveProperty<LevelFlowState> levelFlowState;
            public ReactiveProperty<float> stickLength;
            public ReactiveTrigger stickIsDown;
            public IReadOnlyReactiveTrigger startStickGrow;
            public IReadOnlyReactiveTrigger startStickRotation;
        }

        private Ctx _ctx;
        private CompositeDisposable _disposables;
        
        public void Init(Ctx ctx)
        {
            _ctx = ctx;
            _disposables = new CompositeDisposable();
            _ctx.startStickGrow.Subscribe(GrowStickUp).AddTo(_disposables);
            _ctx.startStickRotation.Subscribe(RotateStick).AddTo(_disposables);
        }
        
        private async void GrowStickUp()
        {
            var stickHeight = 0f;
            var stickWidth = transform.localScale.x;
            while (_ctx.levelFlowState.Value == LevelFlowState.StickGrowsUp)
            {
                stickHeight += Time.deltaTime * 6;
                transform.localScale = new Vector3(stickWidth, stickHeight, 1);
                await UniTask.Yield();
            }
            _ctx.stickLength.Value = stickHeight;
        }
        
        private void RotateStick()
        {
            transform.DORotate(new Vector3(0, 0, -90f), 0.5f)
                .OnComplete(() => _ctx.stickIsDown.Notify());
            _disposables.Dispose();
        }
        
    }
}