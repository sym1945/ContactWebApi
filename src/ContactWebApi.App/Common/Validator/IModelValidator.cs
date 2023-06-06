using ContactWebApi.Domain.Models;

namespace ContactWebApi.App.Common.Validator
{
    public interface IModelValidator<TModel>
    {
        bool IsValid(TModel model, out ModelError[] errors);
    }
}
