using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Profiling;

namespace RayTween.Internal
{
    internal interface IUpdateRunner
    {
        public void Update(double time, double unscaledTime, double realtime);
        public void Reset();
    }

    internal sealed class UpdateRunner<TValue, TPlugin> : IUpdateRunner
        where TValue : unmanaged
        where TPlugin : unmanaged, ITweenPlugin<TValue>
    {
        public UpdateRunner(TweenStorage<TValue, TPlugin> storage)
        {
            this.storage = storage;
        }

        readonly TweenStorage<TValue, TPlugin> storage;
        static readonly string Marker = $"[UpdateRunner<{typeof(TValue).Name},{typeof(TPlugin).Name}>]";

        public unsafe void Update(double time, double unscaledTime, double realtime)
        {
         
            var count = storage.Count;
            if(count==0)return;
          
            
            using var output = new NativeArray<TValue>(count, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            using var completedIndexList = new NativeList<int>(count, Allocator.TempJob);

            fixed (TweenData<TValue, TPlugin>* dataPtr = storage.dataArray)
            {
                // update data
                var job = new TweenUpdateJob<TValue, TPlugin>()
                {
                    DataPtr = dataPtr,
                    Time = time,
                    UnscaledTime = unscaledTime,
                    Realtime = realtime,
                    Output = output,
                    CompletedIndexList = completedIndexList.AsParallelWriter()
                };
                if (count < 32)
                {
                    job.Run(count);
                }
                else job.Schedule(count, 16).Complete();
                Profiler.BeginSample(Marker);
                // invoke delegates
                var callbackSpan = storage.GetCallbacksSpan();
                var outputPtr = (TValue*)output.GetUnsafePtr();
                for (int i = 0; i < callbackSpan.Length; i++)
                {
                    ref var status =ref dataPtr[i].Status;
                    ref var callbackData = ref callbackSpan[i];
                    if (status == TweenStatus.Playing || (status == TweenStatus.Delayed && !callbackData.SkipValuesDuringDelay))
                    {
                        try
                        {
                            callbackData.InvokeUnsafe(in outputPtr[i]);
                        }
                        catch (Exception ex)
                        {
                            TweenDispatcher.GetUnhandledExceptionHandler()?.Invoke(ex);
                            if (callbackData.CancelOnError)
                            {
                                status = TweenStatus.Canceled;
                                callbackData.InvokeAndDispose(new TweenResult(){ResultType = TweenResultType.CancelWithError,Error = ex});

                            }
                        }
                    }
                    else if (status == TweenStatus.Completed)
                    {
                        try
                        {
                            callbackData.InvokeUnsafe(outputPtr[i]);
                        }
                        catch (Exception ex)
                        {
                            TweenDispatcher.GetUnhandledExceptionHandler()?.Invoke(ex);
                            if (callbackData.CancelOnError)
                            {
                                status = TweenStatus.Canceled;
                                callbackData.InvokeAndDispose(new TweenResult(){ResultType = TweenResultType.CancelWithError,Error = ex});

                                continue;
                            }
                        }

                        callbackData.InvokeAndDispose(TweenResult.Completed);

                    }
                }
            }
            Profiler.EndSample();
            storage.RemoveAll(completedIndexList);
        }

        public void Reset()
        {
            storage.Reset();
        }
    }
}