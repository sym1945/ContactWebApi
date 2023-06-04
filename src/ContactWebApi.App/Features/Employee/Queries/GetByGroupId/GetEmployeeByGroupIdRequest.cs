using ContactWebApi.App.Features.Employee.DTOs;
using MediatR;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeeByGroupIdRequest : IStreamRequest<EmployeeDto>
    {
        public int GroupId { get; set; }
    }
}
