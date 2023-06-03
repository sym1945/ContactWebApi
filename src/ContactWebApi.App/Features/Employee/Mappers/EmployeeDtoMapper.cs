using AutoMapper;
using ContactWebApi.App.Features.Employee.DTOs;

namespace ContactWebApi.App.Features.Employee.Mappers
{
    public class EmployeeDtoMapper : Profile
    {
        public EmployeeDtoMapper()
        {
            CreateMap<Domain.Entities.Employee, EmployeeDto>()
                .ForMember(target => target.Joined, option => option.MapFrom(source => DateOnly.FromDateTime(source.Joined)));
        }

    }
}
