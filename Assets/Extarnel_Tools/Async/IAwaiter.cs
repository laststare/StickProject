using System.Runtime.CompilerServices;

namespace External.Extensions.Async
{
  public interface IAwaiter : INotifyCompletion
  {
    bool IsCompleted { get; }
    void GetResult();
    IAwaiter GetAwaiter();
  }
  public interface IAwaiter<out T> : IAwaiter
  {
    new bool IsCompleted { get; }
    new T GetResult();
    new IAwaiter<T> GetAwaiter();
  }
}