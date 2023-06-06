using ContactWebApi.App.Common.Validator;
using ContactWebApi.Domain.Models;
using static ContactWebApi.App.Constants.Employee.EmployeePaging;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeePageRequestValidator : ModelValidatorBase<GetEmployeePageRequest>
    {
        public GetEmployeePageRequestValidator() : base(2)
        {
        }

        public override bool IsValid(GetEmployeePageRequest model, out ModelError[] errors)
        {
            var field = string.Empty;

            // page
            field = nameof(model.Page);
            if (!model.Page.HasValue)
            {
                AddErrorRequired(field);
            }
            else if (model.Page < PageMin)
            {
                AddErrorLessThan(field, PageMin);
            }
            // pageSize
            field = nameof(model.PageSize);
            if (!model.PageSize.HasValue)
            {
                AddErrorRequired(field);
            }
            else if (model.PageSize < PageMin && model.PageSize > PageMax)
            {
                AddErrorRange(field, PageMin, PageMax);
            }

            errors = _Errors.ToArray();
            return (_Errors.Count == 0);
        }

    }
}
