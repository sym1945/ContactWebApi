﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Features.Employee.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeeByNameRequestHandler : IRequestHandler<GetEmployeeByNameRequest, IList<EmployeeDto>>
    {
        private readonly IContactDbContext _Context;
        private readonly IMapper _Mapper;

        public GetEmployeeByNameRequestHandler(IContactDbContext context, IMapper mapper)
        {
            _Context = context;
            _Mapper = mapper;
        }

        public async Task<IList<EmployeeDto>> Handle(GetEmployeeByNameRequest request, CancellationToken cancellationToken)
        {
            // TODO: request validation check

            var result = await _Context.Employees
                                .AsNoTracking()
                                .Where(employee => employee.Name == request.EmployeeName)
                                .ProjectTo<EmployeeDto>(_Mapper.ConfigurationProvider)
                                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
