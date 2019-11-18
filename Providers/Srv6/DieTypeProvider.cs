using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.Srv6
{
    public class DieTypeProvider : IDieTypeProvider
    {
        private readonly IMapper _mapper;
        public DieTypeProvider(IMapper mapper)
        {
            _mapper = mapper;
        }        
        public async Task<DieType> Create(DieTypeUpdatingViewModel dieTypeViewModel)
        {            
            using(var db = new Srv6Context())
            {
                var dieType = new DieType{Name = dieTypeViewModel.Name};
                db.DieTypes.Add(dieType);
                await db.SaveChangesAsync();
                var elementsList = new List<Element>();
                foreach (var element in dieTypeViewModel.ElementsList)
                {
                    var createdElement = new Element{Name = element.Name, Comment = element.Comment, TypeId = element.TypeId};
                    db.Elements.Add(createdElement);
                    elementsList.Add(createdElement);
                    
                }
                await db.SaveChangesAsync();

                foreach (var element in elementsList)
                {
                    db.DieTypeElements.Add(new Entities.DieTypeElement{ElementId = element.ElementId, DieTypeId = dieType.DieTypeId});
                }
                await db.SaveChangesAsync();
                foreach (var idcp in dieTypeViewModel.CodeProductIdsList)
                {
                    db.DieTypeCodeProducts.Add(new DieTypeCodeProduct{DieTypeId = dieType.DieTypeId, CodeProductId = idcp});
                }
                await db.SaveChangesAsync();
                return dieType;
            }
        }

        public async Task<DieType> Update(DieTypeViewModel dieTypeViewModel)
        {
             using(var db = new Srv6Context())
            {
                var dieType = await db.DieTypes.FirstOrDefaultAsync(x => x.DieTypeId == dieTypeViewModel.Id);
                db.Entry(dieType).CurrentValues.SetValues(_mapper.Map<DieTypeViewModel, DieType>(dieTypeViewModel));
                await db.SaveChangesAsync();
                return dieType;
            }
        }

        public async Task<List<DieType>> GetAll()
        {
            using(var db = new Srv6Context())
            {
                return await db.DieTypes.ToListAsync();
            }
        }

        public async Task<DieTypeUpdatingViewModel> GetCodeProductsAndElements(int id)
        {
            using (var db = new Srv6Context())
            {
                var elementsList =       await db.Elements
                                         .Join(db.DieTypeElements
                                         .Where(x => x.DieTypeId == id), c => c.ElementId, p => p.ElementId, (c,p) => p.Element)
                                         .AsNoTracking()
                                         .ToListAsync();
                var codeProductIdsList = await db.CodeProducts
                                         .Join(db.DieTypeCodeProducts
                                         .Where(x => x.DieTypeId == id), c => c.IdCp, p => p.Id, (c,p) => p.CodeProduct)
                                         .AsNoTracking().Select(x => x.IdCp)
                                         .ToListAsync();
                var dieType =            await db.DieTypes
                                         .AsNoTracking()
                                         .FirstOrDefaultAsync(x => x.DieTypeId == id);
                
                var elementsViewModelList = _mapper.Map<List<Element>, List<ElementViewModel>>(elementsList);
                foreach(var element in elementsViewModelList)
                {
                    if(await db.MeasurementRecordingElements.AnyAsync(x => x.ElementId == element.ElementId))
                    {
                        element.IsAvaliableToDelete = false;
                    }
                }
                return new DieTypeUpdatingViewModel{Name = dieType.Name, 
                                                    CodeProductIdsList = codeProductIdsList.ToList(), 
                                                    ElementsList = elementsViewModelList}; 
            }
        }

        

        public async Task<Tuple<CodeProductViewModel, string>> UpdateCodeProductsMap(int dieTypeId, int codeProductId)
        {
            var action = String.Empty;
            using(var db = new Srv6Context())
            {
                var dieTypeCodeProduct = await db.DieTypeCodeProducts.FirstOrDefaultAsync(x => x.DieTypeId == dieTypeId && x.CodeProductId == codeProductId);
                if(dieTypeCodeProduct is null)
                {
                    dieTypeCodeProduct = new DieTypeCodeProduct{CodeProductId = codeProductId, DieTypeId = dieTypeId};
                    await db.AddAsync(dieTypeCodeProduct);
                    action = "INSERTED";
                }
                else
                {
                    db.DieTypeCodeProducts.Remove(dieTypeCodeProduct);
                    action = "DELETED";
                }
                await db.SaveChangesAsync();
                var codeProduct = await db.CodeProducts.FirstOrDefaultAsync(x => x.IdCp == codeProductId);               
                return new Tuple<CodeProductViewModel, string>(_mapper.Map<CodeProduct, CodeProductViewModel>(codeProduct), codeProduct is null ? "ERROR" : action);
            }
        }
    }
}