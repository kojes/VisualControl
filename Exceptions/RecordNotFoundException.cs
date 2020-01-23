using System;
namespace VueExample.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException()
        {
            
        }
        
        public RecordNotFoundException(string message) : base(message)
        {
            
        }
    }
}