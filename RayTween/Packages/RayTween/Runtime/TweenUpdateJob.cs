using Unity.Burst;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace RayTween
{
    /// <summary>
    /// A job that updates the status of the tween data and outputs the current value.
    /// </summary>
    /// <typeparam name="TValue">The type of value to animate</typeparam>
    /// <typeparam name="TOptions">The type of special parameters given to the tween data</typeparam>
    /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
    [BurstCompile]
    public unsafe struct TweenUpdateJob<TValue,TPlugin> : IJobParallelFor
        where TValue : unmanaged  where TPlugin : unmanaged, ITweenPlugin<TValue>
    {
        [NativeDisableUnsafePtrRestriction] public TweenData<TValue, TPlugin>* DataPtr;
        [ReadOnly] public double Time;
        [ReadOnly] public double UnscaledTime;
        [ReadOnly] public double Realtime;

        [WriteOnly] public NativeList<int>.ParallelWriter CompletedIndexList;
        [WriteOnly] public NativeArray<TValue> Output;

        public void Execute([AssumeRange(0, int.MaxValue)] int index)
        {
            ref var data = ref DataPtr[index];

            if (Hint.Likely(data.Status is TweenStatus.Scheduled or TweenStatus.Delayed or TweenStatus.Playing))
            {
                var currentTime = data.TimeKind switch
                {
                    TweenTimeKind.Time => Time,
                    TweenTimeKind.UnscaledTime => UnscaledTime,
                    TweenTimeKind.Realtime => Realtime,
                    _ => default
                };

                var tweenTime = currentTime - data.StartTime;

                double t;
                bool isCompleted;
                bool isDelayed;
                int completedLoops;
                int clampedCompletedLoops;

                if (Hint.Unlikely(data.Duration <= 0f))
                {
                    if (data.DelayType == DelayType.FirstLoop || data.Delay == 0f)
                    {
                        var time = tweenTime - data.Delay;
                        isCompleted = data.Loops >= 0 && time > 0f;
                        if (isCompleted)
                        {
                            t = 1f;
                            completedLoops = data.Loops;
                        }
                        else
                        {
                            t = 0f;
                            completedLoops = time < 0f ? -1 : 0;
                        }
                        clampedCompletedLoops = data.Loops < 0 ? math.max(0, completedLoops) : math.clamp(completedLoops, 0, data.Loops);
                        isDelayed = time < 0;
                    }
                    else
                    {
                        completedLoops = (int)math.floor(tweenTime / data.Delay);
                        clampedCompletedLoops = data.Loops < 0 ? math.max(0, completedLoops) : math.clamp(completedLoops, 0, data.Loops);
                        isCompleted = data.Loops >= 0 && clampedCompletedLoops > data.Loops - 1;
                        isDelayed = !isCompleted;
                        t = isCompleted ? 1f : 0f;
                    }
                }
                else
                {
                    if (data.DelayType == DelayType.FirstLoop)
                    {
                        var time = tweenTime - data.Delay;
                        completedLoops = (int)math.floor(time / data.Duration);
                        clampedCompletedLoops = data.Loops < 0 ? math.max(0, completedLoops) : math.clamp(completedLoops, 0, data.Loops);
                        isCompleted = data.Loops >= 0 && clampedCompletedLoops > data.Loops - 1;
                        isDelayed = time < 0f;

                        if (isCompleted)
                        {
                            t = 1f;
                        }
                        else
                        {
                            var currentLoopTime = time - data.Duration * clampedCompletedLoops;
                            t = math.clamp(currentLoopTime / data.Duration, 0f, 1f);
                        }
                    }
                    else
                    {
                        var currentLoopTime = math.fmod(tweenTime, data.Duration + data.Delay) - data.Delay;
                        completedLoops = (int)math.floor(tweenTime / (data.Duration + data.Delay));
                        clampedCompletedLoops = data.Loops < 0 ? math.max(0, completedLoops) : math.clamp(completedLoops, 0, data.Loops);
                        isCompleted = data.Loops >= 0 && clampedCompletedLoops > data.Loops - 1;
                        isDelayed = currentLoopTime < 0;

                        if (isCompleted)
                        {
                            t = 1f;
                        }
                        else
                        {
                            t = math.clamp(currentLoopTime / data.Duration, 0f, 1f);
                        }
                    }
                }
                
                float progress;
                switch (data.LoopType)
                {
                    default:
                    case LoopType.Restart:
                        progress = EaseUtility.Evaluate((float)t, data.Ease);
                        break;
                    case LoopType.Yoyo:
                        if ((clampedCompletedLoops + (int)t) % 2 == 1)
                        {
                            progress = EaseUtility.Evaluate((float)(1.0-t), data.Ease);
                        }
                        else
                        {
                            progress = EaseUtility.Evaluate((float)t, data.Ease);
                        }
                        break;
                    case LoopType.Incremental:
                        progress = EaseUtility.Evaluate(1f, data.Ease) * clampedCompletedLoops + EaseUtility.Evaluate((float)math.fmod(t, 1f), data.Ease);
                        break;
                    case LoopType.Flip:
                        progress = EaseUtility.Evaluate((float)t, data.Ease);
                        if ((clampedCompletedLoops + (int)t) % 2 == 1)
                        {
                            progress = 1f - progress;
                        }
                        break;
                }

                var totalDuration = data.DelayType == DelayType.FirstLoop
                    ? data.Delay + data.Duration * data.Loops
                    : (data.Delay + data.Duration) * data.Loops;

                if (data.Loops > 0 && tweenTime >= totalDuration)
                {
                    data.Status = TweenStatus.Completed;
                }
                else if (isDelayed)
                {
                    data.Status = TweenStatus.Delayed;
                }
                else
                {
                    data.Status = TweenStatus.Playing;
                }

                Output[index] = data.Evaluate(progress);
            }
            else if (data.Status is TweenStatus.Completed or TweenStatus.Canceled)
            {
                data.Dispose();
                CompletedIndexList.AddNoResize(index);
                data.Status = TweenStatus.None;
            }
        }
    }
}