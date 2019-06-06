using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VueExample.Contexts;

namespace VueExample.Repository
{
    public interface IRepository<T>
    {
        T GetById(int id);
        T GetById(long id);
        Task<T> GetByIdAsync(int id);
        T Add(T entity);
        List<T> GetAll();
        bool RemoveById(int id);
    }

   
}
