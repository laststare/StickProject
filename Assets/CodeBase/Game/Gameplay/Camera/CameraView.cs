using UnityEngine;

namespace CodeBase.Game.Gameplay.Camera
{
    public class CameraView : MonoBehaviour
    {
        public struct Ctx
        {
            
        }
        private Ctx _ctx;
        
        public void Init(Ctx ctx)
        {
            _ctx = ctx;
        }
    }
}