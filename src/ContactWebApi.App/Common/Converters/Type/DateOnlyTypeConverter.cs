using System.Globalization;

namespace ContactWebApi.App.Common.Converters
{
    public class DateOnlyTypeConverter : StringTypeConverterBase<DateOnly?>
    {
        protected override DateOnly? Parse(string s, IFormatProvider? provider) => DateOnly.TryParse(s, provider, DateTimeStyles.None, out DateOnly result) ? result : null;

        protected override string ToIsoString(DateOnly? source, IFormatProvider? provider) => source?.ToString("O", provider) ?? string.Empty;
    }
}
