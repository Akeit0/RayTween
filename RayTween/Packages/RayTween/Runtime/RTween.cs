using UnityEngine;
using System;
using RayTween.Plugins;

namespace RayTween
{
    public static partial class RTween
    {
        public static TweenHandle<float, FloatTweenPlugin> DelayedCall(float seconds, Action action)
        {
            return Create(0f, 0f, seconds).BindNothing().OnComplete(action);
        }

        public static TweenHandle<float, FloatTweenPlugin> DelayedCall<TTarget>(float seconds, TTarget target,
            Action<TTarget> action) where TTarget : class
        {
            return Create(0f, 0f, seconds).BindNothing().OnComplete(target, action);
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
       public static TweenFromTo<Vector3,Path3DTweenPlugin> CreatePath3D(float duration) =>
        new TweenFromTo<Vector3, Path3DTweenPlugin>(Vector3.zero, Vector3.one, duration);

        public static TweenFromTo<Vector3,Path3DTweenPlugin> CreatePath3D(Vector3 offset,Vector3 scale,float duration) =>
            new TweenFromTo<Vector3, Path3DTweenPlugin>(offset, scale,duration);
        
        public static TweenHandle<Vector3,Path3DTweenPlugin> WithPath(this TweenHandle<Vector3,Path3DTweenPlugin> handle,ReadOnlySpan<Vector3> points)
        {
            if (handle.IsIdling)
            {
                ref  var plugin = ref handle.Data.Plugin;
                plugin.SetPath(points);
            }
            return handle;
        }
    }
}