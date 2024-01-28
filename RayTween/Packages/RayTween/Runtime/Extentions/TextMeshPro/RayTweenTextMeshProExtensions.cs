#if RAYTWEEN_SUPPORT_TMP
using System;
using System.Buffers;
using UnityEngine;
using TMPro;
using RayTween.Internal;
using RayTween.Plugins;
#if RAYTWEEN_SUPPORT_ZSTRING
using Cysharp.Text;
#endif

namespace RayTween.Extensions
{
    /// <summary>
    /// Provides binding extension methods for TMP_Text
    /// </summary>
    public static class RayTweenTextMeshProExtensions
    {
        /// <summary>
        /// Create a tween data and bind it to TMP_Text.fontSize
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToFontSize<TPlugin>(
            this TweenFromTo<float, TPlugin> fromTo, TMP_Text text)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) => { target.fontSize = x; });
        }

        /// <summary>
        /// Create a tween data and bind it to TMP_Text.maxVisibleCharacters
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToMaxVisibleCharacters<TPlugin>(
            this TweenFromTo<int, TPlugin> fromTo, TMP_Text text)
            where TPlugin : unmanaged, ITweenPlugin<int>

        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) => { target.maxVisibleCharacters = x; });
        }

        /// <summary>
        /// Create a tween data and bind it to TMP_Text.maxVisibleLines
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToMaxVisibleLines<TPlugin>(
            this TweenFromTo<int, TPlugin> fromTo, TMP_Text text)
            where TPlugin : unmanaged, ITweenPlugin<int>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) => { target.maxVisibleLines = x; });
        }

        /// <summary>
        /// Create a tween data and bind it to TMP_Text.maxVisibleWords
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToMaxVisibleWords<TPlugin>(
            this TweenFromTo<int, TPlugin> fromTo, TMP_Text text)
            where TPlugin : unmanaged, ITweenPlugin<int>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) => { target.maxVisibleWords = x; });
        }

        /// <summary>
        /// Create a tween data and bind it to TMP_Text.color
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Color, TPlugin> BindToColor<TPlugin>(this TweenFromTo<Color, TPlugin> fromTo,
            TMP_Text text)
            where TPlugin : unmanaged, ITweenPlugin<Color>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) => { target.color = x; });
        }

        /// <summary>
        /// Create a tween data and bind it to TMP_Text.color.r
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToColorR<TPlugin>(
            this TweenFromTo<float, TPlugin> fromTo, TMP_Text text)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) =>
            {
                var c = target.color;
                c.r = x;
                target.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to TMP_Text.color.g
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToColorG<TPlugin>(
            this TweenFromTo<float, TPlugin> fromTo, TMP_Text text)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) =>
            {
                var c = target.color;
                c.g = x;
                target.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to TMP_Text.color.b
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToColorB<TPlugin>(
            this TweenFromTo<float, TPlugin> fromTo, TMP_Text text)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) =>
            {
                var c = target.color;
                c.b = x;
                target.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to TMP_Text.color.a
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToColorA<TPlugin>(
            this TweenFromTo<float, TPlugin> fromTo, TMP_Text text)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) =>
            {
                var c = target.color;
                c.a = x;
                target.color = c;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to TMP_Text.text.
        /// </summary>
        /// <remarks>
        /// Note: This extension method uses TMP_Text.SetText() to achieve zero allocation, so it is recommended to use this method when binding to text.
        /// </remarks>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <returns>Handle of the created tween data.</returns>
        public static  TweenHandle<UnsafeString, StringTweenPlugin> BindToText(
            this TweenFromTo<string, StringTweenPlugin> fromTo, TMP_Text text)
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) =>
            {
                var span = x.AsSpan();
                var length = span.Length;
                var buffer = ArrayPool<char>.Shared.Rent(length);
                span.CopyTo(buffer.AsSpan(0, length));
                target.SetText(buffer, 0, length);
                ArrayPool<char>.Shared.Return(buffer);
            });
        }

        /// <summary>
        /// Create a tween data and bind it to TMP_Text.text.
        /// </summary>
        /// <remarks>
        /// Note: This extension method uses TMP_Text.SetText() to achieve zero allocation, so it is recommended to use this method when binding to text.
        /// </remarks>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToText<TPlugin>(this TweenFromTo<int, TPlugin> fromTo,
            TMP_Text text)
            where TPlugin : unmanaged, ITweenPlugin<int>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) =>
            {
                var buffer = ArrayPool<char>.Shared.Rent(128);
                var bufferOffset = 0;
                Utf16StringHelper.WriteInt32(ref buffer, ref bufferOffset, x);
                target.SetText(buffer, 0, bufferOffset);
                ArrayPool<char>.Shared.Return(buffer);
            });
        }

        /// <summary>
        /// Create a tween data and bind it to TMP_Text.text.
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <param name="format">Format string</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<int, TPlugin> BindToText<TPlugin>(this TweenFromTo<int, TPlugin> fromTo,
            TMP_Text text, string format)
            where TPlugin : unmanaged, ITweenPlugin<int>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, format, static (target, format, x) =>
            {
#if RAYTWEEN_SUPPORT_ZSTRING
                target.SetTextFormat(format, x);
#else
                target.text = string.Format(format, x);
#endif
            });
        }


        /// <summary>
        /// Create a tween data and bind it to TMP_Text.text.
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <param name="format">Format string</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<long, TPlugin> BindToText<TPlugin>(this TweenFromTo<long, TPlugin> fromTo,
            TMP_Text text, string format)
            where TPlugin : unmanaged, ITweenPlugin<long>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, format, static (target, format, x) =>
            {
#if RAYTWEEN_SUPPORT_ZSTRING
                target.SetTextFormat(format, x);
#else
                target.text = string.Format(format, x);
#endif
            });
        }

        /// <summary>
        /// Create a tween data and bind it to TMP_Text.text.
        /// </summary>
        /// <remarks>
        /// Note: This extension method uses TMP_Text.SetText() to achieve zero allocation, so it is recommended to use this method when binding to text.
        /// </remarks>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToText<TPlugin>(this TweenFromTo<float, TPlugin> fromTo,
            TMP_Text text)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            const string format = "{0}";
            Error.IsNull(text);
            return fromTo.Bind(text, static (target, x) =>
            {
#if RAYTWEEN_SUPPORT_ZSTRING
                target.SetTextFormat(format, x);
#else
                target.SetText(format, x);
#endif
            });
        }

        /// <summary>
        /// Create a tween data and bind it to TMP_Text.text.
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <param name="format">Format string</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToText<TPlugin>(this TweenFromTo<float, TPlugin> fromTo,
            TMP_Text text, string format)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(text);
            return fromTo.Bind(text, format, static (target, format, x) =>
            {
#if RAYTWEEN_SUPPORT_ZSTRING
                target.SetTextFormat(format, x);
#else
                target.text = string.Format(format, x);
#endif
            });
        }


        /// <summary>
        /// Create tween data and bind it to the character color.
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">This handle</param>
        /// <param name="text">Target TMP_Text</param>
        /// <param name="charIndex">Target character index</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Color, TPlugin> BindToTMPCharColor<TPlugin>(
            this TweenFromTo<Color, TPlugin> fromTo, TMP_Text text, int charIndex)
            where TPlugin : unmanaged, ITweenPlugin<Color>
        {
            Error.IsNull(text);
            var animator = TextMeshProTweenAnimator.Get(text);
            animator.EnsureCapacity(charIndex + 1);
            var handle = fromTo.Bind(animator, ReadOnlyIntBox.Create(charIndex),
                static (animator, charIndex, x) => { animator.charInfoArray[charIndex.Value].color = x; });
            animator.SetOnDispose(handle);
            return handle;
        }
        //
        // /// <summary>
        // /// Create tween data and bind it to the character color.r.
        // /// </summary>
        // /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        // /// <param name="fromTo">From To Duration</param>
        // /// <param name="text">Target TMP_Text</param>
        // /// <param name="charIndex">Target character index</param>
        // /// <returns>Handle of the created tween data.</returns>
        // public static TweenHandle<float, TPlugin> BindToTMPCharColorR<TPlugin>(
        //     this TweenFromTo<float, TPlugin> fromTo, TMP_Text text, int charIndex)
        //     where TPlugin : unmanaged, ITweenPlugin<float>
        // {
        //     Error.IsNull(text);
        //     var animator = TextMeshProTweenAnimator.Get(text);
        //     animator.EnsureCapacity(charIndex + 1);
        //     var handle = fromTo.Bind(animator, ReadOnlyIntBox.Create(charIndex),
        //         static (animator, charIndex, x) => { animator.charInfoArray[charIndex.Value].color.r = x; });
        //     animator.SetOnDispose(handle);
        //     return handle;
        // }
        //
        // /// <summary>
        // /// Create tween data and bind it to the character color.g.
        // /// </summary>
        // /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        // /// <param name="fromTo">This handle</param>
        // /// <param name="text">Target TMP_Text</param>
        // /// <param name="charIndex">Target character index</param>
        // /// <returns>Handle of the created tween data.</returns>
        // public static TweenHandle<float, TPlugin> BindToTMPCharColorG<TPlugin>(
        //     this TweenFromTo<float, TPlugin> fromTo, TMP_Text text, int charIndex)
        //     where TPlugin : unmanaged, ITweenPlugin<float>
        // {
        //     Error.IsNull(text);
        //     var animator = TextMeshProTweenAnimator.Get(text);
        //     animator.EnsureCapacity(charIndex + 1);
        //     var handle = fromTo.Bind(animator, ReadOnlyIntBox.Create(charIndex),
        //         static (animator, charIndex, x) => { animator.charInfoArray[charIndex.Value].color.g = x; });
        //     animator.SetOnDispose(handle);
        //     return handle;
        // }
        //
        // /// <summary>
        // /// Create tween data and bind it to the character color.b.
        // /// </summary>
        // /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        // /// <param name="fromTo">From To Duration</param>
        // /// <param name="text">Target TMP_Text</param>
        // /// <param name="charIndex">Target character index</param>
        // /// <returns>Handle of the created tween data.</returns>
        // public static TweenHandle<float, TPlugin> BindToTMPCharColorB<TPlugin>(
        //     this TweenFromTo<float, TPlugin> fromTo, TMP_Text text, int charIndex)
        //     where TPlugin : unmanaged, ITweenPlugin<float>
        // {
        //     Error.IsNull(text);
        //     var animator = TextMeshProTweenAnimator.Get(text);
        //     animator.EnsureCapacity(charIndex + 1);
        //     var handle = fromTo.Bind(animator, ReadOnlyIntBox.Create(charIndex),
        //         static (animator, charIndex, x) => { animator.charInfoArray[charIndex.Value].color.b = x; });
        //     animator.SetOnDispose(handle);
        //     return handle;
        // }
        //
        // /// <summary>
        // /// Create tween data and bind it to the character color.a.
        // /// </summary>
        // /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        // /// <param name="fromTo">From To Duration</param>
        // /// <param name="text">Target TMP_Text</param>
        // /// <param name="charIndex">Target character index</param>
        // /// <returns>Handle of the created tween data.</returns>
        // public static TweenHandle<float, TPlugin> BindToTMPCharColorA<TPlugin>(
        //     this TweenFromTo<float, TPlugin> fromTo, TMP_Text text, int charIndex)
        //     where TPlugin : unmanaged, ITweenPlugin<float>
        // {
        //     Error.IsNull(text);
        //     var animator = TextMeshProTweenAnimator.Get(text);
        //     animator.EnsureCapacity(charIndex + 1);
        //     var handle = fromTo.Bind(animator, ReadOnlyIntBox.Create(charIndex),
        //         static (animator, charIndex, x) => { animator.charInfoArray[charIndex.Value].color.a = x; });
        //     animator.SetOnDispose(handle);
        //     return handle;
        // }

        /// <summary>
        /// Create tween data and bind it to the character position.
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">This handle</param>
        /// <param name="text">Target TMP_Text</param>
        /// <param name="charIndex">Target character index</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector3, TPlugin> BindToTMPCharPosition<TPlugin>(
            this TweenFromTo<Vector3, TPlugin> fromTo, TMP_Text text, int charIndex)
            where TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            Error.IsNull(text);
            var animator = TextMeshProTweenAnimator.Get(text);
            animator.EnsureCapacity(charIndex + 1);
            var handle = fromTo.Bind(animator, ReadOnlyIntBox.Create(charIndex),
                static (animator, charIndex, x) => { animator.charInfoArray[charIndex.Value].position = x; });
            animator.SetOnDispose(handle);
            return handle;
        }
        /// <summary>
        /// Create tween data and bind it to the character position.
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="tweenTo">This handle</param>
        /// <param name="text">Target TMP_Text</param>
        /// <param name="charIndex">Target character index</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector3, TPlugin> BindToCharPosition<TPlugin>(
            this TweenTo<Vector3, TPlugin> tweenTo, TMP_Text text, int charIndex)
            where TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            return new TweenFromTo<Vector3, TPlugin>(default, tweenTo).BindToTMPCharPosition(text, charIndex);
        }

        /// <summary>
        /// Create tween data and bind it to the character rotation.
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <param name="charIndex">Target character index</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Quaternion, TPlugin> BindToTMPCharRotation<TPlugin>(
            this TweenFromTo<Quaternion, TPlugin> fromTo, TMP_Text text, int charIndex)
            where TPlugin : unmanaged, ITweenPlugin<Quaternion>
        {
            Error.IsNull(text);
            var animator = TextMeshProTweenAnimator.Get(text);
            animator.EnsureCapacity(charIndex + 1);
            var handle = fromTo.Bind(animator, ReadOnlyIntBox.Create(charIndex),
                static (animator, charIndex, x) => { animator.charInfoArray[charIndex.Value].rotation = x; });
            animator.SetOnDispose(handle);
            return handle;
        }

        /// <summary>
        /// Create tween data and bind it to the character rotation (using euler angles).
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <param name="charIndex">Target character index</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector3, TPlugin> BindToTMPCharEulerAngles<TPlugin>(
            this TweenFromTo<Vector3, TPlugin> fromTo, TMP_Text text, int charIndex)
            where TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            Error.IsNull(text);
            var animator = TextMeshProTweenAnimator.Get(text);
            animator.EnsureCapacity(charIndex + 1);
            var handle = fromTo.Bind(animator, ReadOnlyIntBox.Create(charIndex),
                static (animator, charIndex, x) =>
                {
                    animator.charInfoArray[charIndex.Value].rotation = Quaternion.Euler(x);
                });
            animator.SetOnDispose(handle);
            return handle;
        }

        /// <summary>
        /// Create tween data and bind it to the character scale.
        /// </summary>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="fromTo">From To Duration</param>
        /// <param name="text">Target TMP_Text</param>
        /// <param name="charIndex">Target character index</param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<Vector3, TPlugin> BindToTMPCharScale<TPlugin>(
            this TweenFromTo<Vector3, TPlugin> fromTo, TMP_Text text, int charIndex)
            where TPlugin : unmanaged, ITweenPlugin<Vector3>
        {
            Error.IsNull(text);
            var animator = TextMeshProTweenAnimator.Get(text);
            animator.EnsureCapacity(charIndex + 1);
            var handle = fromTo.Bind(animator, ReadOnlyIntBox.Create(charIndex),
                static (animator, charIndex, x) => { animator.charInfoArray[charIndex.Value].scale = x; },
                RelativeMode.AbsoluteScale);
            animator.SetOnDispose(handle);
            return handle;
        }
    }
}
#endif