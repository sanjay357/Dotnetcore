using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webapi_project_1.Interface;
using Webapi_project_1.Model;
using Microsoft.AspNet.OData;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Webapi_project_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeCouchbaseService _service;
        private readonly ILogger<EmployeeController> logger;

        public EmployeeController(IEmployeeCouchbaseService service, ILogger<EmployeeController> logger)
        {
            _service = service;

            this.logger = logger;
            logger.LogInformation( "NLog injected into HomeController");
        }
        // GET: api/<EmployeeController>
        [HttpGet]
       [EnableQuery()]
        public async Task<List<Employees>> Get()
        { 
            logger.LogInformation("geting  all Employees");
            var couchClient = await _service.Initialize();
            var employees = await _service.GetEmployees(couchClient);
            if (employees == null)
            {
                logger.LogWarning("Can't get Employees");

            }
            return employees;
           
        }



        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        //[HttpGet("{department}/{id}")]
        public async Task<Employees> Get(int id)
        {
            
            var couchClient = await _service.Initialize();
            var employees = await _service.GetEmployeById(couchClient, id);
            if (employees.id == 0)
            {
                logger.LogWarning($"Employee With Id- {id} not found");
                logger.LogError("This is an error");
                return null;
            }
            return employees;


        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task Post([FromBody] Employees value)
        {
            logger.LogInformation($"Posting this  {value} Employee");
            var couchClient = await _service.Initialize();
            await _service.PostEmploye(couchClient, value);

    

        } 

        //update
        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public async Task<Employees> Put(int id, [FromBody] Employees value)
        {
            logger.LogInformation($"Updating  this Id-{id}");
            var couchClient = await _service.Initialize();
            await _service.PutEmployeById(couchClient, id,value);

            return null;
           
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public async Task<Employees> Delete(int id)
        {
            logger.LogInformation($"Deleting this Id-{id}");
            var couchClient = await _service.Initialize();
            await _service.DeleteEmployeById(couchClient, id);

            return null;


        }
    }
}
