using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using VueExample.Contexts;

namespace VueExample.Handlers
{
    public class DuplicatesPreventionHandler<T> where T : class 
    {
        public T AddedObject  { get; private set; }
        public string Error { get; private set; }

        public void Add(T addingObject, DbContext dbContext)
        {
            using (var applicationContext = new VisualControlContext())
            {
                applicationContext.Set<T>().Add(addingObject);
                applicationContext.SaveChangesAsync();
            }
        }


    }
}
