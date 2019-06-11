using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.Extensions {
    public static class ListExtensions {
        public static List<string> RemoveFirstIfStringEmpty (this List<string> list) {
            if (string.IsNullOrEmpty (list.FirstOrDefault ())) {
                list.Remove (string.Empty);
            }

            return list;
        }

        public static IEnumerable<T> GetNth<T> (this List<T> list, int n) {
            for (int i = 0; i < list.Count; i += n)
                yield return list[i];
        }
    }
}