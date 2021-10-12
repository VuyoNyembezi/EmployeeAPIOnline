using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class UsersModel
    {
        public int Id { get; set; }
        public string AdminName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string DepartmentName { get; set; }
        public string CityName { get; set; }
    }
}
