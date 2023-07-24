using DG.Tweening;
using External.Reactive;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.Player
{
    public class PlayerView : MonoBehaviour
    {
        public struct Ctx
        {
            public IReadOnlyReactiveTrigger startLevel;
            public IReadOnlyReactiveProperty<float> actualColumnXPosition;
            public IReadOnlyReactiveEvent<float> movePlayerTo;
            public ReactiveTrigger playerFinishMoving;
        }
        private Ctx _ctx;

        public void Init (Ctx ctx)
        {
            _ctx = ctx;
            _ctx.startLevel.Subscribe(() =>
            {
                transform.position = new Vector2( Constant.PlayerOnColumnXOffset, Constant.PlayerYPosition);
                gameObject.SetActive(true);
            }).AddTo(this);

            _ctx.movePlayerTo.SubscribeWithSkip(x => transform.DOMoveX(x, 2).OnComplete(() =>
            {
                _ctx.playerFinishMoving.Notify();
            })).AddTo(this);
        }
    }
}