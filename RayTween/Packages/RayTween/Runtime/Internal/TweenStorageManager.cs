using System;
using System.Runtime.CompilerServices;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace RayTween.Internal
{
    internal struct Entry
    {
        public int Version;
        public int StorageId;
        public int DenseIndex;
        public int Next;
    }

    internal static class TweenStorageManager
    {
        public static void Reset()
        {
            freeEntry = 0;
            var entrySpan = entries.AsSpan();
            for (int i = 0; i < entrySpan.Length; i++)
            {
                entrySpan[i] = new Entry() { Version = 0, StorageId = -1, DenseIndex = -1, Next = i + 1 };
            }

            entrySpan[^1].Next = -1;
        }

        private static Entry[] entries = new Entry[256];
        private static int freeEntry;

        internal static Entry[] Entries => entries;

        internal static void SetDenseIndex(int index, int value)
        {
            entries[index].DenseIndex = value;
        }

        internal static void SetData(int index, (int StorageId, int DenseIndex) data)
        {
            entries[index].StorageId = data.StorageId;
            entries[index].DenseIndex = data.DenseIndex;
        }

        internal static (int EntryIndex, int Version) Alloc()
        {
            if (freeEntry == -1)
            {
                var currentLength = entries.Length;
                EnsureCapacity(currentLength * 2);
                freeEntry = currentLength;
            }

            // Find free entry
            var entryIndex = freeEntry;
            ref var entry = ref entries[entryIndex];
            freeEntry = entry.Next;
            entry.Next = -1;
            entry.DenseIndex = -1;
            entry.StorageId = -1;
            return (entryIndex, ++entry.Version);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Free(int index)
        {
            ref var entry = ref entries[index];
            entry.Next = freeEntry;
            entry.Version++;
            freeEntry = index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Free(TweenHandle handle, bool throwOnFailure = true)
        {
            ref var entry = ref entries[handle.Index];
            if (entry.Version != handle.Version)
            {
                if (throwOnFailure) throw new InvalidOperationException();
                return;
            }

            entry.Next = freeEntry;
            entry.Version++;
            freeEntry = handle.Index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Free(int index, int version, bool throwOnFailure = true)
        {
            ref var entry = ref entries[index];
            if (entry.Version != version)
            {
                if (throwOnFailure) throw new InvalidOperationException();
                return;
            }

            entry.Next = freeEntry;
            entry.Version++;
            freeEntry = index;
        }

        internal static void EnsureCapacity(int capacity)
        {
            var currentLength = entries.Length;
            if (currentLength >= capacity) return;

            Array.Resize(ref entries, capacity);
            for (int i = currentLength; i < entries.Length; i++)
            {
                entries[i] = new()
                    { Next = i == capacity - 1 ? freeEntry : i + 1, DenseIndex = -1, Version = 0, StorageId = -1 };
            }

            freeEntry = currentLength;
        }


        static readonly MinimumList<ITweenStorage> storageList = new();

        public static int CurrentStorageId { get; private set; }

        public static void AddStorage<TValue, TPlugin>(TweenStorage<TValue, TPlugin> storage)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            storageList.Add(storage);
            CurrentStorageId++;
        }

        public static void CompleteTween(TweenHandle handle)
        {
            if (handle.TryGetBuffer(out var buffer))
            {
                buffer.CallbackData.InvokeAndDispose(TweenResult.Completed);
                return;
            }
            ValidCheckWithThrow(handle, out var storageIndex, out var denseIndex);
            storageList[storageIndex].Complete(denseIndex);
        }

        public static void CancelTween(TweenHandle handle)
        {
            if (handle.TryGetBuffer(out var buffer))
            {
                buffer.CallbackData.InvokeAndDispose(TweenResult.Canceled);
                return;
            }
            ValidCheckWithThrow(handle, out var storageIndex, out var denseIndex);
            storageList[storageIndex].Cancel(denseIndex);
        }

        public static bool ValidCheck(TweenHandle handle, out int denseId)
        {
            denseId = 0;
            if (handle.Index < 0 || handle.Index >= entries.Length)
            {
                return false;
            }

            var entry = entries[handle.Index];
            var version = entry.Version;
            if (version <= 0 || version != handle.Version)
            {
                return false;
            }

            denseId = entry.DenseIndex;
            return true;
        }

        public static bool ValidCheck(TweenHandle handle, out int storageIndex, out int denseIndex)
        {
            denseIndex = 0;
            storageIndex = 0;
            if (handle.Index < 0 || handle.Index >= entries.Length)
            {
                return false;
            }

            var entry = entries[handle.Index];
            var version = entry.Version;
            if (version <= 0 || version != handle.Version)
            {
                Debug.Log("version" + version + " " + handle.Version);
                return false;
            }

            storageIndex = entry.StorageId;
            if (storageIndex < 0 || storageIndex >= CurrentStorageId)
            {
                return false;
            }

            denseIndex = entry.DenseIndex;
            return true;
        }

        public static void ValidCheckWithThrow(TweenHandle handle, out int storageIndex, out int denseIndex)
        {
            if (handle.Index < 0 || handle.Index >= entries.Length)
            {
                throw new ArgumentException("Invalid  Sparse Index.");
            }

            var entry = entries[handle.Index];
            var version = entry.Version;
            if (version < 0 || version != handle.Version)
            {
                throw new ArgumentException("Tween has been destroyed or no longer exists.");
            }

            storageIndex = entry.StorageId;
            if (storageIndex < 0 || storageIndex >= CurrentStorageId)
            {
                throw new ArgumentException("Invalid storage id.");
            }

            denseIndex = entry.DenseIndex;
        }

        public static bool IsActive(TweenHandle handle)
        {
            if (handle.IsIdling) return true;
            if (!ValidCheck(handle, out var storageIndex, out var denseIndex))
            {
                return false;
            }

            return storageList[storageIndex].IsActive(denseIndex);
        }

        public static ref TweenCallbackData GetTweenCallbacks(TweenHandle handle)
        {
            if (handle.TryGetBuffer(out var buffer))
                return ref buffer.CallbackData;
            ValidCheckWithThrow(handle, out var storageIndex, out var denseIndex);
            return ref storageList[storageIndex].GetTweenCallbacks(denseIndex);
        }


        public static ref TweenData<TValue, TPlugin> GetTweenData<TValue, TPlugin>(
            TweenHandle<TValue, TPlugin> handle) where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            ValidCheckWithThrow(handle.AsNoType(), out var storageIndex, out var denseIndex);
            return ref ((TweenStorage<TValue, TPlugin>)storageList[storageIndex]).GetDataSpan()[denseIndex];
        }


        // For TweenTracker
        public static (Type ValueType, Type OptionsType, Type PluginType) GetTweenType(TweenHandle handle)
        {
            ValidCheckWithThrow(handle, out var storageIndex, out var denseIndex);
            var storageType = storageList[storageIndex].GetType();
            var valueType = storageType.GenericTypeArguments[0];
            var optionsType = storageType.GenericTypeArguments[1];
            var PluginType = storageType.GenericTypeArguments[2];
            return (valueType, optionsType, PluginType);
        }
    }
}