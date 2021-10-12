using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class PassKeyModel
    {
        public string Email { get; set; }
        public string ResetPasswordKey { get; set; }
    }
}
