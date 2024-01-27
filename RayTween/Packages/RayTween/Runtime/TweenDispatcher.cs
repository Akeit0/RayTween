using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using RayTween.Internal;

namespace RayTween
{
    /// <summary>
    /// Tween dispatcher.
    /// </summary>
    public static class TweenDispatcher
    {
        internal static Action<UpdateTiming> OnUpdateAction;

        static class StorageCache<TValue, TPlugin>
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            static TweenStorage<TValue, TPlugin> initialization;
            static TweenStorage<TValue, TPlugin> earlyUpdate;
            static TweenStorage<TValue, TPlugin> fixedUpdate;
            static TweenStorage<TValue, TPlugin> preUpdate;
            static TweenStorage<TValue, TPlugin> update;
            static TweenStorage<TValue, TPlugin> preLateUpdate;
            static TweenStorage<TValue, TPlugin> postLateUpdate;
            static TweenStorage<TValue, TPlugin> timeUpdate;

            public static TweenStorage<TValue, TPlugin> GetOrCreate(UpdateTiming updateTiming)
            {
                return updateTiming switch
                {
                    UpdateTiming.Initialization => CreateIfNull(ref initialization),
                    UpdateTiming.EarlyUpdate => CreateIfNull(ref earlyUpdate),
                    UpdateTiming.FixedUpdate => CreateIfNull(ref fixedUpdate),
                    UpdateTiming.PreUpdate => CreateIfNull(ref preUpdate),
                    UpdateTiming.Update => CreateIfNull(ref update),
                    UpdateTiming.PreLateUpdate => CreateIfNull(ref preLateUpdate),
                    UpdateTiming.PostLateUpdate => CreateIfNull(ref postLateUpdate),
                    UpdateTiming.TimeUpdate => CreateIfNull(ref timeUpdate),
                    _ => null,
                };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static TweenStorage<TValue, TPlugin> CreateIfNull(ref TweenStorage<TValue, TPlugin> storage)
            {
                if (storage == null)
                {
                    storage = new TweenStorage<TValue, TPlugin>(TweenStorageManager.CurrentStorageId);
                    TweenStorageManager.AddStorage(storage);
                }

                return storage;
            }
        }

        static class RunnerCache<TValue, TPlugin>
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
             static UpdateRunner<TValue, TPlugin> initialization;
             static UpdateRunner<TValue, TPlugin> earlyUpdate;
             static UpdateRunner<TValue, TPlugin> fixedUpdate;
             static UpdateRunner<TValue, TPlugin> preUpdate;
             static UpdateRunner<TValue, TPlugin> update;
             static UpdateRunner<TValue, TPlugin> preLateUpdate;
             static UpdateRunner<TValue, TPlugin> postLateUpdate;
             static UpdateRunner<TValue, TPlugin> timeUpdate;

            public static (UpdateRunner<TValue, TPlugin> runner, bool isCreated) GetOrCreate(
                UpdateTiming updateTiming, TweenStorage<TValue, TPlugin> storage)
            {
                return updateTiming switch
                {
                    UpdateTiming.Initialization => CreateIfNull(updateTiming, ref initialization, storage),
                    UpdateTiming.EarlyUpdate => CreateIfNull(updateTiming, ref earlyUpdate, storage),
                    UpdateTiming.FixedUpdate => CreateIfNull(updateTiming, ref fixedUpdate, storage),
                    UpdateTiming.PreUpdate => CreateIfNull(updateTiming, ref preUpdate, storage),
                    UpdateTiming.Update => CreateIfNull(updateTiming, ref update, storage),
                    UpdateTiming.PreLateUpdate => CreateIfNull(updateTiming, ref preLateUpdate, storage),
                    UpdateTiming.PostLateUpdate => CreateIfNull(updateTiming, ref postLateUpdate, storage),
                    UpdateTiming.TimeUpdate => CreateIfNull(updateTiming, ref timeUpdate, storage),
                    _ => default,
                };
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static (UpdateRunner<TValue, TPlugin>, bool) CreateIfNull(UpdateTiming updateTiming,
                ref UpdateRunner<TValue, TPlugin> runner, TweenStorage<TValue, TPlugin> storage)
            {
                if (runner == null)
                {
                    runner = new UpdateRunner<TValue, TPlugin>(storage);
                    GetRunnerList(updateTiming).Add(runner);
                    return (runner, true);
                }

                return (runner, false);
            }
        }

