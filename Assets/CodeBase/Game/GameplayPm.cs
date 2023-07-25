using System;
using Cysharp.Threading.Tasks;
using External.Framework;
using External.Reactive;
using UniRx;

namespace CodeBase.Game
{
    public class GameplayPm : BaseDisposable
    {
        public struct Ctx
        {
            public ReactiveProperty<LevelFlowState> levelFlowState;
            public IReadOnlyReactiveTrigger startLevel;
        }

        private readonly Ctx _ctx;
        
        public GameplayPm(Ctx ctx)
        {
            _ctx = ctx;
           
        }
        
    }
}