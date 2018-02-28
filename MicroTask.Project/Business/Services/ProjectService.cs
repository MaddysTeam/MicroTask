using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common;
using Infrastructure;

namespace Business
{

    public class ProjectService : IProjectService
    {

        public ProjectService(IProjectRespository repository)
        {
            this.repository = repository;
        }

        public bool AddProject(Project p)
        {
            p.EnsureNotNull(() => { return new ProjectException(); });

            return repository.Insert(p);
        }

        public List<Project> GetProjectByName(string name)
        {
            return repository.Get(p => p.Name.IndexOf(name) >= 0);
        }

        public Project GetProjectById(string id)
        {
            id.EnsureNotNull(() => { return new ProjectException(); });

            var projects = repository.Get(p => p.Id == id);

            return projects.IsNull() ? null : projects.FirstOrDefault();
        }


        IProjectRespository repository;

    }

}
