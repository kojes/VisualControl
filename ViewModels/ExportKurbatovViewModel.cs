using System;
using System.Collections.Generic;
using VueExample.Models.SRV6.Export;

namespace VueExample.ViewModels
{
    public class ExportKurbatovViewModel
    {
        public string Name { get; set; }
        public List<AtomicDieValue> dieValueList = new List<AtomicDieValue>();
    }
   
    public class KurbatovXLSBodyModel
    {
        public List<KurbatovXLSViewModel> kurbatovXLSViewModelList = new List<KurbatovXLSViewModel>();     
     
    }
    public class KurbatovXLSViewModel
    {
        public string OperationNumber { get; set; }
        public string ElementName { get; set; }
        public string StageName {get; set;}
        public bool IsAddedToCommonWorksheet { get; set; } = true;
        public List<KurbatovParameter> parameters = new List<KurbatovParameter>();     
     
    }

   
}