        static readonly MinimumList<IUpdateRunner> initializationRunners = new();
        static readonly MinimumList<IUpdateRunner> earlyUpdateRunners = new();
        static readonly MinimumList<IUpdateRunner> fixedUpdateRunners = new();
        static readonly MinimumList<IUpdateRunner> preUpdateRunners = new();
        static readonly MinimumList<IUpdateRunner> updateRunners = new();
        static readonly MinimumList<IUpdateRunner> preLateUpdateRunners = new();
        static readonly MinimumList<IUpdateRunner> postLateUpdateRunners = new();
        static readonly MinimumList<IUpdateRunner> timeUpdateRunners = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static MinimumList<IUpdateRunner> GetRunnerList(UpdateTiming updateTiming)
        {
            return updateTiming switch
            {
                UpdateTiming.Initialization => initializationRunners,
                UpdateTiming.EarlyUpdate => earlyUpdateRunners,
                UpdateTiming.FixedUpdate => fixedUpdateRunners,
                UpdateTiming.PreUpdate => preUpdateRunners,
                UpdateTiming.Update => updateRunners,
                UpdateTiming.PreLateUpdate => preLateUpdateRunners,
                UpdateTiming.PostLateUpdate => postLateUpdateRunners,
                UpdateTiming.TimeUpdate => timeUpdateRunners,
                _ => null
            };
        }

        static Action<Exception> unhandledException = DefaultUnhandledExceptionHandler;

        static readonly UpdateTiming[] updateTimings = ((UpdateTiming[])Enum.GetValues(typeof(UpdateTiming)))
            .Where(x => x != UpdateTiming.Manual).ToArray();

        static TweenDispatcher()
        {
            Init();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            Clear();
        }

        /// <summary>
        /// Set handling of unhandled exceptions.
        /// </summary>
        public static void RegisterUnhandledExceptionHandler(Action<Exception> unhandledExceptionHandler)
        {
            unhandledException = unhandledExceptionHandler;
        }

        /// <summary>
        /// Get handling of unhandled exceptions.
        /// </summary>
        public static Action<Exception> GetUnhandledExceptionHandler()
        {
            return unhandledException;
        }

        static void DefaultUnhandledExceptionHandler(Exception exception)
        {
            Debug.LogException(exception);
        }

        /// <summary>
        /// Cancel all tweens.
        /// </summary>
        public static void Clear()
        {
            foreach (var updateTiming in updateTimings)
            {
                var list = GetRunnerList(updateTiming);
                if (list != null)
                {
                    foreach (var t in list.AsSpan())
                    {
                        t.Reset();
                    }
                }
            }
        }
        /// <summary>
        /// Ensures the storage capacity until it reaches at least `capacity`.
        /// </summary>
        /// <param name="capacity">The minimum capacity to ensure.</param>
        /// <param name="updateTiming">The update timing to ensure.</param>
        public static void EnsureStorageCapacity<TValue, TPlugin>(int capacity,UpdateTiming updateTiming)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            foreach (var updateTiming2 in updateTimings)
            {
                if(updateTiming2==updateTiming)
                    StorageCache<TValue, TPlugin>.GetOrCreate(updateTiming).EnsureCapacity(capacity);
            }
#if UNITY_EDITOR
            EditorTweenDispatcher.EnsureStorageCapacity<TValue, TPlugin>(capacity);
#endif
        }
        /// <summary>
        /// Ensures the storage capacity until it reaches at least `capacity`.
        /// </summary>
        /// <param name="capacity">The minimum capacity to ensure.</param>
        public static void EnsureStorageCapacity<TValue, TPlugin>(int capacity)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            foreach (var updateTiming in updateTimings)
            {
                StorageCache<TValue, TPlugin>.GetOrCreate(updateTiming).EnsureCapacity(capacity);
            }
#if UNITY_EDITOR
            EditorTweenDispatcher.EnsureStorageCapacity<TValue, TPlugin>(capacity);
#endif
        }

        internal static int Schedule<TValue, TPlugin>(TweenHandle<TValue, TPlugin> handle,
            in TweenData<TValue, TPlugin> data, in TweenCallbackData callbackData, UpdateTiming updateTiming)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            var storage = StorageCache<TValue, TPlugin>.GetOrCreate(updateTiming);
            RunnerCache<TValue, TPlugin>.GetOrCreate(updateTiming, storage);

            storage.Append(handle, data, callbackData);
            return storage.StorageId;
        }

        internal static void Update(UpdateTiming updateTiming)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif

            try
            {
                OnUpdateAction?.Invoke(updateTiming);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            var span = GetRunnerList(updateTiming).AsSpan();
            if (updateTiming == UpdateTiming.FixedUpdate)
            {
                foreach (var t in span)
                    t.Update(Time.fixedTimeAsDouble, Time.fixedUnscaledTimeAsDouble, Time.realtimeSinceStartupAsDouble);
            }
            else
            {
                foreach (var t in span)
                    t.Update(Time.timeAsDouble, Time.unscaledTimeAsDouble, Time.realtimeSinceStartupAsDouble);
            }
        }
    }

