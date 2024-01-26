using Unity.Jobs;
using UnityEngine;
using RayTween;
using RayTween.Plugins;

[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<float, FloatPunchTweenPlugin>))]
[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<Vector2, Vector2PunchTweenPlugin>))]
[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<Vector3, Vector3PunchTweenPlugin>))]

namespace RayTween.Plugins
{
    // Note: Punch tween uses startValue as offset and endValue as vibration strength.

    public  struct FloatPunchTweenPlugin : ITweenPlugin<float, PunchOptions>
    {
        public PunchOptions Options;
        public float Evaluate(ref float startValue, ref float endValue,  float progress)
        {
            VibrationHelper.EvaluateStrength(endValue, Options.Frequency, Options.DampingRatio, progress, out var result);
            return startValue + result;
        }
        public void Init()
        {
            Options =  PunchOptions.Default;
        }
        public void SetOptions(PunchOptions options)
        {
            Options = options;
        }
    }

    public  struct Vector2PunchTweenPlugin : ITweenPlugin<Vector2, PunchOptions>
    {
        public PunchOptions Options;
        public Vector2 Evaluate(ref Vector2 startValue, ref Vector2 endValue,  float progress)
        {
            VibrationHelper.EvaluateStrength(endValue, Options.Frequency, Options.DampingRatio, progress, out var result);
            return startValue + result;
        } public void Init()
        {
            Options =  PunchOptions.Default;
        }
        public void SetOptions(PunchOptions options)
        {
            Options = options;
        }
    }

    public  struct Vector3PunchTweenPlugin : ITweenPlugin<Vector3, PunchOptions>
    {
        public PunchOptions Options;
        public Vector3 Evaluate(ref Vector3 startValue, ref Vector3 endValue,  float progress)
        {
            VibrationHelper.EvaluateStrength(endValue, Options.Frequency, Options.DampingRatio, progress, out var result);
            return startValue + result;
        } public void Init()
        {
            Options =  PunchOptions.Default;
        }
        public void SetOptions(PunchOptions options)
        {
            Options = options;
        }
    }
}