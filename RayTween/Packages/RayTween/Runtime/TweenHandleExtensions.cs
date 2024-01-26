using UnityEngine;

namespace RayTween
{
    public static class TweenHandleExtensions
    {
        
        
        
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
        
        public static TweenHandle<TValue, TPlugin> From<TValue, TPlugin>(this TweenHandle<TValue, TPlugin> handle,TValue options) where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            return handle;
        }
    }
}