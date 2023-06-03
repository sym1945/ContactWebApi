using ContactWebApi.App.Models;
using MediatR;

namespace ContactWebApi.App.Features.Employee.Queries.GetByName
{
    public class GetEmployeeByNameRequest : IRequest<IList<EmployeeDto>>
    {
        public string EmployeeName { get; set; } = string.Empty;
    }
}
