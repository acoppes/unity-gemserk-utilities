using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public static class EnumerableExtensions
    {
        // returns true if anotherList is contained in List
        public static bool ContainsAll<T>(this IEnumerable<T> list, IEnumerable<T> anotherList)
        {
            var enumerable = anotherList.ToList();
            return enumerable.Intersect(list).Count() == enumerable.Count();
        }
        
        public static bool ContainsNone<T>(this IEnumerable<T> list, IEnumerable<T> anotherList)
        {
            return !anotherList.Intersect(list).Any();
        }
    }
}