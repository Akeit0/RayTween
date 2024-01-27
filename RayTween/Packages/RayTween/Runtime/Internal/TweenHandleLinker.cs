using System.Runtime.CompilerServices;
using UnityEngine;

namespace RayTween.Internal
{
    [DisallowMultipleComponent]
    [AddComponentMenu("")]
    internal sealed class TweenHandleLinker : MonoBehaviour
    {
        readonly MinimumList<TweenHandle> handleList = new(8);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Register(TweenHandle handle)
        {
            handleList.Add(handle);
        }

        void OnDestroy()
        {
            var span = handleList.AsSpan();
            for (int i = 0; i < span.Length; i++)
            {
                ref var handle = ref span[i];
                if (handle.IsActive()) handle.Cancel();
            }
        }
    }
}
