

using RayTween.Internal;

namespace RayTween
{
    internal sealed class ManualTweenScheduler : ITweenScheduler
    {
        public ManualTweenScheduler()
        {
            this.Id= TweenScheduler.Add(this);
        }
        
        public double Time => ManualTweenDispatcher.Time;
        public int Id { get; }

        public UpdateTiming UpdateTiming => UpdateTiming.Manual;

        public int Schedule<TValue, TPlugin>(TweenHandle<TValue,TPlugin> handle,ref TweenData<TValue, TPlugin> data, ref TweenCallbackData callbackData)
            where TValue : unmanaged
            where TPlugin : unmanaged, ITweenPlugin<TValue>
        {
            return ManualTweenDispatcher.Schedule(handle,ref data,ref  callbackData);
        }
        
        
    }
}