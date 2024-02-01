using System;
using System.Threading;
using System.Threading.Tasks.Sources;
namespace RayTween.Internal
{
    internal sealed class ValueTaskTweenConfiguredSource : IValueTaskSource, ITweenTaskSourcePoolNode<ValueTaskTweenConfiguredSource>
    {
        static TweenTaskSourcePool<ValueTaskTweenConfiguredSource> pool;

        ValueTaskTweenConfiguredSource nextNode;
        public ref ValueTaskTweenConfiguredSource NextNode => ref nextNode;
        TweenHandle tweenHandle;
        CancellationToken cancellationToken;
        CancellationTokenRegistration cancellationRegistration;

        ManualResetValueTaskSourceCore<object> core;

        static ValueTaskTweenConfiguredSource FromCanceled(out short token)
        {
            if (canceledSource == null)
            {
                canceledSource = new();
                canceledSource.core.SetException(new OperationCanceledException());
            }

            token = canceledSource.Version;
            return canceledSource;
        }
        static ValueTaskTweenConfiguredSource canceledSource;
        

        public static IValueTaskSource Create(TweenHandle tweenHandle, CancellationToken cancellationToken, out short token)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                tweenHandle.Cancel();
                return FromCanceled(out token);
            }

            if (!pool.TryPop(out var result))
            {
                result = new ValueTaskTweenConfiguredSource();
            }

            result.tweenHandle = tweenHandle;
            result.cancellationToken = cancellationToken;

           ref  var callbacks =ref  TweenStorageManager.GetTweenCallbacks(tweenHandle);
           callbacks.Append(result,static (target,tweenResult)=>target.OnDisposeCallbackDelegate(tweenResult));


            if (cancellationToken.CanBeCanceled)
            {
                result.cancellationRegistration = cancellationToken.Register(static x =>
                {
                    var source = (ValueTaskTweenConfiguredSource)x;
                    var tweenHandle = source.tweenHandle;
                    if (tweenHandle.IsActive())
                    {
                        tweenHandle.Cancel();
                    }
                    else
                    {
                        source.core.SetException(new OperationCanceledException());
                    }
                }, result);
            }

            token = result.core.Version;
            return result;
        }

        public short Version => core.Version;
        void OnDisposeCallbackDelegate(TweenResult result)
        {
            if (result.IsCompleted)
            { if (cancellationToken.IsCancellationRequested)
                {
                    core.SetException(new OperationCanceledException());
                }
                else
                {
                    core.SetResult(null);
                }
            }
            else
            {
               
                core.SetException(new OperationCanceledException());
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

        public ValueTaskSourceStatus GetStatus(short token)
        {
            return core.GetStatus(token);
        }

        public void OnCompleted(Action<object> continuation, object state, short token, ValueTaskSourceOnCompletedFlags flags)
        {
            core.OnCompleted(continuation, state, token, flags);
        }

        bool TryReturn()
        {
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