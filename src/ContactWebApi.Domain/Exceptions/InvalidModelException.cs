using ContactWebApi.Domain.Models;

namespace ContactWebApi.Domain.Exceptions
{
    public class InvalidModelException : Exception
    {
        private readonly Dictionary<string, string> _ModelErrors;

        public IReadOnlyDictionary<string, string> ModelErrors => _ModelErrors;

        public InvalidModelException(string? message = null, params ModelError[] modelErrors) : base(message ?? "Invalid models")
        {
            _ModelErrors = new Dictionary<string, string>();

            for (int i = 0; i < modelErrors.Length; ++i)
            {
                var modelError = modelErrors[i];
                _ModelErrors[modelError.Name] = modelError.Description;
            }
            
        }
    }
}
