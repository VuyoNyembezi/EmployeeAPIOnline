using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Administration
{
    public class Response
    {
        public int? ReturnId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public int? Role { get; set; }

        public string Token { get; set; }
    }
}
