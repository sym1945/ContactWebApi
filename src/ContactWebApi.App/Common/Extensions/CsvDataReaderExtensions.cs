using Sylvan.Data.Csv;

namespace ContactWebApi.App.Common.Extensions
{
    public static class CsvDataReaderExtensions
    {
        public static DateOnly? GetDateNullable(this CsvDataReader reader, int ordinal)
        {
            try
            {
                return reader.GetDate(ordinal);
            }
            catch
            {
                return null;
            }
        }
    }
}
