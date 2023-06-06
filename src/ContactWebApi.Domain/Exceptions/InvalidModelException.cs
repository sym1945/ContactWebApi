using ContactWebApi.Domain.Models;

namespace ContactWebApi.Domain.Exceptions
{
    public class InvalidModelException : Exception
    {
        private readonly List<ModelError> _ModelErrors;

        public IReadOnlyList<ModelError> ModelErrors => _ModelErrors;

        public InvalidModelException(string? message = null, params ModelError[] modelErrors) : base(message ?? "Invalid models")
        {
            _ModelErrors = new List<ModelError>(modelErrors);
        }
    }
}
