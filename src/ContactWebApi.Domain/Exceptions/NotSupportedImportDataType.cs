namespace ContactWebApi.Domain.Exceptions
{
    public class NotSupportedImportDataType : Exception
    {
        public NotSupportedImportDataType(string? message = null) : base(message)
        {
        }
    }
}
