using System;
using RayTween.Internal;
using UnityTime = UnityEngine.Time;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RayTween
{
    internal sealed class PlayerLoopTweenScheduler : ITweenScheduler
    {
        internal   readonly UpdateTiming updateTiming;
         internal  readonly TweenTimeKind timeKind;

        internal PlayerLoopTweenScheduler(UpdateTiming updateTiming, TweenTimeKind timeKind)
        {
            this.updateTiming = updateTiming;
            this.timeKind = timeKind;
            this.Id= TweenScheduler.Add(this);
        }

        public double Time
        {
            get
            {
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying)
                {
                  return EditorApplication.timeSinceStartup;
                }
#endif
                if (updateTiming == UpdateTiming.FixedUpdate)
                {
                    return timeKind switch
                    {
                        TweenTimeKind.Time => UnityTime.fixedTimeAsDouble,
                        TweenTimeKind.UnscaledTime => UnityTime.fixedUnscaledTimeAsDouble,
                        TweenTimeKind.Realtime => UnityTime.realtimeSinceStartupAsDouble,
                        _ => throw new NotSupportedException("Invalid TimeKind")
                    };
                }

                return timeKind switch
                {
                    TweenTimeKind.Time => UnityTime.timeAsDouble,
                    TweenTimeKind.UnscaledTime => UnityTime.unscaledTimeAsDouble,
                    TweenTimeKind.Realtime => UnityTime.realtimeSinceStartupAsDouble,
                    _ => throw new NotSupportedException("Invalid TimeKind")
                };
            }
        }
        
        public int Id { get; }
        
        public UpdateTiming UpdateTiming => updateTiming;

        public int Schedule<TValue, TPlugin>(TweenHandle<TValue,TPlugin> handle,ref TweenData<TValue, TPlugin> data, ref TweenCallbackData callbackData)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            
            data.TimeKind = timeKind;
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                return TweenDispatcher.Schedule(handle,data, callbackData, updateTiming);
            }
            else
            {
                return EditorTweenDispatcher.Schedule(handle,data, callbackData);
            }
#else
            return TweenDispatcher.Schedule(data, callbackData, updateTiming);
#endif
        }
    }
}