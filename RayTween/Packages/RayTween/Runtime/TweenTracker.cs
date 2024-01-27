using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RayTween
{
    /// <summary>
    /// Provides functionality for tracking active tweens.
    /// </summary>
    public static class TweenTracker
    {
        public static bool EnableTracking = false;
        public static bool EnableStackTrace = false;

        public static IReadOnlyList<TrackingState> Items => trackings;
        static readonly List<TrackingState> trackings = new(16);

        internal static void AddTracking<TValue, TPlugin>(TweenDataBuffer<TValue, TPlugin> buffer, ITweenScheduler scheduler)where TValue : unmanaged
         
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            var state = TrackingState.Create();
            state.ValueType = typeof(TValue);
            state.PluginType = typeof(TPlugin);
            
            state.Scheduler = scheduler;
            state.CreationTime = DateTime.UtcNow;

            if (EnableStackTrace) state.StackTrace = buffer.StackTrace;
            buffer.CallbackData.Append(state,static (target,result)=>target.OnTweenDispose(result));
            trackings.Add(state);
        }

        public static void Clear()
        {
            trackings.Clear();
        }

        public sealed class TrackingState
        {
            static readonly Stack<TrackingState> pool = new(16);


            public static TrackingState Create()
            {
                if (!pool.TryPop(out var state))
                {
                    state = new();
                }
                return state;
            }

            public Type ValueType;
            public Type PluginType;
            public ITweenScheduler Scheduler;
            public DateTime CreationTime;
            public StackTrace StackTrace;
            public void OnTweenDispose(TweenResult _)
            {
                Release();
            }
            void Release()
            {
                trackings.Remove(this);
                ValueType = default;
                PluginType = default;
                Scheduler = default;
                CreationTime = default;
                StackTrace = default;
                pool.Push(this);
            }
        }
    }
}