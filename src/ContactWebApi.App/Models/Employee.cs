namespace ContactWebApi.App.Models
{
    public class Employee
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public DateOnly Joined { get; set; }
    }
}
