using RayTween.Internal;
using UnityEngine;

namespace RayTween.Extensions
{
    /// <summary>
    /// Provides binding extension methods for RectTransform.
    /// </summary>
    public static class RayTweenRectTransformExtensions
    {
        /// <summary>
        /// Create a tween data and bind it to RectTransform.anchoredPosition
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector2, TPlugin> BindToAnchoredPosition<TPlugin>(this TweenFromTo<Vector2, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<Vector2>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                target.anchoredPosition = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to RectTransform.anchoredPosition.x
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToAnchoredPositionX<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                var p = target.anchoredPosition;
                p.x = x;
                target.anchoredPosition = p;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to RectTransform.anchoredPosition.y
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToAnchoredPositionY<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                var p = target.anchoredPosition;
                p.y = x;
                target.anchoredPosition = p;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to RectTransform.anchoredPosition3D
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector3, TPlugin> BindToAnchoredPosition3D<TPlugin>(this TweenFromTo<Vector3, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                target.anchoredPosition3D = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to RectTransform.anchoredPosition3D.x
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle <float, TPlugin>BindToAnchoredPosition3DX<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                var p = target.anchoredPosition3D;
                p.x = x;
                target.anchoredPosition3D = p;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to RectTransform.anchoredPosition3D.y
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToAnchoredPosition3DY<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                var p = target.anchoredPosition3D;
                p.y = x;
                target.anchoredPosition3D = p;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to RectTransform.anchoredPosition3D.z
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToAnchoredPosition3DZ<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                var p = target.anchoredPosition3D;
                p.z = x;
                target.anchoredPosition3D = p;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to RectTransform.anchorMin
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector2, TPlugin> BindToAnchorMin<TPlugin>(this TweenFromTo<Vector2, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<Vector2>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                target.anchorMin = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to RectTransform.anchorMax
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector2, TPlugin> BindToAnchorMax<TPlugin>(this TweenFromTo<Vector2, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<Vector2>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                target.anchorMax = x;
            });
        }


        /// <summary>
        /// Create a tween data and bind it to RectTransform.sizeDelta
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector2, TPlugin> BindToSizeDelta<TPlugin>(this TweenFromTo<Vector2, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<Vector2>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                target.sizeDelta = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to RectTransform.sizeDelta.x
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToSizeDeltaX<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                var s = target.sizeDelta;
                s.x = x;
                target.sizeDelta = s;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to RectTransform.sizeDelta.y
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToSizeDeltaY<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                var s = target.sizeDelta;
                s.y = x;
                target.sizeDelta = s;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to RectTransform.pivot
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector2, TPlugin> BindToPivot<TPlugin>(this TweenFromTo<Vector2, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<Vector2>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                target.pivot = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to RectTransform.pivot.x
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToPivotX<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                var s = target.pivot;
                s.x = x;
                target.pivot = s;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to RectTransform.pivot.y
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="rectTransform"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToPivotY<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, RectTransform rectTransform)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(rectTransform);
            return fromTo.Bind(rectTransform, static (target, x) =>
            {
                var s = target.pivot;
                s.y = x;
                target.pivot = s;
            });
        }
    }
}