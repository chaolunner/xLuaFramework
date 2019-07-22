using System;
using XLua;

namespace UniEasy
{
    public static partial class IObservableExtensions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext)
        {
            return UniRx.ObservableExtensions.Subscribe(source, onNext);
        }
    }
}
