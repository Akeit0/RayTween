using UnityEditor;
using RayTween.Internal;
namespace RayTween.Editor
{
    internal sealed class EditorUpdateTweenScheduler : ITweenScheduler
    {
        public double Time => EditorApplication.timeSinceStartup;
        public UpdateTiming UpdateTiming => UpdateTiming.EditorUpdate;

        public int Schedule<TValue, TPlugin>(TweenHandle<TValue, TPlugin> handle,ref TweenData<TValue, TPlugin> data, ref TweenCallbackData callbackData)
            where TValue : unmanaged
        where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            data.StartTime = Time;
            return EditorTweenDispatcher.Schedule<TValue, TPlugin>(handle, data, callbackData);
        }
    }
}