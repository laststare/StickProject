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

        }
        private Ctx _ctx;
        private float _columnPositionOffset = 0.5f;

        public void Init (Ctx ctx)
        {
            _ctx = ctx;
            _ctx.startLevel.Subscribe(() =>
            {
                transform.position = new Vector2(_ctx.actualColumnXPosition.Value + _columnPositionOffset, Constant.PlayerYPosition);
                gameObject.SetActive(true);
            }).AddTo(this);
        }
    }
}