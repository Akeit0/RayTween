namespace RayTween.Internal
{
    internal struct UshortRange
    {
        public ushort Start;
        public ushort End;
        public UshortRange(ushort start, ushort end)
        {
            Start = start;
            End = end;
        }
    }
    
    internal enum RichTextSymbolType : byte
    {
        Text,
        TagStart,
        TagEnd
    }
    internal readonly struct RichTextSymbol
    {
        public RichTextSymbol(RichTextSymbolType type,  UshortRange range)
        {
            Type = type;
            Range = range;
        }

        public readonly RichTextSymbolType Type;
        public readonly UshortRange Range;
    }

}