using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Providers
{
    public interface IDefectProvider
    {
        int InsertNewDefect(Defect defect);
        void DeleteById(int defectId);
    }
}
