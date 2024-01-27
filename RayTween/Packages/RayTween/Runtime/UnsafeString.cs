using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace RayTween
{
    
        public   struct UnsafeString
        {
            internal UnsafeList<ushort> Value;
            public int Length => Value.Length;
            public bool IsCreated => Value.IsCreated;
            
            public unsafe Span<char> AsSpan()
            {
                return new Span<char>((char*)Value.Ptr,Value.Length);
            }
            internal UnsafeString(string value,Allocator allocator=Allocator.Persistent)
            {
                if (string.IsNullOrEmpty(value))
                {
                    this = default;
                    return;
                }
                Value = new UnsafeList<ushort>(value.Length, allocator);;
                Value.Length = value.Length;
                value.AsSpan().CopyTo(AsSpan());
            }
            internal void Set(string value,Allocator allocator=Allocator.Persistent)
            {
                if (string.IsNullOrEmpty(value))
                {
                    this = default;
                    return;
                }
                if (!Value.IsCreated)
                {
                    Value = new UnsafeList<ushort>(value.Length, allocator);
                }
                else
                {
                    Value.Clear();
                }
                Value.Length = value.Length;
                value.AsSpan().CopyTo(AsSpan());
            }
            internal void Dispose()
            {
                // if (Value.IsCreated)
                // {
                //     Debug.Log("Dispose "+AsSpan().ToString());
                // }
                Value.Dispose();
                
            }

            public override string ToString()
            {
                return AsSpan().ToString();
            }
        }
    
}