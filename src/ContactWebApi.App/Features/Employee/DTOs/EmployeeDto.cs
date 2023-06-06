namespace ContactWebApi.App.Features.Employee.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public DateOnly? Joined { get; set; }
    }
}
