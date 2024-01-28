#if RAYTWEEN_SUPPORT_UIELEMENTS
using System.Globalization;
using RayTween.Internal;
using RayTween.Plugins;
using UnityEngine;
using UnityEngine.UIElements;
#if RAYTWEEN_SUPPORT_ZSTRING
using Cysharp.Text;
#endif

namespace RayTween.Extensions
{
    /// <summary>
    /// Provides binding extension methods for UIElements.
    /// </summary>
    public static class RayTweenUIToolkitExtensions
    {
        #region VisualElement

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.left
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleLeft<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.left = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.right
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleRight<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.right = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.top
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleTop<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.top = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.bottom
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleBottom<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.bottom = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.width
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleWidth<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.width = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.height
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleHeight<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.height = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.color
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Color, TPlugin> BindToStyleColor<TPlugin>(this TweenFromTo<Color, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<Color>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.color = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.color.r
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleColorR<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                var c = target.style.color.value;
                c.r = x;
                target.style.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.color.g
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleColorG<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                var c = target.style.color.value;
                c.g = x;
                target.style.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.color.b
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleColorB<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                var c = target.style.color.value;
                c.b = x;
                target.style.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.color.a
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleColorA<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                var c = target.style.color.value;
                c.a = x;
                target.style.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.backgroundColor
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Color, TPlugin> BindToStyleBackgroundColor<TPlugin>(this TweenFromTo<Color, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<Color>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.backgroundColor = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.backgroundColor.r
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleBackgroundColorR<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                var c = target.style.backgroundColor.value;
                c.r = x;
                target.style.backgroundColor = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.backgroundColor.g
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleBackgroundColorG<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                var c = target.style.backgroundColor.value;
                c.g = x;
                target.style.backgroundColor = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.backgroundColor.b
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleBackgroundColorB<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                var c = target.style.backgroundColor.value;
                c.b = x;
                target.style.backgroundColor = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.backgroundColor.a
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleBackgroundColorA<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                var c = target.style.backgroundColor.value;
                c.a = x;
                target.style.backgroundColor = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.opacity
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleOpacity<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.opacity = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.fontSize
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleFontSize<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.fontSize = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.wordSpacing
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleWordSpacing<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.wordSpacing = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.translate
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector3, TPlugin> BindToStyleTranslate<TPlugin>(this TweenFromTo<Vector3, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.translate = new Translate(x.x, x.y, x.z);
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.translate
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector2, TPlugin> BindToStyleTranslate<TPlugin>(this TweenFromTo<Vector2, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<Vector2>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.translate = new Translate(x.x, x.y);
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.rotate
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <param name="angleUnit"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToStyleRotate<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, VisualElement visualElement, AngleUnit angleUnit = AngleUnit.Degree)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, new Box<AngleUnit>(angleUnit),static (target,angleUnit, x) =>
            {
                target.style.rotate = new Rotate(new Angle(x, angleUnit.Value));
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.scale
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector3, TPlugin> BindToStyleScale<TPlugin>(this TweenFromTo<Vector3, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.scale = new Scale(x);
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.transformOrigin
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector3, TPlugin> BindToStyleTransformOrigin<TPlugin>(this TweenFromTo<Vector3, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.transformOrigin = new TransformOrigin(x.x, x.y, x.z);
            });
        }

        /// <summary>
        /// Create a tween data and bind it to VisualElement.style.transformOrigin
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="visualElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector2, TPlugin> BindToStyleTransformOrigin<TPlugin>(this TweenFromTo<Vector2, TPlugin> fromTo, VisualElement visualElement)
            where  TPlugin : unmanaged, ITweenPlugin<Vector2>
        {
            Error.IsNull(visualElement);
            return fromTo.Bind(visualElement, static (target, x) =>
            {
                target.style.transformOrigin = new TransformOrigin(x.x, x.y);
            });
        }

        #endregion

        #region AbstractProgressBar

        /// <summary>
        /// Create a tween data and bind it to AbstractProgressBar.value
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="progressBar"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToProgressBar<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, AbstractProgressBar progressBar)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(progressBar);
            return fromTo.Bind(progressBar, static (target, x) =>
            {
                target.value = x;
            });
        }

        #endregion

        #region TextElement

        /// <summary>
        /// Create a tween data and bind it to Text.text
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="text"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<UnsafeString, StringTweenPlugin> BindToText<TPlugin>(
            this TweenFromTo<string, StringTweenPlugin> fromTo, TextElement text)
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) => { target.text = x.ToString(); });
        }

        /// <summary>
        /// Create a tween data and bind it to Text.text
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="textElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToText<TPlugin>(this TweenFromTo<int, TPlugin> fromTo, TextElement textElement)
            where  TPlugin : unmanaged, ITweenPlugin<int>
        {
            Error.IsNull(textElement);
            return fromTo.Bind(textElement, static (target, x) =>
            {
                target.text = x.ToString();
            });
        }

        /// <summary>
        /// Create a tween data and bind it to Text.text
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="textElement"></param>
        /// <param name="format">Format string</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToText<TPlugin>(this TweenFromTo<int, TPlugin> fromTo, TextElement 
            
            textElement, string format)
            where  TPlugin : unmanaged, ITweenPlugin<int>
        {
            Error.IsNull(textElement);
            return fromTo.Bind(textElement, format, static (target, f, x) =>
            {
#if RAYTWEEN_SUPPORT_ZSTRING
                target.text = ZString.Format(f, x);
#else
                target.text = string.Format(f, x);
#endif
            });
        }

        /// <summary>
        /// Create a tween data and bind it to Text.text
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="textElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<long, TPlugin> BindToText<TPlugin>(this TweenFromTo<long, TPlugin> fromTo, TextElement textElement)
            where  TPlugin : unmanaged, ITweenPlugin<long>
        {
            Error.IsNull(textElement);
            return fromTo.Bind(textElement, static (target, x) =>
            {
                target.text = x.ToString();
            });
        }

        /// <summary>
        /// Create a tween data and bind it to Text.text
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="textElement"></param>
        /// <param name="format">Format string</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<long, TPlugin> BindToText<TPlugin>(this TweenFromTo<long, TPlugin> fromTo, TextElement textElement, string format)
            where  TPlugin : unmanaged, ITweenPlugin<long>
        {
            Error.IsNull(textElement);
            return fromTo.Bind(textElement, format, static (target, format, x) =>
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
        /// <param name="textElement"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToText<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, TextElement textElement)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(textElement);
            return fromTo.Bind(textElement, static (target, x) =>
            {
                target.text = x.ToString(CultureInfo.InvariantCulture);
            });
        }

        /// <summary>
        /// Create a tween data and bind it to Text.text
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="textElement"></param>
        /// <param name="format">Format string</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToText<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, TextElement textElement, string format)
            where  TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(textElement);
            return fromTo.Bind(textElement, format, static (target, format, x) =>
            {
#if RAYTWEEN_SUPPORT_ZSTRING
                target.text = ZString.Format(format, x);
#else
                target.text = string.Format(format, x);
#endif
            });
        }
        #endregion
    }
}
#endif
