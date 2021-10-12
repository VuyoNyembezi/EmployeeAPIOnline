using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccessLayer.Models
{
    public partial class Nationality
    {
        public Nationality()
        {
            Employees = new HashSet<Employee>();
        }

        public int NationId { get; set; }
        public string NationalityGroup { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
