using System;

namespace External.Reactive
{
    public interface IReadOnlyReactiveTrigger
    {
        IDisposable Subscribe(Action action);
    }
}