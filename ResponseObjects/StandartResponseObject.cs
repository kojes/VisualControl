using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.ResponseObjects
{
    internal class StandardResponseObject
    {
        public string Body { get; set; }
        public string Error { get; set; }

        public StandardResponseObject(string body, string error)
        {
            this.Body = body;
            this.Error = error;
           
        }

        public StandardResponseObject(string error)
        {
            this.Body = String.Empty;
            this.Error = error;
        }

        public StandardResponseObject()
        {
            this.Body = String.Empty;
            this.Error = String.Empty;
        }

    }

    public class StandardResponseObject<T> where T : class
    {
        public T Body { get; set; }
        public string Error { get; set; }

        public StandardResponseObject(T body, string error)
        {
            Body = body;
            Error = error;
        }

        public StandardResponseObject(T body)
        {
            Body = body;
            Error = String.Empty;
        }

    }
}
