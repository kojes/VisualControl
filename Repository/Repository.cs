using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Contexts;

namespace VueExample.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        public virtual T GetById(int id)
        {
            using (VisualControlContext context = new VisualControlContext())
            {
                return context.Set<T>().Find(id);
            }
        }

        public T Add(T entity)
        {
            using (VisualControlContext context = new VisualControlContext())
            {
                 context.Set<T>().Add(entity);
                 context.SaveChanges();

            }

            return entity;
        }

        public List<T> GetAll()
        {
            using (var context = new VisualControlContext())
            {
                return context.Set<T>().ToList();
            }
        }

        public bool RemoveById(int id)
        {
            using (var context = new VisualControlContext())
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
