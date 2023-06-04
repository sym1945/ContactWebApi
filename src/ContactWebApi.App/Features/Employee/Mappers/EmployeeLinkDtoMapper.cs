using AutoMapper;
using ContactWebApi.App.Features.Employee.DTOs;

namespace ContactWebApi.App.Features.Employee.Mappers
{
    public class EmployeeLinkDtoMapper : Profile
    {
        public EmployeeLinkDtoMapper()
        {
            CreateMap<Domain.Entities.Employee, EmployeeLinkDto>()
                .ForMember(target => target.Joined, option => option.MapFrom(source => DateOnly.FromDateTime(source.Joined)));
        }

    }
}
