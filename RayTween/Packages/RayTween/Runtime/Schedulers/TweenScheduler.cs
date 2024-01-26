using RayTween.Internal;
namespace RayTween
{
    /// <summary>
    /// Schedulers available in Runtime.
    /// </summary>
    public static class TweenScheduler
    {
        
        
        internal static MinimumList<ITweenScheduler> Schedulers = new MinimumList<ITweenScheduler>(23);

        internal static int Add(ITweenScheduler scheduler)
        {
            Schedulers.Add(scheduler);
            return Schedulers.Length - 1;
        }
        
        // internal static TweenHandle Schedule<TValue,TOptions,TPlugin>(TweenHandle<TValue,TOptions,TPlugin> scheduler)where TOptions :  ITweenOptions
        // {
        //    
        // }
        
        
        /// <summary>
        /// Scheduler that updates tween at Initialization.
        /// </summary>
        public static readonly ITweenScheduler Initialization = new PlayerLoopTweenScheduler(UpdateTiming.Initialization, TweenTimeKind.Time);
        /// <summary>
        /// Scheduler that updates tween at Initialization. (Ignore timescale)
        /// </summary>
        public static readonly ITweenScheduler InitializationIgnoreTimeScale = new PlayerLoopTweenScheduler(UpdateTiming.Initialization, TweenTimeKind.UnscaledTime);
        /// <summary>
        /// Scheduler that updates tween at Initialization. (Realtime)
        /// </summary>
        public static readonly ITweenScheduler InitializationRealtime = new PlayerLoopTweenScheduler(UpdateTiming.Initialization, TweenTimeKind.Realtime);

        /// <summary>
        /// Scheduler that updates tween at EarlyUpdate.
        /// </summary>
        public static readonly ITweenScheduler EarlyUpdate = new PlayerLoopTweenScheduler(UpdateTiming.EarlyUpdate, TweenTimeKind.Time);
        /// <summary>
        /// Scheduler that updates tween at EarlyUpdate. (Ignore timescale)
        /// </summary>
        public static readonly ITweenScheduler EarlyUpdateIgnoreTimeScale = new PlayerLoopTweenScheduler(UpdateTiming.EarlyUpdate, TweenTimeKind.UnscaledTime);
        /// <summary>
        /// Scheduler that updates tween at EarlyUpdate. (Realtime)
        /// </summary>
        public static readonly ITweenScheduler EarlyUpdateRealtime = new PlayerLoopTweenScheduler(UpdateTiming.EarlyUpdate, TweenTimeKind.Realtime);

        /// <summary>
        /// Scheduler that updates tween at FixedUpdate.
        /// </summary>
        public static readonly ITweenScheduler FixedUpdate = new PlayerLoopTweenScheduler(UpdateTiming.FixedUpdate, TweenTimeKind.Time);

        /// <summary>
        /// Scheduler that updates tween at PreUpdate.
        /// </summary>
        public static readonly ITweenScheduler PreUpdate = new PlayerLoopTweenScheduler(UpdateTiming.PreUpdate, TweenTimeKind.Time);
        /// <summary>
        /// Scheduler that updates tween at PreUpdate. (Ignore timescale)
        /// </summary>
        public static readonly ITweenScheduler PreUpdateIgnoreTimeScale = new PlayerLoopTweenScheduler(UpdateTiming.PreUpdate, TweenTimeKind.UnscaledTime);
        /// <summary>
        /// Scheduler that updates tween at PreUpdate. (Realtime)
        /// </summary>
        public static readonly ITweenScheduler PreUpdateRealtime = new PlayerLoopTweenScheduler(UpdateTiming.PreUpdate, TweenTimeKind.Realtime);

        /// <summary>
        /// Scheduler that updates tween at Update.
        /// </summary>
        public static readonly ITweenScheduler Update = new PlayerLoopTweenScheduler(UpdateTiming.Update, TweenTimeKind.Time);
        /// <summary>
        /// Scheduler that updates tween at Update. (Ignore timescale)
        /// </summary>
        public static readonly ITweenScheduler UpdateIgnoreTimeScale = new PlayerLoopTweenScheduler(UpdateTiming.Update, TweenTimeKind.UnscaledTime);
        /// <summary>
        /// Scheduler that updates tween at Update. (Realtime)
        /// </summary>
        public static readonly ITweenScheduler UpdateRealtime = new PlayerLoopTweenScheduler(UpdateTiming.Update, TweenTimeKind.Realtime);

        /// <summary>
        /// Scheduler that updates tween at PreLateUpdate.
        /// </summary>
        public static readonly ITweenScheduler PreLateUpdate = new PlayerLoopTweenScheduler(UpdateTiming.PreLateUpdate, TweenTimeKind.Time);
        /// <summary>
        /// Scheduler that updates tween at PreLateUpdate. (Ignore timescale)
        /// </summary>
        public static readonly ITweenScheduler PreLateUpdateIgnoreTimeScale = new PlayerLoopTweenScheduler(UpdateTiming.PreLateUpdate, TweenTimeKind.UnscaledTime);
        /// <summary>
        /// Scheduler that updates tween at PreLateUpdate. (Realtime)
        /// </summary>
        public static readonly ITweenScheduler PreLateUpdateRealtime = new PlayerLoopTweenScheduler(UpdateTiming.PreLateUpdate, TweenTimeKind.Realtime);

        /// <summary>
        /// Scheduler that updates tween at PostLateUpdate.
        /// </summary>
        public static readonly ITweenScheduler PostLateUpdate = new PlayerLoopTweenScheduler(UpdateTiming.PostLateUpdate, TweenTimeKind.Time);
        /// <summary>
        /// Scheduler that updates tween at PostLateUpdate. (Ignore timescale)
        /// </summary>
        public static readonly ITweenScheduler PostLateUpdateIgnoreTimeScale = new PlayerLoopTweenScheduler(UpdateTiming.PostLateUpdate, TweenTimeKind.UnscaledTime);
        /// <summary>
        /// Scheduler that updates tween at PostLateUpdate. (Realtime)
        /// </summary>
        public static readonly ITweenScheduler PostLateUpdateRealtime = new PlayerLoopTweenScheduler(UpdateTiming.PostLateUpdate, TweenTimeKind.Realtime);

        /// <summary>
        /// Scheduler that updates tween at TimeUpdate.
        /// </summary>
        public static readonly ITweenScheduler TimeUpdate = new PlayerLoopTweenScheduler(UpdateTiming.TimeUpdate, TweenTimeKind.Time);
        /// <summary>
        /// Scheduler that updates tween at TimeUpdate. (Ignore timescale)
        /// </summary>
        public static readonly ITweenScheduler TimeUpdateIgnoreTimeScale = new PlayerLoopTweenScheduler(UpdateTiming.TimeUpdate, TweenTimeKind.UnscaledTime);
        /// <summary>
        /// Scheduler that updates tween at TimeUpdate. (Realtime)
        /// </summary>
        public static readonly ITweenScheduler TimeUpdateRealtime = new PlayerLoopTweenScheduler(UpdateTiming.TimeUpdate, TweenTimeKind.Realtime);

        /// <summary>
        /// Scheduler that updates tween with `ManualTweenDispatcher.Update()`
        /// </summary>
        public static readonly ITweenScheduler Manual = new ManualTweenScheduler();
    }
}