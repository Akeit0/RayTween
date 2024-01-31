using System;

namespace RayTween
{
    /// <summary>
    /// Options for punch tween.
    /// </summary>
    public struct PunchOptions : IEquatable<PunchOptions>
    {
        public int Frequency;
        public float DampingRatio;

        public static PunchOptions Default
        {
            get
            {
                return new PunchOptions()
                {
                    Frequency = 10,
                    DampingRatio = 1f
                };
            }
        }

        public readonly bool Equals(PunchOptions other)
        {
            return other.Frequency == Frequency && other.DampingRatio == DampingRatio;
        }

        public readonly override bool Equals(object obj)
        {
            if (obj is PunchOptions options) return Equals(options);
            return false;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Frequency, DampingRatio);
        }
    }
}