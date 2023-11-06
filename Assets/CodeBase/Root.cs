using CodeBase.Data;
using External.Framework;
using UnityEngine;

namespace CodeBase
{
    public class Root : BaseDisposable
    {
        private static Root _instance;
        
        public static Root CreateRoot(Ctx ctx)
        {
            return _instance ??= new Root(ctx);
        }
        
        private readonly RootEntity _rootEntity;
        
        public struct Ctx
        {
            public IContentProvider contentProvider;
            public RectTransform uiRoot;
        }
        private readonly Ctx _ctx;
        
        private Root(Ctx ctx)
        {
            _ctx = ctx;
            var rootCtx = new RootEntity.Ctx
            {
                contentProvider = _ctx.contentProvider,
                uiRoot = _ctx.uiRoot,
            };
            _rootEntity = new RootEntity(rootCtx);
            AddUnsafe(_rootEntity);
        }
    }
}