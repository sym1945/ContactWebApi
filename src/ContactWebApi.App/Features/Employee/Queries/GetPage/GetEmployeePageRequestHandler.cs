using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactWebApi.App.Features.Employee.Queries.GetPage
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
            int takeCount = request.PageSize;
            int skipCount = request.Page * takeCount;

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
