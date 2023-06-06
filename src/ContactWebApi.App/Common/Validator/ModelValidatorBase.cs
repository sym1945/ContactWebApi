using ContactWebApi.Domain.Models;
using System.Text;

namespace ContactWebApi.App.Common.Validator
{
    public abstract class ModelValidatorBase<T> : IModelValidator<T>
    {
        protected readonly List<ModelError> _Errors;

        public ModelValidatorBase(int? capacity = null)
        {
            if (!capacity.HasValue)
                _Errors = new List<ModelError>();
            else
                _Errors = new List<ModelError>(capacity.Value);
        }

        public abstract bool IsValid(T model, out ModelError[] errors);

        protected void AddError(string name, string description)
        {
            _Errors.Add(new ModelError(name, description));
        }

        protected void AddErrorLessThan(string name, int min)
        {
            AddError(name, $"The field {name} can not be less than {min}");
        }

        protected void AddErrorRequired(string name)
        {
            AddError(name, $"The {name} filed is required.");
        }

        protected void AddErrorRange(string name, int min, int max)
        {
            AddError(name, $"The field {name} must be between {min} and {max}.");
        }

        protected void AddErrorCannotBeBlank(string name)
        {
            AddError(name, $"The {name} field cannot be blank");
        }

        protected void AddErrorInvalidFormat(string name, string? value = null)
        {
            var sb = new StringBuilder($"The {name} field is invalid format");
            if (value != null)
                sb.Append($" : {{ '{value}' }}");

            AddError(name, sb.ToString());
        }

        protected void AddErrorWrongData(string name, string? value = null)
        {
            var sb = new StringBuilder($"The {name} field is wrong data");
            if (value != null)
                sb.Append($" : {{ '{value}' }}");

            AddError(name, sb.ToString());
        }

    }
}
