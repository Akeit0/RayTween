using System;
using RayTween.Plugins;
using RayTween.Internal;
#if UNITY_EDITOR
#endif

namespace RayTween
{
    
    public struct TweenFromTo<TValue,TPlugin> 
    {
        public TValue From;
        public TValue To;
        public float Duration;
        public TweenFromTo(TValue from, TValue to, float duration)
        {
            From = from;
            To = to;
            Duration = duration;
        }
        public TweenFromTo(TValue from, TweenTo<TValue,TPlugin> to)
        {
            From = from;
            To = to.To;
            Duration = to.Duration;
        }
    }
    // ReSharper disable once UnusedTypeParameter
    public struct TweenTo<TValue,TPlugin> 
    {
        public TValue To;
        public float Duration;
        public TweenTo( TValue to, float duration)
        {
            To = to;
            Duration = duration;
        }
    }
    public static class FromToExtensions
    {
        public static TweenHandle<UnsafeString, StringTweenPlugin> Bind (in this TweenFromTo<string, StringTweenPlugin> tweenFromTo,Action<UnsafeString> action)
        {
            return TweenHandle<UnsafeString, StringTweenPlugin>.Create(action,new UnsafeString(tweenFromTo.From), new UnsafeString(tweenFromTo.To), tweenFromTo.Duration);
        }
        public static TweenHandle<UnsafeString, StringTweenPlugin> Bind<TTarget> (in this TweenFromTo<string, StringTweenPlugin> tweenFromTo,TTarget target,Action<TTarget,UnsafeString> action)where TTarget : class
        {
            return TweenHandle<UnsafeString, StringTweenPlugin>.Create(target,action,new UnsafeString(tweenFromTo.From), new UnsafeString(tweenFromTo.To), tweenFromTo.Duration);
        }
        
        public static TweenHandle<TValue, TPlugin> Bind<TValue,TPlugin>(in this TweenFromTo<TValue, TPlugin> tweenFromTo,Action<TValue> action,RelativeMode relativeMode=RelativeMode.AbsoluteValue)where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            return TweenHandle<TValue, TPlugin>.Create(action,tweenFromTo.From, tweenFromTo.To, tweenFromTo.Duration,relativeMode);
        }
        
