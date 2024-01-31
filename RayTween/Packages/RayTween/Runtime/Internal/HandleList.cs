using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;

namespace RayTween.Internal
{

  
    internal sealed class HandleList
    {
        static HandleList pool;
        HandleList lastOrHeadNode;
        HandleList prev;
        HandleList nextNode;
        public Handle8 Values;
        public bool IsHead => prev == null;

        public bool HasNext => nextNode == null;

        public ref TweenHandle this[int index] => ref Values.GetSpan()[index];

        public int Length => Values.Length;

        HandleList GetLast() => IsHead ? lastOrHeadNode : lastOrHeadNode.lastOrHeadNode;

        void SetLast(HandleList newLast)
        {
            if (prev == null)
            {
                var last = lastOrHeadNode;
                last.nextNode = newLast;
                newLast.prev = last;
                lastOrHeadNode = newLast;
                newLast.lastOrHeadNode = this;
            }
            else
            {
                var last = lastOrHeadNode.lastOrHeadNode;
                last.nextNode = newLast;
                newLast.prev = last;
                newLast.lastOrHeadNode = lastOrHeadNode;
                lastOrHeadNode.lastOrHeadNode = newLast;
            }
        }
        public static HandleList CreateOrGet(HandleList prev)
        {
            var result = pool;
            if (pool == null)
            {
                result= new HandleList();
            }
            else
            {
                pool = result.nextNode;
                poolCount--;
            }

           
           
            result.prev = prev;
            result.Values = default;
            result.nextNode = null;
            result.lastOrHeadNode = result;
            
        
            return result;
        }

        static int poolCount;

        public static int PoolCount => poolCount;
        public void Release()
        {
            if (IsHead)
            {

                var next = this;
                while (next!=null)
                {
                    foreach (var handle in next.Values.GetSpan())
                    {
                        handle.TryCancel();
                    }
                    next.Values=default;
                    next.prev = next;
                    next = next.nextNode;
                    poolCount++;
                }
                
                lastOrHeadNode.nextNode = pool;
                pool = this;
            }
            
        }

        public bool Add(TweenHandle handle,out HandleList newList)
        {
            
            var last = GetLast();
            if (last.Values.Length < 8)
            {
                last.Values.Add(handle);
                newList = default;
                return false;
            }
            else
            {
                var newLast = CreateOrGet(last);
                SetLast(newLast);
                newLast.Values.Add(handle);
                newList = newLast;
                return true;
            }
        }

        public bool IsPooled => prev == this;

        public void CompressList()
        {
            var last = GetLast();
            if (last == this)
            {
                var span = Values.GetSpan();
                
                for (int i = 0; i < Values.Length; i++)
                {
                    ref var handle = ref span[i];
                    if (!handle.IsActive())
                    {
                        Values.Length--;
                        handle = span[Values.Length];
                        span[Values.Length] = default;
                        i--;
                        // Debug.Log("Compress");
                    }
                }

              
            }
            else
            {
                if (last.Values.Length <= 0)
                {
                    return ;
                }
                var span = Values.GetSpan();
                for (int i = 0; i < Values.Length; i++)
                {
                    ref var handle = ref span[i];
                    if (!handle.IsActive())
                    {
                        handle = last.Values.RemoveLast();
                        if (last.Length == 0)
                        {
                            return ;
                        }
                        i--;
                    }
                }
            }
          
        }
        
        public bool Compress()
        {
            if (prev == null)
            {
                Debug.LogWarning("Don't compress head");
                return true;
            }
            if (prev == this) return true;
            if (Length==0)
            {
                if (lastOrHeadNode.lastOrHeadNode == this)
                {
                    lastOrHeadNode.lastOrHeadNode = prev;
                }
                else 
                {
                    prev.nextNode = nextNode;
                }
                return true;
            }
            var last = GetLast();
            if (last == this)
            {
                var span = Values.GetSpan();
                
                for (int i = 0; i < Values.Length; i++)
                {
                    ref var handle = ref span[i];
                    if (!handle.IsActive())
                    {
                        Values.Length--;
                        handle = span[Values.Length];
                        span[Values.Length] = default;
                        i--;
                       // Debug.Log("Compress");
                    }
                }

              
            }
            else
            {
                if (last.Values.Length <= 0)
                {
                    return false;
                }
                var span = Values.GetSpan();
                for (int i = 0; i < Values.Length; i++)
                {
                    ref var handle = ref span[i];
                    if (!handle.IsActive())
                    {
                        handle = last.Values.RemoveLast();
                        if (last.Length == 0)
                        {
                            (Values, last.Values) = (last.Values, Values);
                            
                          break;
                        }
                        i--;
                    }
                }
            }
            if (Values.Length == 0)
            {
                prev.nextNode = nextNode;
                prev = this;
                nextNode = pool;
                pool = this;
                poolCount++;
                return true;
            }
               
            return false;
        }
      

        public unsafe struct Handle8
        {
            fixed int array[24];
            public ref int GetPinnableReference() => ref array[0];
            public int Length;

            public Span<TweenHandle> GetSpan()
            {
                fixed (int* p = this)
                {
                    return new Span<TweenHandle>(p, Length);
                }
            }

            public Span<TweenHandle> GetFullSpan()
            {
                fixed (int* p = this)
                {
                    return new Span<TweenHandle>(p, 8);
                }
            }

            public TweenHandle RemoveLast()
            {
                if (Length <=0||8<Length )
                {
                    throw new Exception(Length.ToString());
                }
                fixed (int* p = this)
                {
                    return ((TweenHandle*)p)[Length-=1];
                }
            }

            public void Add(TweenHandle handle)
            {
                if (Length <0||7<Length )
                {
                    throw new Exception(Length.ToString());
                }
                fixed (int* p = this)
                {
                    ((TweenHandle*)p)[Length++] = handle;
                }
            }
        }
    }
}