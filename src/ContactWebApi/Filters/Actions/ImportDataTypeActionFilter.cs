using ContactWebApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactWebApi.Filters.Actions
{
    public class ImportDataTypeActionFilter : IAsyncActionFilter
    {
        public static readonly string[] SupportedContentTypes = new string[]
        {
            Constants.ContentTypes.ApplicationJson
            , Constants.ContentTypes.ApplicationWwwFormUrlEncoded
            , Constants.ContentTypes.MultipartFormData
            , Constants.ContentTypes.TextCsv
        };

        private static bool Contains(string? contentType)
        {
            if (contentType == null)
                return false;

            return SupportedContentTypes.Any(x => contentType.StartsWith(x));
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var contentType = context.HttpContext.Request.ContentType;

            if (!Contains(contentType))
            {
                throw new UnsupportedImportContentTypeException(contentType, SupportedContentTypes);
            }

            await next();
        }

    }
}
