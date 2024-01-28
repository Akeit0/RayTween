using RayTween;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Jobs;
using RayTween.Plugins;
using RayTween.Internal;

[assembly: RegisterGenericJobType(typeof(TweenUpdateJob<UnsafeString, StringTweenPlugin>))]

namespace RayTween.Plugins
{
    public unsafe struct StringTweenPlugin : ITweenPlugin<UnsafeString, StringOptions>
    {
        internal UnsafeString CustomScrambleChars;
        public Random RandomState;
        public ScrambleMode ScrambleMode;
        public bool RichTextEnabled;
        public UnsafeString Buffer;

        public UnsafeString Evaluate(ref UnsafeString startValue, ref UnsafeString endValue, float progress)
        {
            if (!Buffer.IsCreated)
            {
                Buffer.Value =
                    new UnsafeList<ushort>(math.max(startValue.Length, endValue.Length), Allocator.Persistent);
            }

            Buffer.Value.Clear();
            StringHelpers.Interpolate(ref Buffer.Value, ref startValue.Value, ref endValue.Value, progress,
                ScrambleMode, RichTextEnabled, ref RandomState, ref CustomScrambleChars.Value);
            return Buffer;
        }

        public void Dispose(ref UnsafeString start, ref UnsafeString end)
        {
            start.Dispose();
            end.Dispose();
            CustomScrambleChars.Dispose();
            Buffer.Dispose();
        }

        public bool HasDisposeImplementation => true;

        public void SetOptions(StringOptions options)
        {
            CustomScrambleChars.Set(options.CustomScrambleChars);
            RandomState = options.RandomState;
            ScrambleMode = options.ScrambleMode;
            RichTextEnabled = options.RichTextEnabled;
        }

        public void Init()
        {
        }
    }
}