using System;

namespace RayTween
{
    /// <summary>
    /// Options for shake tween.
    /// </summary>
    public struct ShakeOptions : IEquatable<ShakeOptions>
    {
        public int Frequency;
        public float DampingRatio;
        public Unity.Mathematics.Random RandomState;

        public static ShakeOptions Default
        {
            get
            {
                return new ShakeOptions()
                {
                    Frequency = 10,
                    DampingRatio = 1f
                };
            }
        }

        public readonly bool Equals(ShakeOptions other)
        {
            return other.Frequency == Frequency &&
                other.DampingRatio == DampingRatio &&
                other.RandomState.state == RandomState.state;
        }

        public readonly override bool Equals(object obj)
        {
            if (obj is ShakeOptions options) return Equals(options);
            return false;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(Frequency, DampingRatio, RandomState);
        }
    }
}