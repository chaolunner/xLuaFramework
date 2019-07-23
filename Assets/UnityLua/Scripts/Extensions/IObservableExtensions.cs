using System;

namespace UniEasy
{
    public static partial class IObservableExtensions
    {
        public static IDisposable luaSubscribe<T>(this IObservable<T> source, Action<T> onNext)
        {
            return UniRx.ObservableExtensions.Subscribe(source, onNext);
        }
    }
}
