using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using RayTween.Internal;
using Unity.Profiling;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace RayTween
{
    public static class LinkValidator
    {
        static Dictionary<(object, Func<object, bool>), HandleList> dictionary = new(16);
        static MinimumList<(object, Func<object, bool>)> tmpList = new(16);

        static MinimumList<HandleList> activeNodes =
            new(16);

        static Dictionary<CancellationToken, HandleList> tokenDict = new(16);
        
        
        static readonly Func<Object, bool> IsNotNullFunc = (m) => (m != null);
        static Func<Behaviour, bool> IsActiveFunc = (m) => (m != null)&&m.isActiveAndEnabled;
        
        static Func<GameObject, bool> IsActiveInHierarchyFunc = (m) => (m != null)&&m.activeInHierarchy;

        public static void Register(Object o, TweenHandle handle) => Register(o, IsNotNullFunc, handle);
        public static void RegisterIsActiveAndEnabled(Behaviour o, TweenHandle handle) => Register(o, IsActiveFunc, handle);
        
        public static void RegisterIsActiveInHierarchy(GameObject o, TweenHandle handle) => Register(o, IsActiveInHierarchyFunc, handle);
        public static void RegisterIsActiveInHierarchy(Component o, TweenHandle handle) => Register(o.gameObject, IsActiveInHierarchyFunc, handle);
        
        public static void Register<T>(T state, Func<T, bool> validateFunc, TweenHandle handle) where T : class
        {
            if (IsRunning)
            {
                throw new InvalidOperationException();
            }
            
            var func2 = UnsafeUtility.As<Func<T, bool>, Func<object, bool>>(ref validateFunc);
            if (dictionary.TryGetValue((state, func2), out var list))
            {
                list.CompressList();
                if (list.Add(handle, out var newList))
                {
                    //Debug.Log("NewList");
                    activeNodes.Add(newList);
                }
            }
            else
            {
                list = HandleList.CreateOrGet(null);
                list.Add(handle, out _);
                dictionary.Add((state, func2), list);
            }
        }

        //static readonly ProfilerMarker linkValidatorMarker = new ProfilerMarker("LinkValidator");

        // static readonly ProfilerMarker linkValidatorCustomFuncMarker = new ProfilerMarker("LinkValidatorCustomFunc");
        static int lastFrame;
        static bool IsRunning;
        internal static void Update(int frame)
        {
            if (lastFrame == frame) return;
            IsRunning = true;
            lastFrame = frame;
            //  linkValidatorMarker.Begin();

            try
            {
                tmpList.Clear();
                // linkValidatorCustomFuncMarker.Begin();
                var total = 0;
                foreach (var (key, value) in dictionary)
                {
                    total++;
                    if (1000000 < total)
                    {
                        throw new InvalidOperationException("Infinite");
                    }

                    try
                    {
                        if (key.Item2(key.Item1)) continue;
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }

                    //Debug.Log("Release");
                    tmpList.Add(key);
                    value.Release();
                }

                foreach (var key in tmpList.AsSpan())
                {
                    dictionary.Remove(key);
                }

                //  linkValidatorCustomFuncMarker.End();
                var activeList = activeNodes;
                var c = frame & 7;

                for (int i = 0; i < activeList.Length; i++)
                {
                    total++;
                    if (1000000 < total)
                    {
                        throw new InvalidOperationException("Infinite");
                    }
                    
                    var list = activeList[i];
                    if (list.IsPooled)
                    {
                        activeList.RemoveAtSwapBack(i);

                        i--;
                    }

                    else if ((i & 7) == c && list.Compress())
                    {
                        activeList.RemoveAtSwapBack(i);
                        i--;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                IsRunning = false;
            }
            // linkValidatorMarker.End();
        }
    }
}