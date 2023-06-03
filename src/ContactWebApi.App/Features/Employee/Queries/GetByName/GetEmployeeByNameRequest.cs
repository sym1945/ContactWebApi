using ContactWebApi.App.Features.Employee.DTOs;
using MediatR;

namespace ContactWebApi.App.Features.Employee.Queries.GetByName
{
    public class GetEmployeeByNameRequest : IRequest<IList<EmployeeDto>>
    {
        public string EmployeeName { get; set; } = string.Empty;
    }
}
