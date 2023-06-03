using AutoMapper;
using ContactWebApi.App.Models;
using System.Text;

namespace ContactWebApi.App.Features.Employee.Mappers
{
    public class EmployeeDtoMapper : Profile
    {
        public EmployeeDtoMapper()
        {
            CreateMap<Domain.Entities.Employee, EmployeeDto>()
                .ForMember(target => target.Joined, option => option.MapFrom(source => DateOnly.FromDateTime(source.Joined)))
                .ForMember(target => target.Tel, option => option.MapFrom(source => FormatTel(source.Tel)));
        }

        private static string FormatTel(string value)
        {
            if (value.Length < 11)
                return value;

            var sb = new StringBuilder();
            sb.Append(value[0]);
            sb.Append(value[1]);
            sb.Append(value[2]);
            sb.Append('-');
            sb.Append(value[3]);
            sb.Append(value[4]);
            sb.Append(value[5]);
            sb.Append(value[6]);
            sb.Append('-');
            sb.Append(value[7]);
            sb.Append(value[8]);
            sb.Append(value[9]);
            sb.Append(value[10]);

            return sb.ToString();
        }
    }
}
