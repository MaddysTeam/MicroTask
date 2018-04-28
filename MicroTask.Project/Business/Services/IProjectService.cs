using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    public interface IProjectService
    {
        bool AddProject(Project p);
        bool UpdateProject(Project p);
        bool RemoveProject(string id);
        Project GetProjectById(string id);
        List<Project> GetProjectByName(string name);
    }

}
