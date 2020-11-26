using Castle.Core.Logging;
using Couchbase;
using Couchbase.Core.Exceptions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapi_project_1.Interface;
using Webapi_project_1.Model;

namespace Webapi_project_1.Services
{
    public class CouchbaseService : ICouchbaseService
    {
        private readonly ILogger<CouchbaseService> logger;

        public CouchbaseService(ILogger<CouchbaseService> logger)
        {
            this.logger = logger;
        }
        public async Task<List<LoginDetails>> GetLoginDetails(ICluster cluster)
        {
            try
            {
                var queryResult = await cluster.QueryAsync<LoginDetails>("SELECT department,logintype,id,photo,username,pwd FROM LoginDetails ", new Couchbase.Query.QueryOptions());

                List<LoginDetails> empList = new List<LoginDetails>();

                await foreach (var row in queryResult)
                {
                    empList.Add(row);
                }
               
                return empList;

            }
            catch (IndexFailureException)
            {
                logger.LogError("Bucket not found");
                throw;
            }
            
        }

    
      

        public async Task<ICluster> Initialize()
        {
            try
            {
                var policy = Policy.Handle<Exception>()
                   .WaitAndRetryAsync(2, count => TimeSpan.FromSeconds(3));
                await policy.ExecuteAsync(async () =>
                {
                             logger.LogInformation("Retrying to connect Couchbase for LoginController...");
                    await retry();
                    });
                return await retry();
            }
            catch (AuthenticationFailureException)
            {
                logger.LogError("Authentication error");
                throw ;
            }   
        }
        public async Task<ICluster> retry()
        {
            var cluster = await Cluster.ConnectAsync("couchbase://localhost", "adm", "sanjay753");
            return cluster;
        }
        public async Task<LoginDetails> PostLogin(ICluster cluster, LoginDetails form)
        {
            try
            {

                var bucket = await cluster.BucketAsync("LoginDetails");
                    var collection = bucket.DefaultCollection();
                    var idvalue = form.id;
                    await collection.InsertAsync(idvalue.ToString(), form);
                   
            
                return null;

            }

            catch (IndexFailureException)
            {
                logger.LogError("Bucket not found");
                throw;
            }
    
        }
    }
}
