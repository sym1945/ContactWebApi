using ContactWebApi.App.Features.Employee.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeePageRequest : IStreamRequest<EmployeeDto>
    {
        [Required]
        public int? Page { get; set; }
        [Required]
        public int? PageSize { get; set; }
    }
}
