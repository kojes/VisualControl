using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Providers.ChipVerification.Abstract;

namespace VueExample.Providers.ChipVerification
{
    public class BasicGraphicProvider : IGraphicProvider
    {
        private readonly ApplicationContext _applicationContext;
        public BasicGraphicProvider(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public async Task<List<Graphic>> GetAvailiableByMeasurementId(int measurementId)
        {
            return await _applicationContext.Point.Where(x => x.MeasurementId == measurementId).Join(_applicationContext.Graphic, 
                                                        p => p.GraphicId, 
                                                        d => d.GraphicId, 
                                                        (p,d) => new Graphic{ GraphicId = p.GraphicId, RussianName = d.RussianName, 
                                                                              Unit = d.Unit, Specification = d.Specification})                                                                              
                                                        .Distinct().ToListAsync();
        }

        public async Task<Graphic> GetGraphicById(int id)
        {
            return await _applicationContext.Graphic.FindAsync(id);
        }
    }
}
