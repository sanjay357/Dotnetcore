using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapi_project_1.Model
{
    public class Employees
    {
        public int id { get; set; }
        public string srccs { get; set; }
        public string name { get; set; }
        public int age { get; set; }
        public string location { get; set; }
        public string address { get; set; }
        public Double phoneNumber { get; set; }
        public int salary { get; set; }
        public string skill { get; set; }
        public string managerName { get; set; }

        public string email { get; set; }
    }
}
