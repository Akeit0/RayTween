using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace RayTween.Internal
{
    internal struct OnDisposeActionList
    {
        Node head;
        public void Append([NotNull] Action<TweenResult> action)
        {
            Node.Append(ref head, action);
        }

        public void Append<TTarget>([NotNull] TTarget target, [NotNull] Action<TTarget,TweenResult> action) where TTarget : class
        {
            Node.Append(ref head, target, action);
        }
        public void AppendOnCancel([NotNull] Action action)
        {
            Node.AppendOnCancel(ref head, action);
        }

        public void AppendOnCancel<TTarget>([NotNull] TTarget target, [NotNull] Action<TTarget> action) where TTarget : class
        {
            Node.AppendOnCancel(ref head, target, action);
        }
        public void AppendOnComplete([NotNull] Action action)
        {
            Node.AppendOnComplete(ref head, action);
        }

        public void AppendOnComplete<TTarget>([NotNull] TTarget target, [NotNull] Action<TTarget> action) where TTarget : class
        {
            Node.AppendOnComplete(ref head, target, action);
        }
        public void RemoveTarget(object target)
        {
            Node.RemoveTarget(ref head, target);
        }

        public void Remove(object target, object action)
        {
            Node. Remove(ref head, target, action);
        }


        public void InvokeAndDispose(TweenResult value)
        {
            if (head == null) return;
            head.InvokeAndDispose(value);
            head = null;
        }
        enum NodeType:byte
        {
            OnDispose,
            OnDisposeWithTarget,
            OnCancel,
            OnCancelWithTarget,
            OnComplete,
            OnCompleteWithTarget
        }
        class Node
        {
            static Node pool;
            object target;
            object action;
            Node nextNode;
            NodeType type;
            public Node LastNode
        {
            get
            {
                var next = this;
                while (next.nextNode != null)
                {
                    next = next.nextNode;
                }

                return next;
            }
        }
            Node()
        {
        }

          
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Node CreateOrGet()
        {
            if (pool == null)
            {
                return new Node();
            }
            else
            {
                ref var poolRef = ref pool;
                var result = poolRef;
                poolRef = result.nextNode;
                result.nextNode = null;
                return result;
            }
        }

        public static void Append(ref Node head, Action<TweenResult> action)
        {
            if (action == null) return;
            if (head == null)
            {
                head = CreateOrGet(null, action,NodeType.OnDispose);
            }
            else
            {
                head.LastNode.nextNode = CreateOrGet(null, action,NodeType.OnDispose);
            }
        }

        public static void Append<TTarget>(ref Node head, TTarget target, Action<TTarget, TweenResult> action)
            where TTarget : class
        {
            if (target == null)
            {
                Debug.LogError("target is null");
                return;
            }

            if (action == null) return;
            if (head == null)
            {
                head = CreateOrGet(target, action,NodeType.OnDisposeWithTarget);
            }
            else
            {
                head.LastNode.nextNode = CreateOrGet(target, action,NodeType.OnDisposeWithTarget);
            }
        }
        public static void AppendOnComplete(ref Node head, Action action)  
        {
            if (action == null) return;
            if (head == null)
            {
                head = CreateOrGet(null, action,NodeType.OnComplete);
            }
            else
            {
                head.LastNode.nextNode = CreateOrGet(null, action,NodeType.OnComplete);
            }
        }
        public static void AppendOnComplete<TTarget>(ref Node head,TTarget target, Action<TTarget> action)  where TTarget : class
        {
            if (action == null) return;
            if (head == null)
            {
                head = CreateOrGet(target, action,NodeType.OnCompleteWithTarget);
            }
            else
            {
                head.LastNode.nextNode = CreateOrGet(target, action,NodeType.OnCompleteWithTarget);
            }
        }
        public static void AppendOnCancel(ref Node head, Action action)  
        {
            if (action == null) return;
            if (head == null)
            {
                head = CreateOrGet(null, action,NodeType.OnCancel);
            }
            else
            {
                head.LastNode.nextNode = CreateOrGet(null, action,NodeType.OnCancel);
            }
        }
        public static void AppendOnCancel<TTarget>(ref Node head,TTarget target, Action<TTarget> action)  where TTarget : class
        {
            if (action == null) return;
            if (head == null)
            {
                head = CreateOrGet(target, action,NodeType.OnCancelWithTarget);
            }
            else
            {
                head.LastNode.nextNode = CreateOrGet(target, action,NodeType.OnCancelWithTarget);
            }
        }
        public static void RemoveTarget(ref Node head, object target)
        {
            if (head == null) return;
            if (head.target == target)
            {
                head = head.nextNode;
                return;
            }

            var next = head;
            while (next.nextNode != null)
            {
                if (next.nextNode.target == target)
                {
                    next.nextNode = next.nextNode.nextNode;
                }

                next = next.nextNode;
            }
        }

        public static void Remove(ref Node head, object target, object action)
        {
            if (head == null) return;
            if (head.target == target && head.action == action)
            {
                head = head.nextNode;
                return;
            }

            var next = head;
            while (next.nextNode != null)
            {
                if (next.nextNode.target == target && next.nextNode.action == action)
                {
                    next.nextNode = next.nextNode.nextNode;
                }

                next = next.nextNode;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node CreateOrGet(object target, object action,NodeType nodeType)
        {
            var result = CreateOrGet();
            result.target = target;
            result.action = action;
            result.type = nodeType;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InvokeAndDispose(TweenResult result)
        {
            var next = this;
            InvokeLabel:
            try
            {
                switch (next.type)
                {
                    case NodeType.OnDispose:
                        UnsafeUtility.As<object,Action< TweenResult>>(ref next.action)( result);
                        break;
                    case NodeType.OnDisposeWithTarget:
                        UnsafeUtility.As<object,Action<object, TweenResult>>(ref next.action)(next.target, result);
                        break;
                    case NodeType.OnCancel:
                        if (!result.IsCompleted)
                        {
                            UnsafeUtility.As<object,Action>(ref next.action)();
                        }
                        break;
                    case NodeType.OnCancelWithTarget:
                        if (!result.IsCompleted)
                        {
                            UnsafeUtility.As<object,Action<object>>(ref next.action)(next.target);
                        }
                        break;
                    case NodeType.OnComplete:
                        if (result.IsCompleted)
                        {
                            UnsafeUtility.As<object,Action>(ref next.action)();
                        }
                        break;
                    case NodeType.OnCompleteWithTarget:
                        if (result.IsCompleted)
                        {
                            UnsafeUtility.As<object,Action<object>>(ref next.action)(next.target);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                TweenDispatcher.GetUnhandledExceptionHandler()?.Invoke(ex);
            }

            next.action = null;
            next.target = null;
            var nextNext = next.nextNode;
            if (nextNext != null)
            {
                next = nextNext;
                goto InvokeLabel;
            }

            next.nextNode = pool;
            pool = this;
        }
        }
    }
}