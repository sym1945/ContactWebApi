using System.Text;

namespace ContactWebApi.Domain.Exceptions
{
    public class UnsupportedImportContentTypeException : Exception
    {
        public UnsupportedImportContentTypeException(string? reqeustContentType, params string[] supportedContentTypes) : base(CreateErrorMessage(reqeustContentType, supportedContentTypes))
        {
        }

        private static string CreateErrorMessage(string? reqeustContentType, params string[] supportedContentTypes)
        {
            var sb = new StringBuilder(50)
                .Append($"'{reqeustContentType ?? "(null)"}' is an unsupported ContentType.");

            if (supportedContentTypes.Length > 0)
            {
                sb.Append(" supported: [ ");

                for (int i = 0; i < supportedContentTypes.Length; ++i)
                { 
                    sb.Append($"'{supportedContentTypes[i]}'");
                    if ((i + 1) < supportedContentTypes.Length)
                        sb.Append(", ");
                }

                sb.Append(" ]");
            }

            return sb.ToString();
        }
    }
}
