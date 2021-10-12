using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ChangeUserRoleModel
    {
        public int Id { get; set; }
        public int FkRoleId { get; set; }
    }
}
