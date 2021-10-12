using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccessLayer.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string DateOfBirth { get; set; }
        public int FkGenderId { get; set; }
        public int FkNationId { get; set; }
        public bool? IsActive { get; set; }

        public virtual Gender FkGender { get; set; }
        public virtual Nationality FkNation { get; set; }
    }
}
