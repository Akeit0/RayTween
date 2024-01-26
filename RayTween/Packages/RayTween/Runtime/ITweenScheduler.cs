using RayTween.Internal;
namespace RayTween
{
    /// <summary>
    /// Provides the function to schedule the execution of a tween.
    /// </summary>
    public interface ITweenScheduler
    {
        /// <summary>
        /// Schedule the tween.
        /// </summary>
        /// <typeparam name="TValue">The type of value to animate</typeparam>
        /// <typeparam name="TOptions">The type of special parameters given to the tween data</typeparam>
        /// <typeparam name="TPlugin">The type of Plugin that support value animation</typeparam>
        /// <param name="handle"></param>
        /// <param name="data">Tween data</param>
        /// <param name="callbackData">Tween callback data</param>
        /// <returns>Tween handle</returns>
        public int Schedule<TValue, TPlugin>(TweenHandle<TValue, TPlugin> handle,
            ref TweenData<TValue, TPlugin> data, ref TweenCallbackData callbackData)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>;

        /// <summary>
        /// Returns the current time.
        /// </summary>
        double Time { get; }
        
        UpdateTiming UpdateTiming { get; }

    }

    /// <summary>
    /// Type of time used to play the tween
    /// </summary>
    public enum TweenTimeKind : byte
    {
        Time = 0,
        UnscaledTime = 1,
        Realtime = 2
    }
}