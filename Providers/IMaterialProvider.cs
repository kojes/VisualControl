using System.Collections.Generic;
using VueExample.Models;

namespace VueExample.Providers
{
    public interface IMaterialProvider
    {
        Material ChangeName(string oldName, string newName);
        List<Material> GetAll();
        Material ChangeMaterialOnMeasurement(int measurementId, int materialId);
    } 
    
}