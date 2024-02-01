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
         public int LocalLength => values.Length;

        public bool HasNext => nextNode == null;

        public ref TweenHandle this[int index] => ref values.GetSpan()[index];

        int Length => values.Length;

        HandleList GetLast() => IsHead ? lastOrHeadNode : lastOrHeadNode.lastOrHeadNode;

       
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
                var total = 0;
                do
                {
                    if (100 < total++)
                    {
                        Debug.LogWarning(total);
                        break;
                    }
                    foreach (var handle in next.values.GetSpan())
                    {
                        handle.TryCancel();
                    }

                    next.values = default;
                    next.prev = next;
                    next = next.nextNode;
                    if(next==lastOrHeadNode)break;
                    poolCount++;
                } while (next != null);
                if(next != null&&next!=this)
                {
                    foreach (var handle in next.values.GetSpan())
                    {
                        handle.TryCancel();
                    }
                    next.values = default;
                    next.prev = next;
                }
                
                lastOrHeadNode.nextNode = pool;
                pool = this;
            }
            
        }

        public bool Add(TweenHandle handle,out HandleList newList)
        {
            if (values.Add(handle))
            {
                newList = null;
                return false;
            }
            var last = lastOrHeadNode;
            if (last!=this&&last.values.Add(handle))
            {
                newList = default;
                return false;
            }
            else
            {
                CompressHead(8);
                if(values.Length<8)
                {
                    values.Add(handle);
                    newList = default;
                    return false;
                }
                newList = CreateOrGet(last);
                newList.prev = lastOrHeadNode;
                lastOrHeadNode.nextNode = newList;
                lastOrHeadNode = newList;
                newList.values.Add(handle);
                newList.values.Add(values.RemoveLast());
                return true;
            }
        }

        public bool IsPooled => prev == this;


         void CompressHead(int threshold)
        {
            if (prev != null)
            {
                Debug.LogWarning("Don't compress");
                return ;
            }
            if(prev==this)return;
            if (Length<threshold)return;
            var span = values.GetSpan();
            Span<TweenHandle> newHandles = stackalloc TweenHandle[values.Length];
            var activeCount = 0;
            for (int i = 0; i < span.Length; i++)
            {
                ref var handle = ref span[i];
                if (handle.IsActive())
                {
                    newHandles[activeCount++] = handle;
                }
            }
            newHandles.CopyTo(span);
            values.Length = activeCount;
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
            var span = values.GetSpan();
            Span<TweenHandle> newHandles = stackalloc TweenHandle[values.Length];
            var activeCount = 0;
            for (int i = 0; i < span.Length; i++)
            {
                ref var handle = ref span[i];
                if (handle.IsActive())
                {
                    newHandles[activeCount++] = handle;
                }
            }
            newHandles.CopyTo(span);
            values.Length = activeCount;
            
            var last = GetLast();
            if (values.Length != 0&&last != this && values.Length + last.values.Length <= 8)
            {
                foreach (ref var h in newHandles[..activeCount])
                {
                    last.values.Add(h);
                }
                values = default;
            }
            //Debug.Log(values.Length);
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
                    Length--;
                    var last = ((TweenHandle*)p)[Length];
                    ((TweenHandle*)p)[Length] = default;
                    return last;
                }
            }

            public bool Add(TweenHandle handle)
            {
                if(Length<0||8<Length)
                {
                    throw new Exception(Length.ToString());
                }
                if (Length == 8) return false;
                fixed (int* p = this)
                {
                    ((TweenHandle*)p)[Length++] = handle;
                }

                return true;
            }
        }
    }
}