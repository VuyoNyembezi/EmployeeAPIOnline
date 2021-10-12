using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccessLayer.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AdminName { get; set; }
        public int FkCityId { get; set; }
        public int FkDepartmentId { get; set; }
        public string VerificationCode { get; set; }
        public int FkRoleId { get; set; }
        public string ResetPasswordKey { get; set; }
        public DateTime? PassKeyCreated { get; set; }

        public virtual City FkCity { get; set; }
        public virtual Department FkDepartment { get; set; }
        public virtual Role FkRole { get; set; }
    }
}
