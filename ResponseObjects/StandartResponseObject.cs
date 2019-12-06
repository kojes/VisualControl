using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.ResponseObjects
{
    public class StandardResponseObject
    {
        public string Body { get; set; }
        public string ResponseType { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }

        

    }

    public class StandardResponseObject<T> where T : class
    {
        public T Body { get; set; }
        public string ResponseType { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }


    }
}
