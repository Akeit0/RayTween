using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using RayTween.Internal;
using Object = UnityEngine.Object;

namespace RayTween
{
    public static class LinkValidator
    {
        static readonly Dictionary<(object, Func<object, bool>), HandleList> dictionary = new(16);
        static readonly MinimumList<(object, Func<object, bool>)> tmpList = new();

        static readonly MinimumList<HandleList> activeNodes =
            new();


        static readonly Func<Object, bool> isNotNullFunc = (m) => (m != null);
        static readonly Func<Behaviour, bool> isActiveAndEnabledFunc = (m) => (m != null)&&m.isActiveAndEnabled;
        
        static readonly Func<Behaviour, bool> isEnabledFunc = (m) => (m != null)&&m.enabled;
        
        static readonly Func<GameObject, bool> isActiveInHierarchyFunc = (m) => (m != null)&&m.activeInHierarchy;

        public static void Register(Object o, TweenHandle handle) => Register(o, isNotNullFunc, handle);
        public static void RegisterIsActiveAndEnabled(Behaviour o, TweenHandle handle) => Register(o, isActiveAndEnabledFunc, handle);
        
        public static void RegisterEnabled(Behaviour o, TweenHandle handle) => Register(o, isEnabledFunc, handle);
        
        public static void RegisterIsActiveInHierarchy(GameObject o, TweenHandle handle) => Register(o, isActiveInHierarchyFunc, handle);
        public static void RegisterIsActiveInHierarchy(Component o, TweenHandle handle) => Register(o.gameObject, isActiveInHierarchyFunc, handle);
        
        public static void Register<T>(T state, Func<T, bool> validateFunc, TweenHandle handle) where T : class
        {
            if (isRunning)
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

        static int lastFrame;
        static bool isRunning;
        internal static void Update(int frame)
        {
            if (lastFrame == frame) return;
            isRunning = true;
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
                isRunning = false;
            }
            // linkValidatorMarker.End();
        }
    }
}