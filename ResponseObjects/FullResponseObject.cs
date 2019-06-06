using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.ResponseObjects
{
    public class FullResponseObject<T> where T : class
    {
        public T Body { get; set; }
        public string ResponseType { get; set; }
        public List<Error> ErrorList { get; set; } = new List<Error>();
        public List<Warning> WarningList { get; set; } = new List<Warning>();

    }
}
