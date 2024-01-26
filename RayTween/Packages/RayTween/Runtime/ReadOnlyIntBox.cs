namespace RayTween
{
    public class ReadOnlyIntBox
    {
        public readonly int Value;

        public ReadOnlyIntBox(int value)
        {
            Value = value;
        }

        static ReadOnlyIntBox[] sharedBoxes = new ReadOnlyIntBox[sharedSize];
        const int sharedSize = 16;

        public static ReadOnlyIntBox Create(int value)
        {
            if (0 <= value && value < sharedSize)
            {
                ref var shared = ref sharedBoxes[value];
                if (shared == null) shared = new ReadOnlyIntBox(value);
                return shared;
            }

            return new ReadOnlyIntBox(value);
        }
    }
}