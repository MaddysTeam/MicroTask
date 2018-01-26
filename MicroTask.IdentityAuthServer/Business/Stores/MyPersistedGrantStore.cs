using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using MicroTask.IdentityAuthServer;
using Steeltoe.Common.Discovery;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business
{

    /// <summary>
    ///  这里是关于授权的持久化逻辑，
    ///  step 1: 得在Startup.cs中 ConfigureService中调用
    ///  step 2: 自动覆盖默认的持久化逻辑
    /// </summary>
    public class MyPersistedGrantStore : IPersistedGrantStore
    {
        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            throw new System.NotImplementedException();
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveAsync(string key)
        {
            throw new System.NotImplementedException();
        }

        public Task StoreAsync(PersistedGrant grant)
        {
            throw new System.NotImplementedException();
        }
    }

}
