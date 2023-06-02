using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactWebApi.Domain.Entities
{
    public class EmployeeGroup
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }

        public IList<Employee>? Employees { get; set; }
    }
}
