#if UNITY_2023_1_OR_NEWER
using System;
using System.Threading;

namespace RayTween.Internal
{
    internal sealed class AwaitableTweenConfiguredSource : ITweenTaskSourcePoolNode<AwaitableTweenConfiguredSource>
    {
        static TweenTaskSourcePool<AwaitableTweenConfiguredSource> pool;

        AwaitableTweenConfiguredSource nextNode;
        public ref AwaitableTweenConfiguredSource NextNode => ref nextNode;

        public static AwaitableTweenConfiguredSource CompletedSource
        {
            get
            {
                if (completedSource == null)
                {
                    completedSource = new();
                    completedSource.core.SetResult();
                }

                return completedSource;
            }
        }

        static AwaitableTweenConfiguredSource completedSource;

        public static AwaitableTweenConfiguredSource CanceledSource
        {
            get
            {
                if (canceledSource == null)
                {
                    canceledSource = new();
                    canceledSource.core.SetCanceled();
                }

                return canceledSource;
            }
        }

        static AwaitableTweenConfiguredSource canceledSource;


        TweenHandle tweenHandle;
        CancellationToken cancellationToken;
        CancellationTokenRegistration cancellationRegistration;

        readonly AwaitableCompletionSource core = new();

        public Awaitable Awaitable => core.Awaitable;


        public static AwaitableTweenConfiguredSource Create(TweenHandle tweenHandle,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                tweenHandle.Cancel();
                return CanceledSource;
            }

            if (!pool.TryPop(out var result))
            {
                result = new AwaitableTweenConfiguredSource();
            }

            result.tweenHandle = tweenHandle;
            result.cancellationToken = cancellationToken;

            ref var callbacks = ref TweenStorageManager.GetTweenCallbacks(tweenHandle);
            callbacks.Append(result, static (target, tweenResult) => target.OnDisposeCallbackDelegate(tweenResult));


            if (cancellationToken.CanBeCanceled)
            {
                result.cancellationRegistration = cancellationToken.Register(static x =>
                {
                    var source = (AwaitableTweenConfiguredSource)x;
                    var tweenHandle = source.tweenHandle;
                    if (tweenHandle.IsActive())
                    {
                        tweenHandle.Cancel();
                    }
                    else
                    {
                        source.core.SetCanceled();
                        source.TryReturn();
                    }
                }, result);
            }

            return result;
        }

        void OnDisposeCallbackDelegate(TweenResult result)
        {
            if (result.IsCompleted)
            {
                if (cancellationToken.IsCancellationRequested)
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

            TryReturn();
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

#endif