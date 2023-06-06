using ContactWebApi.App.Common.Extensions;
using ContactWebApi.App.Common.Validator;
using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.Domain.Models;
using System.Text.RegularExpressions;

namespace ContactWebApi.App.Features.Employee.Commands
{
    public class EmployeeDtoValidator : ModelValidatorBase<EmployeeDto>
    {
        private static readonly Regex _EmailRegex = new(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$");
        private static readonly Regex _TelRegex = new(@"^[0-9]{3}[-]+[0-9]{4}[-]+[0-9]{4}$");

        public EmployeeDtoValidator() : base(4)
        {
        }

        public override bool IsValid(EmployeeDto model, out ModelError[] errors)
        {
            var field = string.Empty;

            // Name
            field = nameof(model.Name);
            if (StringExtensions.IsNullOrEmptyOrWhiteSpace(model.Name))
            {
                AddErrorCannotBeBlank(field);
            }
            // Email
            field = nameof(model.Email);
            if (StringExtensions.IsNullOrEmptyOrWhiteSpace(model.Email))
            {
                AddErrorCannotBeBlank(field);
            }
            else if (!_EmailRegex.IsMatch(model.Email))
            {
                AddErrorInvalidFormat(field, model.Email);
            }
            // Tel
            field = nameof(model.Tel);
            if (StringExtensions.IsNullOrEmptyOrWhiteSpace(model.Tel))
            {
                AddErrorCannotBeBlank(field);
            }
            else if (!_TelRegex.IsMatch(model.Tel))
            {
                AddErrorInvalidFormat(field, model.Tel);
            }
            // Joined
            field = nameof(model.Joined);
            if (model.Joined == DateOnly.MinValue)
            {
                AddErrorWrongData(field, model.Joined.ToString("O"));
            }

            errors = _Errors.ToArray();
            return (_Errors.Count == 0);
        }

    }
}
