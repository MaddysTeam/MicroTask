using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{

    public interface IRepository<T> where T : class
    {
        T GetById(string id);
        List<T> Get(System.Linq.Expressions.Expression<Func<T,bool>> condition);
        bool Insert(T t);
        bool Update(T t);
        bool Delete(string id);
    }

    public interface IProjectRespository : IRepository<Project>
    {

    }

}
