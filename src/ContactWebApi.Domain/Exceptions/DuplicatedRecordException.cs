namespace ContactWebApi.Domain.Exceptions
{
    public class DuplicatedRecordException : Exception
    {
        public DuplicatedRecordException(string? message = null) : base(message ?? "One or more registered data already exist")
        {
        }
    }
}
