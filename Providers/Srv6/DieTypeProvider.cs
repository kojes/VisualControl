using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.Srv6
{
    public class DieTypeProvider : IDieTypeProvider
    {        
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

        public async Task<List<DieType>> GetAll()
        {
            using(var db = new Srv6Context())
            {
                return await db.DieTypes.ToListAsync();
            }
        }

        public Task<AfterDbManipulationObject<DieType>> Update(DieTypeViewModel dieTypeViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<StandardResponseObject> UpdateCodeProductsMap(int dieTypeId, IList<int> codeProductsIdList)
        {
            throw new System.NotImplementedException();
        }
    }
}