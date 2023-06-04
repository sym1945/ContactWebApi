using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Features.Employee.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeeByGroupIdRequestHandler : IStreamRequestHandler<GetEmployeeByGroupIdRequest, EmployeeDto>
    {
        private readonly IContactDbContext _Context;
        private readonly IMapper _Mapper;

        public GetEmployeeByGroupIdRequestHandler(IContactDbContext context, IMapper mapper)
        {
            _Context = context;
            _Mapper = mapper;
        }


        public IAsyncEnumerable<EmployeeDto> Handle(GetEmployeeByGroupIdRequest request, CancellationToken cancellationToken)
        {
            return _Context.Employees
                        .AsNoTracking()
                        .Where(employee => employee.GroupId == request.GroupId)
                        .ProjectTo<EmployeeDto>(_Mapper.ConfigurationProvider)
                        .AsAsyncEnumerable();
        }
    }
}
