using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class AdminPasswordResetModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string VerificationCode { get; set; }
    }
}
