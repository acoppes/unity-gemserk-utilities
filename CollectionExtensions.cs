using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Utilities
{
    public static class CollectionExtensions
    {
        public static T Random<T>(this List<T> list)
        {
            if (list.Count == 0)
                return default;
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        
        public static T GetItemOrLast<T>(this List<T> list, int index)
        {
            if (index >= list.Count)
                index = list.Count - 1;
            
            return list[index];
        }
        
        public static T GetItemOrLast<T>(this T[] array, int index)
        {
            if (index >= array.Length)
                index = array.Length - 1;
            
            return array[index];
        }
        
        public static T GetInterface<T>(this List<GameObject> list, int index) where T : class
        {
            if (index >= list.Count)
                return null;
            return list[index].GetInterface<T>();
        }
    }
}