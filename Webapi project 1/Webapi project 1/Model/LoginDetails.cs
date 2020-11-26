using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapi_project_1.Model
{
    public class LoginDetails
    {
        public int id { get; set; }
        public string pwd { get; set; }
        public string username { get; set; }
        public string department { get; set; }
        public string photo { get; set; }
        public string logintype { get; set; }

    }
}
