using ContactWebApi.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;


namespace ContactWebApi.Filters.Actions
{
    public class ImportDataTypeActionFilter : IAsyncActionFilter
    {
        private static readonly HashSet<string> _SupportedContentTypes = new HashSet<string>
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

            foreach (var supportedContentType in _SupportedContentTypes)
            {
                if (contentType.StartsWith(supportedContentType))
                    return true;
            }
                
            return false;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!Contains(context.HttpContext.Request.ContentType))
            {
                throw new NotSupportedImportDataType();
            }

            await next();
        }

    }
}
