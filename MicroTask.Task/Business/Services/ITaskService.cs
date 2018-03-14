using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{

    public interface ITaskService
    {
        bool AddTask(Task p);
        Task GetTaskById(string id);
        List<Task> GetTaskByName(string name);
    }

}
