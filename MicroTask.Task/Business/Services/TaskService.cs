using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common;
using Infrastructure;

namespace Business
{

    public class TaskService : ITaskService
    {

        public TaskService(ITaskRespository repository)
        {
            this.repository = repository;
        }

        public bool AddTask(Task t)
        {
            t.EnsureNotNull(() => { return new TasktException(); });

            return repository.Insert(t);
        }

        public List<Task> GetTaskByName(string name)
        {
            return repository.Get(t => t.Name.IndexOf(name) >= 0);
        }

        public Task GetTaskById(string id)
        {
            id.EnsureNotNull(() => { return new TasktException(); });

            var projects = repository.Get(t => t.Id == id);

            return projects.IsNull() ? null : projects.FirstOrDefault();
        }

        ITaskRespository repository;

    }

}
