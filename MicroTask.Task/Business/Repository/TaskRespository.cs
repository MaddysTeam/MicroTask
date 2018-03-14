using Chloe.MySql;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using System.Linq.Expressions;

namespace Business
{

    public class TaskRespository : ITaskRespository
    {

        public TaskRespository(MySqlContext context)
        {
            this.context = context;
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public List<Task> GetAll()
        {
            var q = context.Query<Task>();

            return q.ToList();
        }

        public Task GetById(string id)
        {
            throw new NotImplementedException();
        }

        public bool Insert(Task t)
        {
            if (t.IsNull())
                return false;

            return !context.Insert(t).IsNull();
        }

        public bool Update(Task t)
        {
            throw new NotImplementedException();
        }

        public List<Task> Get(Expression<Func<Task, bool>> condition)
        {
            var q= context.Query<Task>().Where(condition);

            return q.ToList();
        }

        private MySqlContext context;

    }

}
