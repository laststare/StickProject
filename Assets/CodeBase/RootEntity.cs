using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Game;
using External.Framework;
using UnityEngine;

namespace CodeBase
{
    public class RootEntity : BaseDisposable
    {
        public struct Ctx
        {
            public IContentProvider contentProvider;
            public RectTransform uiRoot;
        }
        
        private readonly Ctx _ctx;
        private readonly GameEntity _gameEntity;
        
        public RootEntity(Ctx ctx)
        {
            _ctx = ctx;

            var gameEntityCtx = new GameEntity.Ctx
            {
                contentProvider = _ctx.contentProvider,
                uiRoot = _ctx.uiRoot,
            };

            _gameEntity = new GameEntity(gameEntityCtx);
            AddToDisposables(_gameEntity);
        }
    }
}