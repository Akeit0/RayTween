using System;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;

namespace RayTween.Internal
{
    /// <summary>
    /// A structure that holds Tween callbacks.
    /// </summary>
    public struct TweenCallbackData
    {
        public byte TargetCount;
        public bool IsCallbackRunning;
        public bool CancelOnError;
        public bool SkipValuesDuringDelay;
        public object Target1;
        public object Target2;

        public object UpdateAction;
        private OnDisposeActionList OnDispose;
        

        public void Append<T>(T target,Action<T,TweenResult> action)where T :class
        {
            if (IsCallbackRunning)
            {
                throw new InvalidOperationException();
            }
            OnDispose.Append(target,action);
        }
        public void Append(Action<TweenResult> action)
        {
            if (IsCallbackRunning)
            {
                throw new InvalidOperationException();
            }
            OnDispose.Append(action);
        }
        public void AppendOnCancel(Action action)
        {
            if (IsCallbackRunning)
            {
                throw new InvalidOperationException();
            }
            OnDispose.AppendOnCancel(action);
        }
        public void AppendOnCancel<T>(T target,Action<T> action)where T :class
        {
            if (IsCallbackRunning)
            {
                throw new InvalidOperationException();
            }
            OnDispose.AppendOnCancel(target,action);
        }
        public void AppendOnComplete(Action action)
        {
            if (IsCallbackRunning)
            {
                throw new InvalidOperationException();
            }
            OnDispose.AppendOnComplete(action);
        }
        public void AppendOnComplete<T>(T target,Action<T> action)where T :class
        {
            if (IsCallbackRunning)
            {
                throw new InvalidOperationException();
            }
            OnDispose.AppendOnComplete(target,action);
        }
        public void RemoveTarget<T>(T target)where T :class
        {
            if (IsCallbackRunning)
            {
                throw new InvalidOperationException();
            }
            OnDispose.RemoveTarget(target);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InvokeUnsafe<TValue>(in TValue value) where TValue : unmanaged
        {
            
            if(UpdateAction==null)return;
            switch (TargetCount)
            {
                case 0:
                    UnsafeUtility.As<object, Action<TValue>>(ref UpdateAction).Invoke(value);
                    break;
                case 1:
                    UnsafeUtility.As<object, Action<object,TValue>>(ref UpdateAction).Invoke(Target1,value );
                    break;
                case 2:
                    UnsafeUtility.As<object, Action< object, object,TValue>>(ref UpdateAction).Invoke( Target1, Target2,value);
                    break;
            }
        }
        
        public void InvokeAndDispose(TweenResult result)
        {
            if (IsCallbackRunning)
            {
                throw new InvalidOperationException();
            }
            IsCallbackRunning = true;
            OnDispose.InvokeAndDispose(result);
            IsCallbackRunning = false;
        }
    }
}