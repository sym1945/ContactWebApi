using ContactWebApi.App.Features.Employee.DTOs;
using MediatR;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeeByIdRequest : IRequest<EmployeeDto>
    {
        public int EmployeeId { get; set; }
    }
}
