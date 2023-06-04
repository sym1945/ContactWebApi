using System.Text;

namespace ContactWebApi.App.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmptyOrWhiteSpace(this string? value)
        {
            return (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value));
        }

        public static string FormatTel(this string value)
        {
            if (value.Length != 11)
                return value;

            var sb = new StringBuilder();
            sb.Append(value[0]);
            sb.Append(value[1]);
            sb.Append(value[2]);
            sb.Append('-');
            sb.Append(value[3]);
            sb.Append(value[4]);
            sb.Append(value[5]);
            sb.Append(value[6]);
            sb.Append('-');
            sb.Append(value[7]);
            sb.Append(value[8]);
            sb.Append(value[9]);
            sb.Append(value[10]);

            return sb.ToString();
        }
    }
}
