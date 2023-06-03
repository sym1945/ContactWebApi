using ContactWebApi.App.Features.Employee.DTOs;
using MediatR;

namespace ContactWebApi.App.Features.Employee.Queries.GetPage
{
    public class GetEmployeePageRequest : IStreamRequest<EmployeeDto>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
