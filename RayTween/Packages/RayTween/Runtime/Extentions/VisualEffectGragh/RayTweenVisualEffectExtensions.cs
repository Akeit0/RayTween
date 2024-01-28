#if RAYTWEEN_SUPPORT_VFX_GRAPH
using RayTween.Internal;
using UnityEngine;
using UnityEngine.VFX;

namespace RayTween.Extensions
{
    /// <summary>
    /// Provides binding extension methods for VisualEffect.
    /// </summary>
    public static class RayTweenVisualEffectExtensions
    {
        /// <summary>
        /// Create a tween data and bind it to VisualEffect parameter.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualEffect"></param>
        /// <param name="name"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToVisualEffectFloat<TPlugin>(
            this TweenFromTo<float, TPlugin> fromTo, VisualEffect visualEffect, string name)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualEffect);
            return fromTo.Bind(visualEffect, name, static (target, n, x) => { target.SetFloat(n, x); });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualEffect parameter.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualEffect"></param>
        /// <param name="nameID"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToVisualEffectFloat<TPlugin>(
            this TweenFromTo<float, TPlugin> fromTo, VisualEffect visualEffect, int nameID)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualEffect);
            return fromTo.Bind(visualEffect, ReadOnlyIntBox.Create(nameID),
                static (target, nameID, x) => { target.SetFloat(nameID.Value, x); });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualEffect parameter.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualEffect"></param>
        /// <param name="name"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToVisualEffectInt<TPlugin>(this TweenFromTo<int, TPlugin> fromTo,
            VisualEffect visualEffect, string name)
            where TPlugin : unmanaged, ITweenPlugin<int>
        {
            Error.IsNull(visualEffect);
            return fromTo.Bind(visualEffect, name, static (target, n, x) => { target.SetInt(n, x); });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualEffect parameter.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualEffect"></param>
        /// <param name="nameID"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToVisualEffectInt<TPlugin>(this TweenFromTo<int, TPlugin> fromTo,
            VisualEffect visualEffect, int nameID)
            where TPlugin : unmanaged, ITweenPlugin<int>
        {
            Error.IsNull(visualEffect);
            return fromTo.Bind(visualEffect, ReadOnlyIntBox.Create(nameID),
                static (target, nameID, x) => { target.SetFloat(nameID.Value, x); });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualEffect parameter.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualEffect"></param>
        /// <param name="name"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector2, TPlugin> BindToVisualEffectVector2<TPlugin>(
            this TweenFromTo<Vector2, TPlugin> fromTo, VisualEffect visualEffect, string name)
            where TPlugin : unmanaged, ITweenPlugin<Vector2>
        {
            Error.IsNull(visualEffect);
            return fromTo.Bind(visualEffect, name, static (target, n, x) => { target.SetVector2(n, x); });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualEffect parameter.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualEffect"></param>
        /// <param name="nameID"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector2, TPlugin> BindToVisualEffectVector2<TPlugin>(
            this TweenFromTo<Vector2, TPlugin> fromTo, VisualEffect visualEffect, int nameID)
            where TPlugin : unmanaged, ITweenPlugin<Vector2>
        {
            Error.IsNull(visualEffect);
            return fromTo.Bind(visualEffect, ReadOnlyIntBox.Create(nameID),
                static (target, nameID, x) => { target.SetVector2(nameID.Value, x); });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualEffect parameter.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualEffect"></param>
        /// <param name="name"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector3, TPlugin> BindToVisualEffectVector3<TPlugin>(
            this TweenFromTo<Vector3, TPlugin> fromTo, VisualEffect visualEffect, string name)
            where TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            Error.IsNull(visualEffect);
            return fromTo.Bind(visualEffect, name, static (target, n, x) => { target.SetVector3(n, x); });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualEffect parameter.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualEffect"></param>
        /// <param name="nameID"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector3, TPlugin> BindToVisualEffectVector3<TPlugin>(
            this TweenFromTo<Vector3, TPlugin> fromTo, VisualEffect visualEffect, int nameID)
            where TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            Error.IsNull(visualEffect);
            return fromTo.Bind(visualEffect, ReadOnlyIntBox.Create(nameID),
                static (target, nameID, x) => { target.SetVector3(nameID.Value, x); });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualEffect parameter.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualEffect"></param>
        /// <param name="name"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector4, TPlugin> BindToVisualEffectVector4<TPlugin>(
            this TweenFromTo<Vector4, TPlugin> fromTo, VisualEffect visualEffect, string name)
            where TPlugin : unmanaged, ITweenPlugin<Vector4>
        {
            Error.IsNull(visualEffect);
            return fromTo.Bind(visualEffect, name, static (target, n, x) => { target.SetVector4(n, x); });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualEffect parameter.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualEffect"></param>
        /// <param name="nameID"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector4, TPlugin> BindToVisualEffectVector4<TPlugin>(
            this TweenFromTo<Vector4, TPlugin> fromTo, VisualEffect visualEffect, int nameID)
            where TPlugin : unmanaged, ITweenPlugin<Vector4>
        {
            Error.IsNull(visualEffect);
            return fromTo.Bind(visualEffect, ReadOnlyIntBox.Create(nameID),
                static (target, nameID, x) => { target.SetVector4(nameID.Value, x); });
        }
    }
}
#endif