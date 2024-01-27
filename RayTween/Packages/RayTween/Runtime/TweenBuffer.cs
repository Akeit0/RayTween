using RayTween.Internal;
using System;

namespace RayTween
{
    public enum RelativeMode
    {
        None,
        AbsoluteValue,
        RelativeValue,
        AbsoluteScale,
        RelativeScale,
    }

    internal abstract class TweenDataBuffer
    {
        public (int Index, int Version) Handle = new(-1, -1);
        public ITweenScheduler Scheduler;
        public TweenCallbackData CallbackData;
        public bool IsFrom;
        public RelativeMode RelativeMode;
        internal static readonly MinimumList<TweenDataBuffer> BufferList = new MinimumList<TweenDataBuffer>(16);

        public abstract int TypeId { get; }

        public bool HasSameHandle((int, int ) handle)
        {
            return this.Handle == handle;
        }

        public bool HasSameHandle(int index, int version)
        {
            return this.Handle.Index == index && this.Handle.Version == version;
        }

        public bool HasSameHandle(TweenHandle handle)
        {
            return handle.Index == this.Handle.Index && handle.Version == this.Handle.Version &&
                   TypeId == handle.TypeId;
        }
    }

    public readonly struct PreservedTween<TValue, TPlugin> : IDisposable where TValue : unmanaged
        where TPlugin : unmanaged, ITweenPlugin<TValue>
    {
        public readonly TweenHandle<TValue, TPlugin> Handle;
        readonly TweenDataBuffer<TValue, TPlugin> buffer;
        public PreservedTween(TweenHandle<TValue, TPlugin> handle)
        {
            Handle = handle;
            buffer = TweenDataBuffer<TValue, TPlugin>.CreateCopy();
        }
        public bool IsPreserved=> buffer.HasSameHandle(Handle.Index, Handle.Version);
        public void Schedule()
        {
            if (IsPreserved)
            {
                buffer.Schedule();
                buffer.Return();
            }
        }

        public void Dispose()
        {  if (IsPreserved)
            Schedule();
        }
        
        public void SetLoops(int loops)
        {
            if (IsPreserved)
            {
                buffer.SetLoops(loops);
            }
        }
        public void SetLoops(int loops,LoopType loopType)
        {
            if (IsPreserved)
            {
                buffer.SetLoops(loops,loopType);
            }
        }
        public void SetEase(Ease ease)
        {
            if (IsPreserved)
            {
                buffer.SetEase(ease);
            }
        }
        public void SetTimeKind(TweenTimeKind timeKind)
        {
            if (IsPreserved)
            {
                buffer.SetTimeKind(timeKind);
            }
        }
        public void From(TValue start)
        {
            if (IsPreserved)
            {
                buffer.From(start);
            }
        }
        public void From(bool isFrom)
        {
            if (IsPreserved)
            {
                buffer.From(isFrom);
            }
        }
        public void OnComplete(Action action)
        {
            if (IsPreserved)
            {
                buffer.OnComplete(action);
            }
        }
        public void OnComplete<TTarget>(TTarget target, Action<TTarget> action) where TTarget : class
        {
            if (IsPreserved)
            {
                buffer.OnComplete(target,action);
            }
        }
        public void OnCancel(Action action)
        {
            if (IsPreserved)
            {
                buffer.OnCancel(action);
            }
        }
        public void OnCancel<TTarget>(TTarget target, Action<TTarget> action) where TTarget : class
        {
            if (IsPreserved)
            {
                buffer.OnCancel(target,action);
            }
        }
        public void OnDispose(Action<TweenResult> action)
        {
            if (IsPreserved)
            {
                buffer.OnDispose(action);
            }
        }
        public void OnDispose<TTarget>(TTarget target, Action<TTarget, TweenResult> action) where TTarget : class
        {
            if (IsPreserved)
            {
                buffer.OnDispose(target,action);
            }
        }
        
        public void SetScheduler(ITweenScheduler scheduler)
        {
            if (IsPreserved)
            {
                buffer.SetScheduler(scheduler);
            }
        }
        
        public void SetDelay(float delay)
        {
            if (IsPreserved)
            {
                buffer.SetDelay(delay);
            }
        }
        
        public void SetRelative(bool isRelative)
        {
            if (IsPreserved)
            {
                buffer.SetRelative(isRelative);
            }
        }
        
        
    }

