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
   

    public class KurbatovXLSViewModel
    {
        public string OperationNumber { get; set; }
        public string Element { get; set; }
        public List<KurbatovParameter> parameters = new List<KurbatovParameter>();     
     
    }

   
}