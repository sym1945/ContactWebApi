using AutoMapper;
using AutoMapper.QueryableExtensions;
using ContactWebApi.App.Common.Interfaces;
using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.Domain.Exceptions;
using ContactWebApi.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ContactWebApi.App.Features.Employee.Queries
{
    public class GetEmployeePageRequestHandler : IRequestHandler<GetEmployeePageRequest, GetEmployeePageResponse>
    {
        private readonly IContactDbContext _Context;
        private readonly IMapper _Mapper;

        public GetEmployeePageRequestHandler(IContactDbContext context, IMapper mapper)
        {
            _Context = context;
            _Mapper = mapper;
        }

        public async Task<GetEmployeePageResponse> Handle(GetEmployeePageRequest request, CancellationToken cancellationToken)
        {
            var validator = new GetEmployeePageRequestValidator();
            if (!validator.IsValid(request, out ModelError[] errors))
                throw new InvalidModelException(modelErrors: errors);

            var pageNo = request.Page!.Value;
            var pageSize = request.PageSize!.Value;

            var totalRecordCount = await _Context.Employees.AsNoTracking().CountAsync(cancellationToken);
            var totalPageCount = (int)Math.Ceiling((double)totalRecordCount / pageSize);

            var items = await _Context.Employees
                                .AsNoTracking()
                                .OrderBy(employee => employee.Name)
                                .Skip((pageNo - 1) * pageSize)
                                .Take(pageSize)
                                .ProjectTo<EmployeeDto>(_Mapper.ConfigurationProvider)
                                .ToListAsync(cancellationToken);

            var response = new GetEmployeePageResponse(items, pageNo, totalPageCount, pageSize, request.GetPageUriCreator());

            return response;
        }

    }
}
