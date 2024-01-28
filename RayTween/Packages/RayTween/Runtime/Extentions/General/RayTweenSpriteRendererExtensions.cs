using RayTween.Internal;
using UnityEngine;

namespace RayTween.Extensions
{
    /// <summary>
    /// Provides binding extension methods for SpriteRenderer.
    /// </summary>
    public static class RayTweenSpriteRendererExtensions
    {
        /// <summary>
        /// Create a tween data and bind it to SpriteRenderer.color
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">From to duration</param>
        /// <param name="spriteRenderer"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Color,TPlugin> BindToColor<TPlugin>(this TweenFromTo<Color, TPlugin> fromTo, SpriteRenderer spriteRenderer)
            where TPlugin : unmanaged, ITweenPlugin<Color>
        {
            Error.IsNull(spriteRenderer);
            return fromTo.Bind(spriteRenderer, static (m, x) =>
            {
                m.color = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to SpriteRenderer.color.r
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">From to duration</param>
        /// <param name="spriteRenderer"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToColorR<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, SpriteRenderer spriteRenderer)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(spriteRenderer);
            return fromTo.Bind(spriteRenderer, static (m, x) =>
            {
                var c = m.color;
                c.r = x;
                m.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to SpriteRenderer.color.r
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">From to duration</param>
        /// <param name="spriteRenderer"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToColorG<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, SpriteRenderer spriteRenderer)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(spriteRenderer);
            return fromTo.Bind(spriteRenderer, static (m, x) =>
            {
                var c = m.color;
                c.g = x;
                m.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to SpriteRenderer.color.r
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">From to duration</param>
        /// <param name="spriteRenderer"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToColorB<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, SpriteRenderer spriteRenderer)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(spriteRenderer);
            return fromTo.Bind(spriteRenderer, static (m, x) =>
            {
                var c = m.color;
                c.b = x;
                m.color = c;
            });
        }


        /// <summary>
        /// Create a tween data and bind it to SpriteRenderer.color.r
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">From to duration</param>
        /// <param name="spriteRenderer"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToColorA<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, SpriteRenderer spriteRenderer)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(spriteRenderer);
            return fromTo.Bind(spriteRenderer, static (m, x) =>
            {
                var c = m.color;
                c.a = x;
                m.color = c;
            });
        }
    }
}