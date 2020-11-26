using Couchbase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapi_project_1.Model;

namespace Webapi_project_1.Interface
{
    public interface IEmployeeCouchbaseService
{
        Task<ICluster> Initialize();
        Task<List<Employees>> GetEmployees(ICluster cluster);
        Task<Employees> GetEmployeById(ICluster cluster, int id);
        Task<Employees> DeleteEmployeById(ICluster cluster, int id);
        Task<Employees> PostEmploye(ICluster cluster, Employees value );
        Task<Employees> PutEmployeById(ICluster cluster, int id,Employees value);

        //  Task<Employees> PostEmployeValue(ICluster cluster, string value);

    }
}
