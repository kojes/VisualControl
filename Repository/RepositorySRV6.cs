using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Contexts;

namespace VueExample.Repository
{
    public abstract class RepositorySRV6<T> : IRepository<T> where T : class
    {
        public virtual T GetById(int id)
        {
            using (var context = new Srv6Context())
            {
                return context.Set<T>().Find(id);
            }
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            using (var context = new Srv6Context())
            {
                return await context.Set<T>().FindAsync(id);
            }
        }

        public T Add(T entity)
        {
            using (var context = new Srv6Context())
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();

            }

            return entity;
        }

        public List<T> GetAll()
        {
            using (var context = new Srv6Context())
            {
                return context.Set<T>().ToList();
            }
        }

        public bool RemoveById(int id)
        {
            using (var context = new Srv6Context())
            {
                var entity = context.Set<T>().Find(id);
                if (entity == null)
                {
                    return false;
                }

                context.Set<T>().Remove(entity);
                return true;
            }
        }
    }
}
