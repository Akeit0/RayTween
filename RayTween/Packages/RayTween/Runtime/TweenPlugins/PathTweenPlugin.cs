using System;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using RayTween;
using RayTween.Plugins;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<Vector3, Path3DTweenPlugin>))]

namespace RayTween.Plugins
{
    public enum PathType : byte
    {
        Linear,
        CatmullRom
    }

    public struct PathTweenOptions
    {
        public PathType PathType;
        public bool IsClosed;
        public PathTweenOptions(PathType pathType, bool isClosed)
        {
            PathType = pathType;
            IsClosed = isClosed;
        }
    }

    internal unsafe struct Path3DUnsafeOptions
    {
        public Vector3* Points;
        public int Length;
        public PathType PathType;
        public byte IsClosed;
    }

    public unsafe struct Path3DTweenPlugin : ITweenPlugin<Vector3, PathTweenOptions>
    {
        internal Path3DUnsafeOptions Options;

        public Vector3 Evaluate(ref Vector3 offset, ref Vector3 scale, float progress)
        {
            if (Options.Length == 0)
            {
                return offset;
            }

            var length = Options.Length + (Options.IsClosed == 1 ? 1 : 0);
            var pointList = new NativeArray<float3>(length, Allocator.Temp);

            UnsafeUtility.MemCpy((float3*)pointList.GetUnsafePtr(), Options.Points, Options.Length * sizeof(float3));


            if (Options.IsClosed == 1) pointList[^1] = pointList[0];


            float3 currentValue = default;
            switch (Options.PathType)
            {
                case PathType.Linear:
                    CurveUtils.Linear(in pointList, progress, out currentValue);
                    break;
                case PathType.CatmullRom:
                    CurveUtils.CatmullRomSpline(in pointList, progress, out currentValue);
                    break;
            }

            pointList.Dispose();
            return offset + (Vector3)(currentValue * scale);
        }

        public void SetPath(ReadOnlySpan<Vector3> points)
        {
            if (Options.Points != null) UnsafeUtility.Free(Options.Points, Allocator.Persistent);
            Options.Points = (Vector3*)UnsafeUtility.Malloc(points.Length * sizeof(Vector3),
                UnsafeUtility.AlignOf<Vector3>(), Allocator.Persistent);
            Options.Length = points.Length;
            fixed (Vector3* pointsPtr = points)
            {
                UnsafeUtility.MemCpy(Options.Points, pointsPtr, points.Length * sizeof(Vector3));
            }
        }

        public void SetOptions(PathTweenOptions options)
        {
            Options.PathType = options.PathType;
            Options.IsClosed = (byte)(options.IsClosed ? 1 : 0);
        }

        public void Init()
        {
        }
        public bool HasDisposeImplementation => true;
        public void Dispose(ref Vector3 start, ref Vector3 end)
        {
            UnsafeUtility.Free(Options.Points, Allocator.Persistent);
        }

        public void Reverse()
        {
            if (Options.Length <= 1)
            {
                return;
            }

            using var pointList = new NativeArray<float3>(Options.Length, Allocator.Temp);
            var tmpPtr = (float3*)pointList.GetUnsafePtr();
            UnsafeUtility.MemCpy(tmpPtr, Options.Points, Options.Length * sizeof(float3));
            var length = Options.Length;
            for (int i = 0; i < length; i++)
            {
                Options.Points[i] = tmpPtr[length - i];
            }
        }

        internal static class CurveUtils
        {
            public static void CatmullRomSpline(in NativeArray<float3> points, float t, out float3 result)
            {
                int l = points.Length;

                if (l == 0)
                {
                    result = default;
                    return;
                }
                else if (l == 1)
                {
                    result = points[0];
                    return;
                }

                float progress = (l - 1) * t;
                int i = (int)math.floor(progress);
                float weight = progress - i;

                if (Approximately(weight, 0f) && i >= l - 1)
                {
                    i = l - 2;
                    weight = 1;
                }

                float3 p0 = points[i];
                float3 p1 = points[i + 1];

                float3 v0;
                if (i > 0)
                {
                    v0 = 0.5f * (points[i + 1] - points[i - 1]);
                }
                else
                {
                    v0 = points[i + 1] - points[i];
                }

                float3 v1;
                if (i < l - 2)
                {
                    v1 = 0.5f * (points[i + 2] - points[i]);
                }
                else
                {
                    v1 = points[i + 1] - points[i];
                }

                HermiteCurve(p0, p1, v0, v1, weight, out result);
            }

            public static void HermiteCurve(in float3 p0, in float3 p1, in float3 v0, in float3 v1, float t,
                out float3 result)
            {
                float3 c0 = 2f * p0 + -2f * p1 + v0 + v1;
                float3 c1 = -3f * p0 + 3f * p1 + -2f * v0 - v1;
                float3 c2 = v0;
                float3 c3 = p0;

                result = t * t * t * c0 +
                         t * t * c1 +
                         t * c2 +
                         c3;
            }


            public static void Linear(in NativeArray<float3> points, float t, out float3 result)
            {
                int l = points.Length;

                if (l == 0)
                {
                    result = default;
                    return;
                }
                else if (l == 1)
                {
                    result = points[0];
                    return;
                }

                float progress = (l - 1) * t;
                int i = (int)math.floor(progress);
                float weight = progress - i;

                if (Approximately(weight, 0f) && i >= l - 1)
                {
                    i = l - 2;
                    weight = 1;
                }

                result = math.lerp(points[i], points[i + 1], weight);
            }

            public static bool Approximately(float a, float b)
            {
                return math.abs(b - a) < math.max(0.000001f * math.max(math.abs(a), math.abs(b)), math.EPSILON * 8);
            }
        }
    }
}