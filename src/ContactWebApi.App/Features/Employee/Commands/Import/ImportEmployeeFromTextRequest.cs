using MediatR;

namespace ContactWebApi.App.Features.Employee.Commands.Import
{
    public class ImportEmployeeFromTextRequest : IRequest<int>
    {
        public string Text { get; init; }

        public ImportEmployeeFromTextRequest(string text)
        {
            Text = text;
        }
    }

}
