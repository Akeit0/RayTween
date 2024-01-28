#if RAYTWEEN_SUPPORT_UNITASK
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RayTween.Internal;

namespace RayTween
{
    /// <summary>
    /// Provides extension methods for UniTask integration.
    /// </summary>
    public static class RayTweenUniTaskExtensions
    {
        
        public static UniTask.Awaiter GetAwaiter(this TweenHandle handle)
        {
            return ToUniTask(handle).GetAwaiter();
        }
        
        public static UniTask.Awaiter GetAwaiter<TValue,TPlugin> (this TweenHandle<TValue,TPlugin>  handle)where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            if (handle.IsIdling) handle.Schedule();
            return ToUniTask(handle.AsNoType()).GetAwaiter();
        }

        /// <summary>
        /// Convert Tween handle to UniTask.
        /// </summary>
        /// <param name="handle">This Tween handle</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>
        public static UniTask ToUniTask(this TweenHandle handle, CancellationToken cancellationToken = default)
        {
            if (!handle.IsActive())
            {
              
                return UniTask.CompletedTask;
            }
            return new UniTask(TweenConfiguredSource.Create(handle, cancellationToken, out var token), token);
        }

        /// <summary>
        /// Create a Tween data and bind it to AsyncReactiveProperty.
        /// </summary>
        /// <typeparam name="TValue">The type of value to animate</typeparam>
        
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="progress">Target object that implements IProgress</param>
        /// <returns>Handle of the created Tween data.</returns>
        public static TweenHandle<TValue, TPlugin>  BindToAsyncReactiveProperty<TValue, TPlugin>(this TweenFromTo<TValue, TPlugin> handle, AsyncReactiveProperty<TValue> reactiveProperty)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            Error.IsNull(reactiveProperty);
            return handle.Bind(reactiveProperty, static ( target,x) =>
            {
                target.Value = x;
            });
        }
    }

    internal sealed class TweenConfiguredSource : IUniTaskSource, ITaskPoolNode<TweenConfiguredSource>
    {
        static TaskPool<TweenConfiguredSource> pool;
        TweenConfiguredSource nextNode;
        public ref TweenConfiguredSource NextNode => ref nextNode;

        static TweenConfiguredSource()
        {
            TaskPool.RegisterSizeGetter(typeof(TweenConfiguredSource), () => pool.Size);
        }


        TweenHandle tweenHandle;
        CancellationToken cancellationToken;
        CancellationTokenRegistration cancellationRegistration;
        bool canceled;

        UniTaskCompletionSourceCore<AsyncUnit> core;

        TweenConfiguredSource()
        {
          
        }

        public static IUniTaskSource Create(TweenHandle tweenHandle, CancellationToken cancellationToken, out short token)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                tweenHandle.Cancel();
                return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
            }

            if (!pool.TryPop(out var result))
            {
                result = new TweenConfiguredSource();
            }

            result.tweenHandle = tweenHandle;
            result.cancellationToken = cancellationToken;
            result.canceled = false;

            ref var callbacks = ref TweenStorageManager.GetTweenCallbacks(tweenHandle);
            callbacks.Append(result,static (target,tweenResult)=>target.OnDisposeCallbackDelegate(tweenResult));
           

            if (cancellationToken.CanBeCanceled)
            {
                result.cancellationRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(static x =>
                {
                    var source = (TweenConfiguredSource)x;
                    var tweenHandle = source.tweenHandle;
                    if (tweenHandle.IsActive()) tweenHandle.Cancel();
                    source.core.TrySetCanceled(source.cancellationToken);
                }, result);
            }

            TaskTracker.TrackActiveTask(result, 3);

            token = result.core.Version;
            return result;
        }
        void OnDisposeCallbackDelegate(TweenResult result)
        {
            if (result.IsCompleted)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    canceled = true;
                }
                if (canceled)
                {
                    core.TrySetCanceled(cancellationToken);
                }
                else
                {
                    core.TrySetResult(AsyncUnit.Default);
                }
            }
            else
            {
               
                core.TrySetCanceled(cancellationToken);
            }
            
        }


        public void GetResult(short token)
        {
            try
            {
                core.GetResult(token);
            }
            finally
            {
                TryReturn();
            }
        }

        public UniTaskStatus GetStatus(short token)
        {
            return core.GetStatus(token);
        }

        public UniTaskStatus UnsafeGetStatus()
        {
            return core.UnsafeGetStatus();
        }

        public void OnCompleted(Action<object> continuation, object state, short token)
        {
            core.OnCompleted(continuation, state, token);
        }

        bool TryReturn()
        {
            TaskTracker.RemoveTracking(this);
            core.Reset();
            cancellationRegistration.Dispose();

            RestoreOriginalCallback();

            tweenHandle = default;
            cancellationToken = default;
            return pool.TryPush(this);
        }

        void RestoreOriginalCallback()
        {
            if (!tweenHandle.IsActive()) return;
            ref var callbacks = ref TweenStorageManager.GetTweenCallbacks(tweenHandle);
            callbacks.RemoveTarget(this);
        }
    }
}
#endif