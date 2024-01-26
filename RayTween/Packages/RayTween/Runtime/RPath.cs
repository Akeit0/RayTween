using System;
using RayTween.Plugins;
using UnityEngine;

namespace RayTween
{
    
    
    public static class RPath
    {  public static TweenFromTo<Vector3,Path3DTweenPlugin> Create3D(float duration) =>
            new TweenFromTo<Vector3, Path3DTweenPlugin>(Vector3.zero, Vector3.one, duration);

        public static TweenFromTo<Vector3,Path3DTweenPlugin> Create(Vector3 offset,Vector3 scale,float duration) =>
            new TweenFromTo<Vector3, Path3DTweenPlugin>(offset, scale,duration);
        
        public static TweenHandle<Vector3,Path3DTweenPlugin> WithPath(this TweenHandle<Vector3,Path3DTweenPlugin> handle,ReadOnlySpan<Vector3> points)
        {
            if (handle.IsIdling)
            {
              ref   var Plugin = ref handle.Data.Plugin;
              Plugin.SetPath(points);
            }
            return handle;
        }
    }
}