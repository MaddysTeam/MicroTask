using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    public interface IRepository<T> where T : class
    {
        T GetById(string id);
        List<T> GetAll();
        List<T> Get(System.Linq.Expressions.Expression<Func<T,bool>> condition);
        bool Insert(T t);
        bool Update(T t);
        bool Delete(string id);
    }

    public interface ITaskRespository : IRepository<Task>
    {

    }

}
