using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Repository;

namespace VueExample.Providers
{
    public class BasicGraphicProvider :  Repository<Graphic>, IGraphicProvider
    {
        public Graphic GetGraphicById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
