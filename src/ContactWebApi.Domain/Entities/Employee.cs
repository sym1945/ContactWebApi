namespace ContactWebApi.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public DateTime Joined { get; set; }
        public int? GroupId { get; set; }

        public EmployeeGroup? Group { get; set; }
    }

}
