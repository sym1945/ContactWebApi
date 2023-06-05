namespace ContactWebApi.Domain.Exceptions
{
    public class DuplicatedRecordException : Exception
    {
        public DuplicatedRecordException(string? message = null) : base(message)
        {
        }
    }
}
