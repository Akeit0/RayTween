using System;

namespace RayTween
{
    /// <summary>
    /// Type of characters used to fill in invisible strings.
    /// </summary>
    public enum ScrambleMode : byte
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// A-Z
        /// </summary>
        Uppercase = 1,

        /// <summary>
        /// a-z
        /// </summary>
        Lowercase = 2,

        /// <summary>
        /// 0-9
        /// </summary>
        Numerals = 3,

        /// <summary>
        /// A-Z, a-z, 0-9
        /// </summary>
        All = 4,

        /// <summary>
        /// Custom characters.
        /// </summary>
        Custom = 5
    }

    /// <summary>
    /// Options for string type tween.
    /// </summary>
    public  struct StringOptions :IEquatable<StringOptions>
    {
        public string CustomScrambleChars;
        public Unity.Mathematics.Random RandomState;
        public ScrambleMode ScrambleMode;
        public bool RichTextEnabled;
        

        public bool Equals(StringOptions other)
        {
            return CustomScrambleChars == other.CustomScrambleChars && RandomState.Equals(other.RandomState) && ScrambleMode == other.ScrambleMode && RichTextEnabled == other.RichTextEnabled;
        }

        public override bool Equals(object obj)
        {
            return obj is StringOptions other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CustomScrambleChars, RandomState, (int)ScrambleMode, RichTextEnabled);
        }
    }
    
}