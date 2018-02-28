using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    public interface IProjectService
    {
        bool AddProject(Project p);
        Project GetProjectById(string id);
        List<Project> GetProjectByName(string name);
    }

}
