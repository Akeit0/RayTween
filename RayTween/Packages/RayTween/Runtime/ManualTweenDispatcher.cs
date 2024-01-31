using System;
using RayTween.Internal;
using UnityEngine;

namespace RayTween
{
    /// <summary>
    /// Manually updatable TweenDispatcher
    /// </summary>
    public static class ManualTweenDispatcher
    {
        static class Cache<TValue, TPlugin>
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            public static TweenStorage<TValue, TPlugin> updateStorage;
            public static UpdateRunner<TValue, TPlugin> updateRunner;

            public static TweenStorage<TValue, TPlugin> GetOrCreate()
            {
                if (updateStorage == null)
                {
                    Debug.Log(TweenStorageManager.CurrentStorageId);
                    var storage = new TweenStorage<TValue, TPlugin>(TweenStorageManager.CurrentStorageId);
                    TweenStorageManager.AddStorage(storage);
                    updateStorage = storage;
                }

                return updateStorage;
            }
        }

        static readonly MinimumList<IUpdateRunner> updateRunners = new();

        /// <summary>
        /// ManualTweenDispatcher time. It increases every time Update is called.
        /// </summary>
        public static double Time { get; set; }

        /// <summary>
        /// Ensures the storage capacity until it reaches at least `capacity`.
        /// </summary>
        /// <param name="capacity">The minimum capacity to ensure.</param>
        public static void EnsureStorageCapacity<TValue, TPlugin>(int capacity)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            Cache<TValue, TPlugin>.GetOrCreate().EnsureCapacity(capacity);
        }

        /// <summary>
        /// Update all scheduled tweens with TweenScheduler.Manual
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        public static void Update(double deltaTime)
        {
            if (deltaTime < 0f) throw new ArgumentException("deltaTime must be 0 or higher.");
            Time += deltaTime;
            Update();
        }

        /// <summary>
        /// Update all scheduled tweens with TweenScheduler.Manual
        /// </summary>
        public static void Update()
        {
            try
            {
                TweenDispatcher.OnUpdateAction?.Invoke(UpdateTiming.Manual);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            var span = updateRunners.AsSpan();
            for (int i = 0; i < span.Length; i++)
            {
                span[i].Update(Time, Time, Time);
            }
        }

        /// <summary>
        /// Cancel all tweens and reset data.
        /// </summary>
        public static void Reset()
        {
            var span = updateRunners.AsSpan();
            for (int i = 0; i < span.Length; i++)
            {
                span[i].Reset();
            }
        }

        internal static int Schedule<TValue, TPlugin>(TweenHandle<TValue, TPlugin> handle,
            ref TweenData<TValue, TPlugin> data, ref TweenCallbackData callbackData)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            TweenStorage<TValue, TPlugin> storage = Cache<TValue, TPlugin>.GetOrCreate();
            if (Cache<TValue, TPlugin>.updateRunner == null)
            {
                var runner = new UpdateRunner<TValue, TPlugin>(storage, Time, Time, Time);
                updateRunners.Add(runner);
                Cache<TValue, TPlugin>.updateRunner = runner;
            }

            storage.Append(handle, data, callbackData);
            return storage.StorageId;
        }
    }
}