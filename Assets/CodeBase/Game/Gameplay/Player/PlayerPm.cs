using External.Framework;
using UniRx;
using UnityEngine;

namespace CodeBase.Game.Gameplay.Player
{
    public class PlayerPm : BaseDisposable
    {
        public struct Ctx
        {
           
        }
        private readonly Ctx _ctx;

        public PlayerPm (Ctx ctx)
        {
            _ctx = ctx;
           
        }
        
    }
}