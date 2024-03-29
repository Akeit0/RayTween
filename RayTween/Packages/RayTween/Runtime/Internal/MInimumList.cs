﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RayTween.Internal
{
    internal sealed class MinimumList<T>
    {
        public MinimumList(int initialCapacity = 16)
        {
            array = new T[initialCapacity];
        }

        T[] array;
        int tailIndex;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T element)
        {
            if (array.Length == tailIndex)
            {
                Array.Resize(ref array, tailIndex * 2);
            }

            array[tailIndex] = element;
            tailIndex++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAtSwapBack(int index)
        {
            CheckIndex(index);
            tailIndex--;
            array[index] = array[tailIndex];
            array[tailIndex] = default;
         
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            array.AsSpan().Clear();
            tailIndex = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnsureCapacity(int capacity)
        {
            while (array.Length < capacity)
            {
                Array.Resize(ref array, array.Length * 2);
            }
        }

        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => array[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => array[index] = value;
        }

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => tailIndex;
        }

        public Span<T> AsSpan() => array.AsSpan(0, tailIndex);

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        void CheckIndex(int index)
        {
            if (index < 0 || index > tailIndex) throw new IndexOutOfRangeException();
        }
    }
}