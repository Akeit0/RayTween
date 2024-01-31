using System.Runtime.InteropServices;

namespace RayTween
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TweenCommonData
    {
        public TweenStatus Status;
        public double StartTime;
        public float Duration;
        public Ease Ease;
        public TweenTimeKind TimeKind;
        public float Delay;
        public int Loops;
        public DelayType DelayType;
        public LoopType LoopType;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct TweenCustomData<TValue, TPlugin> where TValue : unmanaged
        where TPlugin : ITweenPlugin<TValue>
    {
        public TValue StartValue;
        public TValue EndValue;
        public TPlugin Options;
    }
    /// <summary>
    /// A structure representing Tween data.
    /// </summary>
    /// <typeparam name="TValue">The type of value to animate</typeparam>
    /// <typeparam name="TPlugin">The type of special parameters given to the Tween data</typeparam>
    [StructLayout(LayoutKind.Sequential)]
    public struct TweenData<TValue, TPlugin> where TValue : unmanaged
        where TPlugin : unmanaged, ITweenPlugin<TValue>
    {
        public static bool HasDisposeImplementation = default(TPlugin).HasDisposeImplementation;

        static TweenData()
        {
            var Plugin = default(TPlugin);
            if(Plugin is IRelativeTweenPlugin<TValue> relativeTweenPlugin)
            {
                RelativeModeApplier = relativeTweenPlugin.RelativeModeApplier;
            }
        }
        
        public static readonly RelativeModeApplier<TValue> RelativeModeApplier;

        public TweenStatus Status;
        public double Time;
        public float TweenSpeed;
        public float Duration;
        public Ease Ease;
        public TweenTimeKind TimeKind;
        public float Delay;
        public int Loops;
        public DelayType DelayType;
        public LoopType LoopType;
        public TValue StartValue;
        public TValue EndValue;
        public TPlugin Plugin;
        
        public TValue Evaluate(float progress)
        {
            return Plugin.Evaluate(ref StartValue, ref EndValue, progress);
        }
        public void Dispose()
        {
            Plugin.Dispose(ref StartValue, ref EndValue);
        }
    }
}