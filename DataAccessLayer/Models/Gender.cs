using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccessLayer.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Employees = new HashSet<Employee>();
        }

        public int GenderId { get; set; }
        public string GenderType { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
