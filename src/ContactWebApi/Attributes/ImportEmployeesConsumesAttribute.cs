using ContactWebApi.Filters.Actions;
using Microsoft.AspNetCore.Mvc;


namespace ContactWebApi.Attributes
{
    public class ImportEmployeesConsumesAttribute : ServiceFilterAttribute
    {
        public ImportEmployeesConsumesAttribute() : base(typeof(ImportDataTypeActionFilter))
        {
        }
    }
}
