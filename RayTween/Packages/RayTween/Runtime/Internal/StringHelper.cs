using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections.LowLevel.Unsafe;
namespace RayTween.Internal
{
    [BurstCompile]
    internal  static  class StringHelper
    {
        static readonly char[] LowercaseChars = new char[]
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };
        static readonly char[] UppercaseChars = new char[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };
        static readonly char[] NumeralsChars = new char[]
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };
        static readonly char[] AllChars = new char[]
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        static char GetScrambleChar(ScrambleMode scrambleMode, ref Random random)
        {
            return scrambleMode switch
            {
                ScrambleMode.None => default,
                ScrambleMode.Uppercase => UppercaseChars[random.NextInt(0, UppercaseChars.Length)],
                ScrambleMode.Lowercase => LowercaseChars[random.NextInt(0, LowercaseChars.Length)],
                ScrambleMode.Numerals => NumeralsChars[random.NextInt(0, NumeralsChars.Length)],
                ScrambleMode.All => AllChars[random.NextInt(0, AllChars.Length)],
                _ => default
            };
        }
         internal static void Interpolate(ref  UnsafeList<ushort> result,ref UnsafeList<ushort> start, ref UnsafeList<ushort>  end, float t, ScrambleMode scrambleMode, bool richTextEnabled, ref Random randomState, ref UnsafeList<ushort> customScrambleChars)
        {
            if (richTextEnabled)
            {
                var startTextSymbols=GetRichTextSymbols( start,out var startTextLength);
                var endTextSymbols=GetRichTextSymbols( end,out var endTextLength);
                FillRichText(ref start,ref startTextSymbols, ref end,ref endTextSymbols, startTextLength,endTextLength,t, scrambleMode, ref randomState, ref customScrambleChars,ref result);

                startTextSymbols.Dispose();
                endTextSymbols.Dispose();
            }
            else
            {
                FillText( ref start, ref end, t, scrambleMode, ref randomState, ref customScrambleChars,ref result);
            }
        }
        static void FillRichText(
            ref UnsafeList<ushort> start,
            ref UnsafeList<RichTextSymbol> startSymbols,
            ref UnsafeList<ushort> end,
            ref UnsafeList<RichTextSymbol> endSymbols,
            int startTextLength,
            int endTextLength,
            float t,
            ScrambleMode scrambleMode,
            ref Random randomState,
            ref UnsafeList<ushort> customScrambleChars,
            ref UnsafeList<ushort> result)
        {
            var length = math.max(startTextLength, endTextLength);
            var currentTextLength = (int)math.round(length * t);

            var length1 = AppendSliceSymbols(ref result,ref end, endSymbols, 0, currentTextLength);
            var length2 = AppendSliceSymbols(ref result,ref start,  startSymbols, length - 1,currentTextLength + 1);


            FillScrambleChars(ref result, scrambleMode, ref randomState,  customScrambleChars, length - (length1 + length2));
        }
         static void FillText(
            ref UnsafeList<ushort> start,
            ref UnsafeList<ushort> end,
            float t,
            ScrambleMode scrambleMode,
            ref Random randomState,
            ref UnsafeList<ushort> customScrambleChars,
            ref UnsafeList<ushort> result)
        {
            var length = math.max(start.Length, end.Length);
            var currentTextLength = (int)math.round(length * t);
            for (int i = 0; i < length; i++)
            {
                if (i < currentTextLength)
                {
                    if (i<end.Length)
                    {
                        result.Add(end[i]);
                    }
                }
                else
                {
                    if (i<start.Length)
                    {
                        result.Add(start[i]);
                    }
                }
            }

            FillScrambleChars(ref result, scrambleMode, ref randomState,  customScrambleChars, length - currentTextLength);
        }
         static void FillScrambleChars(
            ref UnsafeList<ushort> target,
            ScrambleMode scrambleMode,
            ref Random randomState,
            UnsafeList<ushort> customScrambleChars,
            int count)
        {
            if (scrambleMode == ScrambleMode.None) return;
            if (randomState.state == 0) randomState.InitState();

            if (scrambleMode == ScrambleMode.Custom)
            {
                for (int i = 0; i < count; i++)
                {
                    target.Add(customScrambleChars[randomState.NextInt(0, customScrambleChars.Length)]);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    
                    target.Add(StringHelper.GetScrambleChar(scrambleMode, ref randomState));
                }
            }
        }
        unsafe static int AppendSliceSymbols(ref UnsafeList<ushort> result,ref UnsafeList<ushort> value, UnsafeList<RichTextSymbol> symbols, int from, int to)
        {
            RichTextSymbol* symbolsPtr = symbols.Ptr;
            var offset = 0;
            var tagIndent = 0;
           var  resultRichTextLength = 0;
            for (int i = 0; i < symbols.Length; i++)
            {
                RichTextSymbol* symbol = symbolsPtr + i;
                var symbolRange = symbol->Range;
                var start = symbolRange.Start;
                var end = symbolRange.End;
                switch (symbol->Type)
                {
                    case RichTextSymbolType.Text:
                        for (int j = start; j < end; j++)
                        {
                            if (from <= offset && offset < to)
                            {
                                result.Add(value[j]);
                                resultRichTextLength++;
                            }
                            offset++;

                            if (offset >= to && tagIndent == 0) goto LOOP_END;
                        }
                        break;
                    case RichTextSymbolType.TagStart:
                        for (int j = start; j < end; j++)
                        { result.Add(value[j]);
                        }
                        tagIndent++;
                        break;
                    case RichTextSymbolType.TagEnd:
                        for (int j = start; j < end; j++)
                        { result.Add(value[j]);
                        }
                        tagIndent--;
                        if (offset >= to && tagIndent == 0) goto LOOP_END;
                        break;
                }
            }

            LOOP_END:
            return resultRichTextLength;
        }

        public static UnsafeList<RichTextSymbol> GetRichTextSymbols(UnsafeList<ushort> source,out int charLength)
        {
            charLength = 0;
            if (source.Length == 0) return default;
            var startRichTextSymbols = new UnsafeList<RichTextSymbol>(8, Allocator.Temp);
            var symbolStart = 0;
            var currentSymbolType = RichTextSymbolType.Text;
            ushort prevRune = default;
            for (var index = 0; index < source.Length; index++)
            {
                var c = source[index];
                if (c == '<' && currentSymbolType is not (RichTextSymbolType.TagStart or RichTextSymbolType.TagEnd))
                {
                    if(symbolStart+1<=index)
                    {
                        startRichTextSymbols.Add(new RichTextSymbol(currentSymbolType,
                            new UshortRange((ushort)symbolStart, (ushort)index)));
                         charLength += index - symbolStart;
                        symbolStart = index;
                    }
                    symbolStart++;
                    currentSymbolType = RichTextSymbolType.TagStart;
                }
                else if (c == '/' && prevRune == '<')
                {
                    currentSymbolType = RichTextSymbolType.TagEnd;
                }
                else if (c == '>' &&
                         currentSymbolType is RichTextSymbolType.TagStart or RichTextSymbolType.TagEnd)
                {
                    if(symbolStart+1<=index)
                    {
                        startRichTextSymbols.Add(new RichTextSymbol(currentSymbolType,
                            new UshortRange((ushort)(symbolStart - 1),(ushort)(index + 1))));
                        symbolStart = index;
                    }
                    symbolStart++;
                    currentSymbolType = RichTextSymbolType.Text;
                }
                prevRune = c;
            }

            return startRichTextSymbols;
        }
    }

    internal class Range
    {
        public Range(int symbolStart, int index)
        {
            throw new System.NotImplementedException();
        }
    }
}