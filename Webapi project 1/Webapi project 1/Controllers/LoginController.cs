using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webapi_project_1.Interface;
using Webapi_project_1.Model;
using Microsoft.AspNet.OData;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace Webapi_project_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ICouchbaseService _service;
        private readonly ILogger<LoginController> logger;

        public LoginController(ICouchbaseService Service, ILogger<LoginController> logger)
        {
            this._service = Service;
            this.logger = logger;
        }
        // GET: api/<LoginController>
        [HttpGet]
       [EnableQuery()]
        public async Task<List<LoginDetails>> Get()
        {logger.LogInformation("Getting all Login details");
            var couchClient = await _service.Initialize();
            var loginDetails = await _service.GetLoginDetails(couchClient);
            if (loginDetails == null)
            {
                logger.LogError("Error in getting Login details");
            }
            return loginDetails;
        }

        // POST api/<LoginController>
        [HttpPost]
        public async Task Post([FromBody] LoginDetails form)
        {
            var couchClient = await _service.Initialize();
           var logindata= await _service.PostLogin(couchClient, form);
            if (logindata != null)
            {
                logger.LogInformation("Successfully posted");
            }

        }

    }
}
