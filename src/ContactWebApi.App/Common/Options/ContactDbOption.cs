namespace ContactWebApi.App.Common.Options
{
    public class ContactDbOption
    {
        public const string Key = nameof(ContactDbOption);

        public bool UseBulkInsert { get; set; }
        public string ConnectionString { get; set; } = string.Empty;
    }
}
