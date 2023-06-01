using ContactWebApi.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWebApi.App.Parsers
{
    public class EmployeeJsonParser : IEmployeeParser
    {
        public IList<Employee> Parse(string text)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Employee> Parse(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}
