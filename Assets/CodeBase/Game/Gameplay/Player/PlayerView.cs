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

        }
        private Ctx _ctx;

        public void Init (Ctx ctx)
        {
            _ctx = ctx;
            _ctx.startLevel.Subscribe(() =>
            {
                transform.position = new Vector2(_ctx.actualColumnXPosition.Value + Constant.PlayerOnColumnXOffset, Constant.PlayerYPosition);
                gameObject.SetActive(true);
            }).AddTo(this);

            _ctx.movePlayerTo.Subscribe(x => transform.DOMoveX(x, 2));
        }
    }
}