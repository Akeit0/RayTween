using System;
using RayTween.Plugins;
using RayTween.Internal;
using UnityEngine;

namespace RayTween.Extensions
{
    
    
    /// <summary>
    /// Provides binding extension methods for Transform.
    /// </summary>
    public static class RayTweenTransformExtensions
    {
//         internal static Vector3 WithLog(this Vector3 v) {
// //            Debug.Log(v);
//             return v;
//         }

        private static Func<object,Vector3> PositionGetter = target => ((Transform) target).position;
        
        
        internal static Vector3 WithX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);
        internal static Vector3 WithY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
        internal static Vector3 WithZ(this Vector3 v, float z) => new Vector3(v.x, v.y, z);
        
        public static TweenHandle<Vector3, TPlugin> BindToPosition<TPlugin>(
            this TweenFromTo<Vector3, TPlugin> tweenFromTo, Transform transform)
            where TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            return tweenFromTo.Bind(transform, static (target, value) =>
                target.position = value);
        }
        public static TweenHandle<float, TPlugin> BindToPositionX<TPlugin>(
            this TweenTo<float, TPlugin> tweenTo, Transform transform)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            return new TweenFromTo<float, TPlugin>(transform.position.x,tweenTo).Bind(transform, static (target, value) =>
                target.position = target.position.WithX(value));
        }
        public static TweenHandle<float, TPlugin> BindToPositionY<TPlugin>(
            this TweenTo<float, TPlugin> tweenTo, Transform transform)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            return new TweenFromTo<float, TPlugin>(transform.position.y,tweenTo).Bind(transform, static (target, value) =>
                target.position = target.position.WithY(value));
        }
        
        public static TweenHandle<float, TPlugin> BindToPositionZ<TPlugin>(
            this TweenFromTo<float, TPlugin> tweenFromTo, Transform transform)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            return tweenFromTo.Bind(transform, static (target, value) =>
                target.position = target.position.WithZ(value));
        }
        
        
        
        
        
        public static TweenHandle<Vector3, TPlugin> BindToPosition<TPlugin>(
            this TweenTo<Vector3, TPlugin> tweenTo, Transform transform)
            where TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            return new TweenFromTo<Vector3, TPlugin>(transform.position,tweenTo).Bind(transform, static (target, value) =>
                target.position = value);
        }
        
        public static TweenHandle<Vector3, TPlugin> BindToLocalScale<TPlugin>(
            this TweenFromTo<Vector3, TPlugin> tweenFromTo, Transform transform)
            where TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            return tweenFromTo.Bind(transform, static (target, value) =>
                target.localScale = value,RelativeMode.AbsoluteScale);
        }

        public static TweenHandle<Vector3, TPlugin> BindToLocalScale<TPlugin>(
            this TweenTo<Vector3, TPlugin> tweenTo, Transform transform)
            where TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            return new TweenFromTo<Vector3, TPlugin>(transform.localScale,tweenTo).Bind(transform, static (target, value) =>
                target.localScale = value,RelativeMode.AbsoluteScale);
        }
        
        
        
    }
}