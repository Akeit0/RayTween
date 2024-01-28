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
            VibrationHelper.EvaluateStrength(endValue, Options.Frequency, Options.DampingRatio, progress, out var s);
            float multipliar;
            if (Options.RandomState.state == 0)
            {
                multipliar = SharedRandom.Random.NextFloat(-1f, 1f);
            }
            else
            {
                multipliar = Options.RandomState.NextFloat(-1f, 1f);
            }
            return startValue + s * multipliar;
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
            VibrationHelper.EvaluateStrength(endValue, Options.Frequency, Options.DampingRatio, progress, out var s);
            Vector2 multipliar;
            if (Options.RandomState.state == 0)
            {
                multipliar = new Vector2(SharedRandom.Random.NextFloat(-1f, 1f), SharedRandom.Random.NextFloat(-1f, 1f));
            }
            else
            {
                multipliar = new Vector2(Options.RandomState.NextFloat(-1f, 1f), Options.RandomState.NextFloat(-1f, 1f));
            }
            return startValue + new Vector2(s.x * multipliar.x, s.y * multipliar.y);
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
            VibrationHelper.EvaluateStrength(endValue, Options.Frequency, Options.DampingRatio, progress, out var s);
            Vector3 multipliar;
            if (Options.RandomState.state == 0)
            {
                multipliar = new Vector3(SharedRandom.Random.NextFloat(-1f, 1f), SharedRandom.Random.NextFloat(-1f, 1f), SharedRandom.Random.NextFloat(-1f, 1f));
            }
            else
            {
                multipliar = new Vector3(Options.RandomState.NextFloat(-1f, 1f), Options.RandomState.NextFloat(-1f, 1f), Options.RandomState.NextFloat(-1f, 1f));
            }
            return startValue + new Vector3(s.x * multipliar.x, s.y * multipliar.y, s.z * multipliar.z);
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