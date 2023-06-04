using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Features.Employee.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeeByIdRequestHandler : IRequestHandler<GetEmployeeByIdRequest, EmployeeDto?>
    {
        private readonly IContactDbContext _Context;
        private readonly IMapper _Mapper;

        public GetEmployeeByIdRequestHandler(IContactDbContext context, IMapper mapper)
        {
            _Context = context;
            _Mapper = mapper;
        }


        public async Task<EmployeeDto?> Handle(GetEmployeeByIdRequest request, CancellationToken cancellationToken)
        {
            var result = await _Context.Employees
                                .AsNoTracking()
                                .Where(employee => employee.Id == request.EmployeeId)
                                .ProjectTo<EmployeeDto>(_Mapper.ConfigurationProvider)
                                .FirstOrDefaultAsync(cancellationToken);
            return result;
        }
    }
}
