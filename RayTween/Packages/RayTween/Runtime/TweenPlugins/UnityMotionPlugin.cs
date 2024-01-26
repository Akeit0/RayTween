using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using RayTween;
using RayTween.Plugins;

[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<Vector2, Vector2TweenPlugin>))]
[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<Vector3, Vector3TweenPlugin>))]
[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<Vector4, Vector4TweenPlugin>))]
[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<Quaternion, QuaternionTweenPlugin>))]
[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<Color, ColorTweenPlugin>))]
[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<Rect, RectTweenPlugin>))]

namespace RayTween.Plugins
{
    public readonly struct Vector2TweenPlugin : IRelativeTweenPlugin<Vector2>
    {

        public Vector2 Evaluate(ref Vector2 startValue, ref Vector2 endValue, float progress)
        {
            return Vector2.LerpUnclamped(startValue, endValue, progress);
        }

        public void Init()
        {
        }

        public  RelativeModeApplier<Vector2> RelativeModeApplier => (mode, from,to) =>
        {
            return mode switch
            {
                RelativeMode.RelativeValue => from + to,
                RelativeMode.RelativeScale => from * to,
                _ => to
            };
        };   }

    public readonly struct Vector3TweenPlugin : IRelativeTweenPlugin<Vector3>
    {
        public Vector3 Evaluate(ref Vector3 startValue, ref Vector3 endValue, float progress)
        {
            return Vector3.LerpUnclamped(startValue, endValue, progress);
        }

        public void Init()
        {
        }
        public  RelativeModeApplier<Vector3> RelativeModeApplier => (mode, from,to) =>
        {
            return mode switch
            {
                RelativeMode.RelativeValue => from + to,
                RelativeMode.RelativeScale => new Vector3(from.x * to.x, from.y * to.y, from.z * to.z ),
                _ => to
            };
        }; 
    }

    public readonly struct Vector4TweenPlugin : IRelativeTweenPlugin<Vector4>
    {
        public Vector4 Evaluate(ref Vector4 startValue, ref Vector4 endValue, float progress)
        {
            return Vector4.LerpUnclamped(startValue, endValue, progress);
        }

        public void Init()
        {
        }
        public  RelativeModeApplier<Vector4> RelativeModeApplier => (mode, from,to) =>
        {
            return mode switch
            {
                RelativeMode.RelativeValue => from + to,
                RelativeMode.RelativeScale => new Vector4(from.x * to.x, from.y * to.y, from.z * to.z , from.w * to.w),
                _ => to
            };
        }; 
    }

    public readonly struct QuaternionTweenPlugin : IRelativeTweenPlugin<Quaternion>
    {
        public Quaternion Evaluate(ref Quaternion startValue, ref Quaternion endValue, float progress)
        {
            return Quaternion.LerpUnclamped(startValue, endValue, progress);
        }

        public void Init()
        {
        }
        public  RelativeModeApplier<Quaternion> RelativeModeApplier => (mode, from,to) =>
        {
            return mode switch
            {
                RelativeMode.RelativeValue => from*to,
                _ => to
            };
        }; 
    }

    public readonly struct ColorTweenPlugin : IRelativeTweenPlugin<Color>
    {
        public Color Evaluate(ref Color startValue, ref Color endValue, float progress)
        {
            return Color.LerpUnclamped(startValue, endValue, progress);
        }

        public void Init()
        {
        }
        public RelativeModeApplier<Color> RelativeModeApplier => (mode, from,to) =>
        {
            return mode switch
            {
                RelativeMode.RelativeValue => from+to,
                RelativeMode.RelativeScale => from*to, 
                _ => to
            };
        }; 
    }

    public readonly struct RectTweenPlugin : IRelativeTweenPlugin<Rect>
    {
        public Rect Evaluate(ref Rect startValue, ref Rect endValue, float progress)
        {
            var x = math.lerp(startValue.x, endValue.x, progress);
            var y = math.lerp(startValue.y, endValue.y, progress);
            var width = math.lerp(startValue.width, endValue.width, progress);
            var height = math.lerp(startValue.height, endValue.height, progress);

            return new Rect(x, y, width, height);
        }

        public void Init()
        {
        }
        public RelativeModeApplier<Rect> RelativeModeApplier => (mode, from,to) =>
        {
            return mode switch
            {
                RelativeMode.RelativeValue => new Rect(from.x+to.x,from.y+to.y,from.width+to.width,from.height+to.height),
                _ => to
            };
        }; 
    }
}