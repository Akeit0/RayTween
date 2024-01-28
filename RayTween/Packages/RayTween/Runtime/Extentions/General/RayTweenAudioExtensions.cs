#if RAYTWEEN_SUPPORT_UNITY_AUDIO
using RayTween.Internal;
using UnityEngine;
using UnityEngine.Audio;

namespace RayTween.Extensions
{
    /// <summary>
    /// Provides binding extension methods for AudioSource and AudioMixer.
    /// </summary>
    public static class RayTweenAudioExtensions
    {
        /// <summary>
        /// Create a tween data and bind it to AudioSource.volume
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="audioSource"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToVolume<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, AudioSource audioSource)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(audioSource);
            return fromTo.Bind(audioSource, static (target, x) =>
            {
                target.volume = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to AudioSource.pitch
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="audioSource"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToPitch<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, AudioSource audioSource)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(audioSource);
            return fromTo.Bind(audioSource, static (target, x) =>
            {
                target.pitch = x;
            });
        }

        /// <summary>
        /// Create a tween data and bind it to AudioMixer exposed parameter.
        /// </summary>
        /// <typeparam name="TPlugin">The type of special parameters given to the tween data</typeparam>
        /// <param name="fromTo">This fromTo</param>
        /// <param name="audioMixer"></param>
        /// <param name="name"></param>
        /// <returns>Handle of the created tween data.</returns>
        public static TweenHandle<float, TPlugin> BindToAudioMixerFloat<TPlugin>(this TweenFromTo<float, TPlugin> fromTo, AudioMixer audioMixer, string name)
            where TPlugin : unmanaged, ITweenPlugin<float>
        {
            Error.IsNull(audioMixer);
            return fromTo.Bind(audioMixer, name, static (target,n,x) =>
            {
                target.SetFloat(n, x);
            });
        }
    }
}
#endif
