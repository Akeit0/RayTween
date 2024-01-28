#if RAYTWEEN_SUPPORT_UGUI
using System.Globalization;
using RayTween.Internal;
using RayTween.Plugins;
using UnityEngine;
using UnityEngine.UI;
#if RAYTWEEN_SUPPORT_ZSTRING
using Cysharp.Text;
#endif

namespace RayTween.Extensions
{
    /// <summary>
    /// Provides binding extension methods for Unity UI (uGUI) components.
    /// </summary>
    public static class RayTweenUGUIExtensions
    {
        /// <summary>
        /// Create a tween data and bind it to Graphic.color
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="graphic"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Color, TPlugin> BindToColor<TPlugin>(this TweenFromTo<Color, TPlugin> fromTo,
            Graphic graphic)
            where TPlugin : unmanaged, ITweenPlugin<Color>
        {
            Error.IsNull(graphic);
            return fromTo.Bind(graphic, static (target, x) => { target.color = x; });
        }

        /// <summary>
        /// Create a tween data and bind it to Graphic.color.r
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="graphic"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToColorR<TPlugin>(this TweenFromTo<float, TPlugin> fromTo,
            Graphic graphic)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(graphic);
            return fromTo.Bind(graphic, static (target, x) =>
            {
                var c = target.color;
                c.r = x;
                target.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to Graphic.color.g
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="graphic"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToColorG<TPlugin>(this TweenFromTo<float, TPlugin> fromTo,
            Graphic graphic)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(graphic);
            return fromTo.Bind(graphic, static (target, x) =>
            {
                var c = target.color;
                c.g = x;
                target.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to Graphic.color.b
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="graphic"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToColorB<TPlugin>(this TweenFromTo<float, TPlugin> fromTo,
            Graphic graphic)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(graphic);
            return fromTo.Bind(graphic, static (target, x) =>
            {
                var c = target.color;
                c.b = x;
                target.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to Graphic.color.a
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="graphic"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToColorA<TPlugin>(this TweenFromTo<float, TPlugin> fromTo,
            Graphic graphic)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(graphic);
            return fromTo.Bind(graphic, static (target, x) =>
            {
                var c = target.color;
                c.a = x;
                target.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to Image.FillAmount
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="image"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToFillAmount<TPlugin>(this TweenFromTo<float, TPlugin> fromTo,
            Image image)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(image);
            return fromTo.Bind(image, static (target, x) => { target.fillAmount = x; });
        }

        /// <summary>
        /// Create a tween data and bind it to Text.fontSize
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="text"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToFontSize<TPlugin>(this TweenFromTo<int, TPlugin> fromTo,
            Text text)
            where TPlugin : unmanaged, ITweenPlugin<int>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) => { target.fontSize = x; });
        }

        /// <summary>
        /// Create a tween data and bind it to Text.text
        /// </summary>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="text"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<UnsafeString, StringTweenPlugin> BindToText(
            this TweenFromTo<string, StringTweenPlugin> fromTo, Text text)
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) => { target.text = x.ToString(); });
        }

        /// <summary>
        /// Create a tween data and bind it to Text.text
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="text"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToText<TPlugin>(this TweenFromTo<int, TPlugin> fromTo, Text text)
            where TPlugin : unmanaged, ITweenPlugin<int>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) => { target.text = x.ToString(); });
        }

        /// <summary>
        /// Create a tween data and bind it to Text.text
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="text"></param>
        /// <param name="format">Format string</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToText<TPlugin>(this TweenFromTo<int, TPlugin> fromTo, Text text,
            string format)
            where TPlugin : unmanaged, ITweenPlugin<int>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, format, static (target, format, x) =>
            {
#if RAYTWEEN_SUPPORT_ZSTRING
                target.text = ZString.Format(format, x);
#else
                target.text = string.Format(format, x);
#endif
            });
        }

        /// <summary>
        /// Create a tween data and bind it to Text.text
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="text"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<long, TPlugin> BindToText<TPlugin>(this TweenFromTo<long, TPlugin> fromTo, Text text)
            where TPlugin : unmanaged, ITweenPlugin<long>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) => { target.text = x.ToString(); });
        }

        /// <summary>
        /// Create a tween data and bind it to Text.text
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="text"></param>
        /// <param name="format">Format string</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<long, TPlugin> BindToText<TPlugin>(this TweenFromTo<long, TPlugin> fromTo, Text text,
            string format)
            where TPlugin : unmanaged, ITweenPlugin<long>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, format, static (target, format, x) =>
            {
#if RAYTWEEN_SUPPORT_ZSTRING
                target.text = ZString.Format(format, x);
#else
                target.text = string.Format(format, x);
#endif
            });
        }

        /// <summary>
        /// Create a tween data and bind it to Text.text
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="text"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToText<TPlugin>(this TweenFromTo<float, TPlugin> fromTo,
            Text text)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) => { target.text = x.ToString(CultureInfo.CurrentCulture); });
        }

        /// <summary>
        /// Create a tween data and bind it to Text.text
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="text"></param>
        /// <param name="format">Format string</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToText<TPlugin>(this TweenFromTo<float, TPlugin> fromTo,
            Text text, string format)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, format, static (target, format, x) =>
            {
#if RAYTWEEN_SUPPORT_ZSTRING
                target.text = ZString.Format(format, x);
#else
                target.text = string.Format(format, x);
#endif
            });
        }
    }
}
#endif