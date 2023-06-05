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


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var contentType = context.HttpContext.Request.ContentType;

            if (contentType == null
                || !_SupportedContentTypes.Contains(contentType))
            {
                throw new NotSupportedImportDataType();
            }

            await next();
        }

    }
}
