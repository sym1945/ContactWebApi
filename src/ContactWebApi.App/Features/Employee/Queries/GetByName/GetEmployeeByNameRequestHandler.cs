using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Features.Employee.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeeByNameRequestHandler : IStreamRequestHandler<GetEmployeeByNameRequest, EmployeeLinkDto>
    {
        private readonly IContactDbContext _Context;
        private readonly IMapper _Mapper;

        public GetEmployeeByNameRequestHandler(IContactDbContext context, IMapper mapper)
        {
            _Context = context;
            _Mapper = mapper;
        }

        public IAsyncEnumerable<EmployeeLinkDto> Handle(GetEmployeeByNameRequest request, CancellationToken cancellationToken)
        {
            return _Context.Employees
                        .AsNoTracking()
                        .Where(employee => employee.Name == request.EmployeeName)
                        .ProjectTo<EmployeeLinkDto>(_Mapper.ConfigurationProvider)
                        .AsAsyncEnumerable();
        }
    }
}