    internal sealed class TweenDataBuffer<TValue, TPlugin> : TweenDataBuffer where TValue : unmanaged
        where TPlugin : unmanaged, ITweenPlugin<TValue>
    {
        static int typeId;
        public override int TypeId => typeId;
        public TweenData<TValue, TPlugin> TweenData;

        TweenDataBuffer<TValue, TPlugin> next;

        public static TweenDataBuffer<TValue, TPlugin> CreateCopy()
        {
            var result = Instance.next;
            if (result != null)
            {
                Instance.next = result.next;
            }
            else
            {
                result = new TweenDataBuffer<TValue, TPlugin>();
            }
            result.Scheduler = Instance.Scheduler;
            result.TweenData = Instance.TweenData;
            result.CallbackData = Instance.CallbackData;
            result.RelativeMode = Instance.RelativeMode;
            result.Handle = Instance.Handle;
            Instance.Init();
            return result;
        }

        public void Return()
        {
            Init();
            Instance.next = this;
        }

        public void Init()
        {
            TweenData = default;
            CallbackData = default;
            RelativeMode = default;
            Handle = (-1, -1);
        }

        public static readonly TweenDataBuffer<TValue, TPlugin> Instance = new TweenDataBuffer<TValue, TPlugin>();

        static TweenDataBuffer()
        {
            typeId = BufferList.Length;
            BufferList.Add(Instance);
        }

        public void InitFromTo(TValue start, TValue end, float duration,
            RelativeMode relativeMode)
        {
            RelativeMode = relativeMode;
            Scheduler = TweenScheduler.Update;
            ref var tweenData = ref TweenData;
            if (TweenData<TValue, TPlugin>.HasDisposeImplementation)
            {
                TweenData.Dispose();
            }

            tweenData.StartValue = start;
            tweenData.EndValue = end;
            tweenData.Duration = duration;
            tweenData.Ease = Ease.Linear;
            tweenData.TimeKind = TweenTimeKind.Time;
            tweenData.Loops = 1;
            tweenData.LoopType = LoopType.Restart;
            tweenData.Plugin.Init();
            CallbackData = default;
            IsFrom = false;
            CallbackData.TargetCount = 0;
        }

        public void InitTo<TTarget>(TTarget target, object setter, TValue end, float duration,
            RelativeMode relativeMode) where TTarget : class
        {
            InitFromTo(default(TValue), end, duration, relativeMode);
            CallbackData.TargetCount = 1;
            CallbackData.Target1 = target;
            CallbackData.UpdateAction = setter;
        }

        public void InitTo(object target1, object target2, object setter, TValue start, TValue end, float duration,
            RelativeMode relativeMode)
        {
            InitFromTo(start, end, duration, relativeMode);
            CallbackData.TargetCount = 2;
            CallbackData.Target1 = target1;
            CallbackData.Target2 = target2;
            CallbackData.UpdateAction = setter;
        }

        public void Set((int, int ) handle)
        {
            this.Handle = handle;
        }


        public void Schedule()
        {
            if (0 <= Handle.Index)
            {
                var handle = new TweenHandle<TValue, TPlugin>(Handle.Index, Handle.Version);
                if (TweenTracker.EnableTracking)
                {
                    TweenTracker.AddTracking(this, Scheduler);
                }

                if (IsFrom)
                {
                    (TweenData.StartValue, TweenData.EndValue) = (TweenData.EndValue, TweenData.StartValue);
                }

                var relativeModeApplier = TweenData<TValue, TPlugin>.RelativeModeApplier;
                if (relativeModeApplier != null)
                    TweenData.EndValue = relativeModeApplier(RelativeMode, TweenData.StartValue, TweenData.EndValue);
                TweenData.Status = TweenStatus.Scheduled;
                Scheduler.Schedule(handle, ref TweenData, ref CallbackData);
                Scheduler = null;
                TweenData = default;
                CallbackData = default;
                Handle.Index = -1;
            }
        }
        public void ScheduleIfMatchTiming(UpdateTiming timing)
        {
            if (Scheduler == null) return;
            if (Scheduler.UpdateTiming != timing) return;
            Schedule();
        }

        public void SetLoops(int loops)
        {
            TweenData.Loops = loops;
        }
        public void SetLoops(int loops,LoopType loopType)
        {
            TweenData.Loops = loops;
            TweenData.LoopType = loopType;
        }
        public void SetEase(Ease ease)
        {
            TweenData.Ease = ease;
        }
        public void SetTimeKind(TweenTimeKind timeKind)
        {
            TweenData.TimeKind = timeKind;
        }
        public void From(TValue start)
        {
            TweenData.StartValue = start;
        }
        public void From(bool isFrom)
        {
            IsFrom = isFrom;
        }
        public void OnComplete(Action action)
        {
            CallbackData.AppendOnComplete(action);
        }
        public void OnComplete<TTarget>(TTarget target, Action<TTarget> action) where TTarget : class
        {
            CallbackData.AppendOnComplete(target, action);
        }
        public void OnCancel(Action action)
        {
            CallbackData.AppendOnCancel(action);
        }
        public void OnCancel<TTarget>(TTarget target, Action<TTarget> action) where TTarget : class
        {
            CallbackData.AppendOnCancel(target, action);
        }
        public void OnDispose(Action<TweenResult> action)
        {
            CallbackData.Append(action);
        }
        public void OnDispose<TTarget>(TTarget target, Action<TTarget, TweenResult> action) where TTarget : class
        {
            CallbackData.Append(target, action);
        }
        
        public void SetScheduler(ITweenScheduler scheduler)
        {
            Scheduler = scheduler;
        }
        
        public void SetDelay(float delay)
        {
            TweenData.Delay = delay;
        }

        
        public void SetRelative(bool isRelative)
        {
            switch (RelativeMode, isRelative)
            {
                case (RelativeMode.AbsoluteValue, true):
                    RelativeMode = RelativeMode.RelativeValue;
                    break;
                case (RelativeMode.RelativeValue, false):
                    RelativeMode = RelativeMode.AbsoluteValue;
                    break;
                case (RelativeMode.AbsoluteScale, true):
                    RelativeMode = RelativeMode.RelativeScale;
                    break;
                case (RelativeMode.RelativeScale, true):
                    RelativeMode = RelativeMode.AbsoluteScale;
                    break;
            }
        }

        ~TweenDataBuffer()
        {
            if (TweenData<TValue, TPlugin>.HasDisposeImplementation)
            {
                TweenData.Dispose();
            }
        }
    }

}