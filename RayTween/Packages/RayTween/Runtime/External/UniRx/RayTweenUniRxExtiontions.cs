#if RAYTWEEN_SUPPORT_UNIRX
using System;
using RayTween.Internal;
using UniRx;

namespace RayTween
{
    /// <summary>
    /// Provides extension methods for UniRx integration.
    /// </summary>
    public static class RayTweenUniRxExtensions
    {
        /// <summary>
        /// Create the motion as IObservable.
        /// </summary>
        /// <typeparam name="TValue">The type of value to animate</typeparam>
        /// <typeparam name="TPlugin">The type of adapter that support value animation</typeparam>
        /// <param name="handle">This fromTo</param>
        /// <returns>Observable of the created motion.</returns>
        public static IObservable<TValue> ToObservable<TValue, TPlugin>(this TweenHandle<TValue, TPlugin> handle)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            var subject = new Subject<TValue>();
            if (handle.TryGetBuffer(out var buffer))
            {
                static void Action(Subject<TValue> subject, TValue x) => subject.OnNext(x);
                buffer.CallbackData.TargetCount = 1;
                buffer.CallbackData.Target1 = subject;
                buffer.CallbackData.UpdateAction = (Action<Subject<TValue>, TValue>)Action;
                handle.OnDispose(subject, static (subject, result) =>
                {
                    if (result.IsCompleted)
                    {
                        subject.OnCompleted();
                    }
                    else
                    {
                        subject.OnError(result.Error);
                    }
                });
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Create a motion data and bind it to ReactiveProperty.
        /// </summary>
        /// <typeparam name="TValue">The type of value to animate</typeparam>
        /// <typeparam name="TPlugin">The type of adapter that support value animation</typeparam>
        /// <param name="builder">This builder</param>
        /// <param name="progress">Target object that implements IProgress</param>
        /// <returns>Handle of the created motion data.</returns>
        public static TweenHandle<TValue, TPlugin> BindToReactiveProperty<TValue, TPlugin>(
            this TweenFromTo<TValue, TPlugin> fromTo, ReactiveProperty<TValue> reactiveProperty)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            Error.IsNull(reactiveProperty);
            return fromTo.Bind(reactiveProperty, static (target, x) => { target.Value = x; });
        }
    }
}
#endif