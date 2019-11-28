using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6.Interfaces;
using System.Collections.Generic;
using VueExample.ViewModels;
using AutoMapper;

namespace VueExample.Providers.Srv6
{
    public class ElementService : IElementService
    {
        private readonly IMapper _mapper;
        public ElementService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task AddToDieType(int elementId, int dieTypeId)
        {
            using(var db = new Srv6Context())
            {
                db.DieTypeElements.Add(new Entities.DieTypeElement{ElementId = elementId, DieTypeId = dieTypeId});
                await db.SaveChangesAsync();
            }
        }

        public async Task Delete(int id) 
        {
            using(var db = new Srv6Context())
            {
                var dieTypeElements = await db.DieTypeElements.Where(x => x.ElementId == id).ToListAsync();
                db.DieTypeElements.RemoveRange(dieTypeElements);
                await db.SaveChangesAsync();
                var element = await db.Elements.FirstOrDefaultAsync(x => x.ElementId == id);
                db.Elements.Remove(element);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Element> Create(ElementViewModel elementViewModel)
        {
            using(var db = new Srv6Context())
            {
               var element = _mapper.Map<ElementViewModel, Element>(elementViewModel);
               await db.Elements.AddAsync(element);
               await db.SaveChangesAsync();
               return element;
            }
        }
        
        public async Task<Element> Update(ElementViewModel elementViewModel)
        {
            using(var db = new Srv6Context())
            {
                var element = await db.Elements.FirstOrDefaultAsync(x => x.ElementId == elementViewModel.ElementId);
                db.Entry(element).CurrentValues.SetValues(_mapper.Map<ElementViewModel, Element>(elementViewModel));
                await db.SaveChangesAsync();
                return element;
            }
        }

        public async Task<List<Element>> GetByDieType(int dieTypeId)
        {
             using (Srv6Context db = new Srv6Context())
             {
                var elementList = await db.Elements.Join(db.DieTypeElements, 
                                                        c => c.ElementId, 
                                                        p => p.ElementId, 
                                                        (c,p) => new {Element = c, DieTypeElement = p})
                                                    .Join(db.DieTypes, 
                                                        c => c.DieTypeElement.DieTypeId,
                                                        p => p.DieTypeId,
                                                        (c,p) => new {DieType = p, Element = c.Element})
                                                    .Where(x => x.DieType.DieTypeId == dieTypeId)
                                                    .Select(x => x.Element)     
                                                    .ToListAsync();
                return elementList;
             }            
        }

        public async Task<Element> GetById(int elementId)
        {
             using (Srv6Context db = new Srv6Context())
             {
                return await db.Elements.FindAsync(elementId);
             }
            
        }

        public async Task<List<Element>> GetByIdmr(int idmr) 
        {
             using (Srv6Context db = new Srv6Context())
             {
                var elementList = await db.Elements.Join(db.MeasurementRecordingElements, 
                                                        c => c.ElementId, 
                                                        p => p.ElementId, 
                                                        (c,p) => new {Element = c, MeasurementRecordingElement = p})
                                                    .Join(db.MeasurementRecordings, 
                                                        c => c.MeasurementRecordingElement.MeasurementRecordingId,
                                                        p => p.Id,
                                                        (c,p) => new {MeasurementRecording = p, Element = c.Element})
                                                    .Where(x => x.MeasurementRecording.Id == idmr)
                                                    .Select(x => x.Element)     
                                                    .ToListAsync();
                return elementList;
             }            
        }

        public async Task<Element> GetByNameAndWafer(string name, string waferId)
        {
             using (Srv6Context db = new Srv6Context())
             {
                 var waferIdSqlParameter = new SqlParameter("waferId", waferId);
                 var elementNameSqlParameter = new SqlParameter("name", name);
                 return await db.Elements.FromSql("EXECUTE dbo.select_element_by_waferId_elementname @waferId, @name", waferIdSqlParameter, elementNameSqlParameter).FirstOrDefaultAsync();
             }
        }

       
        public async Task<Element> UpdateElementOnIdmr(int measurementRecordingId, int newElementId)
        {
            using (Srv6Context db = new Srv6Context())
            {
                var measurementRecordingElement = db.MeasurementRecordingElements.FirstOrDefault(x => x.MeasurementRecordingId == measurementRecordingId);
                if(measurementRecordingElement is null) 
                {
                    db.MeasurementRecordingElements.Add(new MeasurementRecordingElement{ElementId = newElementId, MeasurementRecordingId = measurementRecordingId});
                }
                else
                {
                    measurementRecordingElement.ElementId = newElementId;
                }
                await db.SaveChangesAsync();
                return await db.Elements.FirstOrDefaultAsync(x => x.ElementId == newElementId);
            }
            
        }

       
    }
}