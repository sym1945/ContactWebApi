using AutoMapper;
using ContactWebApi.App.Features.Employee.DTOs;


namespace ContactWebApi.App.Features.Employee.Mappers
{
    public class EmployeeEntityMapper : Profile
    {
        public EmployeeEntityMapper()
        {
            CreateMap<EmployeeDto, Domain.Entities.Employee>()
                .ForMember(target => target.Joined, option => option.MapFrom(source => source.Joined.ToDateTime(TimeOnly.MinValue)));
        }

    }
}
