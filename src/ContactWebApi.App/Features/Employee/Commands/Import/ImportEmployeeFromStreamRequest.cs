using ContactWebApi.Domain.Enums;
using MediatR;

namespace ContactWebApi.App.Features.Employee.Commands.Import
{
    public class ImportEmployeeFromStreamRequest : IRequest<int>
    {
        public EImportDataType DataType { get; init; }
        public Stream DataStream { get; init; }

        public ImportEmployeeFromStreamRequest(EImportDataType dataType, Stream stream)
        {
            DataType = dataType;
            DataStream = stream;
        }

    }
}
