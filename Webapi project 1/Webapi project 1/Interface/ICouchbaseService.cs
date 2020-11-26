using Couchbase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webapi_project_1.Model;

namespace Webapi_project_1.Interface
{
    public interface ICouchbaseService
    {
        Task<ICluster> Initialize();
        Task<List<LoginDetails>> GetLoginDetails(ICluster cluster);
        Task<LoginDetails> PostLogin(ICluster cluster, LoginDetails form);


    }
}
