using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using RayTween.Internal;
using UnityEngine;

namespace RayTween
{
    public static class TweenHandleExtensions
    {
        public static TweenHandle SetLink(this TweenHandle handle,GameObject gameObject,bool cancelOnDisable=false) 
          
        {

            if (cancelOnDisable)
            {
                LinkValidator.RegisterIsActiveInHierarchy(gameObject,handle);
            }
            else
            {
                LinkValidator.Register(gameObject,handle);
            }
            return handle;
        }
        public static TweenHandle SetLink(this TweenHandle handle,Component component,bool cancelOnDisable=false ) 
        {
            if (component is Behaviour behaviour)
            {
                return handle.SetLink(behaviour,cancelOnDisable);
            }
            return handle.SetLink(component.gameObject,cancelOnDisable);
        }
        public static TweenHandle SetLink(this TweenHandle handle,Behaviour behaviour,bool cancelOnDisable=false) 
        {
            if (cancelOnDisable)
            {
                LinkValidator.RegisterIsActiveAndEnabled(behaviour,handle);
            }
            else
            {
                LinkValidator.Register(behaviour,handle);
            }
            return handle;
        }
        
        public static TweenHandle<TValue, TPlugin> SetLink<TValue, TPlugin>(this TweenHandle<TValue, TPlugin> handle,GameObject gameObject,bool cancelOnDisable=false) where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {

            handle.AsNoType().SetLink(gameObject, cancelOnDisable);
            return handle;
        }
        public static TweenHandle<TValue, TPlugin> SetLink<TValue, TPlugin>(this TweenHandle<TValue, TPlugin> handle,Component component,bool cancelOnDisable=false ) where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            handle.AsNoType().SetLink(component, cancelOnDisable);
            return handle;
        }
        public static TweenHandle<TValue, TPlugin> SetLink<TValue, TPlugin>(this TweenHandle<TValue, TPlugin> handle,Behaviour behaviour,bool cancelOnDisable=false) where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            handle.AsNoType().SetLink(behaviour, cancelOnDisable);
            return handle;
        }
        static TComponent GetOrAddComponent<TComponent>(GameObject target) where TComponent : Component
        {
            if (!target.TryGetComponent<TComponent>(out var component))
                component = target.AddComponent<TComponent>();
            return component;
        }
        public static TweenHandle<TValue, TPlugin> SetOptions<TValue, TPlugin,TOptions>(this TweenHandle<TValue, TPlugin> handle,TOptions options) where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue,TOptions>
        {
            handle.Data.Plugin.SetOptions(options);
            return handle;
        }
        public static TweenHandle<TValue,  TPlugin>SetRelative<TValue,  TPlugin>(this TweenHandle<TValue,  TPlugin> handle, bool isRelative=true)where TValue : unmanaged
            where TPlugin : unmanaged, IRelativeTweenPlugin<TValue>
        {
            if (handle.IsIdling)
            {
                var buffer = TweenHandle<TValue,  TPlugin>.Buffer;
                buffer.SetRelative(isRelative);
            }
            else
            {
                Debug.LogWarning("Cannot set relative mode after the tween has scheduled.");
            }
            return handle;
        }
        /// <summary>
        /// Convert TweenHandle to IDisposable.
        /// </summary>
        /// <param name="handle">This tween handle</param>
        /// <returns></returns>
        public static IDisposable ToDisposable<TValue,  TPlugin>(this TweenHandle<TValue,  TPlugin> handle)where TValue : unmanaged
            where TPlugin : unmanaged, IRelativeTweenPlugin<TValue>
        {
            return new TweenHandleDisposable(handle.AsNoType());
        }
        /// <summary>
        /// Convert TweenHandle to IDisposable.
        /// </summary>
        /// <param name="handle">This tween handle</param>
        /// <returns></returns>
        public static IDisposable ToDisposable(this TweenHandle handle)
        {
            return new TweenHandleDisposable(handle);
        }
        /// <summary>
        /// Wait for the tween to finish in a coroutine.
        /// </summary>
        /// <param name="handle">This tween handle</param>
        /// <returns></returns>
        public static IEnumerator ToYieldInteraction<TValue,  TPlugin>(this TweenHandle<TValue,  TPlugin> handle)where TValue : unmanaged
            where TPlugin : unmanaged, IRelativeTweenPlugin<TValue>
        {
            while (handle.IsActive())
            {
                yield return null;
            }
        }
        /// <summary>
        /// Wait for the tween to finish in a coroutine.
        /// </summary>
        /// <param name="handle">This tween handle</param>
        /// <returns></returns>
        public static IEnumerator ToYieldInteraction(this TweenHandle handle)
        {
            while (handle.IsActive())
            {
                yield return null;
            }
        }
        public static ValueTask ToValueTask<TValue,  TPlugin>(this TweenHandle<TValue,  TPlugin> handle, CancellationToken cancellationToken = default)where TValue : unmanaged
            where TPlugin : unmanaged, IRelativeTweenPlugin<TValue>
        {
            if (!handle.IsActive()) return default;
            var source = ValueTaskTweenConfiguredSource.Create(handle.AsNoType(), cancellationToken, out var token);
            return new ValueTask(source, token);
        }
        public static ValueTask ToValueTask(this TweenHandle handle, CancellationToken cancellationToken = default)
        {
            if (!handle.IsActive()) return default;
            var source = ValueTaskTweenConfiguredSource.Create(handle, cancellationToken, out var token);
            return new ValueTask(source, token);
        }

#if UNITY_2023_1_OR_NEWER
        public static Awaitable ToAwaitable(this TweenHandle handle, CancellationToken cancellationToken = default)
        {
            if (!handle.IsActive()) return AwaitableTweenConfiguredSource.CompletedSource.Awaitable;
            return AwaitableTweenConfiguredSource.Create(handle, cancellationToken).Awaitable;
        }
#endif

    }

    internal sealed class TweenHandleDisposable : IDisposable
    {
        public TweenHandleDisposable(TweenHandle handle) => this.handle = handle;
        readonly TweenHandle handle;

        public void Dispose()
        {
            if (handle.IsActive()) handle.Cancel();
        }
    }
    
}