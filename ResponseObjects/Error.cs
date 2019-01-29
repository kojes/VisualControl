namespace VueExample.ResponseObjects
{
    public class Error
    {
        public string Message { get; }
        public string Danger { get; }
        public string Code { get; }

        public Error(string message, string code = "A000", string danger = "A" )
        {
            Message = message;
            Danger = danger;
            Code = code;
        }

       


    }
}
