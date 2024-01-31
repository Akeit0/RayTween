using System;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;
using PlayerLoopType = UnityEngine.PlayerLoop;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RayTween.Internal
{
    /// <summary>
    /// Types of PlayerLoop inserted for Tween updates
    /// </summary>
    internal static class RayTweenLoopRunners
    {
        public struct RayTweenInitialization
        {
        };

        public struct RayTweenEarlyUpdate
        {
        };

        public struct RayTweenFixedUpdate
        {
        };

        public struct RayTweenPreUpdate
        {
        };

        public struct RayTweenUpdate
        {
        };

        public struct RayTweenPreLateUpdate
        {
        };

        public struct RayTweenPostLateUpdate
        {
        };

        public struct RayTweenTimeUpdate
        {
        };
    }



    internal static class PlayerLoopHelper
    {
        public static event Action OnInitialization;
        public static event Action OnEarlyUpdate;
        public static event Action OnFixedUpdate;
        public static event Action OnPreUpdate;
        public static event Action OnUpdate;
        public static event Action OnPreLateUpdate;
        public static event Action OnPostLateUpdate;
        public static event Action OnTimeUpdate;
        static bool initialized;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Init()
        {
#if UNITY_EDITOR
            var domainReloadDisabled = EditorSettings.enterPlayModeOptionsEnabled &&
                                       EditorSettings.enterPlayModeOptions.HasFlag(EnterPlayModeOptions
                                           .DisableDomainReload);
            if (!domainReloadDisabled && initialized) return;
#else
            if (initialized) return;
#endif
            OnInitialization += static () => TweenDispatcher.Update(UpdateTiming.Initialization);
            OnEarlyUpdate += static () => TweenDispatcher.Update(UpdateTiming.EarlyUpdate);
            OnFixedUpdate += static () => TweenDispatcher.Update(UpdateTiming.FixedUpdate);
            OnPreUpdate += static () => TweenDispatcher.Update(UpdateTiming.PreUpdate);
            OnUpdate += static () => TweenDispatcher.Update(UpdateTiming.Update);
            OnPreLateUpdate += static () => TweenDispatcher.Update(UpdateTiming.PreLateUpdate);
            OnPostLateUpdate += static () => TweenDispatcher.Update(UpdateTiming.PostLateUpdate);
            OnTimeUpdate += static () =>
            {
#if !UNITY_EDITOR
LinkValidator.Update(Time.frameCount);
#endif

                TweenDispatcher.Update(UpdateTiming.TimeUpdate);
            };

            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            Initialize(ref playerLoop);
        }

        public static void Initialize(ref PlayerLoopSystem playerLoop)
        {
            initialized = true;
            var newLoop = playerLoop.subSystemList.ToArray();

            InsertLoop(newLoop, typeof(PlayerLoopType.Initialization),
                typeof(RayTweenLoopRunners.RayTweenInitialization), static () => OnInitialization?.Invoke());
            InsertLoop(newLoop, typeof(PlayerLoopType.EarlyUpdate), typeof(RayTweenLoopRunners.RayTweenEarlyUpdate),
                static () => OnEarlyUpdate?.Invoke());
            InsertLoop(newLoop, typeof(PlayerLoopType.FixedUpdate), typeof(RayTweenLoopRunners.RayTweenFixedUpdate),
                static () => OnFixedUpdate?.Invoke());
            InsertLoop(newLoop, typeof(PlayerLoopType.PreUpdate), typeof(RayTweenLoopRunners.RayTweenPreUpdate),
                static () => OnPreUpdate?.Invoke());
            InsertLoop(newLoop, typeof(PlayerLoopType.Update), typeof(RayTweenLoopRunners.RayTweenUpdate),
                static () => OnUpdate?.Invoke());
            InsertLoop(newLoop, typeof(PlayerLoopType.PreLateUpdate), typeof(RayTweenLoopRunners.RayTweenPreLateUpdate),
                static () => OnPreLateUpdate?.Invoke());
            InsertLoop(newLoop, typeof(PlayerLoopType.PostLateUpdate),
                typeof(RayTweenLoopRunners.RayTweenPostLateUpdate), static () => OnPostLateUpdate?.Invoke());
            InsertLoop(newLoop, typeof(PlayerLoopType.TimeUpdate), typeof(RayTweenLoopRunners.RayTweenTimeUpdate),
                static () => OnTimeUpdate?.Invoke());

            playerLoop.subSystemList = newLoop;
            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        static void InsertLoop(PlayerLoopSystem[] loopSystems, Type loopType, Type loopRunnerType,
            PlayerLoopSystem.UpdateFunction updateDelegate)
        {
            var i = FindLoopSystemIndex(loopSystems, loopType);
            ref var loop = ref loopSystems[i];
            loop.subSystemList = InsertRunner(loop.subSystemList, loopRunnerType, updateDelegate);
        }

        static int FindLoopSystemIndex(PlayerLoopSystem[] playerLoopList, Type systemType)
        {
            for (int i = 0; i < playerLoopList.Length; i++)
            {
                if (playerLoopList[i].type == systemType)
                {
                    return i;
                }
            }

            throw new Exception("Target PlayerLoopSystem does not found. Type:" + systemType.FullName);
        }

        static PlayerLoopSystem[] InsertRunner(PlayerLoopSystem[] subSystemList, Type loopRunnerType,
            PlayerLoopSystem.UpdateFunction updateDelegate)
        {
            var source = subSystemList.Where(x => x.type != loopRunnerType).ToArray();
            var dest = new PlayerLoopSystem[source.Length + 1];

            Array.Copy(source, 0, dest, 1, source.Length);

            dest[0] = new PlayerLoopSystem
            {
                type = loopRunnerType,
                updateDelegate = updateDelegate
            };

            return dest;
        }
    }
}