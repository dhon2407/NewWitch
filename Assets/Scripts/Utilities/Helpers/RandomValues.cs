using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utilities.Helpers
{
    public static class RandomValues
    {
        public static float RandomRange(this Vector2 vectorRange)
        {
            return Random.Range(vectorRange.x, vectorRange.y);
        }
        
        public static float RandomRange(this Vector2Int vectorRange)
        {
            return Random.Range(vectorRange.x, vectorRange.y);
        }

        public static T GetRandom<T>(this IList<T> list)
        {
            return list.Count == 0 ? default : list[Random.Range(0, list.Count)];
        }
        
        public static T GetRandom<T>(this T[] arrayList)
        {
            return arrayList.Length == 0 ? default : arrayList[Random.Range(0, arrayList.Length)];
        }
        
        private static readonly System.Random SysRandom = new System.Random(); 
        
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = SysRandom.Next(n + 1);  
                T value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }
    }
}