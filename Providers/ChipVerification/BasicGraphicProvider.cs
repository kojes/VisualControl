using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification
{
    public class BasicGraphicProvider : IGraphicProvider
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IMapper _mapper;
        public BasicGraphicProvider(ApplicationContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }
        public async Task<List<GraphicViewModel>> GetAvailiableByMeasurementId(int measurementId)
        {
            return await _applicationContext.Point.Where(x => x.MeasurementId == measurementId).Join(_applicationContext.Graphic, 
                                                        p => p.GraphicId, 
                                                        d => d.GraphicId, 
                                                        (p,d) => new GraphicViewModel{ GraphicId = p.GraphicId, RussianName = d.RussianName, 
                                                                              Unit = d.Unit, Specification = d.Specification})                                                                              
                                                        .Distinct().AsNoTracking().ToListAsync();
        }

        public async Task<AfterDbManipulationObject<GraphicViewModel>> GetGraphicById(int graphicId)
        {
            var graphic = await _applicationContext.Graphic.FindAsync(graphicId);
            var obj = new AfterDbManipulationObject<GraphicViewModel>(_mapper.Map<GraphicViewModel>(graphic));
            if(graphic is null)
            {
                obj.AddError(new Error("@График не найден"));
            }
            return obj;
        }

        public async Task<AfterDbManipulationObject<GraphicViewModel>> GetGraphicByNameAndType(string name, string type)
        {
            var graphic = await _applicationContext.Graphic.FirstOrDefaultAsync(x => x.Specification == name && x.Type == type);
            var obj = new AfterDbManipulationObject<GraphicViewModel>(_mapper.Map<GraphicViewModel>(graphic));
            if(graphic is null)
            {
                obj.AddError(new Error("@График не найден"));
            }
            return obj;
        }
    }
}
