﻿using Common;
using Common.Cache.Redis;
using Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Business
{

    public class ProjectService : IProjectService
    {
        //private static object _queryLock;

        public ProjectService(IProjectRespository repository, IRedisCacheManager cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        public bool AddProject(Project project)
        {
            project.EnsureNotNull(() => { return new ProjectException(); });

            var result = repository.Insert(project);
            if (result)
            {
                cache.Set(project.Id, project); //强一致性的缓存更新
            }

            return result;
        }

        public List<Project> GetProjectByName(string name)
        {
            return repository.Get(p => p.Name.IndexOf(name) >= 0);
        }

        public Project GetProjectById(string id)
        {
            id.EnsureNotNull(() => { return new ProjectException(); });

            var project = cache.Get<Project>(id);
            if (project == null)
            {
                var projects = repository.Get(p => p.Id == id);
                project = projects.IsNull() ? null : projects.FirstOrDefault();
                if (project != null)
                    cache.Set(id, project); //强一致性的缓存更新
            }

            return project;
        }

        public bool UpdateProject(Project project)
        {
            project.EnsureNotNull(() => { return new ProjectException(); });
            project.Id.EnsureNotNull(() => { return new ProjectException(); });

            var result = repository.Update(project);
            if (result)
            {
                cache.Set(project.Id, project); //强一致性的缓存更新
            }

            return result;
        }

        public bool RemoveProject(string id)
        {
            id.EnsureNotNull(() => { return new ProjectException(); });

            cache.Remove(id);

            return repository.Delete(id);
        }

        private IProjectRespository repository;
        private IRedisCacheManager cache;
    }

}
