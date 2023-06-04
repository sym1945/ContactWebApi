using MediatR;

namespace ContactWebApi.App.Features.Employee.Commands
{
    public class ImportEmployeeFromTextRequest : IRequest<EmployeeImportResult>
    {
        public string Text { get; init; }

        public ImportEmployeeFromTextRequest(string text)
        {
            Text = text;
        }
    }

}
