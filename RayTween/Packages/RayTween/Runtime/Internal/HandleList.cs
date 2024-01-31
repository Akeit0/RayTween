using System;
using UnityEngine;

namespace RayTween.Internal
{

  
    internal sealed class HandleList
    {
        static HandleList pool;
        HandleList lastOrHeadNode;
        HandleList prev;
        HandleList nextNode;
         Handle8 values;
         bool IsHead => prev == null;

        public bool HasNext => nextNode == null;

        public ref TweenHandle this[int index] => ref values.GetSpan()[index];

        int Length => values.Length;

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
            result.values = default;
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
                    foreach (var handle in next.values.GetSpan())
                    {
                        handle.TryCancel();
                    }
                    next.values=default;
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
            if (last.values.Length < 8)
            {
                last.values.Add(handle);
                newList = default;
                return false;
            }
            else
            {
                var newLast = CreateOrGet(last);
                SetLast(newLast);
                newLast.values.Add(handle);
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
                var span = values.GetSpan();
                
                for (int i = 0; i < values.Length; i++)
                {
                    ref var handle = ref span[i];
                    if (!handle.IsActive())
                    {
                        values.Length--;
                        handle = span[values.Length];
                        span[values.Length] = default;
                        i--;
                        // Debug.Log("Compress");
                    }
                }

              
            }
            else
            {
                if (last.values.Length <= 0)
                {
                    return ;
                }
                var span = values.GetSpan();
                for (int i = 0; i < values.Length; i++)
                {
                    ref var handle = ref span[i];
                    if (!handle.IsActive())
                    {
                        handle = last.values.RemoveLast();
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
                var span = values.GetSpan();
                
                for (int i = 0; i < values.Length; i++)
                {
                    ref var handle = ref span[i];
                    if (!handle.IsActive())
                    {
                        values.Length--;
                        handle = span[values.Length];
                        span[values.Length] = default;
                        i--;
                       // Debug.Log("Compress");
                    }
                }

              
            }
            else
            {
                if (last.values.Length <= 0)
                {
                    return false;
                }
                var span = values.GetSpan();
                for (int i = 0; i < values.Length; i++)
                {
                    ref var handle = ref span[i];
                    if (!handle.IsActive())
                    {
                        handle = last.values.RemoveLast();
                        if (last.Length == 0)
                        {
                            (values, last.values) = (last.values, values);
                            
                          break;
                        }
                        i--;
                    }
                }
            }
            if (values.Length == 0)
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
      

        unsafe struct Handle8
        {
            fixed int array[24];
            ref int GetPinnableReference() => ref array[0];
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