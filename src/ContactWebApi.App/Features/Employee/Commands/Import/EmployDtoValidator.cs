using ContactWebApi.App.Common.Extensions;
using ContactWebApi.App.Common.Validator;
using ContactWebApi.App.Features.Employee.DTOs;
using System.Text.RegularExpressions;

namespace ContactWebApi.App.Features.Employee.Commands
{
    public class EmployDtoValidator : IModelValidator<EmployeeDto>
    {
        private static readonly Regex _EmailRegex = new Regex(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$");
        private static readonly Regex _TelRegex = new Regex(@"^[0-9]{3}[-]+[0-9]{4}[-]+[0-9]{4}$");

        public bool IsValid(EmployeeDto model)
        {
            if (StringExtensions.IsNullOrEmptyOrWhiteSpace(model.Name))
                return false;
            if (StringExtensions.IsNullOrEmptyOrWhiteSpace(model.Email))
                return false;
            if (!_EmailRegex.IsMatch(model.Email))
                return false;
            if (StringExtensions.IsNullOrEmptyOrWhiteSpace(model.Tel))
                return false;
            if (!_TelRegex.IsMatch(model.Tel))
                return false;
            if (model.Joined == DateOnly.MinValue)
                return false;

            return true;
        }
    }
}
