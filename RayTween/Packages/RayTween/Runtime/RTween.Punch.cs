using UnityEngine;
using System;
using System.Diagnostics.CodeAnalysis;
using RayTween.Plugins;

namespace RayTween
{
    public static partial class RTween
    {
        [SuppressMessage("ReSharper", "MemberHidesStaticFromOuterClass")]
        public static class Punch
        {
            public static TweenFromTo<float, FloatPunchTweenPlugin>
                Create(float from, float strength, float duration) =>
                new(from, strength, duration);

            public static TweenFromTo<Vector2, Vector2PunchTweenPlugin>
                Create(Vector2 from, Vector2 strength, float duration) =>
                new(from, strength, duration);
            public static TweenFromTo<Vector3, Vector3PunchTweenPlugin>
                Create(Vector3 from, Vector3 strength, float duration) =>
                new(from, strength, duration);

           

            public static TweenTo<float, FloatPunchTweenPlugin>
                Create(float strength, float duration) =>
                new(strength, duration);

            public static TweenTo<Vector2, Vector2PunchTweenPlugin>
                Create(Vector2 strength, float duration) =>
                new(strength, duration);

            public static TweenTo<Vector3, Vector3PunchTweenPlugin>
                Create(Vector3 strength, float duration) =>
                new(strength, duration);
            
        }
    }
}