using UnityEngine;
using System;
using RayTween.Plugins;

namespace RayTween
{
    public static class RTween
    {
        public static TweenHandle<float, FloatTweenPlugin> DelayedCall(float seconds, Action action)
        {
            return Create(0f, 0f, seconds).NoBind().OnComplete(action);
        }

        public static TweenHandle<float, FloatTweenPlugin> DelayedCall<TTarget>(float seconds, TTarget target,
            Action<TTarget> action) where TTarget : class
        {
            return Create(0f, 0f, seconds).NoBind().OnComplete(target, action);
        }

        public static TweenFromTo<float, FloatTweenPlugin>
            Create(float from, float to, float duration) =>
            new(from, to, duration);

        public static TweenFromTo<Vector3, Vector3TweenPlugin>
            Create(Vector3 from, Vector3 to, float duration) =>
            new(from, to, duration);

        public static TweenFromTo<Color, ColorTweenPlugin>
            Create(Color from, Color to, float duration) =>
            new(from, to, duration);

        public static TweenFromTo<int, IntTweenPlugin>
            Create(int from, int to, float duration) =>
            new(from, to, duration);

        public static TweenFromTo<long, LongTweenPlugin>
            Create(long from, long to, float duration) =>
            new(from, to, duration);
        public static TweenTo<float, FloatTweenPlugin>
            Create( float to, float duration) =>
            new(to, duration);

        public static TweenTo<Vector3, Vector3TweenPlugin>
            Create(Vector3 to, float duration) =>
            new( to, duration);

        public static TweenTo<Color, ColorTweenPlugin>
            Create(Color to, float duration) =>
            new( to, duration);

        public static TweenTo<int, IntTweenPlugin>
            Create(int to, float duration) =>
            new( to, duration);

        public static TweenTo<long, LongTweenPlugin>
            Create( long to, float duration) =>
            new(to, duration);
        public static TweenFromTo<string, StringTweenPlugin> Create(string from, string to, float duration) =>
            new(from, to, duration);

        // public static TweenHandle<TValue, TPlugin> To<TValue, TPlugin, TTarget>(TTarget target,
        //     Func<TTarget, TValue> getter, Action<TTarget, TValue> setter, TValue to, float duration,
        //     RelativeMode relativeMode = RelativeMode.AbsoluteValue) where TValue : unmanaged
        //     where TPlugin : unmanaged, ITweenPlugin<TValue>
        //     where TTarget : class
        // {
        //     return TweenHandle<TValue, TPlugin>.Create(target, getter, setter, to, duration, relativeMode);
        // }
        //
        // public static TweenHandle<TValue, TPlugin> FromTo<TValue, TPlugin, TTarget>(TTarget target,
        //     Action<TTarget, TValue> setter, TValue from, TValue to, float duration,
        //     RelativeMode relativeMode = RelativeMode.AbsoluteValue) where TValue : unmanaged
        //     where TPlugin : unmanaged, ITweenPlugin<TValue>
        //     where TTarget : class
        // {
        //     return TweenHandle<TValue, TPlugin>.Create(target, setter, from, to, duration, relativeMode);
        // }
        //
        // public static TweenHandle<TValue, TPlugin> FromTo<TValue, TPlugin, TTarget1, TTarget2>(TTarget1 target1,
        //     TTarget2 target2, Action<TTarget1, TTarget2, TValue> setter, TValue from, TValue to, float duration,
        //     RelativeMode relativeMode = RelativeMode.AbsoluteValue) where TValue : unmanaged
        //     where TPlugin : unmanaged, ITweenPlugin<TValue>
        //     where TTarget1 : class
        //     where TTarget2 : class
        // {
        //     return TweenHandle<TValue, TPlugin>.Create(target1, target2, setter, from, to, duration, relativeMode);
        // }
        //
        // public static TweenHandle<float, FloatTweenPlugin> To<TTarget>(TTarget target, Func<TTarget, float> getter,
        //     Action<TTarget, float> setter, float to, float duration,
        //     RelativeMode relativeMode = RelativeMode.AbsoluteValue) where TTarget : class
        // {
        //     return TweenHandle<float, FloatTweenPlugin>.Create(target, getter, setter, to, duration, relativeMode);
        // }
        //
        // public static TweenHandle<float, FloatTweenPlugin> FromTo<TTarget>(TTarget target,
        //     Action<TTarget, float> setter, float from, float to, float duration) where TTarget : class
        // {
        //     return TweenHandle<float, FloatTweenPlugin>.Create(target, setter, from, to, duration);
        // }
        //
        // public static TweenHandle<float, FloatTweenPlugin> FromTo<TTarget1, TTarget2>(TTarget1 target1,
        //     TTarget2 target2, Action<TTarget1, TTarget2, float> setter, float from, float to, float duration)
        //     where TTarget1 : class where TTarget2 : class
        // {
        //     return TweenHandle<float, FloatTweenPlugin>.Create(target1, target2, setter, from, to, duration);
        // }
        // public static TweenHandle<Vector2, Vector2TweenPlugin> To<TTarget>(TTarget target, Func<TTarget, Vector2> getter,
        //     Action<TTarget, Vector2> setter, Vector2 to, float duration,
        //     RelativeMode relativeMode = RelativeMode.AbsoluteValue) where TTarget : class
        // {
        //     return TweenHandle<Vector2, Vector2TweenPlugin>.Create(target, getter, setter, to, duration, relativeMode);
        // }
        //
        // public static TweenHandle<Vector2, Vector2TweenPlugin> FromTo<TTarget>(TTarget target,
        //     Action<TTarget, Vector2> setter, Vector2 from, Vector2 to, float duration) where TTarget : class
        // {
        //     return TweenHandle<Vector2, Vector2TweenPlugin>.Create(target, setter, from, to, duration);
        // }
        //
        // public static TweenHandle<Vector2, Vector2TweenPlugin> FromTo<TTarget1, TTarget2>(TTarget1 target1,
        //     TTarget2 target2, Action<TTarget1, TTarget2, Vector2> setter, Vector2 from, Vector2 to, float duration)
        //     where TTarget1 : class where TTarget2 : class
        // {
        //     return TweenHandle<Vector2,Vector2TweenPlugin>.Create(target1, target2, setter, from, to, duration);
        // }
        // public static TweenHandle<Vector3, Vector3TweenPlugin> To<TTarget>(TTarget target, Func<TTarget, Vector3> getter,
        //     Action<TTarget, Vector3> setter, Vector3 to, float duration,
        //     RelativeMode relativeMode = RelativeMode.AbsoluteValue) where TTarget : class
        // {
        //     return TweenHandle<Vector3, Vector3TweenPlugin>.Create(target, getter, setter, to, duration, relativeMode);
        // }
        //
        // public static TweenHandle<Vector3, Vector3TweenPlugin> FromTo<TTarget>(TTarget target,
        //     Action<TTarget, Vector3> setter, Vector3 from, Vector3 to, float duration) where TTarget : class
        // {
        //     return TweenHandle<Vector3, Vector3TweenPlugin>.Create(target, setter, from, to, duration);
        // }
        //
        // public static TweenHandle<Vector3, Vector3TweenPlugin> FromTo<TTarget1, TTarget2>(TTarget1 target1,
        //     TTarget2 target2, Action<TTarget1, TTarget2, Vector3> setter, Vector3 from, Vector3 to, float duration)
        //     where TTarget1 : class where TTarget2 : class
        // {
        //     return TweenHandle<Vector3,Vector3TweenPlugin>.Create(target1, target2, setter, from, to, duration);
        // }
    }
}