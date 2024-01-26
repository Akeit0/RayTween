using Unity.Burst;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace RayTween
{
    static class SharedRandom
    {
        readonly struct Key { }
        public static readonly SharedStatic<Random> Random;
        
        static SharedRandom()
        {
            Random = SharedStatic<Random>.GetOrCreate<Key>();
        }
        [RuntimeInitializeOnLoadMethod]
        static void Init()
        {
         
            Random.Data.InitState();
        }
    }
}