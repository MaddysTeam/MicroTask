using Chloe.MySql;
using System;
using System.Collections.Generic;
using Common;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using System.Linq.Expressions;

namespace Domain
{

    public class ProjectRespository : IProjectRespository
    {

        public ProjectRespository(MySqlContext context)
        {
            this.context = context;
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        //public List<Project> GetAll()
        //{
        //    var q = context.Query<Project>();

        //    return q.ToList();
        //}

        public Project GetById(string id)
        {
            throw new NotImplementedException();
        }

        public bool Insert(Project t)
        {
            if (t.IsNull())
                return false;

            return !context.Insert(t).IsNull();
        }

        public bool Update(Project t)
        {
            throw new NotImplementedException();
        }

        public List<Project> Get(Expression<Func<Project, bool>> condition)
        {
            var q= context.Query<Project>().Where(condition);

            return q.ToList();
        }

        private MySqlContext context;

    }

}
