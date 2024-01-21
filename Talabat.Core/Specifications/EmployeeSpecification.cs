using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class EmployeeSpecification:BaseSpecification<Employee>
    {
        public EmployeeSpecification()
        {
            Includes.Add(E => E.Department);
        }
        public EmployeeSpecification(int id) : base(E=>E.Id == id)
        {
            Includes.Add(E => E.Department);
        }
    }
}
