namespace ContactWebApi.Domain.Models
{
    public class ModelError
    {
        public string Name { get; init; }
        public string Description { get; init; }

        public ModelError(string name, string desc)
        {
            Name = name;
            Description = desc;
        }

    }
}
