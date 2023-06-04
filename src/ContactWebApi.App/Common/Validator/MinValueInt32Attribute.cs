using System.ComponentModel.DataAnnotations;

namespace ContactWebApi.App.Common.Validator
{
    public class MinValueInt32Attribute : ValidationAttribute
    {
        private readonly int _MinValue;

        public MinValueInt32Attribute(int min)
        {
            _MinValue = min;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The field {name} can not be less than {_MinValue}";
        }

        public override bool IsValid(object? value)
        {
            if (value is not int num)
                return false;

            return num >= _MinValue;
        }
    }
}
