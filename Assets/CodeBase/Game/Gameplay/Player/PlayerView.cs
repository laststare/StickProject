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
            public IReadOnlyReactiveEvent<float> movePlayerTo;
            public ReactiveTrigger playerFinishMoving;
            public float playerOnColumnXOffset;
            public float playerYPosition;
        }
        private Ctx _ctx;
        private float _playerOnColumnXOffset, _playerYPosition;

        public void Init (Ctx ctx)
        {
            _ctx = ctx;
            _playerOnColumnXOffset = _ctx.playerOnColumnXOffset;
            _playerYPosition = _ctx.playerYPosition;
            _ctx.startLevel.Subscribe(() =>
            {
                transform.position = new Vector2( _playerOnColumnXOffset, _playerYPosition);
                gameObject.SetActive(true);
            }).AddTo(this);

            _ctx.movePlayerTo.SubscribeWithSkip(x => transform.DOMoveX(x, 2).OnComplete(() =>
            {
                _ctx.playerFinishMoving.Notify();
            })).AddTo(this);
        }
    }
}