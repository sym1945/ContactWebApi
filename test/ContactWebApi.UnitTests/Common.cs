﻿using AutoMapper;
using ContactWebApi.App.Features.Employee.DTOs;
using ContactWebApi.App.Features.Employee.Mappers;
using ContactWebApi.App.Features.Employee.Queries;
using System.Reflection;
using System.Text;

namespace ContactWebApi.UnitTests
#nullable disable
{
    public static class Common
    {
        private readonly static IConfigurationProvider _Configuration;
        private readonly static IMapper _Mapper;

        static Common()
        {
            _Configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EmployeeEntityMapper>();
                cfg.AddProfile<EmployeeDtoMapper>();
                cfg.AddProfile<EmployeeLinkDtoMapper>();
            });

            _Mapper = _Configuration.CreateMapper();
        }

        public static EmployeeDto CreateEmployee(string name = "name", string email = "name@gmail.com", string tel = "010-1234-5678", string joined = "2023-06-06")
        {
            var dto = new EmployeeDto()
            {
                Name = name,
                Email = email,
                Tel = tel,
                Joined = DateOnly.TryParse(joined, out DateOnly result) ? result : null
            };

            return dto;
        }

        public static GetEmployeePageRequest CreateEmployeePageRequest(int? page = 1, int? pageSize = 1)
        {
            return new GetEmployeePageRequest { Page = page, PageSize = pageSize };
        }

        public static string CreateString(int length)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < length; ++i)
                sb.Append("x");

            return sb.ToString();
        }

        public static IMapper GetMapper()
        {
            return _Mapper;
        }
    }
}
