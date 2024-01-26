// using System;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Collections.LowLevel.Unsafe;
//
// namespace RayTween.Internal
// {
//     public class StringHelpers
//     {
//         public static UnsafeList<RichTextSymbol> GetRichTextSymbols(UnsafeString source)
//         {
//             var startRichTextSymbols = new UnsafeList<RichTextSymbol>(8, Allocator.Persistent);
//             var symbolStart = 0;
//             var currentSymbolType = RichTextSymbolType.Text;
//             ushort prevRune = default;
//             for (var index = 0; index < source.Value.Length; index++)
//             {
//                 var c = source.Value[index];
//                 if (c == '<' && currentSymbolType is not (RichTextSymbolType.TagStart or RichTextSymbolType.TagEnd))
//                 {
//                     if(symbolStart+1<=index)
//                     {
//                         startRichTextSymbols.Add(new RichTextSymbol(currentSymbolType,
//                             new Range(symbolStart, index)));
//                         symbolStart = index;
//                     }
//                     symbolStart++;
//                     currentSymbolType = RichTextSymbolType.TagStart;
//                 }
//                 else if (c == '/' && prevRune == '<')
//                 {
//                     currentSymbolType = RichTextSymbolType.TagEnd;
//                 }
//                 else if (c == '>' &&
//                          currentSymbolType is RichTextSymbolType.TagStart or RichTextSymbolType.TagEnd)
//                 {
//                     if(symbolStart+1<=index)
//                     {
//                         startRichTextSymbols.Add(new RichTextSymbol(currentSymbolType,
//                             new Range(symbolStart-1, index+1)));
//                         symbolStart = index;
//                     }
//                     symbolStart++;
//                     currentSymbolType = RichTextSymbolType.Text;
//                 }
//                 prevRune = c;
//             }
//
//             return startRichTextSymbols;
//         }
//          [BurstCompile]
//         public static void Interpolate(ref FixedString32Bytes start, ref FixedString32Bytes end, float t, ScrambleMode scrambleMode, bool richTextEnabled, ref Random randomState, ref FixedString64Bytes customScrambleChars, out FixedString32Bytes result)
//         {
//             if (richTextEnabled)
//             {
//                 RichTextParser.GetSymbols(ref start, Allocator.Temp, out var startTextSymbols, out var startTextUtf8Length);
//                 RichTextParser.GetSymbols(ref end, Allocator.Temp, out var endTextSymbols, out var endTextUtf8Length);
//
//                 FillRichText(ref startTextSymbols, ref endTextSymbols, startTextUtf8Length, endTextUtf8Length, t, scrambleMode, ref randomState, ref customScrambleChars, out result);
//
//                 startTextSymbols.Dispose();
//                 endTextSymbols.Dispose();
//             }
//             else
//             {
//                 FillText(ref start, ref end, t, scrambleMode, ref randomState, ref customScrambleChars, out result);
//             }
//         }
//
//         unsafe static void FillText(
//             ref FixedString32Bytes start,
//             ref FixedString32Bytes end,
//             float t,
//             ScrambleMode scrambleMode,
//             ref Random randomState,
//             ref FixedString64Bytes customScrambleChars,
//             out FixedString32Bytes result)
//         {
//             var startTextUtf8Length = GetUtf8CharCount(ref start);
//             var endTextUtf8Length = GetUtf8CharCount(ref end);
//             var length = math.max(startTextUtf8Length, endTextUtf8Length);
//             var currentTextLength = (int)math.round(length * t);
//
//             var enumeratorStart = start.GetEnumerator();
//             var enumeratorEnd = end.GetEnumerator();
//             result = new();
//             
//             for (int i = 0; i < length; i++)
//             {
//                 var startMoveNext = enumeratorStart.MoveNext();
//                 var endMoveNext = enumeratorEnd.MoveNext();
//
//                 if (i < currentTextLength)
//                 {
//                     if (endMoveNext)
//                     {
//                         result.Append(enumeratorEnd.Current);
//                     }
//                 }
//                 else
//                 {
//                     if (startMoveNext)
//                     {
//                         result.Append(enumeratorStart.Current);
//                     }
//                 }
//             }
//
//             FillScrambleChars(ref result, scrambleMode, ref randomState, ref customScrambleChars, length - currentTextLength);
//         }
//
//         unsafe static void FillRichText(
//             ref UnsafeList<RichTextSymbol32Bytes> startSymbols,
//             ref UnsafeList<RichTextSymbol32Bytes> endSymbols,
//             int startTextUtf8Length,
//             int endTextUtf8Length,
//             float t,
//             ScrambleMode scrambleMode,
//             ref Random randomState,
//             ref FixedString64Bytes customScrambleChars,
//             out FixedString32Bytes result)
//         {
//             var length = math.max(startTextUtf8Length, endTextUtf8Length);
//             var currentTextLength = (int)math.round(length * t);
//
//             var slicedText1 = SliceSymbols(ref endSymbols, 0, currentTextLength, out var length1);
//             var slicedText2 = SliceSymbols(ref startSymbols, currentTextLength + 1, length - 1, out var length2);
//
//             result = new FixedString32Bytes();
//             result.Append(slicedText1);
//             result.Append(slicedText2);
//
//             FillScrambleChars(ref result, scrambleMode, ref randomState, ref customScrambleChars, length - (length1 + length2));
//         }
//     }
// }