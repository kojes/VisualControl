using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Models;
using VueExample.Repository;
using VueExample.ViewModels;

namespace VueExample.Providers
{
    public interface IDefectProvider 
    {
        int InsertNewDefect(Defect defect);
        Defect GetById(int defectId);
        Task<Defect> GetByIdAsync(int defectId);
        void DeleteById(int defectId);
        int GetDuplicate(long dieId, int stageId, int defectTypeId);
        List<Defect> GetByWaferId(string waferId);
        List<Defect> GetByWaferIdWithIncludes(string waferId);
    }
}
