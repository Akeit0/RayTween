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
        
        
    }
}