#if UNITY_EDITOR
    internal static class EditorTweenDispatcher
    {
        static class Cache<TValue, TPlugin>
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            static TweenStorage<TValue, TPlugin> storage;
            static UpdateRunner<TValue, TPlugin> updateRunner;

            public static TweenStorage<TValue, TPlugin> GetOrCreateStorage()
            {
                if (storage == null)
                {
                    storage = new TweenStorage<TValue, TPlugin>(TweenStorageManager.CurrentStorageId);
                    TweenStorageManager.AddStorage(storage);
                }

                return storage;
            }

            public static void InitUpdateRunner()
            {
                if (updateRunner == null)
                {
                    updateRunner = new UpdateRunner<TValue, TPlugin>(storage);
                    updateRunners.Add(updateRunner);
                }
            }
        }

        static readonly MinimumList<IUpdateRunner> updateRunners = new();

        public static int Schedule<TValue, TPlugin>(TweenHandle<TValue, TPlugin> handle,
            in TweenData<TValue, TPlugin> data, in TweenCallbackData callbackData)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            var storage = Cache<TValue, TPlugin>.GetOrCreateStorage();
            Cache<TValue, TPlugin>.InitUpdateRunner();

            storage.Append(handle, data, callbackData);
            return storage.StorageId;
        }

        public static void EnsureStorageCapacity<TValue, TPlugin>(int capacity)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            Cache<TValue, TPlugin>.GetOrCreateStorage().EnsureCapacity(capacity);
        }

        static bool isInitialized;

        [InitializeOnLoadMethod]
        static void Init()
        {
            if (isInitialized) return;
            isInitialized = true;
            EditorApplication.update += Update;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
        }
      
        // static void ClearEditorUpdates()
        // {
        //     var span = updateRunners.AsSpan();
        //     foreach (var t in span)
        //     {
        //         t.Reset();
        //     }
        // }
        static EditorTweenDispatcher()
        {
            Init();
        }

        static void OnDomainUnload(object sender, EventArgs e)
        {
            EditorApplication.update -= Update;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }
        public static int  PlayModeVersion { get; private set; }
        static void OnPlayModeStateChanged(PlayModeStateChange s)
        {
            PlayModeVersion++;
            if (s is PlayModeStateChange.ExitingPlayMode )
            {
                
                TweenDispatcher.Clear();
                //TweenStorageManager.Reset();
                //  Debug.Log("Exit PlayMode");
            }
        }

        static void Update()
        {
            if (!EditorApplication.isPlaying)
            {
                for (int i = 0; i < (int)UpdateTiming.Manual; i++)
                {
                    TweenDispatcher.OnUpdateAction?.Invoke((UpdateTiming)i);
                }
            }

            var span = updateRunners.AsSpan();
            foreach (var t in span)
            {
                t.Update(EditorApplication.timeSinceStartup, EditorApplication.timeSinceStartup,
                    Time.realtimeSinceStartupAsDouble);
            }
        }
    }
#endif
}