using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.Extensions
{
    public static class ListExtensions
    {
        public static List<string> RemoveFirstIfStringEmpty(this List<string> list)
        {
            if (string.IsNullOrEmpty(list.FirstOrDefault()))
            {
                list.Remove(string.Empty);
            }

            return list;
        }
    }
}
