using UnityEngine;
using System;
using RayTween.Plugins;

namespace RayTween
{
    public class RPunch
    {
        
      
            public static TweenFromTo<float,  FloatPunchTweenPlugin>
                Create(float from, float strength, float duration) =>
                new (from, strength, duration);

            public static TweenFromTo<Vector2,  Vector2PunchTweenPlugin>
                Create(Vector2 from, Vector2 strength, float duration) =>
                new (from, strength, duration);


            public static TweenTo<Vector3,  Vector3PunchTweenPlugin>
                Create(Vector3 strength, float duration) =>
                new(strength, duration);
            public static TweenTo<float,  FloatPunchTweenPlugin>
                Create(float strength, float duration) =>
                new (strength, duration);

            public static TweenTo<Vector2,  Vector2PunchTweenPlugin>
                Create( Vector2 strength, float duration) =>
                new (strength, duration);


            public static TweenFromTo<Vector3,  Vector3PunchTweenPlugin>
                Create(Vector3 from, Vector3 strength, float duration) =>
                new(from, strength, duration);
            
            

            public static TweenHandle<Vector2, Vector2PunchTweenPlugin> Create<TTarget>(TTarget target,
                Action<TTarget, Vector2> setter, Vector2 from, Vector2 strength, float duration) where TTarget : class
            {
                return TweenHandle<Vector2, Vector2PunchTweenPlugin>.Create(target, setter, from, strength, duration);
            }
            public static TweenHandle<Vector2, Vector2PunchTweenPlugin> Create<TTarget1, TTarget2>(TTarget1 target1,
                TTarget2 target2, Action<TTarget1, TTarget2, Vector2> setter, Vector2 from, Vector2 strength, float duration)
                where TTarget1 : class where TTarget2 : class
            {
                return TweenHandle<Vector2,Vector2PunchTweenPlugin>.Create(target1, target2, setter, from, strength, duration);
            }
    }
}