using ContactWebApi.App.Common.Validator;
using ContactWebApi.Domain.Models;

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
            else if (model.Page < 1)
            {
                AddErrorLessThan(field, 1);
            }
            // pageSize
            field = nameof(model.PageSize);
            if (!model.PageSize.HasValue)
            {
                AddErrorRequired(field);
            }
            else if (model.PageSize < 1 && model.PageSize > 100)
            {
                AddErrorRange(field, 1, 100);
            }

            errors = _Errors.ToArray();
            return (_Errors.Count == 0);
        }

    }
}
