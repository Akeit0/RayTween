using Unity.Jobs;
using UnityEngine;
using RayTween;
using RayTween.Plugins;

[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<float, FloatShakeTweenPlugin>))]
[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<Vector2, Vector2ShakeTweenPlugin>))]
[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<Vector3, Vector3ShakeTweenPlugin>))]

namespace RayTween.Plugins
{
    // Note: Shake tween uses startValue as offset and endValue as vibration strength.

    public  struct FloatShakeTweenPlugin : ITweenPlugin<float, ShakeOptions>
    {
        public ShakeOptions Options;
        public float Evaluate(ref float startValue, ref float endValue,  float progress)
        {
            VibrationHelper.EvaluateStrength(endValue, Options.Frequency, Options.DampingRatio, progress, out var result);
            return startValue + result;
        }
        public void Init()
        {
            Options =  ShakeOptions.Default;
        }
        public void SetOptions(ShakeOptions options)
        {
            Options = options;
        }
    }

    public  struct Vector2ShakeTweenPlugin : ITweenPlugin<Vector2, ShakeOptions>
    {
        public ShakeOptions Options;
        public Vector2 Evaluate(ref Vector2 startValue, ref Vector2 endValue,  float progress)
        {
            VibrationHelper.EvaluateStrength(endValue, Options.Frequency, Options.DampingRatio, progress, out var result);
            return startValue + result;
        }
        public void Init()
        {
            Options =  ShakeOptions.Default;
        }
        public void SetOptions(ShakeOptions options)
        {
            Options = options;
        }
    }

    public  struct Vector3ShakeTweenPlugin : ITweenPlugin<Vector3, ShakeOptions>
    {
        public ShakeOptions Options;
        public Vector3 Evaluate(ref Vector3 startValue, ref Vector3 endValue,  float progress)
        {
            VibrationHelper.EvaluateStrength(endValue, Options.Frequency, Options.DampingRatio, progress, out var result);
            return startValue + result;
        }
        public void Init()
        {
            Options =  ShakeOptions.Default;
        }
        public void SetOptions(ShakeOptions options)
        {
            Options = options;
        }
    }
}