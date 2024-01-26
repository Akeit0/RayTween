using Unity.Jobs;
using Unity.Mathematics;
using RayTween;
using RayTween.Plugins;

[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<float,FloatTweenPlugin>))]
[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<double, DoubleTweenPlugin>))]
[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<int, IntTweenPlugin>))]
[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<long, LongTweenPlugin>))]

namespace RayTween.Plugins
{
    public readonly struct FloatTweenPlugin : IRelativeTweenPlugin<float>
    {
        public float Evaluate(ref float startValue, ref float endValue, float progress)
        {
            return math.lerp(startValue, endValue,progress);
        }
        public void Init(){}

        public RelativeModeApplier<float> RelativeModeApplier => (mode, from,to) =>
        {
            return mode switch
            {
                RelativeMode.RelativeValue => from + to,
                RelativeMode.RelativeScale => from * to,
                _ => to
            };
        };
    }

    public readonly struct DoubleTweenPlugin : IRelativeTweenPlugin<double>
    {
        public double Evaluate(ref double startValue, ref double endValue, float progress)
        {
            return math.lerp(startValue, endValue, progress);
        }
        public void Init(){}
        public  RelativeModeApplier<double> RelativeModeApplier => (mode, from,to) =>
        {
            return mode switch
            {
                RelativeMode.RelativeValue => from + to,
                RelativeMode.RelativeScale => from * to,
                _ => to
            };
        };
    }

    public  struct IntTweenPlugin : IRelativeTweenPlugin<int, IntegerOptions>
    {
        public IntegerOptions Options;

        public int Evaluate(ref int startValue, ref int endValue, float progress)
        {
            var value = math.lerp(startValue, endValue, progress);

            return Options.RoundingMode switch
            {
                RoundingMode.AwayFromZero => value >= 0f ? (int)math.ceil(value) : (int)math.floor(value),
                RoundingMode.ToZero => (int)math.trunc(value),
                RoundingMode.ToPositiveInfinity => (int)math.ceil(value),
                RoundingMode.ToNegativeInfinity => (int)math.floor(value),
                _ => (int)math.round(value),
            };
        }

        public  RelativeModeApplier<int> RelativeModeApplier => (mode, from,to) =>
        {
            return mode switch
            {
                RelativeMode.RelativeValue => from + to,
                RelativeMode.RelativeScale => from * to,
                _ => to
            };
        };
        
        public void SetOptions(IntegerOptions options)
        {
            Options = options;
        }

        public void Init(){}
    }
    public  struct LongTweenPlugin : IRelativeTweenPlugin<long, IntegerOptions>
    {
        public IntegerOptions Options;
        public long Evaluate(ref long startValue, ref long endValue, float progress)
        {
            var value = math.lerp((double)startValue, endValue, progress);

            return Options.RoundingMode switch
            {
                RoundingMode.AwayFromZero => value >= 0f ? (long)math.ceil(value) : (long)math.floor(value),
                RoundingMode.ToZero => (long)math.trunc(value),
                RoundingMode.ToPositiveInfinity => (long)math.ceil(value),
                RoundingMode.ToNegativeInfinity => (long)math.floor(value),
                _ => (long)math.round(value),
            };
        }  
        public  RelativeModeApplier<long> RelativeModeApplier => (mode, from,to) =>
        {
            return mode switch
            {
                RelativeMode.RelativeValue => from + to,
                RelativeMode.RelativeScale => from * to,
                _ => to
            };
        };

        public void SetOptions(IntegerOptions options)
        {
            Options = options;
        }
        public void Init(){}
    }
}