        public static TweenHandle<TValue, TPlugin> Bind<TValue,TPlugin, TTarget>(in this TweenFromTo<TValue, TPlugin> tweenFromTo,TTarget target, Action<TTarget, TValue> action,RelativeMode relativeMode=RelativeMode.AbsoluteValue)where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue> where TTarget : class
        {
            return TweenHandle<TValue, TPlugin>.Create(target,action,tweenFromTo.From, tweenFromTo.To, tweenFromTo.Duration,relativeMode);
        }
        public static TweenHandle<TValue, TPlugin> Bind<TValue,TPlugin, TTarget1,TTarget2>(in this TweenFromTo<TValue, TPlugin> tweenFromTo,TTarget1 target1,TTarget2 target2 ,Action<TTarget1,TTarget2, TValue> action,RelativeMode relativeMode=RelativeMode.AbsoluteValue)where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue> where TTarget1 : class where TTarget2 : class
        {
            return TweenHandle<TValue, TPlugin>.Create(target1,target2,action,tweenFromTo.From, tweenFromTo.To, tweenFromTo.Duration,relativeMode);
        }
        
        
        public static TweenHandle<TValue, TPlugin> NoBind<TValue,TPlugin>(in this TweenFromTo<TValue, TPlugin> tweenFromTo)where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue> 
        {
            return TweenHandle<TValue, TPlugin>.Create(tweenFromTo.From, tweenFromTo.To, tweenFromTo.Duration);
        }
        
        
    }
    public struct TweenHandle
    {
        
        /// <summary>
        /// The ID of Type.
        /// </summary>
        public int TypeId;
        /// <summary>
        /// The ID of tween entity.
        /// </summary>
        public int Index;

        /// <summary>
        /// The shared version of tween entity.
        /// </summary>
        public int Version;

        public readonly bool Equals(TweenHandle other)
        {
            return Index == other.Index && Version == other.Version;
        }

        public override readonly bool Equals(object obj)
        {
            if (obj is TweenHandle handle) return Equals(handle);
            return false;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Index, Version);
        }

        public static bool operator ==(TweenHandle a, TweenHandle b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(TweenHandle a, TweenHandle b)
        {
            return !(a == b);
        }

        public void ValidCheckWithThrow(out int storageId, out int denseId)
        {
            TweenStorageManager.ValidCheckWithThrow(this, out storageId, out denseId);
        }

        public bool ValidCheck(out int denseId)
        {
            return TweenStorageManager.ValidCheck(this, out denseId);
        }
        internal bool TryGetBuffer(out TweenDataBuffer buffer)
            {
             buffer = TweenDataBuffer.BufferList[TypeId];
            if (buffer.HasSameHandle(Index, Version))
            {
                buffer = TweenDataBuffer.BufferList[TypeId];
                return true;
            }
            return false;
}
        public bool IsIdling => TweenDataBuffer.BufferList[TypeId].HasSameHandle(Index, Version);
        public bool IsActive() => TweenStorageManager.IsActive(this);

        public void Complete() => TweenStorageManager.CompleteTween(this);

        public void Cancel() => TweenStorageManager.CancelTween(this);
    }


    public struct TweenHandle<TValue, TPlugin> where TValue : unmanaged
        where TPlugin : unmanaged, ITweenPlugin<TValue>
    {
     
        internal static readonly TweenDataBuffer<TValue, TPlugin> Buffer = TweenDataBuffer<TValue, TPlugin>.Instance;
         private static  TweenHandle<TValue, TPlugin> BufferedHandle=>new TweenHandle<TValue, TPlugin>(Buffer.Handle.Index,Buffer.Handle.Version);
         static TweenHandle()
        {
            TweenDispatcher.OnUpdateAction += Buffer.ScheduleIfMatchTiming;
        }

         public static int TypeId => Buffer.TypeId;
       

        static void ScheduleBuffer()
        {
            Buffer.Schedule();
        }

        public bool IsIdling => this == BufferedHandle;

        /// <summary>
        /// The ID of tween entity.
        /// </summary>
        public int Index;

        /// <summary>
        /// The shared version of tween entity.
        /// </summary>
        public int Version;
        
        internal TweenHandle(int index, int version)
        {
            Index = index;
            Version = version;
        }

        internal ref TweenData<TValue, TPlugin> Data
        {
            get
            {
                if (IsIdling) return ref Buffer.TweenData;
                return ref TweenStorageManager.GetTweenData(this);
            }
        }
        internal ref TweenCallbackData CallbackData
        {
            get
            {
                if (IsIdling) return ref Buffer.CallbackData;
                return ref TweenStorageManager.GetTweenCallbacks(this.AsNoType());
            }
        }

        public static TweenHandle<TValue, TPlugin> Create(TValue start, TValue end, float duration,RelativeMode relativeMode = RelativeMode.AbsoluteValue)
        {
            ScheduleBuffer();
            var (entryIndex, version) = TweenStorageManager.Alloc();
            Buffer.InitFromTo(start, end, duration,relativeMode);
            Buffer.Set( (entryIndex, version) );
            return  new TweenHandle<TValue, TPlugin>()
                { Index = entryIndex, Version = version };
        }
        public static TweenHandle<TValue, TPlugin> Create(Action<TValue> action,TValue start, TValue end, float duration,RelativeMode relativeMode = RelativeMode.AbsoluteValue)
        {
            ScheduleBuffer();
            var (entryIndex, version) = TweenStorageManager.Alloc();
            Buffer.InitFromTo(start, end, duration,relativeMode);
            Buffer.Set( (entryIndex, version) );
            Buffer.CallbackData.UpdateAction = action;
            return  new TweenHandle<TValue, TPlugin>()
                { Index = entryIndex, Version = version };
        }
        public static TweenHandle<TValue, TPlugin> Create<TTarget>(TTarget target,Action<TTarget,TValue> action,TValue start, TValue end, float duration,RelativeMode relativeMode = RelativeMode.AbsoluteValue)where TTarget : class
        {
            ScheduleBuffer();
            var (entryIndex, version) = TweenStorageManager.Alloc();
            Buffer.InitFromTo(start, end, duration,relativeMode);
            Buffer.CallbackData.TargetCount = 1;
            Buffer.CallbackData.Target1 = target;
            Buffer.CallbackData.UpdateAction = action;
            Buffer.Set( (entryIndex, version) );
            return  new TweenHandle<TValue, TPlugin>()
                { Index = entryIndex, Version = version };
        }
        public static TweenHandle<TValue, TPlugin> Create<TTarget1,TTarget2>(TTarget1 target1,TTarget2 target2,Action<TTarget1,TTarget2,TValue> action,TValue start, TValue end, float duration,RelativeMode relativeMode = RelativeMode.AbsoluteValue)where TTarget1 : class where TTarget2 : class
        {
            ScheduleBuffer();
            var (entryIndex, version) = TweenStorageManager.Alloc();
            Buffer.InitFromTo(start, end, duration,relativeMode);
            Buffer.CallbackData.TargetCount = 2;
            Buffer.CallbackData.Target1 = target1;
            Buffer.CallbackData.Target2 = target2;
            Buffer.CallbackData.UpdateAction = action;
            
            Buffer.Set( (entryIndex, version) );
            return  new TweenHandle<TValue, TPlugin>()
                { Index = entryIndex, Version = version };
        }
        
       
        public readonly bool Equals(TweenHandle<TValue, TPlugin> other)
        {
            return Index == other.Index && Version == other.Version;
        }

        public readonly override bool Equals(object obj)
        {
            if (obj is TweenHandle handle) return Equals(handle);
            return false;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(Index, Version);
        }

        public static bool operator ==(TweenHandle<TValue, TPlugin> a,
            TweenHandle<TValue, TPlugin> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(TweenHandle<TValue, TPlugin> a,
            TweenHandle<TValue, TPlugin> b)
        {
            return !(a == b);
        }

        public TweenHandle<TValue, TPlugin> From(TValue from)
        {
            if (IsIdling)
            {
                Data.StartValue = from;
            }
          
            return this;
        }

        public TweenHandle<TValue, TPlugin> SetLoops(int loopCount)
        { if(IsIdling)
                Buffer.SetLoops(loopCount);
            return this;
        }

        public TweenHandle<TValue, TPlugin> SetLoops(int loopCount, LoopType loopType)
        {
            if(IsIdling)
            {
                Buffer.SetLoops(loopCount, loopType);
            }
            

            return this;
        }

        public TweenHandle<TValue, TPlugin> SetEase(Ease ease)
        { if(IsIdling)
                Buffer.SetEase(ease);
            return this;
        }

        public TweenHandle<TValue, TPlugin> SetDelay(float delay)
        {
            if(IsIdling)
                Buffer.SetDelay(delay);
            return this;
        }

       

        public TweenHandle<TValue, TPlugin> SetTimeKind(TweenTimeKind timeKind)
        {
            if(IsIdling)
                Buffer.SetTimeKind(timeKind);
            

            return this;
        }

        public TweenHandle<TValue, TPlugin> OnDispose(Action<TweenResult> action)
        { if(IsIdling)
                Buffer.OnDispose(action);
            return this;
        }

        public TweenHandle<TValue, TPlugin> OnDispose<TTarget>(TTarget target,
            Action<TTarget, TweenResult> action) where TTarget : class
        { if(IsIdling)
                Buffer.OnDispose(target, action);
            return this;
        }

        public TweenHandle<TValue, TPlugin> OnCancel(Action action)
        { if(IsIdling)
                Buffer.OnCancel(action);
            return this;
        }

        public TweenHandle<TValue, TPlugin> OnCancel<TTarget>(TTarget target, Action<TTarget> action)
            where TTarget : class
        { if(IsIdling)
                Buffer.OnCancel(target,action);
            return this;
        }

        public TweenHandle<TValue, TPlugin> OnComplete(Action action)
        { if(IsIdling)
                Buffer.OnComplete(action);

            return this;
        }

        public TweenHandle<TValue, TPlugin> OnComplete<TTarget>(TTarget target, Action<TTarget> action)
            where TTarget : class
        { if(IsIdling)
                Buffer.OnComplete(target,action);
            return this;
        }

        public TweenHandle<TValue, TPlugin> SetScheduler(ITweenScheduler scheduler)
        {
            if (IsIdling) 
                Buffer.SetScheduler(scheduler);
            return this;
        }

        public TweenHandle<TValue, TPlugin> Schedule()
        {
            if (IsIdling) ScheduleBuffer();
            return this;
        }

        public TweenHandle AsNoType()
        {
            return new TweenHandle() { TypeId = TypeId,Index = Index, Version = Version };
        }

        public void Forget()
        {
        }

        public bool IsActive()
        {
            if (IsIdling) return true;
            return TweenStorageManager.IsActive(AsNoType());
        }

        public void Complete()
        {
            if (IsIdling)
            {
                Buffer.CallbackData.InvokeAndDispose(TweenResult.Completed);
                TweenStorageManager.Free(AsNoType());
            }

            TweenStorageManager.CompleteTween(AsNoType());
        }

        public void Cancel()
        {
            if (IsIdling)
            {
                Buffer.CallbackData.InvokeAndDispose(TweenResult.Canceled);
                TweenStorageManager.Free(AsNoType());
            }

            TweenStorageManager.CompleteTween(AsNoType());
        }
        
        public PreservedTween<TValue,TPlugin> Preserve()
        {
            return new PreservedTween<TValue, TPlugin>(this);
        }
        // public  TweenHandle<TValue,TOptionsTarget,TPluginTarget> Convert<TOptionsTarget,TPluginTarget>()where TOptionsTarget : unmanaged, ITweenOptions
        //     where TPluginTarget : unmanaged, ITweenPlugin<TValue, TOptionsTarget>
        // {
        //     if (IsLeft)
        //     {
        //         throw new InvalidOperationException();
        //     }
        //     
        //     Buffer.Handle.Index = -1;
        //     TweenHandle<TValue, TOptionsTarget, TPluginTarget>.ScheduleBuffer();
        //     var handle= new TweenHandle<TValue, TOptionsTarget, TPluginTarget>()
        //         { Index = Index, Version = Version };
        //     TweenHandle<TValue, TOptionsTarget, TPluginTarget>.Buffer.Handle = (Index, Version);
        //     var targetBuffer = TweenHandle<TValue, TOptionsTarget, TPluginTarget>.Buffer;
        //     targetBuffer.CallbackData= Buffer.CallbackData;
        //     targetBuffer.Scheduler = Buffer.Scheduler;
        //     targetBuffer.TweenData.StartValue = Buffer.TweenData.StartValue ;
        //     targetBuffer.TweenData.EndValue = Buffer.TweenData.EndValue ;
        //     targetBuffer.TweenData.Duration = Buffer.TweenData.Duration ;
        //     targetBuffer.TweenData.Ease = Buffer.TweenData.Ease ;
        //     targetBuffer.TweenData.TimeKind = Buffer.TweenData.TimeKind ;
        //     targetBuffer.TweenData.Delay = Buffer.TweenData.Delay ;
        //     targetBuffer.TweenData.Loops = Buffer.TweenData.Loops ;
        //     targetBuffer.TweenData.DelayType = Buffer.TweenData.DelayType ;
        //     targetBuffer.TweenData.LoopType = Buffer.TweenData.LoopType ;
        //     targetBuffer.TweenData.Options = default ;
        //     return handle;
        //
        //
        // }
    }
}