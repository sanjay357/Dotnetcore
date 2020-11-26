using Castle.Components.DictionaryAdapter;
using Castle.Core.Logging;
using Couchbase;
using Couchbase.Core.Exceptions;
using Couchbase.KeyValue;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapi_project_1.Interface;
using Webapi_project_1.Model;

namespace Webapi_project_1.Services
{
    public class EmployeeCouchbaseService : IEmployeeCouchbaseService
    {
        private readonly ILogger<EmployeeCouchbaseService> logger;

        public EmployeeCouchbaseService(ILogger<EmployeeCouchbaseService> logger)
        {
            this.logger = logger;
        }
        public async Task<Employees> DeleteEmployeById(ICluster cluster, int id)
        {
            try
            {
                //  var queryResult = await cluster.QueryAsync<Employees>("SELECT username,name,email,age,location,srccs,phoneNumber,salary,skill,managerName,address,id FROM Employees where id= "  + id.ToString()  , new Couchbase.Query.QueryOptions());
                var bucket = await cluster.BucketAsync("Employees");


                var collection = bucket.DefaultCollection();

                await collection.RemoveAsync(id.ToString());
                return null;
            }
            catch (BucketNotFoundException)
            {
                logger.LogError("Bucket not found");
                throw;
            }
        
        }

        public async Task<Employees> GetEmployeById(ICluster cluster, int id)
        {
            Employees employee = new Employees();
            var queryResult = await cluster.QueryAsync<Employees>("SELECT username,name,email,age,location,srccs,phoneNumber,salary,skill,managerName,address,id FROM Employees where id=" + id, new Couchbase.Query.QueryOptions());
         
            

            await foreach (var row in queryResult)
            {
                employee = row;
            }
            if (employee == null)
            {
                logger.LogWarning($"Id with {id} not available in the bucket");
            }

            return employee;
        }
        


        public async Task<List<Employees>> GetEmployees(ICluster cluster)
        {
            try
            {
                var queryResult = await cluster.QueryAsync<Employees>("SELECT username,name,email,age,location,srccs,phoneNumber,salary,skill,managerName,address,id FROM Employees ", new Couchbase.Query.QueryOptions());

                List<Employees> empList = new List<Employees>();

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

                    logger.LogInformation("Retrying to connect couchbase for EmployeeController...");
                    await retry();
                });
                return await retry();
            }

            catch (AuthenticationFailureException)
            {
                logger.LogError("Authentication error");
                throw;
            }

        }
        public async Task<ICluster> retry()
        {
            var cluster = await Cluster.ConnectAsync("couchbase://localhost", "adm", "sanjay753");
            return cluster;
        }

        public async Task<Employees> PutEmployeById(ICluster cluster ,int id, Employees value)
        {
            var bucket = await cluster.BucketAsync("Employees");
            var collection = bucket.DefaultCollection();
            var collectiondata = await collection.UpsertAsync(id.ToString(),value);

            if (collectiondata == null)
            {
                logger.LogError("Error in update");
            }
            return null;

        }
        //create
        public async Task<Employees> PostEmploye(ICluster cluster, Employees value)
        {

            var bucket = await cluster.BucketAsync("Employees");
            var collection = bucket.DefaultCollection();
            int idvalue = value.id;
            var collectiondataa= await collection.InsertAsync( idvalue.ToString(),value );
            if (collectiondataa == null)
            {
                logger.LogError("Error in creating");
            }

            return null;
        }

       
    }
}
