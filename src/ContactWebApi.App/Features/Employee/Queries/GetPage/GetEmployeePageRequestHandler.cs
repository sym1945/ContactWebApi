using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Features.Employee.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeePageRequestHandler : IStreamRequestHandler<GetEmployeePageRequest, EmployeeDto>
    {
        private readonly IContactDbContext _Context;
        private readonly IMapper _Mapper;

        public GetEmployeePageRequestHandler(IContactDbContext context, IMapper mapper)
        {
            _Context = context;
            _Mapper = mapper;
        }

        public IAsyncEnumerable<EmployeeDto> Handle(GetEmployeePageRequest request, CancellationToken cancellationToken)
        {
            // TODO: request validation check
            // page > 0
            // 0 < pageSize < 100

            int takeCount = request.PageSize.Value;
            int skipCount = (request.Page.Value - 1) * takeCount;

            return _Context.Employees
                        .AsNoTracking()
                        .OrderBy(employee => employee.Name)
                        .Skip(skipCount)
                        .Take(takeCount)
                        .ProjectTo<EmployeeDto>(_Mapper.ConfigurationProvider)
                        .AsAsyncEnumerable();
        }
    }
}
