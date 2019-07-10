namespace VueExample.ResponseObjects
{
    public class Error
    {
        public string Message { get; }
        public string Code { get; }

        public Error(string message, string code = "A000")
        {
            Message = message;
            Code = code;
        }

        public Error()
        {
            Message = "No Errors";
            Code = "AAA";
        }
    
    }
}
