using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AdminName { get; set; }
        public int CityID { get; set; }
        public int DepartmentID { get; set; }

        public string VerificationCode { get; set; }
        public int RoleID { get; set; }
    }
}
