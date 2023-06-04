namespace ContactWebApi.Domain.Exceptions
{
    public class RequestModelInvalidException : Exception
    {
        public RequestModelInvalidException() : base("Invalid request value")
        {
            
        }
    }
}
