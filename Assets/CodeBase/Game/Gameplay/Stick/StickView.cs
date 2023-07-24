using External.Reactive;
using UnityEngine;

namespace CodeBase.Game.Gameplay.Stick
{
    public class StickView : MonoBehaviour
    {
        public struct Ctx
        {
            public ReactiveEvent<bool> startGrowing;
        }

        private Ctx _ctx;
        
        public void Init(Ctx ctx)
        {
            _ctx = ctx;
        }
        
    }
}