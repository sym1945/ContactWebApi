using ContactWebApi.App.Common.Validator;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeePageRequestValidator : IModelValidator<GetEmployeePageRequest>
    {
        public bool IsValid(GetEmployeePageRequest model)
        {
            if (!model.Page.HasValue 
                || !model.PageSize.HasValue)
                return false;

            if (model.Page < 1)
                return false;

            if (model.PageSize < 1 && model.PageSize > 100)
                return false;

            return true;
        }
    }
}
