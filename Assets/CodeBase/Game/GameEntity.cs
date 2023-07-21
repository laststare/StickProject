using CodeBase.Data;
using External.Framework;
using UnityEngine;

namespace CodeBase.Game
{
    public class GameEntity : BaseDisposable
    {
        public struct Ctx
        {
            public ContentProvider contentProvider;
            public RectTransform uiRoot;
        }
        private readonly Ctx _ctx;
        public GameEntity(Ctx ctx)
        {
            _ctx = ctx;
        }
    }
}