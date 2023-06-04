using Microsoft.AspNetCore.Mvc;

namespace ContactWebApi.Attributes
{
    public class ImportEmployeesConsumesAttribute : ConsumesAttribute
    {
        public ImportEmployeesConsumesAttribute()
            : base(
                  Constants.ContentTypes.ApplicationJson
                  , Constants.ContentTypes.ApplicationWwwFormUrlEncoded
                  , Constants.ContentTypes.MultipartFormData
                  , Constants.ContentTypes.TextCsv
            )
        {
        }
    }
}
