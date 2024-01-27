using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

// TODO: Constantize the exception message

namespace RayTween.Internal
{
   

    internal unsafe interface ITweenStorage
    {
        bool IsActive(int denseIndex);
        void Cancel(int denseIndex);
        void Complete(int denseIndex);
       ref TweenCallbackData GetTweenCallbacks(int denseIndex);
        void SetTweenCallbacks(int denseIndex, TweenCallbackData callbacks);
        void Reset();
    }

    

    internal sealed class TweenStorage<TValue, TPlugin>:ITweenStorage where TValue : unmanaged
        where TPlugin : unmanaged, ITweenPlugin<TValue>
    {
        public TweenStorage(int id) => StorageId = id;


        // Data
        public int?[] toEntryIndex = new int?[InitialCapacity];
        public TweenData<TValue, TPlugin>[] dataArray = new TweenData<TValue, TPlugin>[InitialCapacity];
        public TweenCallbackData[] callbacksArray = new TweenCallbackData[InitialCapacity];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<TweenData<TValue, TPlugin>> GetDataSpan() => dataArray.AsSpan(0, tail);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<TweenCallbackData> GetCallbacksSpan() => callbacksArray.AsSpan(0, tail);

        int tail;

        const int InitialCapacity = 8;

        public int StorageId { get; }
        public int Count => tail;

        public void  Append(TweenHandle<TValue,TPlugin> handle,in TweenData<TValue, TPlugin> data, in TweenCallbackData callbacks)
        {
            
            if (tail == dataArray.Length)
            {
                var newLength = tail * 2;
                Array.Resize(ref toEntryIndex, newLength);
                Array.Resize(ref dataArray, newLength);
                Array.Resize(ref callbacksArray, newLength);
            }
            

#if RAYTWEEN_ENABLE_TWEEN_LOG
            UnityEngine.Debug.Log("[Add] Entry:" + handle.Index + " DenseIndex:" + tail + " Version:" + handle.Version+" EndValue:"+data.EndValue );
#endif
            TweenStorageManager.SetData(handle.Index,handle.Version, (StorageId,tail));
            toEntryIndex[tail] = handle.Index;
            dataArray[tail] = data;
            callbacksArray[tail] = callbacks;
            tail++;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void RemoveAt(int denseIndex)
        {
            tail--;
            ref var data = ref dataArray[denseIndex];
            // if(hasToDisposeTPlugin)
            //     data.Dispose();
            // swap elements
            data = dataArray[tail];
            // dataArray[tail] = default;
            callbacksArray[denseIndex] = callbacksArray[tail];
            // callbacksArray[tail] = default;

            // swap entry indexes
            var prevEntryIndex = toEntryIndex[denseIndex];
            var currentEntryIndex = toEntryIndex[denseIndex] = toEntryIndex[tail];
            // toEntryIndex[tail] = default;

            // update entry
            if (currentEntryIndex != null)
            {
                var index = (int)currentEntryIndex;
                TweenStorageManager.SetDenseIndex(index,denseIndex);
            }

            // free entry
            if (prevEntryIndex != null)
            {
                TweenStorageManager.Free((int)prevEntryIndex);
            }

#if RAYTWEEN_ENABLE_TWEEN_LOG
            var v = entries[(int)prevEntryIndex].Version - 1;
            UnityEngine.Debug.Log("[Remove] Entry:" + prevEntryIndex + " DenseIndex:" + denseIndex + " Version:" + v);
#endif
        }

        public void RemoveAll(NativeList<int> indexes)
        {
            
            var entryIndexes = new NativeArray<int>(indexes.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            var lastCallbacksSpan = GetCallbacksSpan();
            for (int i = 0; i < entryIndexes.Length; i++)
            {
                entryIndexes[i] = (int)toEntryIndex[indexes[i]];
            }

            var storageId = StorageId;
            var entries = TweenStorageManager.Entries;
            var entryIndicesSpan=entryIndexes.AsSpan();
            for (int i = 0; i < entryIndicesSpan.Length; i++)
            {
                var entry = entries[entryIndicesSpan[i]];
                if(entry.StorageId==storageId)
                    RemoveAt(entry.DenseIndex);
                else Debug.LogWarning($"{entryIndicesSpan[i]} {entry.DenseIndex} { entry.Version} {storageId}!={entry.StorageId}");
            }

            // Avoid Memory leak
            lastCallbacksSpan[tail..].Clear();
            entryIndexes.Dispose();
        }

        public void EnsureCapacity(int capacity)
        {
            if (capacity > dataArray.Length)
            {
                Array.Resize(ref toEntryIndex, capacity);
                Array.Resize(ref dataArray, capacity);
                Array.Resize(ref callbacksArray, capacity);
                TweenStorageManager.EnsureCapacity(capacity);
            }
        }

        public void Cancel(int denseIndex)
        {
            if (denseIndex < 0 || denseIndex >= dataArray.Length)
            {
                throw new ArgumentException("Tween has been destroyed or no longer exists.");
            }

            ref var tween = ref GetDataSpan()[denseIndex];
            if (tween.Status == TweenStatus.None)
            {
                throw new ArgumentException("Tween has been destroyed or no longer exists.");
            }

            tween.Status = TweenStatus.Canceled;

            ref var callbackData = ref GetCallbacksSpan()[denseIndex];
            callbackData.InvokeAndDispose(TweenResult.Canceled);
        }

        public void Complete(int denseIndex)
        {
            if (denseIndex < 0 || denseIndex >= tail)
            {
                throw new ArgumentException("Tween has been destroyed or no longer exists.");
            }

            ref var tween = ref dataArray[denseIndex];
            if ( tween.Status == TweenStatus.None)
            {
                throw new ArgumentException("Tween has been destroyed or no longer exists.");
            }

            if (tween.Loops < 0)
            {
                UnityEngine.Debug.LogWarning("[RayTween] Complete was ignored because it is not possible to complete a Tween that loops infinitely. If you want to end the Tween, call Cancel() instead.");
                return;
            }

            ref var callbackData = ref callbacksArray[denseIndex];
            if (callbackData.IsCallbackRunning)
            {
                throw new InvalidOperationException("Recursion of Complete call was detected.");
            }
            callbackData.IsCallbackRunning = true;

            // To avoid duplication of Complete processing, it is treated as canceled internally.
            tween.Status = TweenStatus.Canceled;

            float endProgress = tween.LoopType switch
            {
                LoopType.Restart => 1f,
                LoopType.Yoyo => tween.Loops % 2 == 0 ? 0f : 1f,
                LoopType.Incremental => tween.Loops,
                _ => 1f
            };
            var endValue = tween.Plugin.Evaluate(
                ref tween.StartValue,
                ref tween.EndValue,
                EaseUtility.Evaluate(endProgress, tween.Ease) 
            );
            tween.Plugin.Dispose(ref tween.StartValue, ref tween.EndValue);
            try
            {
                callbackData.InvokeUnsafe(endValue);
            }
            catch (Exception ex)
            {
                TweenDispatcher.GetUnhandledExceptionHandler()?.Invoke(ex);
            }

            callbackData.InvokeAndDispose(TweenResult.Completed);

            callbackData.IsCallbackRunning = false;
        }

        public bool IsActive(int denseIndex)
        {
            if (denseIndex < 0 || denseIndex >= dataArray.Length) return false;
            var tween = dataArray[denseIndex];
            return tween.Status is TweenStatus.Scheduled or TweenStatus.Delayed or TweenStatus.Playing;
        }

        public ref  TweenCallbackData GetTweenCallbacks(int denseIndex)
        {
            CheckIndex(denseIndex);
            return ref  callbacksArray[denseIndex];
        }

        public void SetTweenCallbacks(int denseIndex, TweenCallbackData callbacks)
        {
            CheckIndex(denseIndex);
            callbacksArray[denseIndex] = callbacks;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void CheckIndex( int  denseIndex)
        {
           
            if (denseIndex < 0 || denseIndex >= dataArray.Length)
            {
                throw new ArgumentException("Tween has been destroyed or no longer exists.");
            }
            if (dataArray[denseIndex].Status == TweenStatus.None)
            {
                throw new ArgumentException("Tween has been destroyed or no longer exists.");
            }
        }
       
        ~TweenStorage()
        {
            Reset();
        }
        static readonly bool hasToDisposeTPlugin= default(TPlugin).HasDisposeImplementation;
        
        public void Reset()
        {
            if(hasToDisposeTPlugin)
            {
                foreach (ref var d in dataArray.AsSpan())
                {
                   d.Dispose();
                }
            }
            toEntryIndex.AsSpan().Clear();
            for (int i = 0; i < tail; i++)
            {
                if (dataArray[i].Status != TweenStatus.None)
                {
                    dataArray[i].Status = TweenStatus.Canceled;
                    callbacksArray[i].InvokeAndDispose(TweenResult.Canceled);
                }
            }
            dataArray.AsSpan().Clear();
            callbacksArray.AsSpan().Clear();
           
            var toEntryIndexSpan = toEntryIndex.AsSpan(0, tail);
            foreach (var entryIndex in toEntryIndexSpan)
            {
                if (entryIndex != null)
                {
                    TweenStorageManager.Free((int)entryIndex);
                }
            }
           
            tail = 0;
        }
    }
}