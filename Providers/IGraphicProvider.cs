using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Models;
using VueExample.Repository;

namespace VueExample.Providers
{
    public interface IGraphicProvider : IRepository<Graphic>
    {
        Graphic GetGraphicById(int id);
    }
}
