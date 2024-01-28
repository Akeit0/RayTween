namespace RayTween
{
    /// <summary>
    /// Tween Plugin.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface ITweenPlugin<TValue> where TValue : unmanaged

    {
        public TValue Evaluate(ref TValue startValue, ref TValue endValue, float progress);
        public bool HasDisposeImplementation => false;

        public void Dispose(ref TValue startValue, ref TValue endValue)
        {
        }

        public void Init();
    }

    public interface ITweenPlugin<TValue, in TOptions> : ITweenPlugin<TValue> where TValue : unmanaged
    {
        public void SetOptions(TOptions options);
    }

    public delegate TValue RelativeModeApplier<TValue>(RelativeMode mode, TValue from, TValue to)
        where TValue : unmanaged;

    public interface IRelativeTweenPlugin<TValue> : ITweenPlugin<TValue> where TValue : unmanaged
    {
        public RelativeModeApplier<TValue> RelativeModeApplier { get; }
    }

    public interface IRelativeTweenPlugin<TValue, in TOptions> : IRelativeTweenPlugin<TValue>,
        ITweenPlugin<TValue> where TValue : unmanaged
    {
    }
}