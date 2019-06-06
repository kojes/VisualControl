using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.ResponseObjects
{
    public class Warning
    {
        public Warning(string message, string code)
        {
            Message = message;
            Code = code;
        }

        public string Message { get; }
        public string Code { get; }


    }
}
