using ContactWebApi.Domain.Enums;
using MediatR;

namespace ContactWebApi.App.Features.Employee.Commands
{
    public class ImportEmployeeFromTextRequest : IRequest<EmployeeImportResult>
    {
        public EImportDataType DataType { get; init; }
        public string Text { get; init; }

        public ImportEmployeeFromTextRequest(EImportDataType dataType, string text)
        {
            DataType = dataType;
            Text = text;
        }
    }

}
